using IJOS.Application.Common.Interfaces;
using IJOS.Application.Common.Security;
using IJOS.Application.PreCommitScreening.dtos;
using IJOS.Domain.Common.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IJOS.Application.PreCommitScreening.Queries
{
    [Authorize(Features.Pre_Commit_Screening, Privileges.Read)]
    public class PreCommitScreeningQuery : IRequest<PreCommitScreeningVm>
    {
        public long sin { get; set; }
    }

    public class PreCommitScreeningQueryHandler : IRequestHandler<PreCommitScreeningQuery, PreCommitScreeningVm>
    {
        private readonly IApplicationDbContext dbContext;

        public PreCommitScreeningQueryHandler(IApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<PreCommitScreeningVm> Handle(PreCommitScreeningQuery request, CancellationToken cancellationToken)
        {
            var screenings = await dbContext.Screenings
                    .Where(x => x.Sin == request.sin)
                    .Select(x => new PreCommitScreeningDto
                    {
                        Sin = x.Sin,
                        County_name = x.CountyName,
                        Screening_date = x.ScreeningDate,
                        Screening_id = x.ScreeningId,
                        Idjc_staff_present = x.IdjcStaffPresent,
                        Notes = x.Notes,
                        Screening_type_id = x.ScreeningTypeId,
                        Created_by_staff = $"{x.CreatedByStaff.FirstName} {x.CreatedByStaff.LastName}",
                        Modified_date = x.ModifiedDate,
                        Outcomes = x.ScreeningOutcomes.Select(screeningOutcome => new PreCommitScreeningOutcomesDto
                        {
                            Screening_participant_id = screeningOutcome.ScreeningParticipantId,
                            Screening_rec_id = screeningOutcome.ScreeningRecId
                        })
                    })
                    .ToListAsync(cancellationToken);
            return new PreCommitScreeningVm { preCommitScreeningDto = screenings };
        }
    }
}