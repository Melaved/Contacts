using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Contacts.Model.Model;
using Contacts.Model.Model.Services;

namespace Contacts.ViewModel.ViewModel
{
    /// <summary>
    /// Основной ViewModel для управления контактами.
    /// </summary>
    public partial class MainVM : ObservableObject
    {
        /// <summary>
        /// Выбранный контакт.
        /// </summary>
        [ObservableProperty]
        private Contact _selectedContact;

        /// <summary>
        /// Контакт, который редактируется.
        /// </summary>
        [ObservableProperty]
        private Contact _editingContact;

        /// <summary>
        /// Флаг, указывающий, находится ли контакт в режиме редактирования.
        /// </summary>
        [ObservableProperty]
        private bool _isEditing;

        /// <summary>
        /// Коллекция контактов, отображаемых в главном окне.
        /// </summary>
        public ObservableCollection<Contact> Contacts { get; set; } =
            new(ContactSerializer.LoadContacts());

        /// <summary>
        /// Вызывается при измении выбранного контакта.
        /// </summary>
        /// <param name="value">Новый выбранный контакт.</param>
        partial void OnSelectedContactChanged(Contact value)
        {
            OnPropertyChanged(nameof(IsContactSelected));
            IsEditing = false;
            EditingContact = value?.Clone();
            UpdateCommandStates();
        }

        /// <summary>
        /// Вызывается при изменении контакта, который редактируется.
        /// </summary>
        /// <param name="value">Новый контакт, который редактируется.</param>
        partial void OnEditingContactChanged(Contact value)
        {
            if (_editingContact != null)
            {
                _editingContact.ErrorsChanged -= OnEditingContactErrorsChanged;
            }

            if (value != null)
            {
                value.ErrorsChanged += OnEditingContactErrorsChanged;
            }

            OnPropertyChanged(nameof(CanApplyChanges));
            ApplyChangesCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// Вызывается при изменении флага редактирования контакта.
        /// </summary>
        /// <param name="value">Новое значение флага редактировани.</param>
        partial void OnIsEditingChanged(bool value)
        {
            OnPropertyChanged(nameof(CanApplyChanges));
            UpdateCommandStates();
        }

        /// <summary>
        /// Определяет, выбран ли контакт.
        /// </summary>
        public bool IsContactSelected => SelectedContact != null;

        /// <summary>
        /// Проверяет, можно ли добавить новый контакт.
        /// </summary>
        /// <returns>True, если можно добавить контакт.</returns>
        private bool CanAddContact() => !IsEditing;

        /// <summary>
        /// Проверяет, можно ли редактировать контакт.
        /// </summary>
        /// <returns>True, если можно редактировать контакт.</returns>
        private bool CanEditContact() => IsContactSelected && !IsEditing;

        /// <summary>
        /// Проверяет, можно ли удалить контакт.
        /// </summary>
        /// <returns>True, если можно удалить контакт.</returns>
        private bool CanRemoveContact() => IsContactSelected && !IsEditing;

        /// <summary>
        /// Проверяет, можно ли применить изменения к контакту.
        /// </summary>
        /// <returns>True, если можно применить изменения.</returns>
        private bool CanApplyChanges() => IsEditing && EditingContact != null && !EditingContact.HasErrors;

        /// <summary>
        /// Добавляет новый контакт в список.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanAddContact))]
        private void AddContact()
        {
            var newContact = new Contact { IsNewContact = true };
            SelectedContact = null;
            EditingContact = newContact;
            IsEditing = true;
        }

        /// <summary>
        /// Переводит выбранный контакт в режим редактирования.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanEditContact))]
        private void EditContact()
        {
            if (SelectedContact == null)
            {
                return;
            }

            EditingContact = SelectedContact.Clone();
            IsEditing = true;
        }

        /// <summary>
        /// Удаляет выбранный контакт из списка.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanRemoveContact))]
        private void RemoveContact()
        {
            if (SelectedContact == null)
            {
                return;
            }

            Contacts.Remove(SelectedContact);
            ContactSerializer.SaveContacts(Contacts);
            SelectedContact = Contacts.Count > 0 ? Contacts[0] : null;
        }

        /// <summary>
        /// Применяет изменения к контакту, обновляя список контактов.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanApplyChanges))]
        private void ApplyChanges()
        {
            if (EditingContact == null || EditingContact.HasErrors)
            {
                return;
            }

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
            IsEditing = false;
            ContactSerializer.SaveContacts(Contacts);
        }

        /// <summary>
        /// Обрабатывает изменения ошибок в редактируемом контакте.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void OnEditingContactErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            ApplyChangesCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// Обновляет состояния команд.
        /// </summary>
        private void UpdateCommandStates()
        {
            AddContactCommand.NotifyCanExecuteChanged();
            EditContactCommand.NotifyCanExecuteChanged();
            RemoveContactCommand.NotifyCanExecuteChanged();
            ApplyChangesCommand.NotifyCanExecuteChanged();
        }
    }
}