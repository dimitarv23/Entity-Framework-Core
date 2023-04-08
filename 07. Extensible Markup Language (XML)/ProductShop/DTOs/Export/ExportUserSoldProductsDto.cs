using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("User")]
    public class ExportUserSoldProductsDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; } = null!;

        [XmlElement("lastName")]
        public string LastName { get; set; } = null!;

        [XmlElement("age")]
        public int? Age { get; set; }

        [XmlElement("SoldProducts")]
        public virtual ExportSoldProductsDto SoldProducts { get; set; }
    }
}
