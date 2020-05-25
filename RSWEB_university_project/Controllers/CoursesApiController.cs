﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RSWEB_university_project.Data;
using RSWEB_university_project.Models;

namespace RSWEB_university_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesApiController : ControllerBase
    {
        private readonly RSWEB_university_projectContext _context;

        public CoursesApiController(RSWEB_university_projectContext context)
        {
            _context = context;
        }

        // GET: api/CoursesApi
        [HttpGet]
        public List<Course> GetCourse(string titleString, string programmeString, int semestarInt = 0)
        {
            IQueryable<Course> courses = _context.Course.AsQueryable();
            if (!string.IsNullOrEmpty(titleString))
            {
                courses = courses.Where(s => s.Title.Contains(titleString));
            }
            if (!string.IsNullOrEmpty(programmeString))
            {
                courses = courses.Where(s => s.Programme == programmeString);
            }
            if (semestarInt != 0)
            {
                courses = courses.Where(s => s.Semestar == semestarInt);
            }
            return courses.ToList();
        }

        // GET: api/CoursesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/CoursesApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.CourseID)
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

        // POST: api/CoursesApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Course.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.CourseID }, course);
        }

        // DELETE: api/CoursesApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Course>> DeleteCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();

            return course;
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseID == id);
        }
    }
}
