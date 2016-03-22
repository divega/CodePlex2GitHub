using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CodePlex2GitHub.Model
{
    public class FileAttachmentContent
    {
        [Key]
        public int FileAttachmentId { get; set; }
        public byte[] Content { get; set; }
    }
}
