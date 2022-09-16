using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class FileManager<U, S> where S : class, SerializableClass<U>, new()
{
    public static void WriteClass(U uc, string dir, string filename, string extension)
    {
        dir = Application.persistentDataPath + "/" + dir + "/";
        if(!Directory.Exists(dir))
        {    
            Directory.CreateDirectory(dir);
        }
        string path = dir + filename + "." + extension;

        S sc = new S();
        sc.InitClassValues(uc);

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, sc);
        stream.Close();
        
        Debug.Log(uc.GetType().Name + " saved at " + path);
    }

    public static U ReadClass(string dir, string filename, string extension)
    {
        dir = Application.persistentDataPath + "/" + dir + "/";
        string path = dir + filename + "." + extension;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            S sc = formatter.Deserialize(stream) as S;
            stream.Close();

            return sc.ExtractClassData();
        }
        else throw new FileNotFoundException("File " + filename + " not found");
    }
}
