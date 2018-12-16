using Organizer.Models;
using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.UI.Wrappers
{
    public class FriendWrapper : ValidationErrorsNotifier
    {
        public FriendWrapper(Friend model)
        {
            Model = model;
        }

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
            }
        }

        public string Email
        {
            get { return Model.Email; }
            set
            {
                Model.Email = value;
            }
        }

        public Friend Model { get; private set; }

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
                    break;
                case nameof(Email):
                    break;
                default:
                    break;
            }
        }
    }
}
