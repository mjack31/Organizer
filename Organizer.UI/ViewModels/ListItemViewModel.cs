using Organizer.UI.Events;
using Prism.Commands;
using Prism.Events;
using System;

namespace Organizer.UI.ViewModels
{
    public class ListItemViewModel : BaseViewModel
    {
        private IEventAggregator _eventAggregator;
        private string _name;

        public ListItemViewModel(int id, string name, IEventAggregator eventAggregator)
        {
            Id = id;
            Name = name;
            _eventAggregator = eventAggregator;

            SelectItemCommand = new DelegateCommand(OnSelectItemCommand);
        }

        // Handler commanda wyboru frienda
        private void OnSelectItemCommand()
        {
            _eventAggregator.GetEvent<ListItemChosenEvent>().Publish(Id);
        }

        public int Id { get; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnProperyChanged(nameof(Name));
            }
        }

        // command wyboru danego frienda
        public DelegateCommand SelectItemCommand { get; }
    }
}