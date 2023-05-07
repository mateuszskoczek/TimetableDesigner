using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Services.MessageBox
{
    public interface IMessageBoxService : IService
    {
        #region METHODS

        void ShowError(string message);
        void ShowError(string message, string title);

        MessageBoxQuestionResult ShowQuestion(string message, bool hideCancelButton = false);
        MessageBoxQuestionResult ShowQuestion(string message, string title, bool hideCancelButton = false);

        void ShowWarning(string message);
        void ShowWarning(string message, string title);

        void ShowInformation(string message);
        void ShowInformation(string message, string title);

        #endregion
    }
}
