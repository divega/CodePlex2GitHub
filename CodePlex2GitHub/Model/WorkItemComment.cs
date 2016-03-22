﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace CodePlex2GitHub.Model
{
    public class WorkItemComment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime PostedOn { get; set; }
        public User PosteBy { get; set; }
    }
}
