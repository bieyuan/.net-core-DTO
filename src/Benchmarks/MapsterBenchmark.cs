using BenchmarkDotNet.Attributes;
using Mapster;

namespace Benchmarks
{
    public class MapsterBenchmark : BenchmarkBase
    {
        [GlobalSetup]
        public override void Initial()
        {
            var model = GetNomalModel();
            var b = model.Adapt<TestB>();
        }

        [Benchmark]
        public override void Nomal()
        {
            var model = GetNomalModel();
            var b = model.Adapt<TestB>();
        }

        [Benchmark]
        public override void Complex()
        {
            var model = GetComplexModel();
            var b = model.Adapt<TestB>();
        }

        [Benchmark]
        public override void Nest()
        {
            var model = GetNestModel();
            var b = model.Adapt<TestB>();
        }

        [Benchmark]
        public override void List()
        {
            var model = GetListModel();
            var b = model.Adapt<TestB>();
        }
    }
}