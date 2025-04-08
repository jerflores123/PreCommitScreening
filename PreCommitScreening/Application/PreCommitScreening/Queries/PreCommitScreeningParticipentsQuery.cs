using AutoMapper;
using IJOS.Application.Common.Security;
using IJOS.Application.Persistence_Interfaces;
using IJOS.Application.PreCommitScreening.Queries.GetPreCommitScreening;
using IJOS.Domain.Common.Constants;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IJOS.Application.PreCommitScreening.Queries
{
    [Authorize(Features.Pre_Commit_Screening, Privileges.Read)]
    public class PreCommitScreeningParticipentsQuery : IRequest<List<PreCommitScreeningParDto>>
    {
    }

    public class PreCommitScreeningParticipentsQueryHandler : IRequestHandler<PreCommitScreeningParticipentsQuery, List<PreCommitScreeningParDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PreCommitScreeningParticipentsQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<PreCommitScreeningParDto>> Handle(PreCommitScreeningParticipentsQuery request, CancellationToken cancellationToken)
        {
            string query = @"SELECT * FROM IJOS.[REF_SCREENING_PARTICIPANT]";
            var result = await _unitOfWork.PreCommitScreeningParticipantsRepository.QueryAsync(query);
            return _mapper.Map<List<PreCommitScreeningParDto>>(result);
        }
    }
}