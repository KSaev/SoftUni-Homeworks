namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var data =  JsonConvert.DeserializeObject<IEnumerable<DepartamentJsonImportModel>>(jsonString);

            var sb = new StringBuilder();

            foreach (var item in data)
            {
                if (!IsValid(item)
                    || !item.Cells.Any()
                    || !item.Cells.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (item.Cells.Count() == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var departament = new Department
                {
                    Name = item.Name
                };

                foreach (var cellJson in item.Cells)
                {
                   
                    var cell = new Cell
                    {
                        CellNumber = cellJson.CellNumber,
                        HasWindow = cellJson.HasWindow,
                    };

                    departament.Cells.Add(cell);
                }
                sb.AppendLine($"Imported {departament.Name} with {departament.Cells.Count} cells");
                context.Departments.Add(departament);
                context.SaveChanges();
            }

            return sb.ToString().Trim();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var data = JsonConvert.DeserializeObject<IEnumerable<PrisonerJsonImportModel>>(jsonString);

            var sb = new StringBuilder();

            foreach (var item in data)
            {
                if (!IsValid(item)
                    || !item.Mails.Any()
                    || !item.Mails.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var dateIsParsed = DateTime.TryParseExact(
                    item.IncarcerationDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var IncarcerationDate);

                var date = DateTime.TryParseExact(
                   item.IncarcerationDate, "dd/MM/yyyy",
                   CultureInfo.InvariantCulture, DateTimeStyles.None, out var ReleaseDate);

                var prisoner = new Prisoner
                {
                    FullName = item.FullName,
                    Nickname = item.Nickname,
                    Age = item.Age,
                    IncarcerationDate = IncarcerationDate,
                    ReleaseDate = ReleaseDate,
                    Bail = item.Bail,
                    CellId = item.CellId,
                };

                foreach (var mailJson in item.Mails)
                {
                    var mail = new Mail
                    {
                        Description = mailJson.Description,
                        Address = mailJson.Address,
                        Sender = mailJson.Sender,
                    };

                    prisoner.Mails.Add(mail);
                }

                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
                context.Prisoners.Add(prisoner);
                context.SaveChanges();
            }

            return sb.ToString().Trim();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var xml = new XmlSerializer(typeof(OfficerXmlImportModel[]), new XmlRootAttribute("Officers"));
            var text = new StringReader(xmlString);
            var data = (OfficerXmlImportModel[])xml.Deserialize(text);

            var sb = new StringBuilder();

            foreach (var item in data)
            {
                if (!IsValid(item))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var officer = new Officer
                {
                    FullName = item.FullName,
                    Salary = item.Salary,
                    Position = Enum.Parse<Position>(item.Position),
                    Weapon = Enum.Parse<Weapon>(item.Weapon),
                    DepartmentId = item.DepartmentId,
                    OfficerPrisoners = item.Prisoners.Select(x => new OfficerPrisoner
                    {
                        PrisonerId = x.Id,
                    })
                    .ToList()
                };

                
                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
                context.Officers.Add(officer);
                context.SaveChanges();
            }
            return sb.ToString().Trim();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}