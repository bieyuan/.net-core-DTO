# ExpressionMapper
高性能对象映射库
 
## 构造测试类

``` C#
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
```

## 调用如下

  ``` C#
  var mode=new TestA();
  TestB b = Mapper<TestA, TestB>.Map(model);
  
  var a = new TestA();
  a.ID = 1;
  var b = new TestB();
  Mapper<TestA, TestB>.Map(a ,b);
  ```

## Benchmark

``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17741
Intel Core i5-7500 CPU 3.40GHz (Kaby Lake), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=2.1.400
  [Host]     : .NET Core 2.1.2 (CoreCLR 4.6.26628.05, CoreFX 4.6.26629.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.2 (CoreCLR 4.6.26628.05, CoreFX 4.6.26629.01), 64bit RyuJIT


```
|                      Type |  Method |         Mean |       Error |      StdDev |       Median |
|-------------------------- |-------- |-------------:|------------:|------------:|-------------:|
|     NativeMapperBenchmark |   Nomal |     17.31 ns |   0.3750 ns |   0.4605 ns |     17.38 ns |
| ExpressionMapperBenchmark |   Nomal |     36.03 ns |   0.7282 ns |   0.8386 ns |     36.24 ns |
|          MapsterBenchmark |   Nomal |     79.94 ns |   0.6340 ns |   0.5294 ns |     79.89 ns |
|       TinyMapperBenchmark |   Nomal |     95.83 ns |   0.4406 ns |   0.3906 ns |     95.76 ns |
|       AutoMapperBenchmark |   Nomal |    205.25 ns |   2.4091 ns |   2.2535 ns |    205.73 ns |
|     NativeMapperBenchmark | Complex |     33.67 ns |   0.5558 ns |   0.4927 ns |     33.64 ns |
| ExpressionMapperBenchmark | Complex |     62.55 ns |   1.2546 ns |   2.0613 ns |     61.57 ns |
|          MapsterBenchmark | Complex |    105.50 ns |   1.5860 ns |   1.4835 ns |    105.29 ns |
|       TinyMapperBenchmark | Complex |    129.28 ns |   2.3093 ns |   2.1601 ns |    128.99 ns |
|       AutoMapperBenchmark | Complex |  6,640.28 ns | 107.8270 ns |  95.5859 ns |  6,648.63 ns |
|     NativeMapperBenchmark |    List |    213.80 ns |   3.1112 ns |   2.9103 ns |    213.93 ns |
| ExpressionMapperBenchmark |    List |    226.52 ns |   4.4336 ns |   4.1472 ns |    226.23 ns |
|          MapsterBenchmark |    List |    237.45 ns |   3.1549 ns |   2.9511 ns |    237.59 ns |
|       TinyMapperBenchmark |    List |    368.29 ns |   3.3650 ns |   3.1477 ns |    368.41 ns |
|       AutoMapperBenchmark |    List | 12,998.71 ns | 240.7413 ns | 225.1895 ns | 12,981.34 ns |
|     NativeMapperBenchmark |    Nest |     95.87 ns |   1.9204 ns |   2.0548 ns |     95.86 ns |
| ExpressionMapperBenchmark |    Nest |    146.40 ns |   2.1350 ns |   1.9970 ns |    145.73 ns |
|          MapsterBenchmark |    Nest |    176.49 ns |   3.4855 ns |   3.4232 ns |    176.28 ns |
|       TinyMapperBenchmark |    Nest |    213.45 ns |   1.2942 ns |   1.1472 ns |    213.45 ns |
|       AutoMapperBenchmark |    Nest | 25,652.31 ns | 495.8452 ns | 509.1965 ns | 25,614.89 ns |
