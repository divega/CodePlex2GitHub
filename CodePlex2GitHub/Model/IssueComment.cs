using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace CodePlex2GitHub.Model
{
    public class IssueComment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime PostedOn { get; set; }
        public Person PosteByPerson { get; set; }
    }
}
