using Organizer.UI.Events;
using Organizer.UI.Services;
using Prism.Commands;
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
        private IFriendDetailsViewModel _friendDetailsViewModel;

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
            _eventAggregator.GetEvent<FriendDeletedEvent>().Subscribe(OnFriendDeletedEvent);

            CreateNewFriendCommand = new DelegateCommand(OnCreateNewFriendCommand);
        }

        private void OnFriendDeletedEvent(int id)
        {
            FriendDetailsViewModel = null;
        }

        private void OnCreateNewFriendCommand()
        {
            OnListItemChosen(null);
        }

        public async Task LoadDataAsync()
        {
            await FriendsListViewModel.LoadDataAsync();
        }

        public IFriendsListViewModel FriendsListViewModel { get; }
        
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

        // event handler wybrania danego Friendsa lub tworzenia nowego(jeżeli przekazany jest null)
        private async void OnListItemChosen(int? id)
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

        public DelegateCommand CreateNewFriendCommand { get; }
    }
}
