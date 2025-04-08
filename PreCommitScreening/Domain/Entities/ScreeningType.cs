using IJOS.Domain.Common;
using System.Collections.Generic;

namespace IJOS.Domain.Entities
{
    public class ScreeningType : AuditableEntity
    {
        public long ScreeningTypeId { get; set; }
        public string ScreeningType1 { get; set; }

        public ICollection<Screening> Screenings { get; set; }
    }
}