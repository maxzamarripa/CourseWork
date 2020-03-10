using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreAPI.Data;
using CoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository campRepository;
        private readonly IMapper mapper;

        public CampsController(ICampRepository campRepository, IMapper mapper)
        {
            this.campRepository = campRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(bool includeTalks = false)
        {
            try
            {
                var result = await campRepository.GetAllCampsAsync(includeTalks);

                CampModel[] models = mapper.Map<CampModel[]>(result);

                return Ok(models);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message + ex.InnerException?.Message);
            }
        }

        [HttpGet("{moniker}")]
        public async Task<IActionResult> Get(string moniker)
        {
            try
            {
                var result = await campRepository.GetCampAsync(moniker);

                if (result == null)
                {
                    return NotFound();
                }

                var response = mapper.Map<CampModel>(result);

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByDate(DateTime date, bool includeTalks = false)
        {
            try
            {
                var results = await campRepository.GetAllCampsByEventDate(date, includeTalks);

                if (!results.Any())
                {
                    return NotFound();
                }

                var response = mapper.Map<CampModel[]>(results);

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}