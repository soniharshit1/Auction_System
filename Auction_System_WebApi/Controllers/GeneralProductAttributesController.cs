using Auction_System_Library_Database.Data;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using Auction_System_Library_Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auction_System_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralProductAttributesController : ControllerBase
    {
        private readonly IGeneralProductAttributesRepository _generalProductAttributesRepository;

        public GeneralProductAttributesController(IGeneralProductAttributesRepository generalProductAttributesRepository)
        {
            _generalProductAttributesRepository = generalProductAttributesRepository;
        }

        // GET: api/GeneralProductAttributes
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<GeneralProductAttribute>>> GetGeneralProductAttributes(int productId)
        {
            return Ok(await _generalProductAttributesRepository.GetAttributesByProductId(productId));
        }

        // GET: api/GeneralProductAttributes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GeneralProductAttribute>> GetGeneralProductAttributeById(int id)
        {
            var generalProductAttribute = await _generalProductAttributesRepository.GetGeneralProductAttributeById(id);
            if (generalProductAttribute == null)
            {
                return NotFound();
            }
            return Ok(generalProductAttribute);
        }

        // TODO: handle null result
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGeneralProductAttribute(int id, GeneralProductAttributeDTO generalProductAttributeDto)
        {
            var result = await _generalProductAttributesRepository.UpdateGeneralProductAttribute(id, generalProductAttributeDto);
            if (result == $"Attribute with id: {id} not found.")
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> PostGeneralProductAttribute(int productId,GeneralProductAttributeDTO generalProductAttributeDTO)
        {
            var result = await _generalProductAttributesRepository.AddGeneralProductAttribute(generalProductAttributeDTO, productId)
            return Ok(result);
        }

        // DELETE: api/GeneralProductAttributes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGeneralProductAttribute(int id)
        {
            var result = await _generalProductAttributesRepository.DeleteGeneralProductAttribute(id);
            if (result == $"Attribute with id: {id} not found.")
            {
                return NotFound(result);
            }
            return Ok(result);
        }
    }
}
