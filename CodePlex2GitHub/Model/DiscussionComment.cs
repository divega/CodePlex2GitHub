using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePlex2GitHub.Model
{
    public class DiscussionComment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime PostedOn { get; set; }
        public bool IsAnswer { get; set; }
    }
}
