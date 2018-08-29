using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using ExpressionMapper;

namespace Benchmarks
{
    [RankColumn, CategoriesColumn, BenchmarkCategory("ExpressionMapper")]
    public class ExpressionMapperBenchmark : BenchmarkBase
    {
        [GlobalSetup]
        public override void Initial()
        {
            Mapper<TestA, TestB>.Map(new TestA());
        }

        [Benchmark]
        public override void Nomal()
        {
            var model = GetNomalModel();
            var b = Mapper<TestA, TestB>.Map(model);
        }

        [Benchmark]
        public override void Complex()
        {
            var model = GetComplexModel();
            var b = Mapper<TestA, TestB>.Map(model);
        }

        [Benchmark]
        public override void Nest()
        {
            var model = GetNestModel();
            var b = Mapper<TestA, TestB>.Map(model);
        }

        [Benchmark]
        public override void List()
        {
            var model = GetListModel();
            var b = Mapper<TestA, TestB>.Map(model);
        }
    }
}