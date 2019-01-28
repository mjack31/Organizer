using Organizer.Models;
using Organizer.UI.Data.Interfaces;
using Organizer.UI.Events;
using Organizer.UI.Services;
using Organizer.UI.Wrappers;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModels
{
    public class MeetingDetailsViewModel : DetailViewModelBase, IMeetingDetailsViewModel
    {
        private IMessageService _messageService;
        private IMeetingsRepository<Meeting> _meetingsRepo;
        private MeetingWrapper _meeting;

        public MeetingDetailsViewModel(IEventAggregator eventAggregator, IMessageService msgService, IMeetingsRepository<Meeting> meetingsRepo) : base(eventAggregator)
        {
            _messageService = msgService;
            _meetingsRepo = meetingsRepo;
        }

        public MeetingWrapper Meeting
        {
            get { return _meeting; }
            set
            {
                _meeting = value;
                OnProperyChanged(nameof(Meeting));
            }
        }

        public async override Task LoadDetailAsync(int? id)
        {
            if (id.HasValue)
            {
                // normalnie metoda GetAsync nie przyjmuje nullable, ale wystarczy przekazać value id i działa
                var meeting = await _meetingsRepo.GetAsync(id.Value);
                Meeting = new MeetingWrapper(meeting);
            }
            else
            {
                // tworzenie nowego spotkania
                var meeting = CreateNewMeeting();
                Meeting = new MeetingWrapper(meeting);
            }

            Meeting.PropertyChanged += (s, e) =>
            {
                // przy każdej zmianie propery w wrapperze odpalenie eventu
                SaveCommand.RaiseCanExecuteChanged();
                // sprawdzanie kontekstu db czy są zmiany
                HasChanges = _meetingsRepo.HasChanges();
            };

            // sztuczka aby zaraz po naciśnięciu przycisku utworzenia nowego frienda odświerzył się widok formularza i pojawiły się wskazówki walidacyjne
            if (!id.HasValue)
            {
                Meeting.Title = "";
            }

            // sprawdzenie czy można zapisać zmiany
            SaveCommand.RaiseCanExecuteChanged();
        }

        private Meeting CreateNewMeeting()
        {
            // stworzenie nowego pustego spotkania i przekazanie do do kontekstu db
            var meeting = new Meeting();
            return _meetingsRepo.Add(meeting);
        }

        protected override void OnDeleteCommand()
        {
            var result = _messageService.ShowOKCancelMsg("Do you want to delete a meeting?");
            if (!result)
            {
                return;
            }
            _meetingsRepo.Delete(Meeting.Model);
            _eventAggregator.GetEvent<DetailDeletedEvent>().Publish(new DetailDeletedEventArgs { Id = Meeting.Id, ViewModelName = nameof(Meeting) });
        }

        protected async override void OnSaveCommand()
        {
            await _meetingsRepo.SaveAsync();
            _eventAggregator.GetEvent<DetailChangesSavedEvent>().Publish(new DetailChangesSavedEventArgs
            {
                Id = Meeting.Id,
                Name = Meeting.Title,
                ViewModelName = nameof(Meeting)
            });
            // wylączenie przycisku Save po zapisaniu poprzes sprawdzenie zmian w kontekscie
            HasChanges = _meetingsRepo.HasChanges();
        }

        protected override bool OnSaveCoommandCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanges;
        }
    }
}
