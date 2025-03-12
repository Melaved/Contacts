using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using View.Model;
using View.Model.Services;

namespace View.ViewModel
{
    /// <summary>
    /// Основной ViewModel для управления контактами.
    /// </summary>
    public class MainVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Выбранный контакт.
        /// </summary>
        private Contact _selectedContact;

        /// <summary>
        /// Контакт, который редактируется.
        /// </summary>
        private Contact _editingContact;


        /// <summary>
        /// Событие для уведомления об изменении свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Коллекция контактов, отображаемых в главном окне.
        /// </summary>
        public ObservableCollection<Contact> Contacts { get; set; } =
            new (ContactSerializer.LoadContacts());

        /// <summary>
        /// Возвращает и задает выбранный контакт.
        /// </summary>
        public Contact SelectedContact
        {
            get => _selectedContact;
            set
            {
                if (_selectedContact == value)
                {
                    return;
                }

                _selectedContact = value;
                OnPropertyChanged(nameof(SelectedContact));

                EditingContact = _selectedContact?.Clone();
                if (EditingContact != null)
                {
                    EditingContact.IsEditing = false;
                }

                OnPropertyChanged(nameof(IsContactSelected));
            }
        }

        /// <summary>
        /// Возвращает и задает контакт, который редактируется в данный момент.
        /// </summary>
        public Contact EditingContact
        {
            get => _editingContact;
            set
            {
                if (_editingContact != value)
                {
                    _editingContact = value;
                    OnPropertyChanged(nameof(EditingContact));
                }
            }
        }

        /// <summary>
        /// Определяет, выбран ли контакт.
        /// </summary>
        public bool IsContactSelected => SelectedContact != null;

        /// <summary>
        /// Команда для добавления нового контакта.
        /// </summary>
        public ICommand AddCommand => new RelayCommand(AddContact, CanAddContact);

        /// <summary>
        /// Команда для редактирования существующего контакта.
        /// </summary>
        public ICommand EditCommand => new RelayCommand(EditContact, CanEditContact);

        /// <summary>
        /// Команда для удаления выбранного контакта.
        /// </summary>
        public ICommand RemoveCommand => new RelayCommand(RemoveContact, CanRemoveContact);

        /// <summary>
        /// Команда для применения изменений к контакту.
        /// </summary>
        public ICommand ApplyCommand => new RelayCommand(ApplyChanges, CanApplyChanges);

        /// <summary>
        /// Проверяет, можно ли добавить новый контакт.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns>True, если можно добавить контакт.</returns>
        private bool CanAddContact(object parameter) => !IsEditingContact;

        /// <summary>
        /// Проверяет, можно ли редактировать контакт.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns>True, если можно редактировать контакт.</returns>
        private bool CanEditContact(object parameter) => IsContactSelected && !IsEditingContact;

        /// <summary>
        /// Проверяет, можно ли удалить контакт.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns>True, если можно удалить контакт.</returns>
        private bool CanRemoveContact(object parameter) => IsContactSelected && !IsEditingContact;

        /// <summary>
        /// Проверяет, можно ли применить изменения к контакту.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns>True, если можно применить изменения.</returns>
        private bool CanApplyChanges(object parameter) =>
            IsEditingContact && EditingContact != null && !EditingContact.HasErrors;

        /// <summary>
        /// Определяет, находится ли контакт в режиме редактирования.
        /// </summary>
        private bool IsEditingContact => EditingContact?.IsEditing == true;

        /// <summary>
        /// Добавляет новый контакт в список.
        /// </summary>
        private void AddContact(object parameter)
        {
            var newContact = new Contact { IsEditing = true, IsNewContact = true };
            EditingContact = newContact;
            SelectedContact = null;
        }

        /// <summary>
        /// Переводит выбранный контакт в режим редактирования.
        /// </summary>
        private void EditContact(object parameter)
        {
            if (SelectedContact != null)
            {
                EditingContact = SelectedContact.Clone();
                EditingContact.IsEditing = true;
                EditingContact.IsNewContact = false;
                OnPropertyChanged(nameof(EditingContact));
            }
        }

        /// <summary>
        /// Удаляет выбранный контакт из списка.
        /// </summary>
        private void RemoveContact(object parameter)
        {
            if (SelectedContact != null)
            {
                Contacts.Remove(SelectedContact);
                ContactSerializer.SaveContacts(Contacts);
                SelectedContact = Contacts.Count > 0 ? Contacts[0] : null;
            }
        }

        /// <summary>
        /// Применяет изменения к контакту, обновляя список контактов.
        /// </summary>
        private void ApplyChanges(object parameter)
        {
            if (EditingContact == null || EditingContact.HasErrors)
                return;

            if (EditingContact.IsNewContact)
            {
                EditingContact.IsNewContact = false;
                Contacts.Add(EditingContact);
            }
            else if (SelectedContact != null)
            {
                var index = Contacts.IndexOf(SelectedContact);
                if (index >= 0)
                {
                    Contacts[index] = EditingContact;
                }
            }

            SelectedContact = EditingContact;
            ContactSerializer.SaveContacts(Contacts);
        }

        /// <summary>
        /// Вызывает событие <see cref="PropertyChanged"/> при изменении свойства.
        /// </summary>
        /// <param name="propertyName">Имя измененного свойства.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
