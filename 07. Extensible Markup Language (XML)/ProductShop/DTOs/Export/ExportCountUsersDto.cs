using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    public class ExportCountUsersDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public List<ExportUserSoldProductsDto> Users { get; set; }
    }
}
