using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Core
{
    [Serializable]
    public class Project
    {
        #region PROPERTIES

        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public TimetableTemplate TimetableTemplate { get; set; }
        public ICollection<Classroom> Classrooms { get; set; }
        public ICollection<Teacher> Teachers { get; set; }
        public ICollection<Group> Groups { get; set; }
        public ICollection<Subgroup> Subgroups { get; set; }
        public ICollection<Class> Classes { get; set; }

        #endregion



        #region CONSTRUCTORS

        public Project() 
        {
            Name = string.Empty;
            Author = string.Empty;
            Description = string.Empty;
            TimetableTemplate = new TimetableTemplate();
            Classrooms = new HashSet<Classroom>();
            Teachers = new HashSet<Teacher>();
            Groups = new HashSet<Group>();
            Subgroups = new HashSet<Subgroup>();
        }

        #endregion
    }
}
