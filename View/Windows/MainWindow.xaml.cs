using college_events_desktop.DataModels;
using college_events_desktop.Model;
using college_events_desktop.Model.ApiProvider;
using college_events_desktop.View.Controls;
using college_events_desktop.View.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization.Configuration;

namespace college_events_desktop.View.Windows
{
	public partial class MainWindow : Window
	{
        #region Поля класса
        page_EventList eventList;
		internal DataService dataService { get; private set; }
        #endregion

        #region Конструктор
        public MainWindow()
		{
			InitializeComponent();
            dataService = new DataService(new ApiClient());
            eventList = new page_EventList(this, dataService);
			Loaded += MainWindow_Loaded;
		}
        #endregion

        #region Обработчики событий
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			mainframe.Navigate(eventList);
			await eventList.Page_EventList_Loaded();
        }

		private bool isEventsNavMenuOpened = false;
		private void open_events_nav_menu(object sender, RoutedEventArgs e)
		{
			if (!isEventsNavMenuOpened)
			{
				border_events_caret.OpacityMask = change_caret(isEventsNavMenuOpened);
				submenu_events.Content = new events_nav_buttons(this, eventList);
				isEventsNavMenuOpened = true;
			}
			else
			{   
				border_events_caret.OpacityMask = change_caret(isEventsNavMenuOpened);
				submenu_events.Content = null;
				isEventsNavMenuOpened = false;
			}
		}

        private bool isSupervisorsNavMenuOpened = false;
        private void open_users_nav_menu_Click(object sender, RoutedEventArgs e)
        {
            if (!isSupervisorsNavMenuOpened)
            {
                border_supervisors_caret.OpacityMask = change_caret(isSupervisorsNavMenuOpened);
                submenu_users.Content = new users_nav_buttons(this);
                isSupervisorsNavMenuOpened = true;
            }
            else
            {
                border_supervisors_caret.OpacityMask = change_caret(isSupervisorsNavMenuOpened);
                submenu_users.Content = null;
                isSupervisorsNavMenuOpened = false;
            }
        }
        #endregion


        #region Методы класса
        /// <summary>
        /// Метод, изменяющий направление индикатора развернутости меню.
        /// </summary>
        /// <param name="isNavMenuOpened">Развёрнуто ли меню</param>
        /// <returns>Кисть с изображением, взятое из App.xaml</returns>
        private ImageBrush change_caret(bool isNavMenuOpened)
		{
			BitmapImage bitmap;
			ImageBrush brush;
			if (!isNavMenuOpened)
			{
				bitmap = (BitmapImage)Application.Current.FindResource("caret_up");
				brush = new ImageBrush(bitmap);
			}
			else
			{
				bitmap = (BitmapImage)Application.Current.FindResource("caret_down");
				brush = new ImageBrush(bitmap);
			}
			brush.RelativeTransform = new ScaleTransform(1.5, 1.5, 0.5, 0.5);
			return brush;
		}
        #endregion
    }
}
