﻿using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using NUnit.Framework;
using Should;

namespace Tests.Serializer.Json
{
    [TestFixture]
    public class PerformanceTests
    {
        public class SpeedTestCollection
        {
            public List<SpeedTestItem> Value0 { get; set; } public List<SpeedTestItem> Value1 { get; set; }
            public List<SpeedTestItem> Value2 { get; set; } public List<SpeedTestItem> Value3 { get; set; }
            public List<SpeedTestItem> Value4 { get; set; }
        }

        public class SpeedTestItem
        {
            public string Value0 { get; set; } public string Value1 { get; set; }
            public string Value2 { get; set; } public string Value3 { get; set; }
            public string Value4 { get; set; }
        }

        [Test]
        public void should_be_faster_than_the_fcl_xml_serializer()
        {
            var collection = new List<SpeedTestCollection>();
            collection.AddRange(Enumerable.Range(0, 5).Select(x => new SpeedTestCollection
                {
                    Value0 = Enumerable.Range(0, 5).Select(y => new SpeedTestItem {
                            Value0 = "ssdfsfsfd", Value1 = "sfdsfsdf", Value2 = "adasd", Value3 = "wqerqwe", Value4 = "qwerqwer"}).ToList(),
                    Value1 = Enumerable.Range(0, 5).Select(y => new SpeedTestItem {
                            Value0 = "ssdfsfsfd", Value1 = "sfdsfsdf", Value2 = "adasd", Value3 = "wqerqwe", Value4 = "qwerqwer"}).ToList(),
                    Value2 = Enumerable.Range(0, 5).Select(y => new SpeedTestItem {
                            Value0 = "ssdfsfsfd", Value1 = "sfdsfsdf", Value2 = "adasd", Value3 = "wqerqwe", Value4 = "qwerqwer"}).ToList(),
                    Value3 = Enumerable.Range(0, 5).Select(y => new SpeedTestItem {
                            Value0 = "ssdfsfsfd", Value1 = "sfdsfsdf", Value2 = "adasd", Value3 = "wqerqwe", Value4 = "qwerqwer"}).ToList(),
                    Value4 = Enumerable.Range(0, 5).Select(y => new SpeedTestItem {
                            Value0 = "ssdfsfsfd", Value1 = "sfdsfsdf", Value2 = "adasd", Value3 = "wqerqwe", Value4 = "qwerqwer"}).ToList()
                }));

            var stopwatch = new Stopwatch();

            stopwatch.Start();
            for (var i = 0; i < 100; i++) Bender.Serializer.Create().SerializeJson(collection);
            stopwatch.Stop();
            var benderSpeed = stopwatch.ElapsedTicks;

            var javascriptSerializer = new JavaScriptSerializer();
            stopwatch.Start();
            for (var i = 0; i < 100; i++) javascriptSerializer.Serialize(collection);
            stopwatch.Stop();
            var javascriptSerializerSpeed = stopwatch.ElapsedTicks;

            Debug.WriteLine("Bender speed (ticks): {0:#,##0}", benderSpeed);
            Debug.WriteLine("JavaScriptSerializer speed (ticks): {0:#,##0}", javascriptSerializerSpeed);
            (benderSpeed < javascriptSerializerSpeed).ShouldBeTrue();
        }
    }
}
