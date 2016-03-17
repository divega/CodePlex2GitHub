﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePlex2GitHub.Model
{
    public class WorkItemAttachment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public WorkItem WorkItem { get; set; }
    }
}
