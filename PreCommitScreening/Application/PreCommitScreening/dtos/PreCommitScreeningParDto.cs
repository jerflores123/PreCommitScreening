using IJOS.Application.Common.Mappings;
using IJOS.Domain.Entities;
using System;

namespace IJOS.Application.PreCommitScreening.Queries.GetPreCommitScreening
{
    public class PreCommitScreeningParDto : IMapFrom<RefScreeningParticipant>
    {
        public long ScreeningParticipantId { get; set; }
        public string ScreeningParticipant { get; set; }
        public long? OrderBy { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
