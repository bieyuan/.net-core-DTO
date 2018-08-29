using System;
using System.Collections.Generic;
using Xunit;

namespace ExpressionMapper.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Mapping_Between_SimpleType_And_Nullable_Type_Test()
        {
            var time = DateTime.Now;
            var list = new List<POCO>() { new POCO() { Hello = "World" } };

            var sourceObj = new SourceType()
            {
                Int = 10,
                Long = long.MaxValue,
                String = "String",
                Time = time,
                Nullable = null,
                NullableTime = null,
                NullableHasValue = 100,
                NullableTimeHasValue = time,
                List = list,
                NonNullableToNullable = 100,
                NullableToNonNullable = null,
                NullableToNonNullableHasValue = 111,
                NonNullableToNullableTime = time,
                NullableToNonNullableTime = null,
                NullableToNonNullableTimeHasValue = time
            };

            var targetObj = Mapper<SourceType, TargetType>.Map(sourceObj);
            Assert.NotNull(targetObj);
            Assert.Equal(10, targetObj.Int);
            Assert.Equal(long.MaxValue, targetObj.Long);
            Assert.Equal("String", targetObj.String);
            Assert.Equal(time, targetObj.Time);
            Assert.Null(targetObj.Nullable);
            Assert.Null(targetObj.NullableTime);
            Assert.Equal(100, targetObj.NullableHasValue);
            Assert.Equal(time, targetObj.NullableTimeHasValue);
            Assert.Equal(list, targetObj.List);
            Assert.Null(targetObj.NullList);
            Assert.Equal(100, targetObj.NonNullableToNullable);
            Assert.Equal(default(int), targetObj.NullableToNonNullable);
            Assert.Equal(111, targetObj.NullableToNonNullableHasValue);
            Assert.Equal(time, targetObj.NonNullableToNullableTime);
            Assert.Equal(default(DateTime), targetObj.NullableToNonNullableTime);
            Assert.Equal(time, targetObj.NullableToNonNullableTimeHasValue);
        }
    }

    public class SourceType
    {
        public int Int { get; set; } = 100;

        public string String { get; set; }

        public DateTime Time { get; set; }

        public long Long { get; set; }

        public int? Nullable { get; set; }

        public DateTime? NullableTime { get; set; }

        public int? NullableHasValue { get; set; }

        public DateTime? NullableTimeHasValue { get; set; }

        public IEnumerable<POCO> List { get; set; }

        public IEnumerable<POCO> NullList { get; set; }

        public int NonNullableToNullable { get; set; }

        public int? NullableToNonNullable { get; set; }

        public DateTime NonNullableToNullableTime { get; set; }

        public DateTime? NullableToNonNullableTime { get; set; }

        public int? NullableToNonNullableHasValue { get; set; }

        public DateTime? NullableToNonNullableTimeHasValue { get; set; }
    }

    public class TargetType
    {
        public int Int { get; set; }

        public string String { get; set; }

        public DateTime Time { get; set; }

        public long Long { get; set; }

        public int? Nullable { get; set; }

        public DateTime? NullableTime { get; set; }

        public int? NullableHasValue { get; set; }

        public DateTime? NullableTimeHasValue { get; set; }

        public IEnumerable<POCO> List { get; set; }

        public IEnumerable<POCO> NullList { get; set; }

        public int? NonNullableToNullable { get; set; }

        public int NullableToNonNullable { get; set; }

        public DateTime? NonNullableToNullableTime { get; set; }

        public DateTime NullableToNonNullableTime { get; set; }

        public int NullableToNonNullableHasValue { get; set; }

        public DateTime NullableToNonNullableTimeHasValue { get; set; }
    }

    public class POCO
    {
        public string Hello { get; set; }
    }
}