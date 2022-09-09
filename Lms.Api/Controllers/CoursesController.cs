using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Data.Data;
using Lms.Core.Entities;
using Lms.Data.Repositories;
using AutoMapper;
using Lms.Core.Dto;
using Lms.Core.Repositories;
using Microsoft.AspNetCore.JsonPatch;

namespace Lms.Api.Controllers
{
    [Route("api/courses")] 
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly LmsApiContext _context;
        private readonly IMapper mapper;
        private readonly IUnitOfWork uow;
        public CoursesController(LmsApiContext context,IMapper mapper, IUnitOfWork unitOfWork )
        {
            _context = context;
            this.mapper = mapper;
            uow = unitOfWork;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCoursesModules(bool includeModules)
        {
            var course = await uow.CourseRepository.GetAsync(includeModules);
            var dto = mapper.Map<IEnumerable<CourseDto>>(course);
            return Ok(dto);
        }

        //[HttpGet] //GET: api/Courses dto 
        //public async Task<ActionResult<IEnumerable<Course>>> GetAllCourses()
        //{
        //    var courses = await uow.CourseRepository.GetAllCourses();
        //    var dto = mapper.Map<IEnumerable<CourseDto>>(courses);
        //    return Ok(dto);
        //}


        [HttpGet("{id}")] // dto //api/courses/2?includeModules=true , bool includeModules =false)
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
          if (_context.Course == null)
          {
              return NotFound();
          }
            var course = await uow.CourseRepository.FindAsync(id);

            if (course is null)
            {
                return NotFound();
            }
            var dto = mapper.Map<CourseDto>(course);    

            return Ok(dto);
        }

        [HttpPost]// Create med dto
        public async Task<ActionResult<CourseDto>> Create(CourseDto dto)
        {
            if (await uow.CourseRepository.GetCourse(dto.Title) != null)
            {
                ModelState.AddModelError("Title", "Title is in use");
                return BadRequest();
            }
            var course = mapper.Map<Course>(dto);
            await uow.CourseRepository.AddAsync(course);
            await uow.CompleteAsync();

            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, dto);

        }

        [HttpPut("{title}")] //update allt i en course, dto
        public async Task<ActionResult<CourseDto>> PutDto(string title, CourseDto dto)
        {
            var course = await uow.CourseRepository.GetCourse(title);
            if (course == null)
                return NotFound();
            mapper.Map(dto, course);
            await uow.CompleteAsync();
            return Ok(mapper.Map<CourseDto>(course));

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            if (_context.Course == null)
            {
                return NotFound();
            }
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPatch("{Id}")]
        public async Task<ActionResult<CourseDto>> PatchCourse(int id, JsonPatchDocument <CourseDto> patchDocument)
        {
            var course = await uow.CourseRepository.GetCourse(id);   //FindAsync? 

            if (course == null) return NotFound();

            var dto = mapper.Map<CourseDto>(course);

            patchDocument.ApplyTo(dto, ModelState);

            if (!TryValidateModel(dto)) return BadRequest(ModelState);

            mapper.Map(dto, course);

            await uow.CompleteAsync();

            return Ok(mapper.Map<CourseDto>(course));
        }


        



    }
}
