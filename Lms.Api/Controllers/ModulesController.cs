using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Data.Data;
using Lms.Core.Entities;
using AutoMapper;
using Lms.Data.Repositories;
using Lms.Core.Dto;
using Lms.Core.Repositories;

namespace Lms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly LmsApiContext _context;
        private readonly IMapper mapper;
        private readonly IUnitOfWork uow;

        public ModulesController(LmsApiContext context,IMapper mapper, IUnitOfWork unitOfWork)
        {
            _context = context;
            this.mapper = mapper;
            uow = unitOfWork;  
        }

        [HttpGet] //GET: api/Modules dto 
        public async Task<ActionResult<IEnumerable<Course>>> GetAllModules()
        {
            var modules = await uow.ModuleRepository.GetAllModules();
            var dto = mapper.Map<IEnumerable<ModuleDto>>(modules);
            return Ok(dto);
        }


        //// GET: api/Modules
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Module>>> GetModule()
        //{
        //  if (_context.Module == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.Module.ToListAsync();
        //}

        [HttpGet("{id}")]
        //[Route("{id}")]
        public async Task<ActionResult<ModuleDto>> GetModule(int id)
        {
            if ((await uow.ModuleRepository.GetModule(id)) is null)
                return NotFound(new { Error = new[] { $"CodeEvent with Id: [{id}] not found" } });

            var module = await uow.ModuleRepository.GetModule(id);

            if (module is null) return BadRequest();

            return Ok(mapper.Map<ModuleDto>(module));
        }


        //[HttpPost]
        //public async Task<ActionResult<ModuleDto>> Create(int id, ModuleDto dto)
        //{
        //    var module = await uow.ModuleRepository.GetModule(id);
        //    if (course is null)
        //        return NotFound(new { Error = new[] { with id: [{id}] not found" } });

        //    var module = mapper.Map<Module>(dto);
        //    module.Course = course;
        //    await uow.ModuleRepository.Add(module);

        //    await uow.CompleteAsync();
        //    var model = mapper.Map<ModuleDto>(module);
        //    return CreatedAtAction(nameof(GetCourse), new { name = course.Title, id = model.Id }, model);

        //}


        // DELETE: api/Modules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            if (_context.Module == null)
            {
                return NotFound();
            }
            var @module = await _context.Module.FindAsync(id);
            if (@module == null)
            {
                return NotFound();
            }

            _context.Module.Remove(@module);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModuleExists(int id)
        {
            return (_context.Module?.Any(e => e.Id == id)).GetValueOrDefault();
        }



        // PUT:
    }

}
