﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorBookApi.Models
{
    public class AuthorToCreateDto
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Genre { get; set; }
        public ICollection<BookForCreationDto> Books { get; set; } = new List<BookForCreationDto>();
    }
}
