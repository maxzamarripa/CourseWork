using System;
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
    [Route("api/camps/{moniker}/[controller]")]
    [ApiController]
    public class TalksController : ControllerBase
    {
        private readonly ICampRepository campRepository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;

        public TalksController(ICampRepository campRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            this.campRepository = campRepository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string moniker)
        {
            try
            {
                var talks = await campRepository.GetTalksByMonikerAsync(moniker);

                if (!talks.Any())
                {
                    return NotFound();
                }

                var response = mapper.Map<TalkModel[]>(talks);

                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message + ex.InnerException?.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(string moniker, int id)
        {
            try
            {
                var talk = await campRepository.GetTalkByMonikerAsync(moniker, id);

                if (talk == null)
                {
                    return NotFound();
                }

                var response = mapper.Map<TalkModel>(talk);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message + ex.InnerException?.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(string moniker, TalkModel model)
        {
            try
            {
                //Get camp
                var camp = await campRepository.GetCampAsync(moniker, true);
                if (camp == null)
                {
                    return BadRequest("Camp does not exists!");
                }

                //Check duplicate Title
                var existing = camp.Talks;
                if (existing != null && existing.Any(t => t.Title == model.Title))
                {
                    return BadRequest("You already have a talk with that Title!");
                }

                //Map talk to entity 
                var talk = mapper.Map<Talk>(model);
                //confirm association
                talk.Camp = camp;

                campRepository.Add(talk);

                if (await campRepository.SaveChangesAsync())
                {
                    var url = linkGenerator.GetPathByAction(
                        HttpContext, 
                        "Get", 
                        values: new { moniker, id = talk.TalkId });

                    return Created(url, mapper.Map<TalkModel>(talk));
                } else
                {
                    return BadRequest("Failed to add Talk");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message + ex.InnerException?.Message);
            }
        }
    }
}