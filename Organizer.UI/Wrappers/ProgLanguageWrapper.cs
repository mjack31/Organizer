using Organizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.UI.Wrappers
{
    public class ProgLanguageWrapper : ValidationErrorsNotifier
    {
        public ProgLanguageWrapper(ProgramingLang model)
        {
            Model = model;
        }

        public ProgramingLang Model { get; private set; }

        public string Name
        {
            get { return Model.LanguageName; }
            set
            {
                Model.LanguageName = value;
                Validate(nameof(Name));
                OnProperyChanged(nameof(Name));
            }
        }

        protected override void Validate(string propName)
        {
            ClearErrors(propName);
            switch (propName)
            {
                case nameof(Name):
                    if (string.Equals(Name, "", StringComparison.OrdinalIgnoreCase))
                    {
                        AddError(nameof(Name), "To pole nie może być puste");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
