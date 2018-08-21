using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    public class AutoMapperBenchmark : BenchmarkBase
    {
        [GlobalSetup]
        public override void Initial()
        {
            AutoMapper.Mapper.Initialize(cfg => cfg.CreateMap<TestA, TestB>());
        }

        [Benchmark]
        public override void Complex()
        {
            var model = GetComplexModel();
            var b = AutoMapper.Mapper.Map<TestA, TestB>(model);
        }

        [Benchmark]
        public override void List()
        {
            var model = GetListModel();
            var b = AutoMapper.Mapper.Map<TestA, TestB>(model);
        }

        [Benchmark]
        public override void Nest()
        {
            var model = GetNestModel();
            var b = AutoMapper.Mapper.Map<TestA, TestB>(model);
        }

        [Benchmark]
        public override void Nomal()
        {
            var model = GetNomalModel();
            var b = AutoMapper.Mapper.Map<TestA, TestB>(model);
        }
    }
}