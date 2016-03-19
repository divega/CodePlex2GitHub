using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePlex2GitHub.Model
{
    public class Thread
    {
        [Key]
        public int ThreadId { get; set; }
        public string Title { get; set; }
        public ThreadTag Tag { get; set; }
        public ICollection<ThreadPost> Posts  { get; set; }
    }
}
