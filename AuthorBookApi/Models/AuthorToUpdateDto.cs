using System;
using System.Collections.Generic;

namespace AuthorBookApi.Models
{
    public class AuthorToUpdateDto
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Genre { get; set; }
        public ICollection<BookForCreationDto> Books { get; set; } = new List<BookForCreationDto>();
    }
}
