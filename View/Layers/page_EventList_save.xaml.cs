using college_events_desktop.DataModels;
using college_events_desktop.Model;
using college_events_desktop.View.Layers.Tables;
using college_events_desktop.View.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace college_events_desktop.View.Layers
{
    public partial class page_EventList_save : Page
    {
        #region Поля класса
        private MainWindow mainWindow;
        private DataService _dataService;
        private Event _Event;
        #endregion

        #region Конструктор
        public page_EventList_save(Window win, DataService dataService, Event _event)
        {
            InitializeComponent();
            mainWindow = win as MainWindow;
            _dataService = dataService;
            _Event = _event;

            Loaded += Page_EventList_save_Loaded;
        }
        #endregion

        #region Обработчики событий
        private void Page_EventList_save_Loaded(object sender, RoutedEventArgs e)
        {
            text_header.Text = _Event.title;
            edit_event_name.Text = _Event.title;
            edit_organizer_name.Text = $"{_Event.organizerSurname} {_Event.organizerName} {_Event.organizerLastname}";
            edit_event_direction.Text = _Event.categoryName;
            edit_event_place.Text = _Event.place;
            edit_date.Text = _Event.startDate;
            edit_startTime.Text = _Event.startTime;
            edit_endTime.Text = _Event.endTime;
            edit_description.Text = _Event.description;

            frame_table.Navigate(new page_table_EventGroup_save(this, _dataService, _Event));
        }

        private void goback_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.mainframe.GoBack();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion
    }
}
