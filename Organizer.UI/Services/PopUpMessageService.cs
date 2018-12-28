using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
