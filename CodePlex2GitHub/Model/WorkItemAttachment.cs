using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodePlex2GitHub.Model
{
    public class WorkItemAttachment
    {
        [Key]
        public int WorkItemId { get; set; }
        [Key]
        [ForeignKey("FileAttachmentId")]
        public FileAttachment File { get; set; }
    }
}
