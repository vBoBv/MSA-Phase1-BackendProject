using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentSIMS.Models;
using StudentSIMS.Models.Dtos;
using StudentSIMS.Repository.IRepository;

namespace StudentSIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class AddressesController : Controller
    {
        private readonly IAddressRepository _addressRepo;
        private readonly IMapper _mapper;

        public AddressesController(IAddressRepository addressRepo, IMapper mapper)
        {
            _addressRepo = addressRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of addresses
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AddressDto>))]
        public IActionResult GetAddresses()
        {
            var objList = _addressRepo.GetAddresses();
            var objDto = new List<AddressDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<AddressDto>(obj));
            }

            return Ok(objList);
        }

        /// <summary>
        /// Get individual address
        /// </summary>
        /// <param name="addressId">The id of the address</param>
        /// <returns></returns>
        [HttpGet("{addressId:int}", Name = "GetAddress")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetAddress(int addressId)
        {
            var obj = _addressRepo.GetAddress(addressId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<AddressDto>(obj);
            return Ok(objDto);
        }

        /// <summary>
        /// Add a new address
        /// </summary>
        /// <param name="addressDto">The properties of an address</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(List<AddressDto>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateAddress([FromBody] AddressCreateDto addressDto)
        {
            if (addressDto == null)
            {
                return BadRequest(ModelState);
            }

            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            var addressObj = _mapper.Map<Address>(addressDto);
            if (!_addressRepo.CreateAddress(addressObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {addressObj.addressId}");
                return StatusCode(500, ModelState);
            }
            //return Ok();
            return CreatedAtAction("GetAddress", new { addressId = addressObj.addressId }, addressObj);
        }

        /// <summary>
        /// Update an address
        /// </summary>
        /// <param name="addressId">The id of the address to be updated</param>
        /// <param name="addressDto">The property of the address to be updated</param>
        /// <returns></returns>
        [HttpPatch("{addressId:int}", Name = "UpdateAddress")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateAddress(int addressId, [FromBody] AddressUpdateDto addressDto)
        {
            if (addressDto == null || addressId != addressDto.addressId)
            {
                return BadRequest(ModelState);
            }

            var addressObj = _mapper.Map<Address>(addressDto);
            
            if (!_addressRepo.UpdateAddress(addressObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {addressObj.addressId}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete an address
        /// </summary>
        /// <param name="addressId">The id of the address to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{addressId:int}", Name = "DeleteAddress")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteAddress(int addressId)
        {
            if (!_addressRepo.AddressExists(addressId))
            {
                return NotFound();
            }

            var addressObj = _addressRepo.GetAddress(addressId);
            if (!_addressRepo.DeleteAddress(addressObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {addressObj.addressId}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpGet("[action]/{addressId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetAddressesOfStudent(int studentId)
        {
            var objList = _addressRepo.GetAddressesOfStudent(studentId);
            if (objList == null)
            {
                return NotFound();
            }

            var objDto = new List<AddressDto>();
            foreach(var obj in objList)
            {
                objDto.Add(_mapper.Map<AddressDto>(obj));
            }
            return Ok(objDto);
        }
    }
}
