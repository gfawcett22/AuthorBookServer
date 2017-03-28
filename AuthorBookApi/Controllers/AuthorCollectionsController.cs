using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorBookApi.Data;
using Microsoft.AspNetCore.Mvc;
using AuthorBookApi.Services;
using AuthorBookApi.Models;
using AutoMapper;

namespace AuthorBookApi.Controllers
{
    [Route("api/authorcollections")]
    public class AuthorCollectionsController : Controller
    {
        private ILibraryRepository _libRepo;
        public AuthorCollectionsController(ILibraryRepository _libRepo)
        {
            this._libRepo = _libRepo;
        }
       
        [HttpPost]
        public IActionResult CreateAuthorCollection([FromBody] IEnumerable<AuthorToCreateDto> authors)
        {
            if (authors == null) return BadRequest();
            var authorsToCreate = Mapper.Map<IEnumerable<Author>>(authors);
            foreach (var author in authorsToCreate)
            {
                _libRepo.AddAuthor(author);
            }
            if (!_libRepo.Save()) throw new Exception($"There was an error saving the collection of authors.");
            return Ok();
        }
    }
}