using SoftJail.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class OfficerXmlImportModel
    {
        [XmlElement("Name")]
        [Required]
        public string FullName { get; set; }

        [XmlElement("Money")]
        public decimal Salary { get; set; }

        [XmlElement("Position")]
        [EnumDataType(typeof(Position))]
        [Required]
        public string Position { get; set; }
        

        [XmlElement("Weapon")]
        [EnumDataType(typeof(Weapon))]
        [Required]
        public string Weapon { get; set; }

        [XmlElement("DepartmentId")]
        public int DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        public PrisonerXmlImportModel[] Prisoners { get; set; }
    }
}
