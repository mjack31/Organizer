using Organizer.Models;
using Organizer.UI.Data.Interfaces;
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
    public class MeetingDetailsViewModel : DetailViewModelBase
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

        public override Task LoadDetailAsync(int? id)
        {
            throw new NotImplementedException();
        }

        protected override void OnDeleteCommand()
        {
            throw new NotImplementedException();
        }

        protected override void OnSaveCommand()
        {
            throw new NotImplementedException();
        }

        protected override bool OnSaveCoommandCanExecute()
        {
            throw new NotImplementedException();
        }
    }
}
