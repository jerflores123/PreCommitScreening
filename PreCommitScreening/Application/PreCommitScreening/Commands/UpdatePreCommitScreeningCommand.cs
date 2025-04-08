using AutoMapper;
using IJOS.Application.Common.Interfaces;
using IJOS.Application.Common.Security;
using IJOS.Application.Persistence_Interfaces;
using IJOS.Application.PreCommitScreening.dtos;
using IJOS.Domain.Common.Constants;
using IJOS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IJOS.Application.PreCommitScreening.Commands
{
    [Authorize(Domain.Common.Constants.Features.Pre_Commit_Screening, Privileges.Modify)]
    public class UpdatePreCommitScreeningCommand : IRequest
    {
        public PreCommitScreeningVm ScreeningVm { get; set; }
    }

    public class UpdatePreCommitScreeningCommandHandler : IRequestHandler<UpdatePreCommitScreeningCommand>
    {
        private readonly IApplicationDbContext dbContext;

        public UpdatePreCommitScreeningCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Unit> Handle(UpdatePreCommitScreeningCommand request, CancellationToken cancellationToken)
        {
            var requestScreening = request.ScreeningVm.preCommitScreeningDto[0];
            var dbScreening = await dbContext.Screenings.Include(x => x.ScreeningOutcomes).SingleAsync(x => x.ScreeningId == requestScreening.Screening_id, cancellationToken);
            dbScreening.ScreeningDate = requestScreening.Screening_date;
            dbScreening.CountyName = requestScreening.County_name;
            dbScreening.IdjcStaffPresent = requestScreening.Idjc_staff_present;
            dbScreening.Notes = requestScreening.Notes;
            dbScreening.ScreeningTypeId = requestScreening.Screening_type_id;
            dbScreening.ScreeningOutcomes = requestScreening.Outcomes.Select(x => new ScreeningOutcome
            {
                ScreeningParticipantId = x.Screening_participant_id,
                ScreeningRecId = x.Screening_rec_id
            }).ToList();
            await dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}