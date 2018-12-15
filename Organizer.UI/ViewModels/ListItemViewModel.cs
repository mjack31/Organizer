using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModels
{
    public class ListItemViewModel : BaseViewModel
    {
        public ListItemViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                var propName = nameof(Name);
                OnProperyChanged(propName);
            }
        }
    }
}