using System.ComponentModel;
using View.Model;

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
        private Contact _contact;

        /// <summary>
        /// Создает новый экземпляр класса <see cref="MainVM"/>.
        /// </summary>
        public MainVM()
        {
            _contact = new Contact();
            LoadCommand = new LoadCommand(loadContact => UpdateContact(loadContact));
            SaveCommand = new SaveCommand(() => Contact);
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
        public Contact Contact
        {
            get => _contact;
            
            set
            {
                _contact = value;
                OnPropertyChanged(nameof(Contact));
            }
        }

        /// <summary>
        /// Свойство для доступа к имени контакта.
        /// </summary>
        public string Name
        {
            get => _contact.Name;

            set
            {
                _contact.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// Свойство для доступа к номеру телефона контакта.
        /// </summary>
        public string PhoneNumber
        {
            get => _contact.PhoneNumber;
            
            set
            {
                _contact.PhoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        /// <summary>
        /// Свойство для доступа к почте контакта.
        /// </summary>
        public string Email
        {
            get => _contact.Email;

            set
            {
                _contact.Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        /// <summary>
        /// Команда для загрузки контакта.
        /// </summary>
        public LoadCommand LoadCommand { get; }

        /// <summary>
        /// Команда для сохранения контакта.
        /// </summary>
        public SaveCommand SaveCommand { get; }

        /// <summary>
        /// Обновляет текущий контакт.
        /// </summary>
        /// <param name="contact">Загруженный контакт.</param>
        private void UpdateContact(Contact contact)
        {
            if (contact != null)
            {
                Contact = contact;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(PhoneNumber));
                OnPropertyChanged(nameof(Email));
            }
        }
    }
}