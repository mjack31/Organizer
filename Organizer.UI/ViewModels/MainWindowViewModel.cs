using Organizer.UI.Events;
using Organizer.UI.Services;
using Organizer.UI.ViewModels.Interfaces;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;
using Organizer.Models;

namespace Organizer.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private Func<IFriendDetailsViewModel> _friendDetailsViewModelCreator;
        private IMessageService _messageService;
        private Func<IMeetingDetailsViewModel> _meetingDetailsViewModelCreator;
        private Func<IProgLanguagesViewModel> _progLanguagesRepo;
        private IDetailsViewModel _detailsViewModel;
        private IDetailsViewModel _selectedViewModel;

        public MainWindowViewModel(IFriendsListViewModel friendsListViewModel, Func<IFriendDetailsViewModel> friendDetailsViewModelCreator, 
            IEventAggregator eventAggregator, IMessageService msgService, Func<IMeetingDetailsViewModel> meetingDetailsViewModelCreator,
            Func<IProgLanguagesViewModel> progLanguagesRepo)
        {
            FriendsListViewModel = friendsListViewModel;
            _eventAggregator = eventAggregator;
            _friendDetailsViewModelCreator = friendDetailsViewModelCreator;
            _messageService = msgService;

            _meetingDetailsViewModelCreator = meetingDetailsViewModelCreator;
            _progLanguagesRepo = progLanguagesRepo;

            _eventAggregator.GetEvent<OpenDetailViewEvent>().Subscribe(OnOpendDetailView);
            _eventAggregator.GetEvent<DetailDeletedEvent>().Subscribe(OnDetailDeletedEvent);
            _eventAggregator.GetEvent<TabClosedEvent>().Subscribe(OnTabClosedEvent);

            CreateNewFriendCommand = new DelegateCommand(OnCreateNewFriendCommand);
            CreateNewMeetingCommand = new DelegateCommand(OnCreateNewMeetingCommand);
            EditProgLanguagesCommand = new DelegateCommand(OnEditProgLanguagesCommand);

            DetailViewModels = new ObservableCollection<IDetailsViewModel>();
        }

        public DelegateCommand CreateNewFriendCommand { get; }
        public DelegateCommand CreateNewMeetingCommand { get; }
        public DelegateCommand EditProgLanguagesCommand { get; }

        public async Task LoadDataAsync()
        {
            await FriendsListViewModel.LoadDataAsync();
        }

        public IFriendsListViewModel FriendsListViewModel { get; }

        public ObservableCollection<IDetailsViewModel> DetailViewModels { get; }

        public IDetailsViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnProperyChanged(nameof(SelectedViewModel));
            }
        }

        public IDetailsViewModel DetailsViewModel
        {
            get { return _detailsViewModel; }
            set
            {
                _detailsViewModel = value;
                OnProperyChanged(nameof(DetailsViewModel));
            }
        }

        private async void OnOpendDetailView(OpenDetailViewEventArgs eventArgs)
        {
            if (!DetailViewModels.Any(f => f.Id == eventArgs.Id))
            {
                switch (eventArgs.ViewModelName)
                {
                    case nameof(FriendDetailsViewModel):
                        DetailsViewModel = _friendDetailsViewModelCreator();
                        await DetailsViewModel.LoadDetailAsync(eventArgs.Id);
                        DetailViewModels.Add(DetailsViewModel);
                        break;
                    case nameof(MeetingDetailsViewModel):
                        DetailsViewModel = _meetingDetailsViewModelCreator();
                        await DetailsViewModel.LoadDetailAsync(eventArgs.Id);
                        DetailViewModels.Add(DetailsViewModel);
                        break;
                    case nameof(ProgLanguagesViewModel):
                        DetailsViewModel = _progLanguagesRepo();
                        await DetailsViewModel.LoadDetailAsync(eventArgs.Id);
                        DetailViewModels.Add(DetailsViewModel);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                DetailsViewModel = DetailViewModels.FirstOrDefault(f => f.Id == eventArgs.Id);
            }

            SelectedViewModel = DetailsViewModel;
        }

        private void OnCreateNewFriendCommand()
        {
            OnOpendDetailView(new OpenDetailViewEventArgs { ViewModelName = nameof(FriendDetailsViewModel) });
        }

        private void OnDetailDeletedEvent(DetailDeletedEventArgs obj)
        {
            DetailsViewModel = null;
        }

        private void OnTabClosedEvent(TabClosedEventArgs args)
        {
            DetailViewModels.Remove(args.DetailViewModel);
        }

        private void OnCreateNewMeetingCommand()
        {
            OnOpendDetailView(new OpenDetailViewEventArgs { ViewModelName = nameof(MeetingDetailsViewModel) });
        }

        private void OnEditProgLanguagesCommand()
        {
            OnOpendDetailView(new OpenDetailViewEventArgs { ViewModelName = nameof(ProgLanguagesViewModel), Id=-99999 });
        }
    }
}
