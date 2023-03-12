using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimetableDesigner.MessageBox.Properties;

namespace TimetableDesigner.MessageBox
{
    public static class MessageBoxService
    {
        #region PUBLIC METHODS

        public static void ShowError(string message) => System.Windows.MessageBox.Show(message, Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);

        public static MessageBoxYesNoCancelResult ShowYesNoCancelQuestion(string message)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show(message, Resources.Error, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes: return MessageBoxYesNoCancelResult.Yes;
                case MessageBoxResult.No: return MessageBoxYesNoCancelResult.No;
                case MessageBoxResult.Cancel: return MessageBoxYesNoCancelResult.Cancel;
                default: return MessageBoxYesNoCancelResult.None;
            }
        }

        #endregion

    }
}
