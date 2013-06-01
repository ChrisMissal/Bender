using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Xml
{
    [TestFixture]
    public class DictionaryTests
    {
        public class ComplexType { public int Value { get; set; } }

        [Test]
        public void should_serialize_simple_key_dictionary()
        {
            const string xml = @"
                <ArrayOfKeyValuePairOfInt32String>
                    <KeyValuePairOfInt32String><Key>55</Key><Value>hai</Value></KeyValuePairOfInt32String>
                    <KeyValuePairOfInt32String><Key>66</Key><Value>yada</Value></KeyValuePairOfInt32String>
                </ArrayOfKeyValuePairOfInt32String>";
            var result = Bender.Deserializer.Create().DeserializeXml<Dictionary<int, string>>(xml);
            result.Count.ShouldEqual(2);
            result[55].ShouldEqual("hai");
            result[66].ShouldEqual("yada");
        }

        [Test]
        public void should_serialize_complex_key_dictionary()
        {
        }

        [Test]
        public void should_serialize_object_key_dictionary()
        {
        }

        [Test]
        public void should_serialize_simple_value_dictionary()
        {
        }

        [Test]
        public void should_serialize_complex_value_dictionary()
        {
        }

        [Test]
        public void should_serialize_object_value_dictionary()
        {
        }

        public class InheritedDictionary : Dictionary<string, ComplexType> { }

        [Test]
        public void should_serialize_inherited_dictionary()
        {
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
        }

        [Test]
        public void should_serialize_complex_key_dictionary_property()
        {
        }

        [Test]
        public void should_serialize_object_key_dictionary_property()
        {
        }

        [Test]
        public void should_serialize_simple_value_dictionary_property()
        {
        }

        [Test]
        public void should_serialize_complex_value_dictionary_property()
        {
        }

        [Test]
        public void should_serialize_object_value_dictionary_property()
        {
        }

        public class InheritedDictionaryProperty { public InheritedDictionary Dictionary { get; set; } }

        [Test]
        public void should_serialize_inherited_dictionary_property()
        {
        }
    }
}
