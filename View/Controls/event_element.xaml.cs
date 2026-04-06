using college_events_desktop.DataModels;
using college_events_desktop.Model;
using college_events_desktop.View.Layers;
using college_events_desktop.View.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Xml.Linq;

namespace college_events_desktop.View.Controls
{
	public partial class event_element : UserControl
	{
		#region Поля класса:
		private readonly MainWindow mainWindow;
		private DataService _dataService;
		private Event _Event;

		//Сопоставление номера статуса с его цветом
		private Dictionary<string, Color> markerColors = new Dictionary<string, Color>()
		{
			//{null, Color.FromArgb(0xFF, 0xFF, 0x49, 0x49)}, //исключительный цвет
            {"1", Color.FromArgb(0xFF, 0xFF, 0xDD, 0x3C)}, //жёлтый
            {"2", Color.FromArgb(0xFF, 0x66, 0xFF, 0x3D)}, //зелёный
            {"3", Color.FromArgb(0xFF, 0x48, 0xAF, 0xFF)}, //голубой
            {"4", Color.FromArgb(0xFF, 0xA7, 0xA7, 0xA7)}, //серый
            {"5", Color.FromArgb(0xFF, 0xA6, 0xFF, 0x7D)}, //перенесено мероприятие (цвет чуть светлее зелёного)
            {"-1", Color.FromArgb(0xFF, 0xFF, 0x49, 0x49)} //красный
        };
		#endregion

		public event_element(MainWindow win, DataService dataService, Event _event)
		{
			InitializeComponent();
            mainWindow = win;
			_dataService = dataService;
			_Event = _event;
			SetElementTag();

			Loaded += Event_element_Loaded;
		}

		//___Обработчики событий:
		private void Event_element_Loaded(object sender, RoutedEventArgs e)
		{
			//Меняем цвет точки слева сверху на плашке мероприятия в зависимости от статуса (статус хранится в тэге элемента)
			event_status.Background = new SolidColorBrush(markerColors[Tag.ToString()]);

			text_name.Text = _Event.title;
			text_organizer_name.Text = $"{_Event.organizerSurname} {_Event.organizerName} {_Event.organizerLastname}";
			text_start_time.Text = _Event.startTime.ToString();
			text_end_time.Text = _Event.endTime.ToString();
			text_date.Text = _Event.startDate.ToString();
			text_direction.Text = _Event.categoryName;
			text_place.Text = _Event.place;

			//Изменение видимости красной точки на боковом меню
			Ellipse mainWindow_ellipse_new_event_status = mainWindow.ellipse_new_event_status;
			if (mainWindow_ellipse_new_event_status.Visibility != Visibility.Visible
				&& (Tag.ToString() == "4" || Tag.ToString() == "-1"))
			{
				mainWindow_ellipse_new_event_status.Visibility = Visibility.Visible;
			}

			//Изменение видимости кнопок действий над мероприятиями в зависимости от статуса
			switch (Tag.ToString())
			{
				case "1":
					stack_electoral_buttons.Visibility = Visibility.Visible;
					return;

				case "2":
					btn_edit.Visibility = Visibility.Visible;
					return;

				case "3":
					btn_create_event_report.Visibility = Visibility.Visible;
					return;

				case "4":
					btn_see_event_report.Visibility = Visibility.Visible;
					return;

				case "5":
					btn_edit.Visibility = Visibility.Visible;
					return;

				case "-1":
					stack_electoral_buttons.Visibility = Visibility.Visible;
					return;
			}
		}

		private void btn_create_event_report_Click(object sender, RoutedEventArgs e)
		{
			mainWindow.mainframe.Navigate(new page_EventList_save());
		}

		private void btn_see_event_report_Click(object sender, RoutedEventArgs e)
		{
			mainWindow.mainframe.Navigate(new page_EventList_seeEvent());
		}

		private void btn_edit_Click(object sender, RoutedEventArgs e)
		{
			var page = new page_EventList_edit(mainWindow, _dataService, _Event)
			{
				Title = _Event.title + ". Редактирование мероприятия"
			};
			mainWindow.mainframe.Navigate(page);
		}

		private async void event_text_copy_Click(object sender, MouseButtonEventArgs e)
		{
			await Application.Current.Dispatcher.InvokeAsync(() =>
			{
				var text = sender as TextBlock;
				Clipboard.SetText(text.Text);
			});

		}

		private void SetElementTag()
		{
            //Проверка на конфликт времени
            var sameDayEvents = _dataService.events
                .Where(d => d.startDate == _Event.startDate && d != _Event)
                .ToList();
            bool hasConflict = sameDayEvents.Any(otherEvent =>
            {
                DateTime currentStart = Convert.ToDateTime(_Event.startTime);
                DateTime currentEnd = Convert.ToDateTime(_Event.endTime);
                DateTime otherStart = Convert.ToDateTime(otherEvent.startTime);
                DateTime otherEnd = Convert.ToDateTime(otherEvent.endTime);

                return (currentStart < otherEnd && currentEnd > otherStart);
            });

            Tag = _Event.statusId;
            if (hasConflict) Tag = -1;
            else if ((int)Tag == 5) Tag = 2;
        }
	}
}
