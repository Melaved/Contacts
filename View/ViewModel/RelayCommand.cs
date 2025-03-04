using System;
using System.Windows.Input;

namespace View.ViewModel
{
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// Конструктор команды.
        /// </summary>
        /// <param name="execute">Метод, который будет выполнен при вызове команды.</param>
        /// <param name="canExecute">Метод, который проверяет, можно ли выполнить команду.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            ExecuteAction = execute ?? throw new ArgumentNullException(nameof(execute));
            CanExecutePredicate = canExecute;
        }

        /// <summary>
        /// Метод, который будет выполнен при вызове команды.
        /// </summary>
        private Action<object> ExecuteAction { get; }

        /// <summary>
        /// Метод, который проверяет, можно ли выполнить команду.
        /// </summary>
        private Predicate<object> CanExecutePredicate { get; }

        /// <summary>
        /// Проверяет, можно ли выполнить команду.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns>True, если команду можно выполнить, иначе False.</returns>
        public bool CanExecute(object parameter)
        {
            return CanExecutePredicate == null || CanExecutePredicate(parameter);
        }

        /// <summary>
        /// Выполняет команду.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        public void Execute(object parameter)
        {
            ExecuteAction(parameter);
        }

        /// <summary>
        /// Событие, которое вызывается при изменении возможности выполнения команды.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
