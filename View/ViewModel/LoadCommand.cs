using System.Windows.Input;
using View.Model;
using View.Model.Services;

namespace View.ViewModel
{
    public class LoadCommand : ICommand
    {
        private readonly ContactSerializer _contactSerializer;
        private readonly Action<Contact> _onContactLoaded;

        public event EventHandler? CanExecuteChanged;

        public LoadCommand(ContactSerializer contactSerializer, Action<Contact> onContactLoaded)
        {
            _contactSerializer = contactSerializer;
            _onContactLoaded = onContactLoaded;
        }
        public bool CanExecute(object? parameter)
        {
           return true;
        }

        public void Execute(object? parameter)
        {
            var contact = _contactSerializer.LoadContact();

            if(contact != null)
            {
                _onContactLoaded?.Invoke(contact);
            }
        }
    }
}
