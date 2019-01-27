using Organizer.Models;
using System.Text.RegularExpressions;

namespace Organizer.UI.Wrappers
{
    public class PhoneNumberWrapper : ValidationErrorsNotifier
    {
        public PhoneNumberWrapper(PhoneNumber model)
        {
            Model = model;
        }

        public PhoneNumber Model { get; private set; }

        public string Number
        {
            get { return Model.Number; }
            set
            {
                Model.Number = value;
                Validate(nameof(Number));
                OnProperyChanged(nameof(Number));
            }
        }

        protected override void Validate(string propName)
        {
            ClearErrors(propName);
            switch (propName)
            {
                case nameof(Number):
                    var r = new Regex(@"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{3})$");
                    if (!r.IsMatch(Number))
                    {
                        AddError(nameof(Number), "Numer ma nieprawidłowy format");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
