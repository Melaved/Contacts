using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
// TODO: Допилить  класс(добавить валидацию для свойств).
namespace View.Model
{
    public class Contact
    {
        /// <summary>
        /// Имя контакта.
        /// </summary>
        private string _name;

        /// <summary>
        /// Номер телефона контакта.
        /// </summary>
        private int _phoneNumber;

        /// <summary>
        /// Почта контакта.
        /// </summary>
        private string _email;

        /// <summary>
        /// Возвращает и задает имя контакта
        /// </summary>
        public string Name
        {
            get 
            { 
                return _name; 
            }
            set 
            {
                _name = value; 
            }
        }

        /// <summary>
        /// Возвращает и задает телефонный номер контакта
        /// </summary>
        public int PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                if (value.ToString().Length != 11 || !value.ToString().StartsWith("7"))
                {
                    throw new ArgumentException(
                        "Номер телефона должен состоять из 11 цифр и начинаться с 7.");
                }
                _phoneNumber = value;
            }
        }

        /// <summary>
        /// Возвращает и задает почту контакта.
        /// </summary>
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (!IsValidEmail(value))
                {
                    throw new ArgumentException("Некорректный формат почты.");
                }
                _email = value;
            }
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Используем регулярное выражение для проверки формата почты
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Конструктор класса <see cref = "Contact"/>
        /// </summary>
        /// <param name="name">Имя контакта.</param>
        /// <param name="phoneNumber">Номер телефона контакта.</param>
        /// <param name="email">Почта контакта.</param>
        public Contact(string name, int phoneNumber, string email)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
        }
        //TODO: придумать как реализовать валидацию для номера(нужно ли определенное кол-во символов и обязан ли начинаться на +7).

        /// <summary>
        /// Пустой конструктор класса <see cref = "Contact"/>
        /// </summary>
        public Contact()
        {
            Name = string.Empty;
            PhoneNumber = 0;
            Email = string.Empty;
        }
    }
}
