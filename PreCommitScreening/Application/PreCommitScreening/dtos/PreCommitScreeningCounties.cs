using IJOS.Application.Common.Mappings;
using System;
using e = IJOS.Domain.Entities;

namespace IJOS.Application.PreCommitScreening.dtos
{
    public class PreCommitScreeningCounties : IMapFrom<e.County>
    {
        public string County_name { get; set; }
        public string District { get; set; }
        public string Region { get; set; }
        public DateTime? Created_date { get; set; }
        public string Created_by { get; set; }
        public DateTime? Modified_date { get; set; }
        public string Modified_by { get; set; }
    }
}
