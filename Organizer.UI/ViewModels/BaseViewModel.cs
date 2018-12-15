using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnProperyChanged(string propName)
        {
            // TODO - Dodać sprawdzanie czy PropertyChanged jest nullem, ale narazie sprwdzić co się stanie gdy będzie bez tego. Edit: Po dodaniu OnProperyChanged do ListViewModel wywala tutaj NullExeptiona. CZEMU?!
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        //Można to zrobić tak:
        //protected virtual void OnProperyChanged([CallerMemberName]string propName = null)
        //{
        //    PropertyChanged(this, new PropertyChangedEventArgs(propName));
        //}
        //Wtedy nazwa wpisuje się automatycznie
    }
}
