using System.Collections.Generic;

namespace Benchmarks
{
    public abstract class BenchmarkBase
    {
        public TestA GetNomalModel()
        {
            return new TestA
            {
                Id = 1,
                Name = "张三",
            };
        }

        public TestA GetComplexModel()
        {
            return new TestA
            {
                Id = 1,
                Name = "张三",
                TestClass = new TestC
                {
                    Id = 2,
                    Name = "lisi",
                },
            };
        }

        public TestA GetNestModel()
        {
            return new TestA
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
        }

        public TestA GetListModel()
        {
            return new TestA
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
        }

        public abstract void Initial();

        public abstract void Nomal();

        public abstract void Complex();

        public abstract void Nest();

        public abstract void List();
    }

    public class TestA
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TestC TestClass { get; set; }

        public IEnumerable<TestC> TestLists { get; set; }
    }

    public class TestB
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TestD TestClass { get; set; }

        public TestD[] TestLists { get; set; }
    }

    public class TestC
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TestC SelfClass { get; set; }
    }

    public class TestD
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TestD SelfClass { get; set; }
    }
}