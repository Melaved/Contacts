using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace View.Model
{
    /// <summary>
    /// Класс, представляющий контакт с именем, номером телефона и электронной почтой.
    /// </summary>
    public class Contact : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        /// <summary>
        /// Максимальная длина имени контакта.
        /// </summary>
        private const int MaxNameLength = 100;

        /// <summary>
        /// Максимальная длина номера телефона.
        /// </summary>
        private const int MaxPhoneNumberLength = 100;

        /// <summary>
        /// Максимальная длина электронной почты.
        /// </summary>
        private const int MaxEmailLength = 100;

        /// <summary>
        /// Регулярное выражение для проверки номера телефона.
        /// </summary>
        public static readonly Regex SimplePhoneNumberRegex = new Regex(@"^[0-9+() -]*$");
        public static readonly Regex StrictPhoneNumberRegex = new Regex(@"^\+?(\d{1,3})?[-. (]*(\d{1,4})[-. )]*(\d{1,4})[-. ]*(\d{1,9})$");

        /// <summary>
        /// Словарь ошибок данных.
        /// </summary>
        private readonly Dictionary<string, string> _errors = new();

        /// <summary>
        /// Имя контакта.
        /// </summary>
        private string _name;

        /// <summary>
        /// Номер телефона контакта.
        /// </summary>
        private string _phoneNumber;

        /// <summary>
        /// Электронная почта контакта.
        /// </summary>
        private string _email;

        /// <summary>
        /// Флаг, указывающий, находится ли контакт в режиме редактирования.
        /// </summary>
        private bool _isEditing;

        /// <summary>
        /// Создает новый экземпляр класса <see cref="Contact"/> с заданными параметрами.
        /// </summary>
        /// <param name="name">Имя контакта.</param>
        /// <param name="phoneNumber">Номер телефона контакта.</param>
        /// <param name="email">Электронная почта контакта.</param>
        public Contact(string name, string phoneNumber, string email)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        /// <summary>
        /// Создает новый экземпляр класса <see cref="Contact"/> с пустыми значениями.
        /// </summary>
        public Contact()
        {
            Name = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;
        }

        /// <summary>
        /// Событие, возникающее при изменении свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Событие, возникающее при изменении ошибок данных.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        /// <summary>
        /// Возвращает и задает флаг, указывающий, находится ли контакт в режиме редактирования.
        /// </summary>
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                if (_isEditing != value)
                {
                    _isEditing = value;
                    OnPropertyChanged(nameof(IsEditing));
                }
            }
        }

        /// <summary>
        /// Возвращает и задает флаг, указывающий, является ли контакт новым.
        /// </summary>
        public bool IsNewContact { get; set; } = true;

        /// <summary>
        /// Возвращает и задает имя контакта.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                ValidateName();
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// Возвращает и задает номер телефона контакта.
        /// </summary>
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                ValidatePhoneNumber();
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        /// <summary>
        /// Возвращает и задает электронную почту контакта.
        /// </summary>
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                ValidateEmail();
                OnPropertyChanged(nameof(Email));
            }
        }

        /// <summary>
        /// Определяет, содержит ли контакт ошибки.
        /// </summary>
        public bool HasErrors => _errors.Count > 0;

        /// <summary>
        /// Получает ошибки для указанного свойства.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <returns>Ошибки, связанные с указанным свойством.</returns>
        public IEnumerable GetErrors(string? propertyName)
        {
            if (propertyName != null && _errors.ContainsKey(propertyName))
            {
                yield return _errors[propertyName];
            }
        }

        /// <summary>
        /// Проверяет, соответствует ли имя контакта допустимым требованиям.
        /// </summary>
        private void ValidateName()
        {
            if (Name.Length > MaxNameLength)
            {
                _errors[nameof(Name)] = $"Имя не может превышать {MaxNameLength} символов.";
            }
            else
            {
                _errors.Remove(nameof(Name));
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Name)));
        }

        /// <summary>
        /// Проверяет, соответствует ли номер телефона контакта допустимым требованиям.
        /// </summary>
        private void ValidatePhoneNumber()
        {
            if (PhoneNumber.Length > MaxPhoneNumberLength
                || !Contact.StrictPhoneNumberRegex.IsMatch(PhoneNumber))
            {
                _errors[nameof(PhoneNumber)] = $"Номер телефона должен быть в формате '+7 (999) 123-45-67' "
                                               + $"и не превышать {MaxPhoneNumberLength} символов.";
            }
            else
            {
                _errors.Remove(nameof(PhoneNumber));
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(PhoneNumber)));
        }

        /// <summary>
        /// Проверяет, соответствует ли электронная почта контакта допустимым требованиям.
        /// </summary>
        private void ValidateEmail()
        {
            if (Email.Length > MaxEmailLength
                || !Regex.IsMatch(Email, @"[^@ \t\r\n]+@[^@ \t\r\n]+\.[^@ \t\r\n]+"))
            {
                _errors[nameof(Email)] = $"Электронная почта должна содержать '@', "
                                         + $"иметь корректный формат и не превышать "
                                         + $"{MaxEmailLength} символов.";
            }
            else
            {
                _errors.Remove(nameof(Email));
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Email)));
        }

        /// <summary>
        /// Создает копию текущего контакта.
        /// </summary>
        /// <returns>Копия контакта.</returns>
        public Contact Clone()
        {
            return new Contact(Name, PhoneNumber, Email)
            {
                IsEditing = this.IsEditing,
                IsNewContact = this.IsNewContact
            };
        }

        /// <summary>
        /// Вызывает событие <see cref="PropertyChanged"/>, уведомляя об изменении свойства.
        /// </summary>
        /// <param name="propertyName">Имя измененного свойства.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}