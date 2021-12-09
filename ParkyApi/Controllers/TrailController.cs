using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Models;
using ParkyApi.Models.DTO;
using ParkyApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : ControllerBase
    {
        private readonly ITrailRepository trailRepository;
        private readonly IMapper mapper;

        public TrailsController(ITrailRepository trailRepository, IMapper mapper)
        {
            this.trailRepository = trailRepository;
            this.mapper = mapper;
        }





        /// <summary>
        /// get list of Trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TrailDTO>))]
        [ProducesDefaultResponseType]
        public IActionResult getTrails()
        {
            var TrailList = trailRepository.GetTrails();
            var TrailDTOList = new List<TrailDTO>();
            foreach (var item in TrailList)
            {
                TrailDTOList.Add(mapper.Map<TrailDTO>(item));
            }
            return Ok(TrailDTOList);
        }




        /// <summary>
        /// get individual Trail
        /// </summary>
        /// <param name="TrailId">nationa;</param>
        /// <returns></returns>
        [HttpGet("{TrailId:int}", Name = "getTrail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrailDTO))]
        [ProducesDefaultResponseType]
        public IActionResult getTrail(int TrailId)
        {
            var obj = trailRepository.GetTrail(TrailId);
            if (obj == null)
            {
                return NotFound();
            }
            var objDTO = mapper.Map<TrailDTO>(obj);
            return Ok(objDTO);
        }




        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TrailDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateTrail([FromBody] TrailInsertDTO objDTO)
        {
            if (objDTO == null)
            {
                return BadRequest(ModelState);
            }
            if (trailRepository.CheckTrailExists(objDTO.Name))
            {
                ModelState.AddModelError(string.Empty, "Trail exists");
                return StatusCode(404, ModelState);
            }
            var obj = mapper.Map<Trail>(objDTO);
            if (!trailRepository.CreateTrail(obj))
            {
                ModelState.AddModelError(string.Empty, $"something went wrong when adding {obj.Name}");
                return StatusCode(500, ModelState);
            }
            // to create and return the created object 
            return CreatedAtRoute("getTrail", new { TrailId = obj.Id }, obj);
        }





        // update Trail
        [HttpPatch("{TrailId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateTrail(int TrailId, [FromBody] TrailUpdateDTO objDTO)
        {
            if (objDTO == null || TrailId != objDTO.Id)
            {
                return BadRequest(ModelState);
            }

            var objN = trailRepository.GetTrail(objDTO.Name);

            if (objN != null)
            {
                if (objN.Id != TrailId)
                {
                    ModelState.AddModelError(string.Empty, "Trail Exist");
                    return StatusCode(404, ModelState);
                }
            }


            var obj = mapper.Map<Trail>(objDTO);
            var objFromDB = trailRepository.GetTrail(obj.Id);
            objFromDB.Name = obj.Name;
            objFromDB.Distance = obj.Distance;
            objFromDB.Difficulty = obj.Difficulty;
            objFromDB.NationalParkId = obj.NationalParkId;


            if (!trailRepository.UpdateTrail(objFromDB))
            {
                ModelState.AddModelError(string.Empty, $"something went wrong when updATING {objFromDB.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }








        [HttpDelete("{TrailId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult DeleteTrail(int TrailId)
        {
            if (!trailRepository.CheckTrailExists(TrailId))
            {
                return NotFound();
            }
            var obj = trailRepository.GetTrail(TrailId);
            if (!trailRepository.DeleteTrail(obj))
            {
                ModelState.AddModelError(string.Empty, $"something went wrong when deleting record {obj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
