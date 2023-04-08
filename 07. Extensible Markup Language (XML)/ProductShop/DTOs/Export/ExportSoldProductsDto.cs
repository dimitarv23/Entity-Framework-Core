using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("SoldProducts")]
    public class ExportSoldProductsDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public List<ExportProductDto> Products { get; set; }
    }
}
