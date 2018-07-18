# .net-core-DTO
高性能对象映射库
 
  构造测试类

 1     public class TestA
 2     {
 3         public int Id { get; set; }
 4         public string Name { get; set; }
 5 
 6         public TestC TestClass { get; set; }
 7 
 8         public IEnumerable<TestC> TestLists { get; set; }
 9     }
10 
11     public class TestB
12     {
13         public int Id { get; set; }
14         public string Name { get; set; }
15 
16         public TestD TestClass { get; set; }
17 
18         public TestD[] TestLists { get; set; }
19     }
20 
21     public class TestC
22     {
23         public int Id { get; set; }
24         public string Name { get; set; }
25 
26         public TestC SelfClass { get; set; }
27     }
28 
29     public class TestD
30     {
31         public int Id { get; set; }
32         public string Name { get; set; }
33 
34         public TestD SelfClass { get; set; }
35     }
 
 

  调用如下

14 
15     class Program
16     {
17         static void Main(string[] args)
18         {
19             var testA = new TestA { Id = 1, Name = "张三" };
20             var func = Map<TestA, TestB>();
21             TestB testB = func(testA);
22             Console.WriteLine($"testB.Id={testB.Id},testB.Name={testB.Name}");
23             Console.ReadLine();
24         }
25     }

