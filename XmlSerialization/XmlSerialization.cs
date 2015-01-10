using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
namespace GenXmlSerialization
{
    public static class XmlSerialization
    {
        public static void ObjectSerialize<T>(T obj, string path)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            TextWriter textWriter = new StreamWriter(path);
            serializer.Serialize(textWriter, obj);
            textWriter.Flush();
            textWriter.Close();
        }
        public static void AdvancedObjectSerialize<T>(T obj, string path)
        {
            DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
            XmlWriter textWriter = XmlWriter.Create(path);
            serializer.WriteObject(textWriter, obj);
            textWriter.Flush();
            textWriter.Close();
        }

        public static void CollectionSerialize<T>(ICollection<T> obj, string path)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            TextWriter textWriter = new StreamWriter(path);
            serializer.Serialize(textWriter, obj);
            textWriter.Flush();
            textWriter.Close();
        }
        public static void AdvancedCollectionSerialize<T>(ICollection<T> obj, string path)
        {
            DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
            XmlWriter textWriter = XmlWriter.Create(path);
            serializer.WriteObject(textWriter, obj);
            textWriter.Flush();
            textWriter.Close();
        }
        public static ICollection<T> CollectionDeserialize<T>(string path)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(ICollection<T>));
            TextReader textReader = new StreamReader(path);
            ICollection<T> serializedData;
            serializedData = (ICollection<T>)deserializer.Deserialize(textReader);
            textReader.Close();
            return serializedData;
        }
        public static ICollection<T> AdvancedCollectionDeserialize<T>(string path)
        {
            DataContractSerializer deserializer = new DataContractSerializer(typeof(ICollection<T>));
            XmlDictionaryReader textReader = XmlDictionaryReader.CreateTextReader(new StreamReader(path).BaseStream, Encoding.UTF8, new XmlDictionaryReaderQuotas(), null);
            ICollection<T> serializedData;
            serializedData = (ICollection<T>)deserializer.ReadObject(textReader);
            textReader.Close();
            return serializedData;
        }
        public static T AdvancedObjectDeserialize<T>(string path)
        {
            DataContractSerializer deserializer = new DataContractSerializer(typeof(T));
            XmlDictionaryReader textReader = XmlDictionaryReader.CreateTextReader(new StreamReader(path).BaseStream, Encoding.UTF8, new XmlDictionaryReaderQuotas(), null);
            T serializedData;
            serializedData = (T)deserializer.ReadObject(textReader);
            textReader.Close();
            return serializedData;
        }
        public static T ObjectDeserialize<T>(string path)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            TextReader textReader = new StreamReader(path);
            T serializedData;
            serializedData = (T) deserializer.Deserialize(textReader);
            textReader.Close();
            return serializedData;
        }
    }
}
