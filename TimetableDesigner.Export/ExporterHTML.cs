using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;

namespace TimetableDesigner.Export
{
    public class ExporterHTML : Exporter
    {
        #region CONSTRUCTORS

        public ExporterHTML(Project project) : base(project) { }

        #endregion



        #region PUBLIC METHODS

        public override void Export(string path) => Export(path, null);
        public void Export(string path, string? css)
        {
            string content = BuildDocument(css);

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(content);
            }
        }

        #endregion



        #region PRIVATE METHODS

        private string BuildDocument(string? css)
        {
            if (css == null)
            {
                StringBuilder cssBuilder = new StringBuilder();
                cssBuilder.AppendLine("h1 {text-align: center;}");
                cssBuilder.AppendLine("table, td, th { border: 1px solid black; border-collapse: collapse; }");
                cssBuilder.AppendLine("td, th { padding: 10px; }");
                css = cssBuilder.ToString();
            } 

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<html>");
            builder.AppendLine("<head>");
            builder.AppendLine("<style>");
            builder.AppendLine(css);
            builder.AppendLine("</style>");
            builder.AppendLine("</head>");
            builder.AppendLine("<body>");

            if (Groups.Count > 0)
            {
                builder.AppendLine("<h1>Groups</h1>");

                foreach (Group group in Groups)
                {
                    builder.AppendLine("<h2>");
                    builder.AppendLine(group.Name);
                    builder.AppendLine("</h2>");
                    builder.AppendLine(BuildTable(group));
                }
            }

            if (Subgroups.Count > 0)
            {
                builder.AppendLine("<h1>Subgroups</h1>");

                foreach (Subgroup subgroup in Subgroups)
                {
                    builder.AppendLine("<h2>");
                    builder.AppendLine(subgroup.Name);
                    builder.AppendLine("</h2>");
                    builder.AppendLine(BuildTable(subgroup));
                }
            }

            if (Teachers.Count > 0)
            {
                builder.AppendLine("<h1>Teachers</h1>");

                foreach (Teacher teacher in Teachers)
                {
                    builder.AppendLine("<h2>");
                    builder.AppendLine(teacher.Name);
                    builder.AppendLine("</h2>");
                    builder.AppendLine(BuildTable(teacher));
                }
            }

            if (Classrooms.Count > 0)
            {
                builder.AppendLine("<h1>Classrooms</h1>");

                foreach (Classroom classroom in Classrooms)
                {
                    builder.AppendLine("<h2>");
                    builder.AppendLine(classroom.Name);
                    builder.AppendLine("</h2>");
                    builder.AppendLine(BuildTable(classroom));
                }
            }

            builder.AppendLine("</body>");
            builder.AppendLine("</html>");

            return builder.ToString();
        }

        private string BuildTable(Unit unit)
        {
            IEnumerable<Class> classes = _project.Classes.Where(c => c.Classroom == unit || c.Teacher == unit || c.Group == unit || (unit is Group g && c.Group is Subgroup s && g.AssignedSubgroups.Contains(s)))
                                                         .Where(c => c.Slot is not null && c.Day is not null);
            IEnumerable<TimetableDay> days = _project.TimetableTemplate.Days;
            IEnumerable<TimetableSpan> slots = _project.TimetableTemplate.Slots;

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<table>");
            builder.AppendLine("<tr>");
            builder.AppendLine("<th></th>");
            foreach (TimetableDay day in days)
            {
                builder.AppendLine($"<th>{day.Name}</th>");
            }
            builder.AppendLine("<tr>");
            foreach (TimetableSpan slot in slots)
            {
                builder.AppendLine("<tr>");
                builder.AppendLine($"<th>{slot}</th>");
                foreach (TimetableDay day in days)
                {
                    IEnumerable<Class> slotClasses = classes.Where(x => x.Day == day && x.Slot == slot);

                    builder.AppendLine("<td>");
                    foreach (Class slotClass in slotClasses)
                    {
                        string group = slotClass.Group is null ? "none" : slotClass.Group.Name;
                        string teacher = slotClass.Teacher is null ? "none" : slotClass.Teacher.Name;
                        string classroom = slotClass.Classroom is null ? "none" : slotClass.Classroom.Name;
                        builder.AppendLine($"<p title=\"Group: {group}\nTeacher: {teacher}\nClassroom: {classroom}\">");
                        builder.AppendLine(slotClass.Name);
                        builder.AppendLine("</p>");
                    }
                    builder.AppendLine("</td>");
                }
                builder.AppendLine("</tr>");
            }
            builder.AppendLine("</table>");

            return builder.ToString();

        }

        #endregion
    }
}
