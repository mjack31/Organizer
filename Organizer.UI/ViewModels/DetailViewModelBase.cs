using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModels
{
    public abstract class DetailViewModelBase : ViewModelBase, IDetailsViewModel
    {
        private bool _hasChanges;
        protected IEventAggregator _eventAggregator;

        public DetailViewModelBase(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            // inicjalizacja property SaveCommand - konstruktor przyjmuje 2 delegaty/metody. Dobrą praktyką jest nazwyać pierwszą z "on" na początku bo to handler
            // a drugą z CanExecute na końcu ponieważ ona zezwala na odpalenie handlera - wyszaża przycisk
            SaveCommand = new DelegateCommand(OnSaveCommand, OnSaveCoommandCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteCommand);
        }

        // command przycisku zapisz zmiany
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand DeleteCommand { get; }

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                _hasChanges = value;
                // informacja o zmianie tego prop raczej nie potrzebna bo i tak jest odpalany event RaiseCanExecuteChanged
                OnProperyChanged(nameof(HasChanges));
                // event odpalany po to aby zaktualizowac przycisk save
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public abstract Task LoadDetailAsync(int? id);

        protected abstract void OnDeleteCommand();

        protected abstract bool OnSaveCoommandCanExecute();

        protected abstract void OnSaveCommand();
    }
}
