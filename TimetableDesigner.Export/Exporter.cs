using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;

namespace TimetableDesigner.Export
{
    public abstract class Exporter
    {
        #region FIELDS

        protected Project _project;
        protected HashSet<Group> _groups;
        protected HashSet<Subgroup> _subgroups;
        protected HashSet<Teacher> _teachers;
        protected HashSet<Classroom> _classrooms;

        #endregion



        #region PROPERTIES

        public ICollection<Group> Groups => _groups;
        public ICollection<Subgroup> Subgroups => _subgroups;
        public ICollection<Teacher> Teachers => _teachers;
        public ICollection<Classroom> Classrooms => _classrooms;


        #endregion



        #region CONSTRUCTORS

        protected Exporter(Project project)
        {
            _project = project;
            _groups = new HashSet<Group>();
            _subgroups = new HashSet<Subgroup>();
            _teachers = new HashSet<Teacher>();
            _classrooms = new HashSet<Classroom>();
        }

        #endregion



        #region PUBLIC METHODS

        public abstract void Export(string path);

        #endregion
    }
}
