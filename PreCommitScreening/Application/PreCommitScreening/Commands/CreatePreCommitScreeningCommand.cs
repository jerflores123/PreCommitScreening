using AutoMapper;
using IJOS.Application.Common.Interfaces;
using IJOS.Application.Common.Security;
using IJOS.Application.Persistence_Interfaces;
using IJOS.Application.PreCommitScreening.dtos;
using IJOS.Domain.Common.Constants;
using IJOS.Domain.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IJOS.Application.PreCommitScreening.Commands
{
    [Authorize(Domain.Common.Constants.Features.Pre_Commit_Screening, Privileges.Create)]
    public class CreatePreCommitScreeningCommand : IRequest
    {
        public PreCommitScreeningVm ScreeningVm { get; set; }
    }

    public class CreatePreCommitScreeningCommandHandler : IRequestHandler<CreatePreCommitScreeningCommand>
    {
        private readonly IApplicationDbContext dbContext;

        public CreatePreCommitScreeningCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(CreatePreCommitScreeningCommand request, CancellationToken cancellationToken)
        {
            var preCommitScreening = request.ScreeningVm.preCommitScreeningDto[0];
            dbContext.Screenings.Add(
                new Screening
                {
                    Sin = preCommitScreening.Sin,
                    ScreeningDate = preCommitScreening.Screening_date,
                    CountyName = preCommitScreening.County_name,
                    IdjcStaffPresent = preCommitScreening.Idjc_staff_present,
                    Notes = preCommitScreening.Notes,
                    ScreeningTypeId = preCommitScreening.Screening_type_id,
                    ScreeningOutcomes = preCommitScreening.Outcomes.Select(outcome => new ScreeningOutcome
                    {
                        ScreeningRecId = outcome.Screening_rec_id,
                        ScreeningParticipantId = outcome.Screening_participant_id
                    }).ToArray()
                });
            await dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
