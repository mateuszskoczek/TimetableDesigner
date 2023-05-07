using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Globals
{
    public static class Path
    {
        public static readonly string ApplicationData = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\TimetableDesigner";
        public static readonly string RecentProjectsFile = $"{ApplicationData}\\recent";
    }
}
