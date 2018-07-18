# .net-core-DTO
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
  ar b = Mapper<TestA, TestB>.Map(model);
  ```


