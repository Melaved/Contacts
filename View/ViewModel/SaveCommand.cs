//using System.Windows.Input;
//using View.Model;
//using View.Model.Services;


//namespace View.ViewModel
//{
//    /// <summary>
//    /// Команда сохранения контакта.
//    /// </summary>
//    public class SaveCommand : ICommand
//    {
//        /// <summary>
//        /// Инициализирует новый экземпляр команды <see cref="SaveCommand"/>.
//        /// </summary>
//        /// <param name="onContactSaved">Функция, возвращающая текущий контакт.</param>
//        public SaveCommand(Func<Contact> onContactSaved)
//        {
//            OnContactSaved = onContactSaved;
//        }

//        /// <summary>
//        /// Делегат, который возвращает текущий контакт из ViewModel.
//        /// Используется для получения актуального состояния объекта Contact.
//        /// </summary>
//        public Func<Contact> OnContactSaved { get; }

//        /// <summary>
//        /// Событие, которое происходит при изменении возможности выполнения команды.
//        /// </summary>
//        public event EventHandler? CanExecuteChanged;

//        /// <summary>
//        /// Выполняет команду сохранения контакта в файл.
//        /// </summary>
//        /// <param name="parameter">Не используется.</param>
//        public void Execute(object? parameter)
//        {
//            var contact = OnContactSaved();

//            if (contact != null)
//            {
//                ContactSerializer.SaveContact(contact);
//            }
//        }

//        /// <summary>
//        /// Определяет, можно ли выполнить команду.
//        /// </summary>
//        /// <param name="parameter">Не используется.</param>
//        /// <returns>Всегда возвращает true.</returns>
//        public bool CanExecute(object? parameter)
//        {
//            return true;
//        }

//    }
//}