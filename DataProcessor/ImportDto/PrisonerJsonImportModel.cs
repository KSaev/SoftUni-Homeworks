using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class PrisonerJsonImportModel
    {
        [MinLength(3)]
        [MaxLength(20)]
        [Required]
        public string FullName { get; set; }

        [RegularExpression("The [A-Z][a-zA-z]*")]
        [Required]
        public string Nickname { get; set; }

        [Range(18,65)]
        public int Age { get; set; }

        [Required]
        public string IncarcerationDate { get; set; }

        public string ReleaseDate { get; set; }

        [Range(1,double.MaxValue)]
        public decimal? Bail { get; set; }

        public int? CellId { get; set; }

        public IEnumerable<MailJsonImportModel> Mails { get; set; }
    }
}
