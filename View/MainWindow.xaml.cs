using System.Windows;
using System.Windows.Controls;

namespace View
{
    /// <summary>
    /// Главное окно приложения.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Инициализирует главное окно приложения и устанавливает контекст данных.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVM();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var nameExpression = NameTextBox.GetBindingExpression(TextBox.TextProperty);
            var phoneExpression = PhoneNumberTextBox.GetBindingExpression(TextBox.TextProperty);
            var emailExpression = EmailTextBox.GetBindingExpression(TextBox.TextProperty);

            nameExpression?.UpdateSource();
            phoneExpression?.UpdateSource();
            emailExpression?.UpdateSource();

            if (DataContext is MainVM ViewModel)
            {
                ViewModel.ApplyContact(null);
            }
        }
    }
}