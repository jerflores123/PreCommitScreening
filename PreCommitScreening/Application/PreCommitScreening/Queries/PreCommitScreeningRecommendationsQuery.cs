using IJOS.Application.Common.Interfaces;
using IJOS.Application.Common.Security;
using IJOS.Application.PreCommitScreening.Queries.GetPreCommitScreening;
using IJOS.Domain.Common.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IJOS.Application.PreCommitScreening.Queries
{
    [Authorize(Features.Pre_Commit_Screening, Privileges.Read)]
    public class PreCommitScreeningRecommendationsQuery : IRequest<List<PreCommitScreeningRecommendationsDto>>
    {
        public int AgencyId { get; set; }
    }

    public class PreCommitScreeningRecommendationsQueryHandler : IRequestHandler<PreCommitScreeningRecommendationsQuery, List<PreCommitScreeningRecommendationsDto>>
    {
        private readonly IApplicationDbContext dbContext;

        public PreCommitScreeningRecommendationsQueryHandler(IApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<PreCommitScreeningRecommendationsDto>> Handle(PreCommitScreeningRecommendationsQuery request, CancellationToken cancellationToken)
        {
            return await dbContext.RefScreeningRecommendations
                .Where(x => x.Settings.Any(y => y.AgencyId == request.AgencyId))
                .OrderBy(x => x.ScreeningRecommendation)
                .Select(x => new PreCommitScreeningRecommendationsDto
                {
                    ScreeningRecommendation = x.ScreeningRecommendation,
                    ScreeningRecId = x.ScreeningRecId
                }).ToListAsync(cancellationToken);
        }
    }
}
