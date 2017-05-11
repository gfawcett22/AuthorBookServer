using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorBookApi.Data;
using AuthorBookApi.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using AuthorBookApi.Models;

namespace AuthorBookApi.Controllers
{
    [Route("api/authors/{authorId}/books")]
    public class BooksController : Controller
    {
        private ILibraryRepository _libRepo;

        public BooksController(ILibraryRepository libRepo)
        {
            _libRepo = libRepo;
        }

        [HttpGet]
        public IActionResult GetBooksForAuthor(int authorId)
        {
            if (!_libRepo.AuthorExists(authorId)) return NotFound();
            var booksForAuthorFromRepo = _libRepo.GetBooksForAuthor(authorId);
            var booksForAuthor = Mapper.Map<IEnumerable<BookDto>>(booksForAuthorFromRepo);

            return Ok(booksForAuthor);
        }

        [HttpGet("{id}", Name ="GetBookForAuthor")]
        public IActionResult GetBookForAuthor(int authorId, int id)
        {
            if (!_libRepo.AuthorExists(authorId)) return NotFound();
            var bookFromRepo = _libRepo.GetBookForAuthor(authorId, id);
            var book = Mapper.Map<BookDto>(bookFromRepo);
            return Ok(book); 
        }

        [HttpPost()]
        public IActionResult CreateBookForAuthor(int authorId, [FromBody] BookForCreationDto book)
        {
            if (book == null) return BadRequest();
            if (!_libRepo.AuthorExists(authorId)) return NotFound();
            var bookToCreate = Mapper.Map<Book>(book);
            _libRepo.AddBookForAuthor(authorId, bookToCreate);
            if (!_libRepo.Save())
            {
                throw new Exception($"Creating a book for author {authorId} failed to save.");
            }
            var bookToReturn = Mapper.Map<BookDto>(bookToCreate);
            return CreatedAtRoute("GetBookForAuthor", new {authorId = authorId, id = bookToReturn.Id}, bookToReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBookForAuthor(int authorId, int id)
        {
            if (!_libRepo.AuthorExists(authorId)) return NotFound();
            var bookFromRepo = _libRepo.GetBookForAuthor(authorId, id);
            if (bookFromRepo == null) return NotFound();
            _libRepo.DeleteBook(bookFromRepo);
            if (!_libRepo.Save()) throw new Exception($"Failed deleting book {id} for author {authorId}");
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBookForAuthor(int authorId, int id, [FromBody]BookForUpdateDto bookForUpdate)
        {
            if (bookForUpdate == null) return BadRequest();

            var bookFromRepo = _libRepo.GetBookForAuthor(authorId, id);
            if (bookFromRepo == null) return NotFound();
            Mapper.Map(bookFromRepo, bookForUpdate);
            _libRepo.UpdateBookForAuthor(bookFromRepo);
            if (!_libRepo.Save())
            {
                throw new Exception($"Error updating book {id} for author {authorId}");
            }
            return NoContent();
        }

    }
}