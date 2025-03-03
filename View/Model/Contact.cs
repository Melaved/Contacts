using System.ComponentModel;

namespace View.Model
{
    /// <summary>
    /// Класс контакта, который хранит его имя, почту и номер телефона.
    /// </summary>
    public class Contact : INotifyPropertyChanged
    {
        /// <summary>
        /// Конструктор класса <see cref = "Contact"/>.
        /// </summary>
        /// <param name="name">Имя контакта.</param>
        /// <param name="phoneNumber">Номер телефона контакта.</param>
        /// <param name="email">Почта контакта.</param>
        public Contact(string name, string phoneNumber, string email)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        /// <summary>
        /// Пустой конструктор класса <see cref = "Contact"/>.
        /// </summary>
        public Contact()
        {
            Name =string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;
        }

        private string _name;
        private string _phoneNumber;
        private string _email;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}