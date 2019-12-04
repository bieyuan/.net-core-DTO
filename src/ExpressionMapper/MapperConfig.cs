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
    public class MapperConfig<TSource, TTarget> where TSource : class where TTarget : class, new()
    {
        /// <summary>
        /// 指定列转换
        /// </summary>
        public Action<TSource, TTarget> BindAction { get;  set; }

        /// <summary>
        /// 忽略需要转换的列
        /// </summary>
        public  List<string> IgnoreColoums { get; private set; }

        /// <summary>
        /// 忽略大小写
        /// </summary>
        public  bool IgnoreCase { get;  set; }

        public void Bind(Action<TSource, TTarget> action)
        {
            BindAction = action;
        }


        public void Ignore(Expression<Func<TTarget,object>> expression)
        {
            LambdaExpression lambda = expression as LambdaExpression;
            if (lambda == null)
                throw new ArgumentNullException("method");

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr = ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
                throw new ArgumentException("method");

            if (IgnoreColoums == null)
                IgnoreColoums = new List<string>();

            IgnoreColoums.Add(memberExpr.Member.Name);
        }
    }

}
