using System.ComponentModel;
using View.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using View.Model.Services;

namespace View.ViewModel
{
    /// <summary>
    /// Класс для управления контактами и их сохранением и загрузкой.
    /// </summary>
    public class MainVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Текущий контакт.
        /// </summary>
        private Contact _selectedContact;

        private bool _isApplyButtonVisible;

        public ObservableCollection<Contact> Contacts { get; set; } = new ObservableCollection<Contact>();

        /// <summary>
        /// Создает новый экземпляр класса <see cref="MainVM"/>.
        /// </summary>
        public MainVM()
        {
            Contacts = new ObservableCollection<Contact>(ContactSerializer.LoadContacts());
            AddCommand = new RelayCommand(AddContact, CanAddContact);
            EditCommand = new RelayCommand(EditContact, CanEditContact);
            RemoveCommand = new RelayCommand(RemoveContact, CanRemoveContact);
            ApplyCommand = new RelayCommand(ApplyContact, CanApplyContact);
        }

        /// <summary>
        /// Событие, которое происходит при изменении свойства.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Вызывает событие <see cref="PropertyChanged"/>.
        /// </summary>
        /// <param name="propertyName">Имя измененного свойства.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Свойство для доступа к текущему контакту.
        /// </summary>
        public Contact SelectedContact
        {
            get => _selectedContact;
            
            set
            {
                _selectedContact = value;
                OnPropertyChanged(nameof(SelectedContact));
                OnPropertyChanged(nameof(IsContactSelected));
                OnPropertyChanged(nameof(IsApplyButtonVisible));
            }
        }

        public bool IsApplyButtonVisible
        {
            get => _isApplyButtonVisible;
            set
            {
                _isApplyButtonVisible = value;
                OnPropertyChanged(nameof(IsApplyButtonVisible));
            }
        }

        public bool IsContactSelected => _selectedContact != null;

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand ApplyCommand { get; }

        public void AddContact(object parameter)
        {
            SelectedContact = new Contact();
            IsApplyButtonVisible = true;
        }

        public void EditContact(object parameter)
        {
            IsApplyButtonVisible = true;
        }

        public void RemoveContact(object parameter)
        {
            if (SelectedContact != null)
            {
                int index = Contacts.IndexOf(SelectedContact);
                Contacts.Remove(SelectedContact);

                if (Contacts.Any())
                {
                    SelectedContact = index < Contacts.Count ? Contacts[index] : Contacts.Last();
                }
                else
                {
                    SelectedContact = null;
                }
                ContactSerializer.SaveContacts(Contacts);
            }
        }

        public void ApplyContact(object parameter)
        {
            if (SelectedContact != null)
            {
                if (Contacts.Contains(SelectedContact))
                {
                    Contacts.Add(SelectedContact);
                }
                IsApplyButtonVisible = false;
                ContactSerializer.SaveContacts(Contacts);
            }
        }

        private bool CanAddContact(object parameter) => !IsApplyButtonVisible;

        private bool CanEditContact(object parameter) => IsContactSelected && !IsApplyButtonVisible;

        private bool CanRemoveContact(object parameter) => IsContactSelected && !IsApplyButtonVisible;
        private bool CanApplyContact(object parameter) => IsApplyButtonVisible;
    }
}