using System.Windows.Input;
using View.Model;
using View.Model.Services;

namespace View.ViewModel
{
    public class SaveCommand : ICommand
    {
        private readonly ContactSerializer _contactSerializer;
        private readonly Func<Contact> _onContactSaved;

        public SaveCommand(ContactSerializer contactSerializer, Func<Contact> onContactSaved)
        {
            _contactSerializer = contactSerializer;
            _onContactSaved = onContactSaved;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            var contact = _onContactSaved();

            if (contact != null)
            {
                _contactSerializer.SaveContact(contact);
            }
        }
    }
}
