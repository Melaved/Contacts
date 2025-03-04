using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using View.Model.Services;
using View.Model;
using View.ViewModel;
using System.Windows.Data;
using System.Windows.Controls;

public class MainVM : INotifyPropertyChanged
{
    private Contact _selectedContact;
    private bool _isApplyButtonVisible;
    private bool _isEditMode;
    private bool _isAddingNewContact;
    private bool _isEditingContact; 

    public ObservableCollection<Contact> Contacts { get; set; } = new ObservableCollection<Contact>();

    public MainVM()
    {
        Contacts = new ObservableCollection<Contact>(ContactSerializer.LoadContacts());
        AddCommand = new RelayCommand(AddContact, CanAddContact);
        EditCommand = new RelayCommand(EditContact, CanEditContact);
        RemoveCommand = new RelayCommand(RemoveContact, CanRemoveContact);
        ApplyCommand = new RelayCommand(ApplyContact, CanApplyContact);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

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

    public bool IsApplyButtonVisible
    {
        get => _isApplyButtonVisible;
        set
        {
            _isApplyButtonVisible = value;
            OnPropertyChanged(nameof(IsApplyButtonVisible));
        }
    }

    public bool IsEditMode
    {
        get => _isEditMode;
        set
        {
            _isEditMode = value;
            OnPropertyChanged(nameof(IsEditMode));
        }
    }

    public bool IsAddingNewContact
    {
        get => _isAddingNewContact;
        set
        {
            _isAddingNewContact = value;
            OnPropertyChanged(nameof(IsAddingNewContact));
        }
    }

    public bool IsEditingContact
    {
        get => _isEditingContact;
        set
        {
            _isEditingContact = value;
            OnPropertyChanged(nameof(IsEditingContact));
        }
    }

    public bool IsContactSelected => _selectedContact != null;

    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand RemoveCommand { get; }
    public ICommand ApplyCommand { get; }

    public void AddContact(object parameter)
    {
        SelectedContact = null; 
        SelectedContact = new Contact(); 
        IsApplyButtonVisible = true; 
        IsEditMode = true; 
        IsAddingNewContact = true; 
    }

    public void EditContact(object parameter)
    {
        IsApplyButtonVisible = true; 
        IsEditMode = true; 
        IsEditingContact = true; 
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

    private bool CanAddContact(object parameter) => !IsApplyButtonVisible;

    private bool CanEditContact(object parameter) => IsContactSelected && !IsApplyButtonVisible;

    private bool CanRemoveContact(object parameter) => IsContactSelected && !IsApplyButtonVisible;

    private bool CanApplyContact(object parameter) => IsApplyButtonVisible;
}