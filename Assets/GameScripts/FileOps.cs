using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class FileOps
{
    private static string basePath = Application.persistentDataPath;

    public static void Save<T>(T saveData, string filepath)
    {
        string fullPath = Path.Combine(basePath, filepath);
        FileStream file = File.Create(fullPath);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, saveData);
        file.Close();
        Debug.Log("Save successful");
    }

    public static T Load<T>(string filepath)
    {
        if (CheckIfFileExists(filepath))
        {
            FileStream file = File.Open(Path.Combine(basePath, filepath), FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            T data = (T)bf.Deserialize(file);
            file.Close();
            Debug.Log("loaded successfully");
            return data;
        }
        else
        {
            Debug.Log("File doesnt exist at the path " + basePath + filepath);
            return default;
        }
    }

    public static bool CheckIfFileExists(string path)
    {
        return File.Exists(Path.Combine(basePath, path));
    }

    public static bool CheckIfDirectoryExists(string path)
    {
        return Directory.Exists(Path.Combine(basePath, path));
    }

    public static void CreateDirectory(string directoryPath)
    {
        Directory.CreateDirectory(Path.Combine(basePath, directoryPath));
    }
}
