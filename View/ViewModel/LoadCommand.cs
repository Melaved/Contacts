using System.Windows.Input;
using View.Model;
using View.Model.Services;

namespace View.ViewModel
{
    /// <summary>
    /// Команда загрузки контакта.
    /// </summary>
    public class LoadCommand : ICommand
    {
        /// <summary>
        /// Инициализирует новый экземпляр команды <see cref="LoadCommand"/>.
        /// </summary>
        /// <param name="onContactLoaded">Делегат для передачи загруженного контакта в ViewModel.</param>
        public LoadCommand(Action<Contact> onContactLoaded)
        {
            OnContactLoaded = onContactLoaded;
        }

        /// <summary>
        /// Делегат, который устанавливает загруженный контакт в ViewModel.
        /// Используется для обновления состояния объекта Contact.
        /// </summary>
        public Action<Contact> OnContactLoaded { get; }

        /// <summary>
        /// Событие, которое происходит при изменении возможности выполнения команды.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Выполняет команду загрузки контакта.
        /// </summary>
        /// <param name="parameter">Не используется.</param>
        public void Execute(object? parameter)
        {
            var contact = ContactSerializer.LoadContact();

            if (contact != null)
            {
                OnContactLoaded?.Invoke(contact);
            }
        }

        /// <summary>
        /// Определяет, может ли команда выполняться.
        /// </summary>
        /// <param name="parameter">Не используется.</param>
        /// <returns>Всегда возвращает true.</returns>
        public bool CanExecute(object? parameter)
        {
            return true;
        }

    }
}