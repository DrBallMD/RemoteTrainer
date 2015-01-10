using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace GenXmlSerialization
{
    public static class BinSerialization
    {
        /// <summary>
        /// Сохраняет объект в бинарный файл
        /// </summary>
        /// <typeparam name="T">Тип сохраняемого объекта</typeparam>
        /// <param name="obj">Сохраняемый объект</param>
        /// <param name="path">Путь к файлу</param>
        public static void Serialize<T>(T obj, string path)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, obj);
            stream.Close();
        }
        /// <summary>
        /// Восстанавливает объект из бинарного файла
        /// </summary>
        /// <typeparam name="T">Тип восстанавливаемого объекта</typeparam>
        /// <param name="path">Путь к файлу</param>
        /// <returns>Возвращает объект заданного типа</returns>
        public static T Deserialize<T>(string path)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream readStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            T result = (T)formatter.Deserialize(readStream);
            readStream.Close();
            return result;
        }
        /// <summary>
        /// Конвертирует любой объект в массив байт
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] OgjectToByte(Object obj)
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, obj);
            stream.Close();
            return stream.ToArray();
        }
        /// <summary>
        /// Преобразует в объект массив байт
        /// </summary>
        /// <param name="arrBytes"></param>
        /// <returns></returns>
        public static object ByteToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);

            return binForm.Deserialize(memStream);
        }
    }
}
