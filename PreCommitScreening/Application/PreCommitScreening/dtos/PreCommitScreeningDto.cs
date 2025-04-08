using System;
using System.Collections.Generic;

namespace IJOS.Application.PreCommitScreening.dtos
{
    public class PreCommitScreeningDto
    {
        public long Sin { get; set; }
        public long Screening_id { get; set; }
        public DateTime? Screening_date { get; set; }
        public string County_name { get; set; }
        public string Idjc_staff_present { get; set; }
        public string Notes { get; set; }
        public long? Screening_type_id { get; set; }
        public string Created_by_staff { get; set; }
        public DateTime? Modified_date{get; set; }
        public IEnumerable<PreCommitScreeningOutcomesDto> Outcomes { get; set; }
    }
}
