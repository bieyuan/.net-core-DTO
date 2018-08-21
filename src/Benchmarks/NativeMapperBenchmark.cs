using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;

namespace Benchmarks
{
    [RankColumn, CategoriesColumn, BenchmarkCategory("NativeMapper")]
    public class NativeMapperBenchmark : BenchmarkBase
    {
        [Benchmark]
        public override void Nomal()
        {
            var model = GetNomalModel();
            if (model != null)
            {
                var b = new TestB
                {
                    Id = model.Id,
                    Name = model.Name,
                };
            }
        }

        [Benchmark]
        public override void Complex()
        {
            var model = GetComplexModel();
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
                        Id = 1,
                        Name = "name",
                    };
                }
            }
        }

        [Benchmark]
        public override void Nest()
        {
            var model = GetNestModel();
            if (model != null)
            {
                var b = new TestB
                {
                    Id = model.Id,
                    Name = model.Name,
                    TestClass = model.TestClass == null ? null : new TestD
                    {
                        Id = model.TestClass.Id,
                        Name = model.TestClass.Name,
                        SelfClass = model.TestClass.SelfClass == null ? null : new TestD
                        {
                            Id = model.TestClass.SelfClass.Id,
                            Name = model.TestClass.SelfClass.Name,
                            SelfClass = model.TestClass.SelfClass.SelfClass == null ? null : new TestD
                            {
                                Id = model.TestClass.SelfClass.SelfClass.Id,
                                Name = model.TestClass.SelfClass.SelfClass.Name,
                                SelfClass = model.TestClass.SelfClass.SelfClass.SelfClass == null ? null : new TestD
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

        [Benchmark]
        public override void List()
        {
            var model = GetListModel();
            if (model != null)
            {
                var b = new TestB
                {
                    Id = model.Id,
                    Name = model.Name,
                    TestLists = new List<TestD> {
                        new TestD{
                            Id = model.TestLists.ElementAt(0).Id,
                            Name =  model.TestLists.ElementAt(0).Name,
                        },
                        new TestD{
                            Id =  model.TestLists.ElementAt(1).Id,
                            Name =  model.TestLists.ElementAt(1).Name,
                        },
                    }.ToArray()
                };
            }
        }

        public override void Initial()
        {
            throw new System.NotImplementedException();
        }
    }
}