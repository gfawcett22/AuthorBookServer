using System;
using System.Collections.Generic;
using AuthorBookApi.Data;
using AuthorBookApi.Models;
using AuthorBookApi.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AuthorBookApi.Controllers
{
    [Route("/api/authors")]
    public class AuthorsController : Controller
    {
        private ILibraryRepository _libRepo;

        public AuthorsController(ILibraryRepository libRepo)
        {
            _libRepo = libRepo;
        }

        [HttpGet]
        public IActionResult GetAuthors()
        {
            try
            {
                var authorsFromRepo = _libRepo.GetAuthors();
                var authors = Mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);
                return Ok(authors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "There was an internal server error.");
            }
        }

        [HttpGet("{id}", Name = "GetAuthor")]
        public IActionResult GetAuthor([FromRoute] int id)
        {
            try
            {
                if (!_libRepo.AuthorExists(id))
                {
                    return NotFound();
                }
                var author = Mapper.Map<AuthorDto>(_libRepo.GetAuthor(id));
                return Ok(author);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "There was a server error");
            }
        }

        [HttpPost]
        public IActionResult CreateAuthor([FromBody] AuthorToCreateDto author)
        {
            try
            {
                if (author == null) return BadRequest();


                var authorEntity = Mapper.Map<Author>(author);
                _libRepo.AddAuthor(authorEntity);
                if (!_libRepo.Save())
                {
                    //let middle ware handle exception
                    throw new Exception("Creating an author failed.");
                }
                var authorToReturn = Mapper.Map<AuthorDto>(authorEntity);
                return CreatedAtRoute("GetAuthor", new { id = authorToReturn.Id }, authorToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "There was an internal server error.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(int id)
        {
            var authorFromRepo = _libRepo.GetAuthor(id);
            if (authorFromRepo == null) return NotFound();
            _libRepo.DeleteAuthor(authorFromRepo);
            if (!_libRepo.Save()) throw new Exception($"There was an error deleting author {id}");
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAuthor(int id, [FromBody] AuthorToUpdateDto authorToUpdate)
        {
            if (authorToUpdate == null) return BadRequest();
            var authorFromRepo = _libRepo.GetAuthor(id);

            if (authorFromRepo == null) return NotFound();
            try
            {
                Mapper.Map(authorToUpdate, authorFromRepo);
                //authorFromRepo.Id = id;
                _libRepo.UpdateAuthor(authorFromRepo); 
                _libRepo.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating author {id}");
            }
            return NoContent();
        }
    }
}