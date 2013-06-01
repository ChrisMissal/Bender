using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Serializer.Xml
{
    [TestFixture]
    public class DictionaryTests
    {
        public class ComplexType { public int Value { get; set; } }

        // Key/Value structure

        [Test]
        public void should_serialize_simple_key_dictionary()
        {
            var xml = Bender.Serializer.Create().SerializeXml(new Dictionary<int, string> { { 55, "hai" }, { 66, "yada" } });
            var items = xml.ParseXml().Element("ArrayOfKeyValuePairOfInt32String").Elements("KeyValuePairOfInt32String");
            items.Count().ShouldEqual(2);
            items.First().Element("Key").Value.ShouldEqual("55");
            items.First().Element("Value").Value.ShouldEqual("hai");
            items.Second().Element("Key").Value.ShouldEqual("66");
            items.Second().Element("Value").Value.ShouldEqual("yada"); 
        }

        [Test]
        public void should_serialize_complex_key_dictionary()
        {
            var xml = Bender.Serializer.Create().SerializeXml(new Dictionary<ComplexType, string>
                {
                    { new ComplexType { Value = 55 }, "hai" }, 
                    { new ComplexType { Value = 66 }, "yada" }
                });
            var items = xml.ParseXml().Element("ArrayOfKeyValuePairOfComplexTypeString").Elements("KeyValuePairOfComplexTypeString");
            items.Count().ShouldEqual(2);
            var key = items.First().Element("Key");
            key.Element("Value").Value.ShouldEqual("55");
            items.First().Element("Value").Value.ShouldEqual("hai");
            key = items.Second().Element("Key");
            key.Element("Value").Value.ShouldEqual("66");
            items.Second().Element("Value").Value.ShouldEqual("yada");
        }

        [Test]
        public void should_serialize_object_key_dictionary()
        {
            var xml = Bender.Serializer.Create().SerializeXml(new Dictionary<object, string>
                {
                    { new ComplexType { Value = 55 }, "hai" }, 
                    { new ComplexType { Value = 66 }, "yada" }
                });
            var items = xml.ParseXml().Element("ArrayOfKeyValuePairOfObjectString").Elements("KeyValuePairOfObjectString");
            items.Count().ShouldEqual(2);
            var key = items.First().Element("Key");
            key.Element("Value").Value.ShouldEqual("55");
            items.First().Element("Value").Value.ShouldEqual("hai");
            key = items.Second().Element("Key");
            key.Element("Value").Value.ShouldEqual("66");
            items.Second().Element("Value").Value.ShouldEqual("yada");
        }

        [Test]
        public void should_serialize_simple_value_dictionary()
        {
            var xml = Bender.Serializer.Create().SerializeXml(new Dictionary<string, int> { { "hai", 55 }, { "yada", 66 } });
            var items = xml.ParseXml().Element("ArrayOfKeyValuePairOfStringInt32").Elements("KeyValuePairOfStringInt32");
            items.Count().ShouldEqual(2);
            items.First().Element("Key").Value.ShouldEqual("hai");
            items.First().Element("Value").Value.ShouldEqual("55");
            items.Second().Element("Key").Value.ShouldEqual("yada");
            items.Second().Element("Value").Value.ShouldEqual("66");
        }

        [Test]
        public void should_serialize_complex_value_dictionary()
        {
            var xml = Bender.Serializer.Create().SerializeXml(new Dictionary<string, ComplexType>
                {
                    { "hai", new ComplexType { Value = 55 } }, 
                    { "yada", new ComplexType { Value = 66 } }
                });
            var items = xml.ParseXml().Element("ArrayOfKeyValuePairOfStringComplexType").Elements("KeyValuePairOfStringComplexType");
            items.Count().ShouldEqual(2);
            items.First().Element("Key").Value.ShouldEqual("hai");
            var value = items.First().Element("Value");
            value.Element("Value").Value.ShouldEqual("55");
            items.Second().Element("Key").Value.ShouldEqual("yada");
            value = items.Second().Element("Value");
            value.Element("Value").Value.ShouldEqual("66");
        }

        [Test]
        public void should_serialize_object_value_dictionary()
        {
            var xml = Bender.Serializer.Create().SerializeXml(new Dictionary<string, object>
                {
                    { "hai", new ComplexType { Value = 55 } }, 
                    { "yada", new ComplexType { Value = 66 } }
                });
            var items = xml.ParseXml().Element("ArrayOfKeyValuePairOfStringObject").Elements("KeyValuePairOfStringObject");
            items.Count().ShouldEqual(2);
            items.First().Element("Key").Value.ShouldEqual("hai");
            var value = items.First().Element("Value");
            value.Element("Value").Value.ShouldEqual("55");
            items.Second().Element("Key").Value.ShouldEqual("yada");
            value = items.Second().Element("Value");
            value.Element("Value").Value.ShouldEqual("66");
        }

        public class InheritedDictionary : Dictionary<string, ComplexType> { }

        [Test]
        public void should_serialize_inherited_dictionary()
        {
            var xml = Bender.Serializer.Create().SerializeXml(new InheritedDictionary
                {
                    { "hai", new ComplexType { Value = 55 } }, 
                    { "yada", new ComplexType { Value = 66 } }
                });
            var items = xml.ParseXml().Element("InheritedDictionary").Elements("KeyValuePairOfStringComplexType");
            items.Count().ShouldEqual(2);
            items.First().Element("Key").Value.ShouldEqual("hai");
            var value = items.First().Element("Value");
            value.Element("Value").Value.ShouldEqual("55");
            items.Second().Element("Key").Value.ShouldEqual("yada");
            value = items.Second().Element("Value");
            value.Element("Value").Value.ShouldEqual("66");
        }

        [XmlRoot("DictionaryProperty")]
        public class DictionaryProperty<TKey, TValue>
        {
            [XmlArrayItem("Item")]
            public Dictionary<TKey, TValue> Dictionary { get; set; }
        }

        [Test]
        public void should_serialize_simple_key_dictionary_property()
        {
            var xml = Bender.Serializer.Create().SerializeXml(new DictionaryProperty<int, string>
                {
                    Dictionary = new Dictionary<int, string> { { 55, "hai" }, { 66, "yada" } }
                });
            var items = xml.ParseXml().Element("DictionaryProperty").Element("Dictionary").Elements("Item");
            items.Count().ShouldEqual(2);
            items.First().Element("Key").Value.ShouldEqual("55");
            items.First().Element("Value").Value.ShouldEqual("hai");
            items.Second().Element("Key").Value.ShouldEqual("66");
            items.Second().Element("Value").Value.ShouldEqual("yada");
        }

        [Test]
        public void should_serialize_complex_key_dictionary_property()
        {
            var xml = Bender.Serializer.Create().SerializeXml(new DictionaryProperty<ComplexType, string>
                {
                    Dictionary = new Dictionary<ComplexType, string>
                    {
                        { new ComplexType { Value = 55 }, "hai" }, 
                        { new ComplexType { Value = 66 }, "yada" }
                    }
                });
            var items = xml.ParseXml().Element("DictionaryProperty").Element("Dictionary").Elements("Item");
            items.Count().ShouldEqual(2);
            var key = items.First().Element("Key");
            key.Element("Value").Value.ShouldEqual("55");
            items.First().Element("Value").Value.ShouldEqual("hai");
            key = items.Second().Element("Key");
            key.Element("Value").Value.ShouldEqual("66");
            items.Second().Element("Value").Value.ShouldEqual("yada");
        }

        [Test]
        public void should_serialize_object_key_dictionary_property()
        {
            var xml = Bender.Serializer.Create().SerializeXml(new DictionaryProperty<object, string>
                {
                    Dictionary = new Dictionary<object, string>
                    {
                        { new ComplexType { Value = 55 }, "hai" }, 
                        { new ComplexType { Value = 66 }, "yada" }
                    }
                });
            var items = xml.ParseXml().Element("DictionaryProperty").Element("Dictionary").Elements("Item");
            items.Count().ShouldEqual(2);
            var key = items.First().Element("Key");
            key.Element("Value").Value.ShouldEqual("55");
            items.First().Element("Value").Value.ShouldEqual("hai");
            key = items.Second().Element("Key");
            key.Element("Value").Value.ShouldEqual("66");
            items.Second().Element("Value").Value.ShouldEqual("yada");
        }

        [Test]
        public void should_serialize_simple_value_dictionary_property()
        {
            var xml = Bender.Serializer.Create().SerializeXml(new DictionaryProperty<string, int>
                {
                    Dictionary = new Dictionary<string, int> { { "hai", 55 }, { "yada", 66 } }
                });
            var items = xml.ParseXml().Element("DictionaryProperty").Element("Dictionary").Elements("Item");
            items.Count().ShouldEqual(2);
            items.First().Element("Key").Value.ShouldEqual("hai");
            items.First().Element("Value").Value.ShouldEqual("55");
            items.Second().Element("Key").Value.ShouldEqual("yada");
            items.Second().Element("Value").Value.ShouldEqual("66");
        }

        [Test]
        public void should_serialize_complex_value_dictionary_property()
        {
            var xml = Bender.Serializer.Create().SerializeXml(new DictionaryProperty<string, ComplexType>
                {
                    Dictionary = new Dictionary<string, ComplexType>
                    {
                        { "hai", new ComplexType { Value = 55 } }, 
                        { "yada", new ComplexType { Value = 66 } } 
                    }
                });
            var items = xml.ParseXml().Element("DictionaryProperty").Element("Dictionary").Elements("Item");
            items.Count().ShouldEqual(2);
            items.First().Element("Key").Value.ShouldEqual("hai");
            var value = items.First().Element("Value");
            value.Element("Value").Value.ShouldEqual("55");
            items.Second().Element("Key").Value.ShouldEqual("yada");
            value = items.Second().Element("Value");
            value.Element("Value").Value.ShouldEqual("66");
        }

        [Test]
        public void should_serialize_object_value_dictionary_property()
        {
            var xml = Bender.Serializer.Create().SerializeXml(new DictionaryProperty<string, object>
                {
                    Dictionary = new Dictionary<string, object>
                    {
                        { "hai", new ComplexType { Value = 55 } }, 
                        { "yada", new ComplexType { Value = 66 } }
                    }
                });
            var items = xml.ParseXml().Element("DictionaryProperty").Element("Dictionary").Elements("Item");
            items.Count().ShouldEqual(2);
            items.First().Element("Key").Value.ShouldEqual("hai");
            var value = items.First().Element("Value");
            value.Element("Value").Value.ShouldEqual("55");
            items.Second().Element("Key").Value.ShouldEqual("yada");
            value = items.Second().Element("Value");
            value.Element("Value").Value.ShouldEqual("66");
        }

        public class InheritedDictionaryProperty { public InheritedDictionary Dictionary { get; set; } }

        [Test]
        public void should_serialize_inherited_dictionary_property()
        {
            var xml = Bender.Serializer.Create().SerializeXml(new InheritedDictionaryProperty
            {
                Dictionary = new InheritedDictionary
                {
                    { "hai", new ComplexType { Value = 55 } }, 
                    { "yada", new ComplexType { Value = 66 } }
                }
            });
            var items = xml.ParseXml().Element("InheritedDictionaryProperty").Element("Dictionary").Elements();
            items.Count().ShouldEqual(2);
            items.First().Element("Key").Value.ShouldEqual("hai");
            var value = items.First().Element("Value");
            value.Element("Value").Value.ShouldEqual("55");
            items.Second().Element("Key").Value.ShouldEqual("yada");
            value = items.Second().Element("Value");
            value.Element("Value").Value.ShouldEqual("66");
        }

        // name/value pair
    }
}
