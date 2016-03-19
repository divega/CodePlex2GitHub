﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePlex2GitHub.Model
{
    public class Person
    {
        [Key]
        public string Alias { get; set; }
        public string GitHubAlias { get; set; }
        public bool IsTeamMember { get; set; }
    }
}
