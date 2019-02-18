using Organizer.UI.Events;
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
        private string _name;

        public DetailViewModelBase(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            // inicjalizacja property SaveCommand - konstruktor przyjmuje 2 delegaty/metody. Dobrą praktyką jest nazwyać pierwszą z "On" na początku bo to handler
            // a drugą z CanExecute na końcu ponieważ ona zezwala na odpalenie handlera
            SaveCommand = new DelegateCommand(OnSaveCommand, OnSaveCoommandCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteCommand);
            CloseTabCommand = new DelegateCommand(OnCloseTabCommand);
        }

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand DeleteCommand { get; }
        public DelegateCommand CloseTabCommand { get; }

        public abstract Task LoadDetailAsync(int? id);

        public string Name
        {
            get { return _name; }
            protected set
            {
                _name = value;
                OnProperyChanged(nameof(Name));
            }
        }


        public int Id { get; protected set; }

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                _hasChanges = value;
                OnProperyChanged(nameof(HasChanges));
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        protected abstract void OnCloseTabCommand();

        protected abstract void OnDeleteCommand();

        protected abstract bool OnSaveCoommandCanExecute();

        protected abstract void OnSaveCommand();
    }
}
