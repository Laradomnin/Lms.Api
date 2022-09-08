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
            //  uow = new UnitOfWork(_context);
            uow = unitOfWork;
        }
 

        [HttpGet] //GET: api/Courses dto 
        public async Task<ActionResult<IEnumerable<Course>>> GetAllCourses()
        {
            var courses = await uow.CourseRepository.GetAllCourses();
            var dto = mapper.Map<IEnumerable<CourseDto>>(courses);
            return Ok(dto);
        }


        [HttpGet("{id}")] // dto
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

        // private bool CourseExists(int id)
        //{
        //    return (_context.Course?.Any(e => e.Id == id)).GetValueOrDefault();
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCourse(int id, Course course)
        //{
        //    if (id != course.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(course).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CourseExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}


        //[HttpPost]
        //public async Task<ActionResult<Course>> AddAcync(Course course)
        //{
        //  if (_context.Course == null)
        //  {
        //      return Problem("Entity set 'LmsApiContext.Course'  is null.");
        //  }
        //    _context.Course.Add(course);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        //}


        // [HttpGet] ////GET: api/Courses
        // public async Task<ActionResult<IEnumerable<Course>>> GetAllCourses()
        // {
        //  var courses = await uow.CourseRepository.GetAllCourses();
        //     return Ok(courses);
        // }
    }
}
