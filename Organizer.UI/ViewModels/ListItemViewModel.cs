using Organizer.UI.Events;
using Prism.Commands;
using Prism.Events;
using System;

namespace Organizer.UI.ViewModels
{
    public class ListItemViewModel : BaseViewModel
    {
        private IEventAggregator _eventAggregator;
        private string _viewModelName;
        private string _name;

        public ListItemViewModel(int id, string name, IEventAggregator eventAggregator, string viewModelName)
        {
            Id = id;
            Name = name;
            _eventAggregator = eventAggregator;

            // można było po prostu zrobić np. nameof(FriendDetailsViewModel) w tej klasie ale 
            // przekazywany jest do kontruktora bo klasa ListItemViemModel jest uniwersalna
            _viewModelName = viewModelName;

            SelectItemCommand = new DelegateCommand(OnSelectItemCommand);
        }

        // command wyboru danego frienda
        public DelegateCommand SelectItemCommand { get; }

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

        // Handler commanda wyboru frienda
        private void OnSelectItemCommand()
        {
            _eventAggregator.GetEvent<ListItemChosenEvent>().Publish(new ListItemChosenEventArgs { Id = Id, ViewModelName = _viewModelName });
        }
    }
}