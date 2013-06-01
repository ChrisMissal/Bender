using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Should;

namespace Tests.Serializer.Json
{
    [TestFixture]
    public class DictionaryTests
    {
        public class ComplexType { public int Value { get; set; } }

        // Key/Value structure

        [Test]
        public void should_serialize_simple_key_dictionary()
        {
            var json = Bender.Serializer.Create().SerializeJson(new Dictionary<int, string> { { 55, "hai" }, { 66, "yada" } });
            var items = json.ParseJson().JsonRootObjectArray();
            items.Count().ShouldEqual(2);
            items.JsonObjectItem(1).JsonNumberField("Key").Value.ShouldEqual("55");
            items.JsonObjectItem(1).JsonStringField("Value").Value.ShouldEqual("hai");
            items.JsonObjectItem(2).JsonNumberField("Key").Value.ShouldEqual("66"); 
            items.JsonObjectItem(2).JsonStringField("Value").Value.ShouldEqual("yada"); 
        }

        [Test]
        public void should_serialize_complex_key_dictionary()
        {
            var json = Bender.Serializer.Create().SerializeJson(new Dictionary<ComplexType, string>
                {
                    { new ComplexType { Value = 55 }, "hai" }, 
                    { new ComplexType { Value = 66 }, "yada" }
                });
            var items = json.ParseJson().JsonRootObjectArray();
            items.Count().ShouldEqual(2);
            var key = items.JsonObjectItem(1).JsonObjectField("Key");
            key.JsonNumberField("Value").Value.ShouldEqual("55");
            items.JsonObjectItem(1).JsonStringField("Value").Value.ShouldEqual("hai");
            key = items.JsonObjectItem(2).JsonObjectField("Key");
            key.JsonNumberField("Value").Value.ShouldEqual("66");
            items.JsonObjectItem(2).JsonStringField("Value").Value.ShouldEqual("yada"); 
        }

        [Test]
        public void should_serialize_object_key_dictionary()
        {
            var json = Bender.Serializer.Create().SerializeJson(new Dictionary<object, string>
                {
                    { new ComplexType { Value = 55 }, "hai" }, 
                    { new ComplexType { Value = 66 }, "yada" }
                });
            var items = json.ParseJson().JsonRootObjectArray();
            items.Count().ShouldEqual(2);
            var key = items.JsonObjectItem(1).JsonObjectField("Key");
            key.JsonNumberField("Value").Value.ShouldEqual("55");
            items.JsonObjectItem(1).JsonStringField("Value").Value.ShouldEqual("hai");
            key = items.JsonObjectItem(2).JsonObjectField("Key");
            key.JsonNumberField("Value").Value.ShouldEqual("66");
            items.JsonObjectItem(2).JsonStringField("Value").Value.ShouldEqual("yada"); 
        }

        [Test]
        public void should_serialize_simple_value_dictionary()
        {
            var json = Bender.Serializer.Create().SerializeJson(new Dictionary<string, int> { { "hai", 55 }, { "yada", 66 } });
            var items = json.ParseJson().JsonRootObjectArray();
            items.Count().ShouldEqual(2);
            items.JsonObjectItem(1).JsonStringField("Key").Value.ShouldEqual("hai");
            items.JsonObjectItem(1).JsonNumberField("Value").Value.ShouldEqual("55");
            items.JsonObjectItem(2).JsonStringField("Key").Value.ShouldEqual("yada");
            items.JsonObjectItem(2).JsonNumberField("Value").Value.ShouldEqual("66"); 
        }

        [Test]
        public void should_serialize_complex_value_dictionary()
        {
            var json = Bender.Serializer.Create().SerializeJson(new Dictionary<string, ComplexType>
                {
                    { "hai", new ComplexType { Value = 55 } }, 
                    { "yada", new ComplexType { Value = 66 } }
                });
            var items = json.ParseJson().JsonRootObjectArray();
            items.Count().ShouldEqual(2);
            items.JsonObjectItem(1).JsonStringField("Key").Value.ShouldEqual("hai");
            var value = items.JsonObjectItem(1).JsonObjectField("Value");
            value.JsonNumberField("Value").Value.ShouldEqual("55");
            items.JsonObjectItem(2).JsonStringField("Key").Value.ShouldEqual("yada"); 
            value = items.JsonObjectItem(2).JsonObjectField("Value");
            value.JsonNumberField("Value").Value.ShouldEqual("66");
        }

        [Test]
        public void should_serialize_object_value_dictionary()
        {
            var json = Bender.Serializer.Create().SerializeJson(new Dictionary<string, object>
                {
                    { "hai", new ComplexType { Value = 55 } }, 
                    { "yada", new ComplexType { Value = 66 } }
                });
            var items = json.ParseJson().JsonRootObjectArray();
            items.Count().ShouldEqual(2);
            items.JsonObjectItem(1).JsonStringField("Key").Value.ShouldEqual("hai");
            var value = items.JsonObjectItem(1).JsonObjectField("Value");
            value.JsonNumberField("Value").Value.ShouldEqual("55");
            items.JsonObjectItem(2).JsonStringField("Key").Value.ShouldEqual("yada");
            value = items.JsonObjectItem(2).JsonObjectField("Value");
            value.JsonNumberField("Value").Value.ShouldEqual("66");
        }

        public class InheritedDictionary : Dictionary<string, ComplexType> { }

        [Test]
        public void should_serialize_inherited_dictionary()
        {
            var json = Bender.Serializer.Create().SerializeJson(new InheritedDictionary
                {
                    { "hai", new ComplexType { Value = 55 } }, 
                    { "yada", new ComplexType { Value = 66 } }
                });
            var items = json.ParseJson().JsonRootObjectArray();
            items.Count().ShouldEqual(2);
            items.JsonObjectItem(1).JsonStringField("Key").Value.ShouldEqual("hai");
            var value = items.JsonObjectItem(1).JsonObjectField("Value");
            value.JsonNumberField("Value").Value.ShouldEqual("55");
            items.JsonObjectItem(2).JsonStringField("Key").Value.ShouldEqual("yada");
            value = items.JsonObjectItem(2).JsonObjectField("Value");
            value.JsonNumberField("Value").Value.ShouldEqual("66");
        }

        public class DictionaryProperty<TKey, TValue> { public Dictionary<TKey, TValue> Dictionary { get; set; } }

        [Test]
        public void should_serialize_simple_key_dictionary_property()
        {
            var json = Bender.Serializer.Create().SerializeJson(new DictionaryProperty<int, string>
                {
                    Dictionary = new Dictionary<int, string> { { 55, "hai" }, { 66, "yada" } }
                });
            var items = json.ParseJson().JsonRoot().JsonObjectArrayField("Dictionary");
            items.Count().ShouldEqual(2);
            items.JsonObjectItem(1).JsonNumberField("Key").Value.ShouldEqual("55");
            items.JsonObjectItem(1).JsonStringField("Value").Value.ShouldEqual("hai");
            items.JsonObjectItem(2).JsonNumberField("Key").Value.ShouldEqual("66");
            items.JsonObjectItem(2).JsonStringField("Value").Value.ShouldEqual("yada");
        }

        [Test]
        public void should_serialize_complex_key_dictionary_property()
        {
            var json = Bender.Serializer.Create().SerializeJson(new DictionaryProperty<ComplexType, string>
                {
                    Dictionary = new Dictionary<ComplexType, string>
                    {
                        { new ComplexType { Value = 55 }, "hai" }, 
                        { new ComplexType { Value = 66 }, "yada" }
                    }
                });
            var items = json.ParseJson().JsonRoot().JsonObjectArrayField("Dictionary");
            items.Count().ShouldEqual(2);
            var key = items.JsonObjectItem(1).JsonObjectField("Key");
            key.JsonNumberField("Value").Value.ShouldEqual("55");
            items.JsonObjectItem(1).JsonStringField("Value").Value.ShouldEqual("hai");
            key = items.JsonObjectItem(2).JsonObjectField("Key");
            key.JsonNumberField("Value").Value.ShouldEqual("66");
            items.JsonObjectItem(2).JsonStringField("Value").Value.ShouldEqual("yada");
        }

        [Test]
        public void should_serialize_object_key_dictionary_property()
        {
            var json = Bender.Serializer.Create().SerializeJson(new DictionaryProperty<object, string>
                {
                    Dictionary = new Dictionary<object, string>
                    {
                        { new ComplexType { Value = 55 }, "hai" }, 
                        { new ComplexType { Value = 66 }, "yada" }
                    }
                });
            var items = json.ParseJson().JsonRoot().JsonObjectArrayField("Dictionary");
            items.Count().ShouldEqual(2);
            var key = items.JsonObjectItem(1).JsonObjectField("Key");
            key.JsonNumberField("Value").Value.ShouldEqual("55");
            items.JsonObjectItem(1).JsonStringField("Value").Value.ShouldEqual("hai");
            key = items.JsonObjectItem(2).JsonObjectField("Key");
            key.JsonNumberField("Value").Value.ShouldEqual("66");
            items.JsonObjectItem(2).JsonStringField("Value").Value.ShouldEqual("yada");
        }

        [Test]
        public void should_serialize_simple_value_dictionary_property()
        {
            var json = Bender.Serializer.Create().SerializeJson(new DictionaryProperty<string, int>
                {
                    Dictionary = new Dictionary<string, int> { { "hai", 55 }, { "yada", 66 } }
                });
            var items = json.ParseJson().JsonRoot().JsonObjectArrayField("Dictionary");
            items.Count().ShouldEqual(2);
            items.JsonObjectItem(1).JsonStringField("Key").Value.ShouldEqual("hai");
            items.JsonObjectItem(1).JsonNumberField("Value").Value.ShouldEqual("55");
            items.JsonObjectItem(2).JsonStringField("Key").Value.ShouldEqual("yada");
            items.JsonObjectItem(2).JsonNumberField("Value").Value.ShouldEqual("66");
        }

        [Test]
        public void should_serialize_complex_value_dictionary_property()
        {
            var json = Bender.Serializer.Create().SerializeJson(new DictionaryProperty<string, ComplexType>
                {
                    Dictionary = new Dictionary<string, ComplexType>
                    {
                        { "hai", new ComplexType { Value = 55 } }, 
                        { "yada", new ComplexType { Value = 66 } } 
                    }
                });
            var items = json.ParseJson().JsonRoot().JsonObjectArrayField("Dictionary");
            items.Count().ShouldEqual(2);
            items.JsonObjectItem(1).JsonStringField("Key").Value.ShouldEqual("hai");
            var value = items.JsonObjectItem(1).JsonObjectField("Value");
            value.JsonNumberField("Value").Value.ShouldEqual("55");
            items.JsonObjectItem(2).JsonStringField("Key").Value.ShouldEqual("yada");
            value = items.JsonObjectItem(2).JsonObjectField("Value");
            value.JsonNumberField("Value").Value.ShouldEqual("66");
        }

        [Test]
        public void should_serialize_object_value_dictionary_property()
        {
            var json = Bender.Serializer.Create().SerializeJson(new DictionaryProperty<string, object>
                {
                    Dictionary = new Dictionary<string, object>
                    {
                        { "hai", new ComplexType { Value = 55 } }, 
                        { "yada", new ComplexType { Value = 66 } }
                    }
                });
            var items = json.ParseJson().JsonRoot().JsonObjectArrayField("Dictionary");
            items.Count().ShouldEqual(2);
            items.JsonObjectItem(1).JsonStringField("Key").Value.ShouldEqual("hai");
            var value = items.JsonObjectItem(1).JsonObjectField("Value");
            value.JsonNumberField("Value").Value.ShouldEqual("55");
            items.JsonObjectItem(2).JsonStringField("Key").Value.ShouldEqual("yada");
            value = items.JsonObjectItem(2).JsonObjectField("Value");
            value.JsonNumberField("Value").Value.ShouldEqual("66");
        }

        public class InheritedDictionaryProperty { public InheritedDictionary Dictionary { get; set; } }

        [Test]
        public void should_serialize_inherited_dictionary_property()
        {
            var json = Bender.Serializer.Create().SerializeJson(new InheritedDictionaryProperty
            {
                Dictionary = new InheritedDictionary
                {
                    { "hai", new ComplexType { Value = 55 } }, 
                    { "yada", new ComplexType { Value = 66 } }
                }
            });
            var items = json.ParseJson().JsonRoot().JsonObjectArrayField("Dictionary");
            items.Count().ShouldEqual(2);
            items.JsonObjectItem(1).JsonStringField("Key").Value.ShouldEqual("hai");
            var value = items.JsonObjectItem(1).JsonObjectField("Value");
            value.JsonNumberField("Value").Value.ShouldEqual("55");
            items.JsonObjectItem(2).JsonStringField("Key").Value.ShouldEqual("yada");
            value = items.JsonObjectItem(2).JsonObjectField("Value");
            value.JsonNumberField("Value").Value.ShouldEqual("66");
        }

        // name/value pair
    }
}
