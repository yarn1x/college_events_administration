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
    public partial class page_EventList_edit : Page
    {
        #region Поля класса
        private MainWindow mainWindow;
        private DataService _dataService;
        private Event _Event;
        page_table_EventGroup _table;
        #endregion

        #region Конструктор класса
        public page_EventList_edit(Window win, DataService dataService, Event _event)
        {
            InitializeComponent();
            mainWindow = win as MainWindow;
            _dataService = dataService;
            _Event = _event;
            _table = new page_table_EventGroup(mainWindow, this, _dataService, _Event);

            Loaded += Page_EventList_edit_Loaded;
        }
        #endregion

        #region Обработчики событий
        private void Page_EventList_edit_Loaded(object sender, RoutedEventArgs e)
        {
            LoadInformation();
            frame_table.Navigate(_table);
        }

        private void goback_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.mainframe.GoBack();
        }
        #endregion

        #region Методы класса
        private async void LoadInformation()
        {
            //данные о мероприятии
            text_header.Text = _Event.title;
            edit_event_name.Text = _Event.title;
            combobox_organizer_name.Text = $"{_Event.organizerSurname} {_Event.organizerName} {_Event.organizerLastname}";
            combobox_event_direction.Text = _Event.categoryName;
            combobox_event_place.Text = _Event.place;
            datePicker_date.Text = _Event.startDate;
            edit_startTime.Text = _Event.startTime;
            edit_endTime.Text = _Event.endTime;
            edit_description.Text = _Event.desctiption;

            await _dataService.LoadGroupsListAsync();
            await _dataService.LoadPlacesListAsync();
            await _dataService.LoadOrganizerListAsync();

            //дополнительно
            try
            {
                foreach (Place item in _dataService.places)
                {
                    combobox_event_place.Items.Add(item.place);
                }
                foreach (Organizer item in _dataService.organizers)
                {
                    combobox_organizer_name.Items.Add($"{item.lastName} {item.firstName} {item.middleName}");
                }
                foreach (Category item in _dataService.categories)
                {
                    combobox_event_direction.Items.Add(item.categoryName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public int validation_errors_count = 0;
        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (validation_errors_count == 0)
            {
                MessageBox.Show("Информация успещно обновлена!");
            }
            else
            {
                MessageBox.Show($"У вас есть ошибки ввода. Пожалуйста, исправьте их ({validation_errors_count} ошибок) перед выполнением сохранения.\n\nПодсказка:\n1. Количество участников не может быть отрицательным или содержать символы кроме цифр", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

    }
}
