using Nelibur.ObjectMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ConsoleApp7
{
    class Program
    {

        static void Main(string[] args)
        {
            AutoMapper.Mapper.Initialize(cfg => cfg.CreateMap<TestA, TestB>());
            TinyMapper.Bind<TestA, TestB>();
            Mapper<TestA, TestB>.Map(new TestA());


            MapperTest.Count = 1_0000;
            MapperTest.Nomal();
            MapperTest.Complex();
            MapperTest.Nest();
            MapperTest.List();

            MapperTest.Count = 10_0000;
            MapperTest.Nomal();
            MapperTest.Complex();
            MapperTest.Nest();
            MapperTest.List();

            MapperTest.Count = 100_0000;
            MapperTest.Nomal();
            MapperTest.Complex();
            MapperTest.Nest();
            MapperTest.List();

            MapperTest.Count = 1000_0000;
            MapperTest.Nomal();
            MapperTest.Complex();
            MapperTest.Nest();
            MapperTest.List();


            Console.WriteLine($"------------结束--------------------");
            Console.ReadLine();
        }
    }
}
