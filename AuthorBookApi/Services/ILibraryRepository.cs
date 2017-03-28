﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorBookApi.Data;

namespace AuthorBookApi.Services
{
    public interface ILibraryRepository
    {
        IEnumerable<Author> GetAuthors();
        Author GetAuthor(int authorId);
        IEnumerable<Author> GetAuthors(IEnumerable<int> authorIds);
        void AddAuthor(Author author);
        void DeleteAuthor(Author author);
        void UpdateAuthor(Author author);
        bool AuthorExists(int authorId);
        IEnumerable<Book> GetBooksForAuthor(int authorId);
        Book GetBookForAuthor(int authorId, int bookId);
        void AddBookForAuthor(int authorId, Book book);
        void UpdateBookForAuthor(Book book);
        void DeleteBook(Book book);
        bool Save();
    }
}
