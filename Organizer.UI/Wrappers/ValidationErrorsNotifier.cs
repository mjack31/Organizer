﻿using Organizer.UI.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Organizer.UI.Wrappers
{
    public class ValidationErrorsNotifier : BaseViewModel, INotifyDataErrorInfo
    {
        private Dictionary<string, List<string>> _errorsByPropName = new Dictionary<string, List<string>>();

        public bool HasErrors => _errorsByPropName.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propName)
        {
            if(string.IsNullOrEmpty(propName)) {
                return null;
            }
            return _errorsByPropName[propName];
        }

        public void OnErrorsChanged(string propName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propName));
            OnProperyChanged(nameof(HasErrors));
        }

        protected void AddError(string propName, string error)
        {
            if (!_errorsByPropName.ContainsKey(propName))
            {
                _errorsByPropName.Add(propName, new List<string>());
            }
            if (!_errorsByPropName[propName].Contains(error))
            {
                _errorsByPropName[propName].Add(error);
            }
        }

        protected void ClearErrors(string propName)
        {
            //if (_errorsByPropName.ContainsKey(propName))
            //{
            //    _errorsByPropName[propName].Clear();
            //}
            // Potrzebne jest pełne czyśżczenie ponieważ HasError jest na true nawet gdy obiekt(property np FirstName) jest pusty
            _errorsByPropName.Clear();
        }
    }
}