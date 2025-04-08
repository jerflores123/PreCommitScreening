using IJOS.Application.PreCommitScreening.Commands;
using IJOS.Application.PreCommitScreening.dtos;
using IJOS.Application.PreCommitScreening.Queries;
using IJOS.Application.PreCommitScreening.Queries.GetPreCommitScreening;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IJOS.WebUI.Controllers
{
    public class PreCommitScreeningController : ApiControllerBase
    {
        private readonly ILogger<PreCommitScreeningController> _logger;

        public PreCommitScreeningController(ILogger<PreCommitScreeningController> logger)
        {
            _logger = logger;
        }

        [HttpGet("ScreeningTypes/{agencyid}")]
        public async Task<ActionResult<List<PreCommitScreeningTypeDto>>> GetScreeningTypes(int agencyid)
        {
            try
            {
                return await Mediator.Send(new PreCommitScreeningTypesQuery() { AgencyId = agencyid });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }


        [HttpGet("ScreeningParticipents")]
        public async Task<ActionResult<List<PreCommitScreeningParDto>>> GetScreeningParticipents()
        {
            try
            {
                return await Mediator.Send(new PreCommitScreeningParticipentsQuery());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }


        [HttpGet("ScreeningRecommendations,{agencyid}")]
        public async Task<ActionResult<List<PreCommitScreeningRecommendationsDto>>> GetScreeningRecommendations(int agencyid)
        {
            try
            {
                return await Mediator.Send(new PreCommitScreeningRecommendationsQuery() { AgencyId = agencyid });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpGet("{sin}")]
        public async Task<ActionResult<PreCommitScreeningVm>> GetScreenings(long sin)
        {
            try
            {
                return await Mediator.Send(new PreCommitScreeningQuery() { sin = sin });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreatePreCommitScreeningCommand command)
        {
            try
            {
                await Mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdatePreCommitScreeningCommand command)
        {
            try
            {
                if (command.ScreeningVm == null)
                {
                    return BadRequest();
                }
                await Mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await Mediator.Send(new DeletePreCommitScreeningCommand() { screening_id = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}