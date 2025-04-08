using IJOS.Application.Common.Mappings;
using System;
using e = IJOS.Domain.Entities;

namespace IJOS.Application.PreCommitScreening.Queries.GetPreCommitScreening
{
    public class PreCommitScreeningTypeDto : IMapFrom<e.Screening_Types>
    {
        public long Screening_type_id { get; set; }
        public string Screening_type { get; set; }
        public string Agency_name { get; set; }
        public long? Order_by { get; set; }
        public DateTime Created_date { get; set; }
        public DateTime Modified_date { get; set; }
        public long Modified_by { get; set; }
        public long Created_by { get; set; }
    }
}
