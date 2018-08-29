using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var benchmarks = new List<Benchmark>();
            var benchTypes = Assembly.GetEntryAssembly().DefinedTypes.Where(t => t.IsSubclassOf(typeof(BenchmarkBase)));
            foreach (var b in benchTypes)
            {
                benchmarks.AddRange(BenchmarkConverter.TypeToBenchmarks(b).Benchmarks);
            }
            BenchmarkRunner.Run(benchmarks.ToArray(), null);
        }
    }
}