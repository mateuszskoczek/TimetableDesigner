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
        #region FIELDS

        private Guid _guid;
        private string _name;
        private string _author;
        private string _description;
        private TimetableTemplate _timetableTemplate;
        private HashSet<Classroom> _classrooms;
        private HashSet<Teacher> _teachers;
        private HashSet<Group> _groups;
        private HashSet<Subgroup> _subgroups;
        private HashSet<Class> _classes;

        #endregion



        #region PROPERTIES

        public Guid Guid => _guid;
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public string Author
        {
            get => _author;
            set => _author = value;
        }
        public string Description
        {
            get => _description;
            set => _description = value;
        }
        public TimetableTemplate TimetableTemplate => _timetableTemplate;
        public ICollection<Classroom> Classrooms => _classrooms;
        public ICollection<Teacher> Teachers => _teachers;
        public ICollection<Group> Groups => _groups;
        public ICollection<Subgroup> Subgroups => _subgroups;
        public ICollection<Class> Classes => _classes;

        #endregion



        #region CONSTRUCTORS

        public Project() 
        {
            _guid = Guid.NewGuid();
            _name = string.Empty;
            _author = string.Empty;
            _description = string.Empty;
            _timetableTemplate = new TimetableTemplate();
            _classrooms = new HashSet<Classroom>();
            _teachers = new HashSet<Teacher>();
            _groups = new HashSet<Group>();
            _subgroups = new HashSet<Subgroup>();
            _classes = new HashSet<Class>();
        }

        #endregion
    }
}
