using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace CodePlex2GitHub.Model
{
    public class FileAttachment
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public FileAttachmentContent Content { get; set; }
    }
}
