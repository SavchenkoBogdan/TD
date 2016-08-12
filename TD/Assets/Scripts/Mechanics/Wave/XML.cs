using System.IO;
using System.Xml;
using System.Xml.Serialization;

public static class XML {

    public static string Serialize(object details)
    {
        return Serialize(details, details.GetType());
    }

    public static string Serialize<T>(object details)
    {
        return Serialize(details, typeof(T));
    }

    public static string Serialize(object details, System.Type type)
    {
        XmlSerializer serializer = new XmlSerializer(type);
        MemoryStream stream = new MemoryStream();
        serializer.Serialize(stream, details);
        StreamReader reader = new StreamReader(stream);
        stream.Position = 0;
        string retSrt = reader.ReadToEnd();
        stream.Flush();
        stream = null;
        reader = null;
        return retSrt;
    }
    
    public static T Deserialize<T>(string details)
    {
        return (T) Deserialize(details, typeof(T));
    }

    public static object Deserialize(string details, System.Type type)
    {
        XmlSerializer serializer = new XmlSerializer(type);
        XmlReader reader = XmlReader.Create(new StringReader(details));
        return serializer.Deserialize(reader);
    }
}
