using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using Organizer.Models;
using Organizer.UI.Data;
using Organizer.UI.Events;
using Organizer.UI.Services;
using Organizer.UI.Wrappers;
using Prism.Events;
using Prism.Commands;

namespace Organizer.UI.ViewModels
{
    public class ProgLanguagesViewModel : DetailViewModelBase, IProgLanguagesViewModel
    {
        private IProgLanguagesRepository<ProgramingLang> _progLanguagesRepository;
        private IMessageService _msgService;
        private ProgLanguageWrapper _selectedLanguage;

        public ProgLanguagesViewModel(IEventAggregator eventAggregator, IProgLanguagesRepository<ProgramingLang> progLanguagesRepository,
            IMessageService msgService) : base(eventAggregator)
        {
            Name = "Edit programming languages";
            Id = -99999;
            ProgrammingLanguages = new ObservableCollection<ProgLanguageWrapper>();

            _progLanguagesRepository = progLanguagesRepository;
            _msgService = msgService;

            AddLanguageCommand = new DelegateCommand(OnAddLanguageCommand);
            DeleteLanguageCommand = new DelegateCommand(OnDeleteLanguageCommand, OnDeleteLanguageCommandCanExecute);
        }

        public DelegateCommand AddLanguageCommand { get; }
        public DelegateCommand DeleteLanguageCommand { get; }

        public ObservableCollection<ProgLanguageWrapper> ProgrammingLanguages { get; }

        public ProgLanguageWrapper SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _selectedLanguage = value;
                OnProperyChanged(nameof(SelectedLanguage));
                DeleteLanguageCommand.RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadDetailAsync(int? id)
        {
            ProgrammingLanguages.Clear();
            var languages = await _progLanguagesRepository.GetAll();
            foreach (var lang in languages)
            {
                var wrappedLanguage = new ProgLanguageWrapper(lang);
                wrappedLanguage.PropertyChanged += WrappedLanguage_PropertyChanged;
                ProgrammingLanguages.Add(wrappedLanguage);
            }
        }

        private void WrappedLanguage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
            HasChanges = _progLanguagesRepository.HasChanges();
        }

        protected override void OnCloseTabCommand()
        {
            if (HasChanges)
            {
                var result = _msgService.ShowOKCancelMsg("Do you want to close a tab without save?");
                if (!result)
                {
                    return;
                }
            }
            _eventAggregator.GetEvent<TabClosedEvent>().Publish(new TabClosedEventArgs { DetailViewModel = this });
        }

        protected override void OnDeleteCommand()
        {
            throw new NotImplementedException();
        }

        protected async override void OnSaveCommand()
        {
            await _progLanguagesRepository.SaveAsync();
            SaveCommand.RaiseCanExecuteChanged();
            HasChanges = _progLanguagesRepository.HasChanges();
        }

        protected override bool OnSaveCoommandCanExecute()
        {
            return HasChanges && !ProgrammingLanguages.Any(f => f.HasErrors);
        }

        private bool OnDeleteLanguageCommandCanExecute()
        {
            return SelectedLanguage != null;
        }

        private void OnDeleteLanguageCommand()
        {
            _progLanguagesRepository.Delete(SelectedLanguage.Model);
            ProgrammingLanguages.Remove(SelectedLanguage);

            SelectedLanguage = null;
            SaveCommand.RaiseCanExecuteChanged();
            DeleteLanguageCommand.RaiseCanExecuteChanged();
            HasChanges = _progLanguagesRepository.HasChanges();
        }

        private void OnAddLanguageCommand()
        {
        }
    }
}
