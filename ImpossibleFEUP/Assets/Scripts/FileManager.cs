using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;

public static class FileManager {

	public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
	{
		using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
		{
			var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			binaryFormatter.Serialize(stream, objectToWrite);
		}
	}

	public static T ReadFromBinaryFile<T>(string filePath)
	{
		using (Stream stream = File.Open(filePath, FileMode.Open))
		{
			var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			return (T)binaryFormatter.Deserialize(stream);
		}
	}
}