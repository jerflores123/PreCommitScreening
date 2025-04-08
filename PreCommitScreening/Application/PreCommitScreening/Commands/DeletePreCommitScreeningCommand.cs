using AutoMapper;
using IJOS.Application.Common.Security;
using IJOS.Application.Persistence_Interfaces;
using IJOS.Domain.Common.Constants;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IJOS.Application.PreCommitScreening.Commands
{
    [Authorize(Features.Pre_Commit_Screening, Privileges.Delete)]
    public class DeletePreCommitScreeningCommand : IRequest
    {
        public long screening_id { get; set; }
    }

    public class DeletePreCommitScreeningCommandHandler : IRequestHandler<DeletePreCommitScreeningCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeletePreCommitScreeningCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeletePreCommitScreeningCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.PreCommitScreeningOutcomesRepository.QueryAsync(@"
                    DELETE FROM [IJOS].[SCREENING_OUTCOMES] WHERE [SCREENING_ID] = @screening_id
                ", new { screening_id = request.screening_id });

            await _unitOfWork.PreCommitScreeningRepository.QueryAsync(@"
                    DELETE FROM [IJOS].[SCREENINGS] WHERE [SCREENING_ID] = @screening_id
                ", new { screening_id = request.screening_id });

            return Unit.Value;
        }
    }
}