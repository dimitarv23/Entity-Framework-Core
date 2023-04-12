using System.Text;
using System.Xml.Serialization;

namespace Theatre.Utilities
{
    public class XmlHelper
    {
        public T Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRoot = 
                new XmlRootAttribute(rootName);
            XmlSerializer serializer =
                new XmlSerializer(typeof(T), xmlRoot);

            StringReader reader = new StringReader(inputXml);
            T dto = (T)serializer.Deserialize(reader)!;

            return dto;
        }

        public string Serialize<T>(T obj, string rootName)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = 
                new XmlRootAttribute(rootName);
            XmlSerializer serializer =
                new XmlSerializer(typeof(T), xmlRoot);

            XmlSerializerNamespaces namespaces = 
                new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(sb);
            serializer.Serialize(writer, obj, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
