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
|                      Type |  Method |         Mean |       Error |      StdDev |       Median | Rank |
|-------------------------- |-------- |-------------:|------------:|------------:|-------------:|-----:|
|     NativeMapperBenchmark |   Nomal |     17.10 ns |   0.3747 ns |   0.3505 ns |     16.98 ns |    1 |
| ExpressionMapperBenchmark |   Nomal |     34.09 ns |   0.9305 ns |   2.0228 ns |     33.30 ns |    2 |
|       TinyMapperBenchmark |   Nomal |     95.86 ns |   0.5447 ns |   0.4828 ns |     95.80 ns |    5 |
|       AutoMapperBenchmark |   Nomal |    203.07 ns |   3.9543 ns |   5.6712 ns |    200.94 ns |    8 |
|     NativeMapperBenchmark | Complex |     34.23 ns |   0.7240 ns |   1.4625 ns |     33.70 ns |    2 |
| ExpressionMapperBenchmark | Complex |     59.94 ns |   0.7706 ns |   0.7209 ns |     59.73 ns |    3 |
|       TinyMapperBenchmark | Complex |    127.37 ns |   2.5729 ns |   2.7529 ns |    126.56 ns |    6 |
|       AutoMapperBenchmark | Complex |  6,418.38 ns | 104.6438 ns |  97.8838 ns |  6,383.24 ns |   13 |
|     NativeMapperBenchmark |    List |    259.16 ns |   5.2001 ns |   6.7616 ns |    258.74 ns |   11 |
| ExpressionMapperBenchmark |    List |    221.74 ns |   3.7919 ns |   3.3614 ns |    220.77 ns |   10 |
|       TinyMapperBenchmark |    List |    362.09 ns |   5.7394 ns |   5.3686 ns |    361.99 ns |   12 |
|       AutoMapperBenchmark |    List | 12,664.82 ns | 133.3537 ns | 111.3564 ns | 12,698.64 ns |   14 |
|     NativeMapperBenchmark |    Nest |     85.11 ns |   1.0783 ns |   1.0087 ns |     84.87 ns |    4 |
| ExpressionMapperBenchmark |    Nest |    143.98 ns |   1.1260 ns |   0.9982 ns |    144.01 ns |    7 |
|       TinyMapperBenchmark |    Nest |    213.47 ns |   2.8471 ns |   2.6632 ns |    212.91 ns |    9 |
|       AutoMapperBenchmark |    Nest | 25,040.79 ns | 436.7933 ns | 408.5767 ns | 25,053.18 ns |   15 |
