using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePlex2GitHub.Model
{
    public class Discussion
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public DiscussionTopic Topic { get; set; }
    }
}
