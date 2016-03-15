using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePlex2GitHub.Model
{
    public class Release
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsReleased { get; set; }

    }
}
