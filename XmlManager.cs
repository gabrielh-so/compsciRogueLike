using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml.Serialization;

namespace MajorProject
{
    public class XmlManager<T>
    {
        public Type type;
        public XmlManager()
        {
            type = typeof(T);
        }

        public T Load(string path)
        {
            //var path1 = Directory.GetCurrentDirectory();
            T instance;
            using (TextReader reader = new StreamReader(path))
            {
                XmlSerializer xml = new XmlSerializer(type);
                instance = (T)xml.Deserialize(reader);
            }
            return instance;
        }

        public void Save(string path, object obj)
        {
            using (TextWriter writer = new StreamWriter(path))
            {
                XmlSerializer xml = new XmlSerializer(type);
                xml.Serialize(writer, obj);
            }
        }
    }
}
