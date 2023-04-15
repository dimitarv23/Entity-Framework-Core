using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Creator")]
    public class ExportCreatorDto
    {
        [XmlAttribute("BoardgamesCount")]
        public int BoardgamesCount { get; set; }

        [XmlElement("CreatorName")]
        public string Name { get; set; }

        [XmlArray("Boardgames")]
        public List<ExportBoardgameDto> Boardgames { get; set; }
            = new List<ExportBoardgameDto>();
    }
}
