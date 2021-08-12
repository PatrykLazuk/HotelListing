using AutoMapper;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;

        public CountryController(ILogger<CountryController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries([FromQuery] RequestParams requestParams)
        {
            var coutries = await _unitOfWork.Countries.GetAll(requestParams);
            var results = _mapper.Map<IList<CountryDTO>>(coutries);
            return Ok(results);
            
        }

        [HttpGet("{id:int}",Name = "GetCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id)
        {
            
            var country = await _unitOfWork.Countries.Get(q => q.Id == id, include: q => q.Include(x => x.Hotels));
            var results = _mapper.Map<CountryDTO>(country);
            return Ok(results);
            
        }

        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]

        public async Task<IActionResult> CreateCountry(CreateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt with {nameof(CreateCountry)}");
                return BadRequest(ModelState);
            }
            var country = _mapper.Map<Country>(countryDTO);
            await _unitOfWork.Countries.Insert(country);
            await _unitOfWork.Save();

            return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
            
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCoutry(int id, UpdateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid || id<0)
            {
                _logger.LogError($"Invalid UPDATE attempt with {nameof(UpdateCoutry)}");
                return BadRequest(ModelState);
            }

            var country = await _unitOfWork.Countries.Get(c => c.Id == id);

            if (country == null)
            {
                _logger.LogError($"Invalid UPDATE attempt with {nameof(UpdateCoutry)}");
                return BadRequest("Submitted data is invalid");
            }
            _mapper.Map(countryDTO, country);
            _unitOfWork.Countries.Update(country);
            await _unitOfWork.Save();

            return NoContent();

        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (id < 0)
            {
                _logger.LogError($"Invalid Delete attempt in {nameof(DeleteCountry)}");
                return BadRequest("Invalid country id");
            }

            var hotel = _unitOfWork.Countries.Get(h => h.Id == id);

            if (hotel == null)
            {
                _logger.LogError($"Invalid Delete attempt in {nameof(DeleteCountry)}");
                return BadRequest("Invalid country id");
            }

            await _unitOfWork.Countries.Delete(id);
            await _unitOfWork.Save();
            return NoContent();
        }
    }
}
