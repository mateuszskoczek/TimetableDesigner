using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Services.FileDialog
{
    public interface IFileDialogService : IService
    {
        string? OpenFile();
        string? OpenFile(IDictionary<string, IEnumerable<string>> types, bool allowAllFiles = false);

        string? SaveFile();
        string? SaveFile(IDictionary<string, IEnumerable<string>> types, bool allowAllFiles = false);
    }
}
