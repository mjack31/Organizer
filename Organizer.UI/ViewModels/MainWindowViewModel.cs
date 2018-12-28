using Organizer.UI.Events;
using Organizer.UI.Services;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Organizer.UI.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private IEventAggregator _eventAggregator;
        private Func<IFriendDetailsViewModel> _friendDetailsViewModelCreator;
        private IMessageService _messageService;

        public MainWindowViewModel(IFriendsListViewModel friendsListViewModel, Func<IFriendDetailsViewModel> friendDetailsViewModelCreator, 
            IEventAggregator eventAggregator, IMessageService msgService)
        {
            FriendsListViewModel = friendsListViewModel;
            _eventAggregator = eventAggregator;
            _friendDetailsViewModelCreator = friendDetailsViewModelCreator;
            _messageService = msgService;

            // dodanie subskrybenta (handlera) do eventu EventAggregatora Prism'a. Metoda Subscribe przyjmuje Action<T>
            // ale można zamiast delegaty dodawać normalnie metody - ułatwienie
            // przeniesiony do MainWindow aby można było tworzyć nowy ViewModel dla każdego friendsa, a tym samym nowy dbContext dla każdego friendsa
            _eventAggregator.GetEvent<ListItemChosenEvent>().Subscribe(OnListItemChosen);
        }

        public async Task LoadDataAsync()
        {
            await FriendsListViewModel.LoadDataAsync();
        }

        public IFriendsListViewModel FriendsListViewModel { get; }
        private IFriendDetailsViewModel _friendDetailsViewModel;

        public IFriendDetailsViewModel FriendDetailsViewModel
        {
            get { return _friendDetailsViewModel; }
            set
            {
                _friendDetailsViewModel = value;
                // odpalić trzeba OnPropertyChanged ponieważ w widoku podpięty do tej zmiennej jest DataContext
                OnProperyChanged(nameof(FriendDetailsViewModel));
            }
        }

        // event handler wybrania danego Friendsa
        private async void OnListItemChosen(int id)
        {
            if(FriendDetailsViewModel != null && FriendDetailsViewModel.HasChanges)
            {
                var result = _messageService.ShowOKCancelMsg("This friend has changes. Do you want to change a friend?");
                if (!result)
                {
                    return;
                }
            }
            FriendDetailsViewModel = _friendDetailsViewModelCreator();
            await FriendDetailsViewModel.LoadFriendAsync(id);
        }
    }
}
