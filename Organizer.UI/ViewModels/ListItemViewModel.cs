using Organizer.UI.Events;
using Prism.Commands;
using Prism.Events;

namespace Organizer.UI.ViewModels
{
    public class ListItemViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private string _viewModelName;
        private string _name;

        public ListItemViewModel(int id, string name, IEventAggregator eventAggregator, string viewModelName)
        {
            Id = id;
            Name = name;
            _eventAggregator = eventAggregator;
            _viewModelName = viewModelName;

            SelectItemCommand = new DelegateCommand(OnSelectItemCommand);
        }

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

        private void OnSelectItemCommand()
        {
            _eventAggregator.GetEvent<ListItemChosenEvent>().Publish(new ListItemChosenEventArgs { Id = Id, ViewModelName = _viewModelName });
        }
    }
}