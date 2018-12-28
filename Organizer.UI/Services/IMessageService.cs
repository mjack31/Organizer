using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Organizer.UI.Services
{
    public interface IMessageService
    {
        bool ShowOKCancelMsg(string msg);
    }
}
