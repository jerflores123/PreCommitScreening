using IJOS.Domain.Common;

namespace IJOS.Domain.Entities
{
    public class ScreeningOutcome : AuditableEntity
    {
        public long Id { get; set; }
        public long ScreeningId { get; set; }
        public long? ScreeningRecId { get; set; }
        public long? ScreeningParticipantId { get; set; }

        public Screening Screening { get; set; }
    }
}
