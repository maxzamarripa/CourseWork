﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreAPI.Data;
using CoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [ApiController]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository campRepository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;

        public CampsController(ICampRepository campRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            this.campRepository = campRepository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
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
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Get(string moniker, bool includeTalks = false)
        {
            try
            {
                var result = await campRepository.GetCampAsync(moniker, includeTalks);

                if (result == null)
                {
                    return NotFound();
                }

                var response = mapper.Map<CampModel>(result);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message + ex.InnerException?.Message);
            }
        }

        [HttpGet("{moniker}")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetV1_1(string moniker)
        {
            try
            {
                var result = await campRepository.GetCampAsync(moniker, true);

                if (result == null)
                {
                    return NotFound();
                }

                var response = mapper.Map<CampModel>(result);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message + ex.InnerException?.Message);
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message + ex.InnerException?.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CampModel model)
        {
            try
            {
                //Check duplicate external identifier
                var existing = await campRepository.GetCampAsync(model.Moniker);
                if(existing != null)
                {
                    return BadRequest("Moniker in use!");
                }

                var location = linkGenerator.GetPathByAction("Get", "Camps", new { moniker = model.Moniker });
                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Cannot use current moniker");
                }

                //Add new camp
                var newCamp = mapper.Map<Camp>(model);

                campRepository.Add(newCamp);

                if (await campRepository.SaveChangesAsync())
                {
                    return Created(location, mapper.Map<CampModel>(newCamp));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message + ex.InnerException?.Message);
            }

            return BadRequest();
        }

        [HttpPut("{moniker}")]
        public async Task<IActionResult> Put(string moniker, CampModel model)
        {
            try
            {
                var oldCamp = campRepository.GetCampAsync(moniker).Result;
                if(oldCamp == null)
                {
                    return NotFound($"Could not find camp with moniker of {moniker}");
                }

                mapper.Map(model, oldCamp);

                if(await campRepository.SaveChangesAsync())
                {
                    return Ok(mapper.Map<CampModel>(oldCamp));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message + ex.InnerException?.Message);
            }

            return BadRequest();
        }

        [HttpDelete("{moniker}")]
        public async Task<IActionResult> Delete(string moniker)
        {
            try
            {
                var oldCamp = campRepository.GetCampAsync(moniker).Result;
                if (oldCamp == null)
                {
                    return NotFound($"Could not find camp with moniker of {moniker}");
                }

                //Delete related talks
                var talks = campRepository.GetTalksByMonikerAsync(moniker).Result;
                if (talks != null && talks.Any())
                {
                    foreach(var talk in talks)
                    {
                        campRepository.Delete(talk);
                    }
                }

                campRepository.Delete(oldCamp);

                if (await campRepository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message + ex.InnerException?.Message);
            }

            return BadRequest();
        }
    }
}