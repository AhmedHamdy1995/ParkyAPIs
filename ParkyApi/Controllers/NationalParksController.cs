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
    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository npRepository;
        private readonly IMapper mapper;

        public NationalParksController(INationalParkRepository npRepository , IMapper mapper)
        {
            this.npRepository = npRepository;
            this.mapper = mapper;
        }





        /// <summary>
        /// get list of national parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK,Type=typeof(List<NationalParkDTO>))]
        [ProducesDefaultResponseType]
        public IActionResult getNationalParks()
        {
            var NationalParkList = npRepository.GetNationalParks();
            var NationalParkDTOList = new List<NationalParkDTO>();
            foreach (var item in NationalParkList)
            {
                NationalParkDTOList.Add(mapper.Map<NationalParkDTO>(item));
            }
            return Ok(NationalParkDTOList);
        }




        /// <summary>
        /// get individual national park
        /// </summary>
        /// <param name="nationalParkId">nationa;</param>
        /// <returns></returns>
        [HttpGet("{nationalParkId:int}",Name = "getNationalPark")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NationalParkDTO))]
        [ProducesDefaultResponseType]
        public IActionResult getNationalPark(int nationalParkId)
        {
            var obj = npRepository.GetNationalPark(nationalParkId);
            if (obj == null)
            {
                return NotFound();
            }
            var objDTO = mapper.Map<NationalParkDTO>(obj);
            return Ok(objDTO);
        }




        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created , Type = typeof(NationalParkDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateNationalPark([FromBody] NationalParkDTO objDTO)
        {
            if(objDTO == null)
            {
                return BadRequest(ModelState);
            }
            if (npRepository.CheckNationalParkExists(objDTO.Name))
            {
                ModelState.AddModelError(string.Empty, "National park exists");
                return StatusCode(404, ModelState);
            }
            var obj = mapper.Map<NationalPark>(objDTO);
            if (!npRepository.CreateNationalPark(obj))
            {
                ModelState.AddModelError(string.Empty, $"something went wrong when adding {obj.Name}");
                return StatusCode(500, ModelState);
            }
            // to create and return the created object 
            return CreatedAtRoute("getNationalPark",new { nationalParkId=obj.Id }, obj);
        } 





        // update national park
        [HttpPatch("{NationalParkId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateNationalPark(int NationalParkId,[FromBody] NationalParkDTO objDTO)
        {
            if (objDTO == null || NationalParkId!=objDTO.Id)
            {
                return BadRequest(ModelState);
            }

            var objN = npRepository.GetNationalPark(objDTO.Name);

            if(objN != null)
            {
                if(objN.Id != NationalParkId)
                {
                    ModelState.AddModelError(string.Empty, "National Park Exist");
                    return StatusCode(404, ModelState);
                }
            }


            var obj = mapper.Map<NationalPark>(objDTO);
            var objFromDB = npRepository.GetNationalPark(obj.Id);
            objFromDB.Name = obj.Name;
            objFromDB.State = obj.State;
            objFromDB.Created = obj.Created;
            objFromDB.Established = obj.Established;


            if (!npRepository.UpdateNationalPark(objFromDB))
            {
                ModelState.AddModelError(string.Empty, $"something went wrong when updATING {objFromDB.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }








        [HttpDelete("{NationalParkId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult DeleteNationalPark(int NationalParkId)
        {
            if (!npRepository.CheckNationalParkExists(NationalParkId))
            {
                return NotFound();
            }
            var obj = npRepository.GetNationalPark(NationalParkId);
            if (!npRepository.DeleteNationalPark(obj))
            {
                ModelState.AddModelError(string.Empty, $"something went wrong when deleting record {obj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
