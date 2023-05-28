using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Properties;

namespace TimetableDesigner.Services.FileDialog
{
    public class FileDialogService : IFileDialogService
    {
        #region PUBLIC METHODS

        public string? OpenFile() => OpenFile(new Dictionary<string, IEnumerable<string>>(), true);
        public string? OpenFile(IDictionary<string, IEnumerable<string>> types, bool allowAllFiles = false)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            return BuildAndShowDialog(fileDialog, types, allowAllFiles);
        }

        public string? SaveFile() => SaveFile(new Dictionary<string, IEnumerable<string>>(), true);
        public string? SaveFile(IDictionary<string, IEnumerable<string>> types, bool allowAllFiles = false)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            return BuildAndShowDialog(fileDialog, types, allowAllFiles);
        }

        #endregion



        #region PRIVATE METHODS

        private static string? BuildAndShowDialog(Microsoft.Win32.FileDialog fileDialog, IDictionary<string, IEnumerable<string>> types, bool allowAllFiles)
        {
            fileDialog.Filter = BuildFilterString(types, allowAllFiles);

            if (fileDialog.ShowDialog() == true)
            {
                return fileDialog.FileName;
            }
            else
            {
                return null;
            }
        }

        private static string BuildFilterString(IDictionary<string, IEnumerable<string>> types, bool allFiles)
        {
            List<string> typeStrings = new List<string>();
            foreach (KeyValuePair<string, IEnumerable<string>> type in types) 
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{type.Key}|");

                List<string> extensionStrings = new List<string>();
                foreach (string extension in type.Value)
                {
                    extensionStrings.Add($"*.{extension.ToLower()}");
                }

                sb.Append(string.Join(';', extensionStrings));

                typeStrings.Add(sb.ToString());
            }

            if (allFiles)
            {
                typeStrings.Add($"{Resources.Global_AllFiles}|*.*");
            }

            return string.Join(" | ", typeStrings);
        }

        #endregion
    }
}
