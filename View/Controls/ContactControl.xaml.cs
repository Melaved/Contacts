using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace View.Controls
{
    /// <summary>
    /// Логика взаимодействия для ContactControl.xaml
    /// Контрол для отображения и ввода контактной информации.
    /// </summary>
    public partial class ContactControl : UserControl
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ContactControl"/>.
        /// </summary>
        public ContactControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обрабатывает ввод текста в поле номера телефона.
        /// Разрешает ввод только цифр, символов '+', '(', ')', '-' и пробелов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события, содержащие введённый текст.</param>
        private void PhoneNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"^[0-9+() -]*$");

            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Обрабатывает вставку текста в поле номера телефона.
        /// Запрещает вставку текста, если он содержит недопустимые символы.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события, содержащие вставляемый текст.</param>
        private void PhoneNumber_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));

                var regex = new Regex(@"^[0-9+() -]*$");

                if (!regex.IsMatch(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}
