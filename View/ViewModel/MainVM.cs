using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using View.Model.Services;
using View.Model;
using View.ViewModel;
using System.Windows.Data;
using System.Windows.Controls;

/// <summary>
/// Представляет ViewModel для главного окна приложения.
/// </summary>
public class MainVM : INotifyPropertyChanged
{
    /// <summary>
    /// Выбранный контакт.
    /// </summary>
    private Contact _selectedContact;

    /// <summary>
    /// Значение, указывающее, видна ли кнопка "Apply".
    /// </summary>
    private bool _isApplyButtonVisible;

    /// <summary>
    /// Значение, указывающее, находится ли приложение в режиме редактирования.
    /// </summary>
    private bool _isEditMode;

    /// <summary>
    /// Значение, указывающее, добавляется ли новый контакт.
    /// </summary>
    private bool _isAddingNewContact;

    /// <summary>
    /// Значение, указывающее, редактируется ли контакт.
    /// </summary>
    private bool _isEditingContact;

    /// <summary>
    /// Коллекция контактов, отображаемых в главном окне.
    /// </summary>
    public ObservableCollection<Contact> Contacts { get; set; } = new ObservableCollection<Contact>();

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MainVM"/>.
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
    /// Событие, которое происходит при изменении значения свойства.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Вызывает событие <see cref="PropertyChanged"/> для указанного свойства.
    /// </summary>
    /// <param name="propertyName">Имя измененного свойства.</param>
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Получает или задает выбранный контакт.
    /// </summary>
    public Contact SelectedContact
    {
        get => _selectedContact;
        set
        {
            if (_isAddingNewContact && value != null)
            {
                _isAddingNewContact = false;
                IsApplyButtonVisible = false;
                IsEditMode = false;
                OnPropertyChanged(nameof(IsAddingNewContact));
            }

            if (_isEditingContact && value != null)
            {
                _isEditingContact = false;
                IsApplyButtonVisible = false;
                IsEditMode = false;
                OnPropertyChanged(nameof(IsEditingContact));
            }

            _selectedContact = value;
            OnPropertyChanged(nameof(SelectedContact));
            OnPropertyChanged(nameof(IsContactSelected));
            OnPropertyChanged(nameof(IsApplyButtonVisible));
        }
    }

    /// <summary>
    /// Получает или задает значение, указывающее, видна ли кнопка "Применить".
    /// </summary>
    public bool IsApplyButtonVisible
    {
        get => _isApplyButtonVisible;
        set
        {
            _isApplyButtonVisible = value;
            OnPropertyChanged(nameof(IsApplyButtonVisible));
        }
    }

    /// <summary>
    /// Получает или задает значение, указывающее, находится ли приложение в режиме редактирования.
    /// </summary>
    public bool IsEditMode
    {
        get => _isEditMode;
        set
        {
            _isEditMode = value;
            OnPropertyChanged(nameof(IsEditMode));
        }
    }

    /// <summary>
    /// Получает или задает значение, указывающее, добавляется ли новый контакт.
    /// </summary>
    public bool IsAddingNewContact
    {
        get => _isAddingNewContact;
        set
        {
            _isAddingNewContact = value;
            OnPropertyChanged(nameof(IsAddingNewContact));
        }
    }

    /// <summary>
    /// Получает или задает значение, указывающее, редактируется ли контакт.
    /// </summary>
    public bool IsEditingContact
    {
        get => _isEditingContact;
        set
        {
            _isEditingContact = value;
            OnPropertyChanged(nameof(IsEditingContact));
        }
    }

    /// <summary>
    /// Получает значение, указывающее, выбран ли контакт.
    /// </summary>
    public bool IsContactSelected => _selectedContact != null;

    /// <summary>
    /// Команда для добавления нового контакта.
    /// </summary>
    public ICommand AddCommand { get; }

    /// <summary>
    /// Команда для редактирования выбранного контакта.
    /// </summary>
    public ICommand EditCommand { get; }

    /// <summary>
    /// Команда для удаления выбранного контакта.
    /// </summary>
    public ICommand RemoveCommand { get; }

    /// <summary>
    /// Команда для применения изменений к контакту.
    /// </summary>
    public ICommand ApplyCommand { get; }

    /// <summary>
    /// Добавляет новый контакт.
    /// </summary>
    /// <param name="parameter">Параметр команды.</param>
    public void AddContact(object parameter)
    {
        SelectedContact = null;
        SelectedContact = new Contact();
        IsApplyButtonVisible = true;
        IsEditMode = true;
        IsAddingNewContact = true;
    }

    /// <summary>
    /// Редактирует выбранный контакт.
    /// </summary>
    /// <param name="parameter">Параметр команды.</param>
    public void EditContact(object parameter)
    {
        IsApplyButtonVisible = true;
        IsEditMode = true;
        IsEditingContact = true;
    }

    /// <summary>
    /// Удаляет выбранный контакт.
    /// </summary>
    /// <param name="parameter">Параметр команды.</param>
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

    /// <summary>
    /// Применяет изменения к выбранному контакту.
    /// </summary>
    /// <param name="parameter">Параметр команды.</param>
    public void ApplyContact(object parameter)
    {
        if (SelectedContact != null)
        {
            if (!Contacts.Contains(SelectedContact))
            {
                Contacts.Add(SelectedContact);
            }

            IsApplyButtonVisible = false;
            IsEditMode = false;
            IsAddingNewContact = false;
            IsEditingContact = false;
            ContactSerializer.SaveContacts(Contacts);
        }
    }

    /// <summary>
    /// Определяет, можно ли выполнить команду добавления контакта.
    /// </summary>
    /// <param name="parameter">Параметр команды.</param>
    /// <returns>True, если команда может быть выполнена; иначе False.</returns>
    private bool CanAddContact(object parameter) => !IsApplyButtonVisible;

    /// <summary>
    /// Определяет, можно ли выполнить команду редактирования контакта.
    /// </summary>
    /// <param name="parameter">Параметр команды.</param>
    /// <returns>True, если команда может быть выполнена; иначе False.</returns>
    private bool CanEditContact(object parameter) => IsContactSelected && !IsApplyButtonVisible;

    /// <summary>
    /// Определяет, можно ли выполнить команду удаления контакта.
    /// </summary>
    /// <param name="parameter">Параметр команды.</param>
    /// <returns>True, если команда может быть выполнена; иначе False.</returns>
    private bool CanRemoveContact(object parameter) => IsContactSelected && !IsApplyButtonVisible;

    /// <summary>
    /// Определяет, можно ли выполнить команду применения изменений к контакту.
    /// </summary>
    /// <param name="parameter">Параметр команды.</param>
    /// <returns>True, если команда может быть выполнена; иначе False.</returns>
    private bool CanApplyContact(object parameter) => IsApplyButtonVisible;
}