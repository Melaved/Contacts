namespace View.Model
{
    /// <summary>
    /// Класс контакта, который хранит его имя, почту и номер телефона.
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// Конструктор класса. <see cref = "Contact"/>
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
        /// Пустой конструктор класса. <see cref = "Contact"/>
        /// </summary>
        public Contact()
        {
            Name =string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;
        }

        /// <summary>
        /// Возвращает и задает имя контакта.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Возвращает и задает телефонный номер контакта.
        /// </summary>
        public string PhoneNumber { get; set; } 

        /// <summary>
        /// Возвращает и задает почту контакта.
        /// </summary>
        public string Email { get; set; }

    }
}