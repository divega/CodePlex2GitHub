using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePlex2GitHub.Model
{
    public class Release
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        [Column("DevelopmentStatusId")]
        public DevelopmentStatus? DevelopmentStatus { get; set; }
    }

    public enum DevelopmentStatus
    {
        Planning = 1,
        Alpha = 2,
        Beta = 3,
        Stable = 4
    }
}
