using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.Model;
using View.Model.Services;

namespace View.ViewModel
{
    public class MainVM : INotifyPropertyChanged
    {
        private Contact _contact;
        private readonly ContactSerializer _contactSerializer;

        public Contact Contact
        {
            get
            {
                return _contact;
            }
            set
            {
                _contact = value;
                OnPropertyChanged(nameof(Contact));
            }
        }
        public string Name
        {
            get 
            {
                return _contact.Name; 
            }
            set
            {
                _contact.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int PhoneNumber
        {
            get
            {
                return _contact.PhoneNumber;
            }
            set
            {
                _contact.PhoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public string Email
        {
            get
            {
                return _contact.Email;
            }
            set
            {
                _contact.Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public LoadCommand LoadCommand { get; }

        public SaveCommand SaveCommand { get; }

        private void UpdateContact(Contact contact)
        {
            if (contact != null)
            {
                Contact = contact;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(PhoneNumber));
                OnPropertyChanged(nameof(Email));
            }
        }

        public MainVM()
        {
            _contactSerializer = new ContactSerializer();
            _contact = new Contact();
            LoadCommand = new LoadCommand(_contactSerializer, loadContact => UpdateContact(loadContact));
            SaveCommand = new SaveCommand(_contactSerializer, () => Contact);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
