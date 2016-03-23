using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePlex2GitHub.Model
{
    public class ThreadPost
    {
        [Key]
        public int PostId { get; set; }
        public string Text { get; set; }
        public DateTime PostedDate { get; set; }
        public string PostedBy { get; set; } 
        public string MarkedAsAnswerBy { get; set; }
        public DateTime? MarkedAsAnswerDate { get; set; }
    }
}
