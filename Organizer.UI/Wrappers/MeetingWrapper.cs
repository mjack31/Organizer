using Organizer.Models;
using System;
using System.Collections.Generic;

namespace Organizer.UI.Wrappers
{
    public class MeetingWrapper : ValidationErrorsNotifier
    {
        public Meeting Model;

        public int Id
        {
            get { return Model.Id; }
        }


        public MeetingWrapper(Meeting model)
        {
            Model = model;
        }

        public string Title
        {
            get { return Model.Title; }
            set
            {
                Model.Title = value;
                Validate(nameof(Title));
                OnProperyChanged(nameof(Title));
            }
        }

        public DateTime FromDate
        {
            get { return Model.FromDate; }
            set
            {
                Model.FromDate = value;
                Validate(nameof(FromDate));
                OnProperyChanged(nameof(FromDate));
            }
        }

        public DateTime ToDate
        {
            get { return Model.ToDate; }
            set
            {
                Model.ToDate = value;
                Validate(nameof(ToDate));
                OnProperyChanged(nameof(ToDate));
            }
        }

        public List<Friend> Friends
        {
            get { return Model.Friends; }
            set
            {
                Model.Friends = value;
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
