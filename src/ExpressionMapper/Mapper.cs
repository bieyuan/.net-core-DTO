using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Linq.Expressions.Expression;
namespace System
{
    public class Mapper<TSource, TTarget> where TSource : class where TTarget : class, new()
    {
        public readonly static Func<TSource, TTarget> MapFunc = GetMapFunc();

        public readonly static Action<TSource, TTarget> MapAction = GetMapAction();

        private static MapperConfig<TSource, TTarget> _config { get; set; } = new MapperConfig<TSource, TTarget>();

        public static void CreatConfig(Action<MapperConfig<TSource, TTarget>> action)
        {
            var config = new MapperConfig<TSource, TTarget>();
            action.Invoke(config);
            _config = config;
        }

        /// <summary>
        /// 将对象TSource转换为TTarget
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TTarget Map(TSource source, Action<TSource, TTarget> action = null)
        {
            if (source == null) return null;

            var target = MapFunc(source);
            _config?.BindAction?.Invoke(source, target);
            action?.Invoke(source, target);
            return target;
        }

        public static void Map(TSource source, TTarget target, Action<TSource, TTarget> action = null)
        {
            if (source == null || target == null) return;

            MapAction(source, target);
            _config?.BindAction?.Invoke(source, target);
            action?.Invoke(source, target);
        }


        public static List<TTarget> MapList(IEnumerable<TSource> sources, Action<TSource, TTarget> action = null)
        {
            if (sources == null) return null;

            var list = new List<TTarget>();
            foreach (var item in sources)
            {
                list.Add(Map(item, action));
            }
            return list;
        }


        private static Func<TSource, TTarget> GetMapFunc()
        {
            return source =>
            {
                var target = new TTarget();
                MapAction(source, target);
                return target;
            };
        }

        private static Action<TSource, TTarget> GetMapAction()
        {
            _config = new MapperConfig<TSource, TTarget>();

            var sourceType = typeof(TSource);
            var targetType = typeof(TTarget);

            if (IsEnumerable(sourceType) || IsEnumerable(targetType))
                throw new NotSupportedException("Enumerable types are not supported,please use MapList method.");

            //Func委托传入变量
            var sourceParameter = Parameter(sourceType, "p");

            var targetParameter = Parameter(targetType, "t");

            //创建一个表达式集合
            var expressions = new List<Expression>();

            // x.GetIndexParameters().Length ==0  过滤 this[index]  索引项
            var targetTypes = targetType.GetProperties().Where(x => x.GetIndexParameters().Length == 0 && (x.PropertyType.IsPublic || x.PropertyType.IsNestedPublic) && x.CanWrite);

            //过滤忽略项
            if (_config.IgnoreColoums != null && _config.IgnoreColoums.Count > 0)
            {
                targetTypes = targetTypes.Where(x => !_config.IgnoreColoums.Contains(x.Name));
            }

            var sourceTypes = sourceType.GetProperties().Where(x => x.GetIndexParameters().Length == 0 && (x.PropertyType.IsPublic || x.PropertyType.IsNestedPublic) && x.CanRead);
            foreach (var targetItem in targetTypes)
            {
                var sourceItem = sourceTypes.FirstOrDefault(x => string.Compare(x.Name, targetItem.Name, _config.IgnoreCase) == 0);

                //判断实体的读写权限
                if (sourceItem == null || !sourceItem.CanRead || sourceItem.PropertyType.IsNotPublic)
                    continue;

                //标注NotMapped特性的属性忽略转换
                if (sourceItem.GetCustomAttribute<NotMappedAttribute>() != null)
                    continue;

                var sourceProperty = Property(sourceParameter, sourceItem);
                var targetProperty = Property(targetParameter, targetItem);


                //当非值类型且类型不相同时
                if (!sourceItem.PropertyType.IsValueType && sourceItem.PropertyType != targetItem.PropertyType
                    && sourceItem.PropertyType != typeof(string) && targetItem.PropertyType != typeof(string)
                    )
                {
                    //判断都是(非泛型、非数组)class
                    if (sourceItem.PropertyType.IsClass && targetItem.PropertyType.IsClass
                        && !sourceItem.PropertyType.IsArray && !targetItem.PropertyType.IsArray
                        && !sourceItem.PropertyType.IsGenericType && !targetItem.PropertyType.IsGenericType)
                    {
                        var expression = GetClassExpression(sourceProperty, sourceItem.PropertyType, targetItem.PropertyType);
                        expressions.Add(Assign(targetProperty, expression));
                    }

                    //集合数组类型的转换
                    if (typeof(IEnumerable).IsAssignableFrom(sourceItem.PropertyType) && typeof(IEnumerable).IsAssignableFrom(targetItem.PropertyType))
                    {
                        var expression = GetListExpression(sourceProperty, sourceItem.PropertyType, targetItem.PropertyType);
                        expressions.Add(Assign(targetProperty, expression));
                    }

                    continue;
                }

                //可空类型转换到非可空类型，当可空类型值为null时，用默认值赋给目标属性；不为null就直接转换
                if (IsNullableType(sourceItem.PropertyType) && !IsNullableType(targetItem.PropertyType))
                {
                    var hasValueExpression = Equal(Property(sourceProperty, "HasValue"), Constant(true));
                    var conditionItem = Condition(hasValueExpression, Convert(sourceProperty, targetItem.PropertyType), Default(targetItem.PropertyType));
                    expressions.Add(Assign(targetProperty, conditionItem));
                    continue;
                }

                //非可空类型转换到可空类型，直接转换
                if (!IsNullableType(sourceItem.PropertyType) && IsNullableType(targetItem.PropertyType))
                {
                    var memberExpression = Convert(sourceProperty, targetItem.PropertyType);
                    expressions.Add(Assign(targetProperty, memberExpression));
                    continue;
                }

                if (targetItem.PropertyType != sourceItem.PropertyType)
                    continue;


                expressions.Add(Assign(targetProperty, sourceProperty));

            }

            //当Target!=null判断source是否为空
            var testSource = NotEqual(sourceParameter, Constant(null, sourceType));
            var ifTrueSource = Block(expressions);
            var conditionSource = IfThen(testSource, ifTrueSource);

            //判断target是否为空
            var testTarget = NotEqual(targetParameter, Constant(null, targetType));
            var conditionTarget = IfThen(testTarget, conditionSource);

            var lambda = Lambda<Action<TSource, TTarget>>(conditionTarget, sourceParameter, targetParameter);
            return lambda.Compile();
        }

        private static Expression GetClassExpression(Expression sourceProperty, Type sourceType, Type targetType)
        {
            //条件p.Item!=null
            var testItem = NotEqual(sourceProperty, Constant(null, sourceType));

            //构造回调 Mapper<TSource, TTarget>.Map()
            var mapperType = typeof(Mapper<,>).MakeGenericType(sourceType, targetType);
            var actionType = typeof(Action<,>).MakeGenericType(sourceType, targetType);

            var iftrue = Call(mapperType.GetMethod(nameof(Map), new Type[] { sourceType, actionType }), sourceProperty, Constant(null, actionType));

            var conditionItem = Condition(testItem, iftrue, Constant(null, targetType));

            return conditionItem;
        }

        private static Expression GetListExpression(Expression sourceProperty, Type sourceType, Type targetType)
        {
            //条件p.Item!=null
            var testItem = NotEqual(sourceProperty, Constant(null, sourceType));

            //构造回调 Mapper<TSource, TTarget>.MapList()
            var sourceArg = sourceType.IsArray ? sourceType.GetElementType() : sourceType.GetGenericArguments()[0];
            var targetArg = targetType.IsArray ? targetType.GetElementType() : targetType.GetGenericArguments()[0];
            var mapperType = typeof(Mapper<,>).MakeGenericType(sourceArg, targetArg);
            var actionType = typeof(Action<,>).MakeGenericType(sourceArg, targetArg);

            var mapperExecMap = Call(mapperType.GetMethod(nameof(MapList), new Type[] { sourceType, actionType }), sourceProperty, Constant(null, actionType));

            Expression iftrue;
            if (targetType == mapperExecMap.Type)
            {
                iftrue = mapperExecMap;
            }
            else if (targetType.IsArray)//数组类型调用ToArray()方法
            {
                iftrue = Call(typeof(Enumerable), nameof(Enumerable.ToArray), new[] { mapperExecMap.Type.GenericTypeArguments[0] }, mapperExecMap);
            }
            else if (typeof(IDictionary).IsAssignableFrom(targetType))
            {
                iftrue = Constant(null, targetType);//字典类型不转换
            }
            else
            {
                iftrue = Convert(mapperExecMap, targetType);
            }

            var conditionItem = Condition(testItem, iftrue, Constant(null, targetType));

            return conditionItem;
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static bool IsEnumerable(Type type)
        {
            return type.IsArray
                   || type.GetInterfaces().Any(x => x == typeof(ICollection) || x == typeof(IEnumerable));
        }
    }
}
