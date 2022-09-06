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

namespace Lms.Api.Controllers
{
    [Route("api/courses")] 
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly LmsApiContext _context;
        private readonly IMapper mapper;
        private readonly UnitOfWork uow;
        public CoursesController(LmsApiContext context,IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            uow = new UnitOfWork(_context);
        }

        //GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetAllCourses()
        {
         var courses = await uow.CourseRepository.GetAllCourses();
            return Ok(courses);
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
          if (_context.Course == null)
          {
              return NotFound();
          }
            var course = await uow.CourseRepository.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPut("{title}")]
        public async Task<ActionResult<CourseDto>> PutDto(string title, CourseDto dto)
        {
            var course = await uow.CourseRepository.GetCourse(title);
            if (course == null)
                return NotFound();
            mapper.Map(dto,course);
            await uow.CompleteAsync();
            return Ok(mapper.Map<CourseDto>(course));

        }





        [HttpPost]
        public async Task<ActionResult<Course>> AddAcync(Course course)
        {
          if (_context.Course == null)
          {
              return Problem("Entity set 'LmsApiContext.Course'  is null.");
          }
            _context.Course.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        }

        [HttpPost]//med dto
        public async Task<ActionResult<CourseDto>> Create(CourseDto dto)
        {
            var course = mapper.Map<Course>(dto);
            await uow.CourseRepository.AddAsync(course);
            uow.CourseRepository.Update(course);
            //return CreatedAtAction("AddAsync", new { id = course.Id }, course);
            return Ok(mapper.Map<CourseDto>(course));
        }
        // DELETE: api/Courses/5
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

        private bool CourseExists(int id)
        {
            return (_context.Course?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
