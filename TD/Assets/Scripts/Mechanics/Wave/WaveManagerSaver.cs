using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("WaveManager")]
public class WaveManagerSaver
{
    [XmlArray("levels"), XmlArrayItem("level")]
    public List<Level> levels = new List<Level>(); 
    public class Level
    {
        public List<WaveManager.MiniWave> waves;
        [XmlAttribute("name")]
        public string name;
    }
    //[XmlArray("waves"), XmlArrayItem("wave")]
    //public List<Thing.MiniWave> waves = new List<Thing.MiniWave>();
    //public 
    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(WaveManagerSaver));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
            stream.Close();
        }
    }

    public static WaveManagerSaver Load(string path)
    {
        var serializer = new XmlSerializer(typeof(WaveManagerSaver));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as WaveManagerSaver;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static WaveManagerSaver LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(WaveManagerSaver));
        return serializer.Deserialize(new StringReader(text)) as WaveManagerSaver;
    }
}

