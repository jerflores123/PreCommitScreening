using IJOS.Domain.Common;
using System;
using System.Collections.Generic;

namespace IJOS.Domain.Entities
{
    public class Screening : AuditableEntity
    {
        public long ScreeningId { get; set; }
        public long Sin { get; set; }
        public DateTime? ScreeningDate { get; set; }
        public string CountyName { get; set; }
        public string IdjcStaffPresent { get; set; }
        public string Notes { get; set; }
        public long? ScreeningTypeId { get; set; }
        public bool? MarkDelete { get; set; }

        public ScreeningType ScreeningType { get; set; }

        public ICollection<ScreeningOutcome> ScreeningOutcomes { get; set; }
    }
}