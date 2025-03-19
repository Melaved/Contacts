using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Contacts.Model
{
    /// <summary>
    /// Класс, представляющий контакт с именем, номером телефона и электронной почтой.
    /// </summary>
    public partial class Contact : ObservableObject, INotifyDataErrorInfo
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
        /// Регулярное выражение для маски номера телефона.
        /// </summary>
        public static readonly Regex PhoneNumberMask = new Regex(@"^[0-9+() -]*$");

        /// <summary>
        /// Регулярное выражение для проверки номера телефона.
        /// </summary>
        public static readonly Regex PhoneNumberRegex =
            new Regex(@"^\+?(\d{1,3})?[-. (]*(\d{1,4})[-. )]*(\d{1,4})[-. ]*(\d{1,9})$");

        /// <summary>
        /// Регулярное выражение для проверки электронной почты.
        /// </summary>
        public static readonly Regex EmailRegex =
            new Regex(@"^[^@ \t\r\n]+@[^@ \t\r\n]+\.[^@ \t\r\n]+$");

        /// <summary>
        /// Словарь ошибок данных.
        /// </summary>
        private readonly Dictionary<string, string> _errors = new();

        /// <summary>
        /// Имя контакта.
        /// </summary>
        [ObservableProperty]
        private string _name;

        /// <summary>
        /// Номер телефона контакта.
        /// </summary>
        [ObservableProperty]
        private string _phoneNumber;

        /// <summary>
        /// Электронная почта контакта.
        /// </summary>
        [ObservableProperty]
        private string _email;

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
        /// Событие, возникающее при изменении ошибок данных.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        /// <summary>
        /// Определяет, содержит ли контакт ошибки.
        /// </summary>
        public bool HasErrors => _errors.Count > 0;

        /// <summary>
        /// Вызывается при изменении имени контакта и выполняет его валидацию.
        /// </summary>
        /// <param name="value">Новое значение имени.</param>
        partial void OnNameChanged(string value) => ValidateName();

        /// <summary>
        /// Вызывается при изменении номера телефона контакта и выполняет его валидацию.
        /// </summary>
        /// <param name="value">Новое значение номера телефона.</param>
        partial void OnPhoneNumberChanged(string value) => ValidatePhoneNumber();

        /// <summary>
        /// Вызывается при изменении электронной почты контакта и выполняет ее валидацию.
        /// </summary>
        /// <param name="value">Новое значение электронной почты.</param>
        partial void OnEmailChanged(string value) => ValidateEmail();

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
                || !PhoneNumberRegex.IsMatch(PhoneNumber))
            {
                _errors[nameof(PhoneNumber)] =
                    $"Номер телефона должен быть в формате " +
                    $"'+7 (999) 123 4567' и не превышать " +
                    $"{MaxPhoneNumberLength} символов.";
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
                || !EmailRegex.IsMatch(Email))
            {
                _errors[nameof(Email)] =
                    $"Электронная почта должна содержать '@', " +
                    $"иметь корректный формат и не превышать " +
                    $"{MaxEmailLength} символов.";
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
            return new Contact
            {
                Name = Name,
                PhoneNumber = PhoneNumber,
                Email = Email
            };
        }
    }
}