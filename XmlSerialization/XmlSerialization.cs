using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GenXmlSerialization
{
    public static class XmlSerialization
    {
        public static void Serialize<T>(T obj, string path)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            TextWriter textWriter = new StreamWriter(path);
            serializer.Serialize(textWriter, obj);
            textWriter.Flush();
            textWriter.Close();
        }
        public static void Serialize<T>(List<T> obj, string path)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            TextWriter textWriter = new StreamWriter(path);
            serializer.Serialize(textWriter, obj);
            textWriter.Flush();
            textWriter.Close();
        }
        public static List<T> DeserializeCollection<T>(string path)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<T>));
            TextReader textReader = new StreamReader(path);
            List<T> serializedData;
            serializedData = (List<T>)deserializer.Deserialize(textReader);
            textReader.Close();
            return serializedData;
        }
        public static T DeserializeObject<T>(string path)
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
