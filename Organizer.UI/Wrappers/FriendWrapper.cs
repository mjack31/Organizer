using Organizer.Models;
using System;
using System.Collections.Generic;

namespace Organizer.UI.Wrappers
{
    public class FriendWrapper : ValidationErrorsNotifier
    {
        public FriendWrapper(Friend model)
        {
            Model = model;
        }

        public Friend Model { get; private set; }

        public int Id
        {
            get { return Model.Id; }
        }

        public string FirstName
        {
            get { return Model.FirstName; }
            set
            {
                Model.FirstName = value;
                Validate(nameof(FirstName));
                OnProperyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get { return Model.LastName; }
            set
            {
                Model.LastName = value;
                Validate(nameof(LastName));
                OnProperyChanged(nameof(LastName));
            }
        }

        public string Email
        {
            get { return Model.Email; }
            set {
                Model.Email = value;
                Validate(nameof(Email));
                OnProperyChanged(nameof(Email));
            }

        }

        public int? FavoriteProgLangId
        {
            get { return  Model.FavoriteLanguageId; }
            set
            {
                Model.FavoriteLanguageId = value;
                Validate(nameof(FavoriteProgLangId));
                OnProperyChanged(nameof(FavoriteProgLangId));
            }
        }

        public List<PhoneNumber> PhoneNumbers
        {
            get { return Model.PhoneNumbers; }
            set
            {
                Model.PhoneNumbers = value;
            }
        }


        private void Validate(string propName)
        {
            ClearErrors(propName);
            switch (propName)
            {
                case nameof(FirstName):
                    if (string.Equals(FirstName, "", StringComparison.OrdinalIgnoreCase))
                    {
                        AddError(nameof(FirstName), "To pole nie może być puste");
                    }
                    break;
                case nameof(LastName):
                    if (string.Equals(LastName, "", StringComparison.OrdinalIgnoreCase))
                    {
                        AddError(nameof(LastName), "To pole nie może być puste");
                    }
                    break;
                case nameof(Email):
                    break;
                default:
                    break;
            }
        }
    }
}
