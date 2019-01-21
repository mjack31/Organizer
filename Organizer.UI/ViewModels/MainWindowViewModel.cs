using Organizer.UI.Events;
using Organizer.UI.Services;
using Organizer.UI.ViewModels.Interfaces;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private Func<IFriendDetailsViewModel> _friendDetailsViewModelCreator;
        private IMessageService _messageService;
        private IDetailsViewModel _detailsViewModel;

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
            _eventAggregator.GetEvent<DetailDeletedEvent>().Subscribe(OnDetailDeletedEvent);

            CreateNewFriendCommand = new DelegateCommand(OnCreateNewFriendCommand);
        }

        public DelegateCommand CreateNewFriendCommand { get; }

        public async Task LoadDataAsync()
        {
            await FriendsListViewModel.LoadDataAsync();
        }

        public IFriendsListViewModel FriendsListViewModel { get; }
        
        // Uniwersalna zmienna przechowująca view modele friendsów i spotkan
        public IDetailsViewModel DetailsViewModel
        {
            get { return _detailsViewModel; }
            set
            {
                _detailsViewModel = value;
                // odpalić trzeba OnPropertyChanged ponieważ w widoku podpięty do tej zmiennej jest DataContext
                OnProperyChanged(nameof(DetailsViewModel));
            }
        }

        // event handler wybrania danego Friendsa lub tworzenia nowego(jeżeli przekazany jest null)
        private async void OnListItemChosen(ListItemChosenEventArgs eventArgs)
        {
            if(DetailsViewModel != null && DetailsViewModel.HasChanges)
            {
                var result = _messageService.ShowOKCancelMsg("This friend has changes. Do you want to change a friend?");
                if (!result)
                {
                    return;
                }
            }
            // stworzenie viewModelu - mimo ze DetailsViewModel jest typu IDetailsViewModel (rzutowanie klasy na interfejs) to przechowuje 
            // ona instancję FriendDetailsViewModel i dzięki temu dobrze się to wyświetla w ContenContro MainWindow
            DetailsViewModel = _friendDetailsViewModelCreator();
            await DetailsViewModel.LoadDetailAsync(eventArgs.Id);
        }

        private void OnCreateNewFriendCommand()
        {
            OnListItemChosen(new ListItemChosenEventArgs { ViewModelName = nameof(FriendDetailsViewModel) });
        }

        private void OnDetailDeletedEvent(DetailDeletedEventArgs obj)
        {
            DetailsViewModel = null;
        }
    }
}
