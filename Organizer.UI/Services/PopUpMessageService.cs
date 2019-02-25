using System.Windows;

namespace Organizer.UI.Services
{
    public class PopUpMessageService : IMessageService
    {
        public bool ShowOKCancelMsg(string msg)
        {
            var result = MessageBox.Show(msg, "Question", MessageBoxButton.OKCancel);
            return result == MessageBoxResult.OK ? true : false;
        }

        public void ShowInfoMsg(string msg)
        {
            MessageBox.Show(msg, "Information", MessageBoxButton.OK);
        }
    }
}
