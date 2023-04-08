using ProductShop.Models;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("User")]
    public class ExportUserDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; } = null!;

        [XmlElement("lastName")]
        public string LastName { get; set; } = null!;

        [XmlElement("age")]
        public int? Age { get; set; }

        [XmlArray("soldProducts")]
        public virtual List<ExportProductDto> ProductsSold { get; set; } = null!;
    }
}
