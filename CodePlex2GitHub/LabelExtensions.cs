using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePlex2GitHub
{
    public static class LableExtensions
    {
        public static HashSet<string> AddIf(this HashSet<string> source, bool condition, string labelToAdd)
        {
            if (condition)
            {
                source.Add(labelToAdd);
            }
            return source;
        }
    }
}
