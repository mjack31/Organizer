using Organizer.Models;
using System;
using System.Collections.Generic;

namespace Organizer.UI.Wrappers
{
    public class MeetingWrapper : ValidationErrorsNotifier
    {
        private Meeting _model;

        public MeetingWrapper(Meeting model)
        {
            _model = model;
        }

        public string Title
        {
            get { return _model.Title; }
            set
            {
                _model.Title = value;
                Validate(nameof(Title));
                OnProperyChanged(nameof(Title));
            }
        }

        public DateTime FromDate
        {
            get { return _model.FromDate; }
            set
            {
                _model.FromDate = value;
                Validate(nameof(FromDate));
                OnProperyChanged(nameof(FromDate));
            }
        }

        public DateTime ToDate
        {
            get { return _model.ToDate; }
            set
            {
                _model.ToDate = value;
                Validate(nameof(ToDate));
                OnProperyChanged(nameof(ToDate));
            }
        }

        public List<Friend> Friends
        {
            get { return _model.Friends; }
            set
            {
                _model.Friends = value;
                Validate(nameof(Friends));
                OnProperyChanged(nameof(Friends));
            }
        }

        protected override void Validate(string propName)
        {
            ClearErrors(propName);
            switch (propName)
            {
                case nameof(Title):
                    if (string.Equals(Title, "", StringComparison.OrdinalIgnoreCase))
                    {
                        AddError(nameof(Title), "To pole nie może być puste");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
