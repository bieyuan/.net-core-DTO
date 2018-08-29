using System;
using System.Collections.Generic;
using System.Diagnostics;
using ExpressionMapper;
using Nelibur.ObjectMapper;

namespace Benchmarks
{
    public static class MapperTest
    {
        //执行次数
        public static int Count = 100000;

        //简单类型
        public static void Nomal()
        {
            Console.WriteLine($"******************简单类型:{Count / 10000}万次执行时间*****************");
            var model = new TestA
            {
                Id = 1,
                Name = "张三",
            };

            //计时
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < Count; i++)
            {
                if (model != null)
                {
                    var b = new TestB
                    {
                        Id = model.Id,
                        Name = model.Name,
                    };
                }
            }
            sw.Stop();
            Console.WriteLine($"原生的时间:{sw.ElapsedMilliseconds}ms");

            Exec(model);
        }

        //复杂类型
        public static void Complex()
        {
            Console.WriteLine($"********************复杂类型:{Count / 10000}万次执行时间*********************");
            var model = new TestA
            {
                Id = 1,
                Name = "张三",
                TestClass = new TestC
                {
                    Id = 2,
                    Name = "lisi",
                },
            };

            //计时
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < Count; i++)
            {
                if (model != null)
                {
                    var b = new TestB
                    {
                        Id = model.Id,
                        Name = model.Name,
                    };
                    if (model.TestClass != null)
                    {
                        b.TestClass = new TestD
                        {
                            Id = i,
                            Name = "lisi",
                        };
                    }
                }
            }
            sw.Stop();
            Console.WriteLine($"原生的时间:{sw.ElapsedMilliseconds}ms");
            Exec(model);
        }

        //嵌套类型
        public static void Nest()
        {
            Console.WriteLine($"*****************嵌套类型:{Count / 10000}万次执行时间*************************");
            var model = new TestA
            {
                Id = 1,
                Name = "张三",
                TestClass = new TestC
                {
                    Id = 1,
                    Name = "lisi",
                    SelfClass = new TestC
                    {
                        Id = 2,
                        Name = "lisi",
                        SelfClass = new TestC
                        {
                            Id = 3,
                            Name = "lisi",
                            SelfClass = new TestC
                            {
                                Id = 4,
                                Name = "lisi",
                            },
                        },
                    },
                },
            };
            //计时
            var item = model;
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < Count; i++)
            {
                //这里每一步需要做非空判断的，书写太麻烦省去了
                if (model != null)
                {
                    var b = new TestB
                    {
                        Id = model.Id,
                        Name = model.Name,
                        TestClass = new TestD
                        {
                            Id = model.TestClass.Id,
                            Name = model.TestClass.Name,
                            SelfClass = new TestD
                            {
                                Id = model.TestClass.SelfClass.Id,
                                Name = model.TestClass.SelfClass.Name,
                                SelfClass = new TestD
                                {
                                    Id = model.TestClass.SelfClass.SelfClass.Id,
                                    Name = model.TestClass.SelfClass.SelfClass.Name,
                                    SelfClass = new TestD
                                    {
                                        Id = model.TestClass.SelfClass.SelfClass.SelfClass.Id,
                                        Name = model.TestClass.SelfClass.SelfClass.SelfClass.Name,
                                    },
                                },
                            },
                        },
                    };
                }
            }
            sw.Stop();
            Console.WriteLine($"原生的时间:{sw.ElapsedMilliseconds}ms");

            Exec(model);
        }

        //集合
        public static void List()
        {
            Console.WriteLine($"********************集合类型:{Count / 10000}万次执行时间***************************");

            var model = new TestA
            {
                Id = 1,
                Name = "张三",
                TestLists = new List<TestC> {
                            new TestC{
                             Id = 1,
                            Name =  "张三",
                           },
                            new TestC{
                            Id = -1,
                            Name =  "张三",
                           },
                        }
            };

            //计时
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < Count; i++)
            {
                var item = model;
                if (item != null)
                {
                    var b = new TestB
                    {
                        Id = item.Id,
                        Name = item.Name,
                        TestLists = new List<TestD> {
                            new TestD{
                                   Id = item.Id,
                            Name = item.Name,
                           },
                            new TestD{
                            Id = -item.Id,
                            Name = item.Name,
                           },
                        }.ToArray()
                    };
                }
            }
            sw.Stop();
            Console.WriteLine($"原生的时间:{sw.ElapsedMilliseconds}ms");

            Exec(model);
        }

        public static void Exec(TestA model)
        {
            //表达式
            Mapper<TestA, TestB>.Map(model);
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < Count; i++)
            {
                var b = Mapper<TestA, TestB>.Map(model);
            }
            sw.Stop();
            Console.WriteLine($"表达式的时间:{sw.ElapsedMilliseconds}ms");

            //AutoMapper
            sw.Restart();
            for (int i = 0; i < Count; i++)
            {
                var b = AutoMapper.Mapper.Map<TestA, TestB>(model);
            }
            sw.Stop();
            Console.WriteLine($"AutoMapper时间:{sw.ElapsedMilliseconds}ms");

            //TinyMapper
            sw.Restart();
            for (int i = 0; i < Count; i++)
            {
                var b = TinyMapper.Map<TestA, TestB>(model);
            }
            sw.Stop();
            Console.WriteLine($"TinyMapper时间:{sw.ElapsedMilliseconds}ms");
        }
    }
}