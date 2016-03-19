using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePlex2GitHub.Model
{
    public class WorkItemReasonClosed
    {
        [Key]
        public string Name { get; set; }
        public string GitHubName { get; set; }
    }
}
