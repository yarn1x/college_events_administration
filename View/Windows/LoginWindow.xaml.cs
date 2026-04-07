using college_events_desktop.Model;
using college_events_desktop.Model.ApiProvider;
using System;
using System.Windows;

namespace college_events_desktop.View.Windows
{
    public partial class LoginWindow : Window
    {
        DataService dataService;
        public LoginWindow()
        {
            InitializeComponent();
            dataService = new DataService(new ApiClient());
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await dataService.GetSessionToken(edit_login.Text, edit_password.Password);

                if (dataService._jwtToken == null)
                {
                    MessageBox.Show("Логин или пароль введён неверно, либо у вас нет прав администратора.", "Аккаунт не найден!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                new MainWindow().Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Система не смогла проверить ваши введённые данные. Попробуйте позже.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
