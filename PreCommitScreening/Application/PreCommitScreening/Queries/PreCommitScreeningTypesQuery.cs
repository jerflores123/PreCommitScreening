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
    public class PreCommitScreeningTypesQuery : IRequest<List<PreCommitScreeningTypeDto>>
    {
        public int AgencyId { get; set; }
    }

    public class PreCommitScreeningTypesQueryHandler : IRequestHandler<PreCommitScreeningTypesQuery, List<PreCommitScreeningTypeDto>>
    {
        private readonly IApplicationDbContext dbContext;

        public PreCommitScreeningTypesQueryHandler(IApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<PreCommitScreeningTypeDto>> Handle(PreCommitScreeningTypesQuery request, CancellationToken cancellationToken)
        {
            return await dbContext.ScreeningTypes.OrderBy(x => x.ScreeningType1)
                .Select(x => new PreCommitScreeningTypeDto
                {
                    Created_by = x.CreatedBy,
                    Created_date = x.CreatedDate,
                    Modified_by = x.ModifiedBy,
                    Modified_date = x.ModifiedDate,
                    Screening_type = x.ScreeningType1,
                    Screening_type_id = x.ScreeningTypeId
                }).ToListAsync(cancellationToken);
        }
    }
}
