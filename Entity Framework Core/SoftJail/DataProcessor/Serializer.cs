namespace SoftJail.DataProcessor
{

    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .Where(x => ids.Contains(x.Id))
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.FullName,
                    CellNumber = x.Cell.CellNumber,
                    Officers = x.PrisonerOfficers.Select(o => new
                    {
                        OfficerName = o.Officer.FullName,
                        Department = o.Officer.Department.Name,
                    })
                    .OrderBy(x => x.OfficerName)
                    .ToList(),
                    TotalOfficerSalary = decimal.Parse(x.PrisonerOfficers
                    .Sum(x => x.Officer.Salary).ToString("f2")),
                    
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToList();

            return JsonConvert.SerializeObject(prisoners,Formatting.Indented);
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var validatePrisoners = prisonersNames.Split(',').ToArray();

            var data = context.Prisoners
                .Where(x => validatePrisoners.Contains(x.FullName))
                .Select(x => new PrisonerXmlExportModel
                {
                    Id = x.Id,
                    Name = x.FullName,
                    IncarcerationDate = x.IncarcerationDate.ToString("yyyy-MM-dd"),
                    Message = x.Mails.Select(m => new MessageXmlExportModel
                    {
                        Description = string.Join("",m.Description.Reverse()),
                    })
                    
                    .ToArray()
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToArray();
                

            var xml = new XmlSerializer(typeof(PrisonerXmlExportModel[]), new XmlRootAttribute("Prisoners"));
            var sw = new StringWriter();
            XmlSerializerNamespaces nm = new XmlSerializerNamespaces();
            nm.Add("", "");
            xml.Serialize(sw, data, nm);

            return sw.ToString().Trim();
        }
    }
}