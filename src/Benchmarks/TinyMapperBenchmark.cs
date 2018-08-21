using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using Nelibur.ObjectMapper;

namespace Benchmarks
{
    [RankColumn, CategoriesColumn, BenchmarkCategory("TinyMapper")]
    public class TinyMapperBenchmark : BenchmarkBase
    {
        [GlobalSetup]
        public override void Initial()
        {
            TinyMapper.Bind<TestA, TestB>();
        }

        [Benchmark]
        public override void Complex()
        {
            var model = GetComplexModel();
            var b = TinyMapper.Map<TestA, TestB>(model);
        }

        [Benchmark]
        public override void List()
        {
            var model = GetListModel();
            var b = TinyMapper.Map<TestA, TestB>(model);
        }

        [Benchmark]
        public override void Nest()
        {
            var model = GetNestModel();
            var b = TinyMapper.Map<TestA, TestB>(model);
        }

        [Benchmark]
        public override void Nomal()
        {
            var model = GetNomalModel();
            var b = TinyMapper.Map<TestA, TestB>(model);
        }
    }
}