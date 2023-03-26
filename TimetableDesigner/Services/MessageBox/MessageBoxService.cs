using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using TimetableDesigner.Properties;

namespace TimetableDesigner.Services.MessageBox
{
    public class MessageBoxService : IMessageBoxService
    {
        #region PUBLIC METHODS

        public void ShowError(string message) => ShowError(message, Resources.MessageBox_Error);
        public void ShowError(string message, string title) => System.Windows.Forms.MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);

        public MessageBoxQuestionResult ShowQuestion(string message, bool hideCancelButton = false) => ShowQuestion(message, Resources.MessageBox_Question, hideCancelButton);
        public MessageBoxQuestionResult ShowQuestion(string message, string title, bool hideCancelButton = false)
        {
            MessageBoxButton buttons = MessageBoxButton.YesNoCancel;
            if (hideCancelButton)
            {
                buttons = MessageBoxButton.YesNo;
            }

            MessageBoxResult result = System.Windows.MessageBox.Show(message, title, buttons, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes: return MessageBoxQuestionResult.Yes;
                case MessageBoxResult.No: return MessageBoxQuestionResult.No;
                default: return MessageBoxQuestionResult.Cancel;
            }
        }

        #endregion
    }
}
