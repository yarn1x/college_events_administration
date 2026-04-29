using college_events_desktop.DataModels;
using college_events_desktop.Model;
using college_events_desktop.View.Controls;
using college_events_desktop.View.Layers;
using college_events_desktop.View.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace college_events_desktop.ViewModels
{
    public class EventListViewModel
    {
        DataService _dataService;
        page_EventList _page;
        MainWindow mainWindow;
        public EventListViewModel(MainWindow win, page_EventList page, DataService dataService)
        {
            mainWindow = win;
            _page = page;
            _dataService = dataService;
        }

        public void LoadCategoryCombobox(List<Category> categories)
        {
            var combobox_categories = _page.combobox_categories;
            //Очищаем combobox от старых направлений и заполняем их новыми
            combobox_categories.Items.Clear();
            combobox_categories.Items.Add("Не выбрано");
            foreach (var category in categories)
            {
                combobox_categories.Items.Add(category.categoryName);
            }
            combobox_categories.SelectedValue = "Не выбрано";
        }

        public void LoadEventsInStack(List<Event> events)
        {
            var stack_events = _page.stack_events;
            //Очищаем StackPanel от дочерних элементов и выводим список мероприятий в него же
            stack_events.Children.Clear();
            foreach (Event i in events)
            {
                var element = new event_element(mainWindow, _dataService, i)
                {
                    Margin = new Thickness(5, 7, 5, 0),
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
                stack_events.Children.Add(element);
            }
        }
    }
}
