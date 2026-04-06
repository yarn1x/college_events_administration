using college_events_desktop.DataModels;
using college_events_desktop.Model;
using college_events_desktop.Model.ApiProvider;
using college_events_desktop.View.Controls;
using college_events_desktop.View.Windows;
using college_events_desktop.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace college_events_desktop.View.Layers
{
	public partial class page_EventList : Page
	{
        private MainWindow mainWindow;
        /// <summary>
        /// Словарь состояний активности фильтров по статусу мероприятия
        /// </summary>
        private Dictionary<string, bool> filterStates = new Dictionary<string, bool>()
        {
            {"1", false},
            {"2", false},
            {"3", false},
            {"4", false},
            {"5", false},
            {"-1", false},
        };
        
        
        EventListViewModel viewModel;
        private DataService _dataService;
        public page_EventList(Window win, DataService dataService)
		{
			InitializeComponent();
			mainWindow = win as MainWindow;
            _dataService = dataService;
            viewModel = new EventListViewModel(mainWindow, this, _dataService);
		}

        
        internal async Task Page_EventList_Loaded()
        {
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                stack_events.Children.Clear();
                stack_events.Children.Add(new loading_interface() { Margin = new Thickness(0, 20, 0, 0)});
                try
                {
                    await _dataService.LoadEventsAsync();
                    await _dataService.LoadCategoriesAsync();

                    viewModel.LoadEventsInStack(_dataService.events);
                    viewModel.LoadCategoryCombobox(_dataService.categories);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при получении списка мероприятий.\n\nКод ошибки: AA002\nИнформация для разработчика:\n{ex.Message}\n\nТело ошибки скопировано в буфер обмена", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    MessageBox.Show("Показан список до обновления страницы!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Clipboard.SetText(ex.ToString());
                }

                UpdateBottomCounters();
            });
        }

        /// <summary>
        /// Применение фильтра к списку мероприятий. Все кнопки фильтра статусов обращаются именно к нему.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        private async void btn_any_filter_click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                //Определяем, какой был активирован статус фильтрации
                string colorKey = button.Tag.ToString();
                //Инвертируем value в словаре фильтров (filterStates)
                filterStates[colorKey] = !filterStates[colorKey];

                //Анимируем цвета кнопки
                await AnimateButtonColorsAsync(button, filterStates[colorKey]);
                //применяем фильтр
                await apply_filter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка применения фильтра поиска.\n\nКод ошибки: AA001\nИнформация для разработчика:\n{ex.Message}\n\nТело ошибки скопировано в буфер обмена", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                Clipboard.SetText(ex.ToString());
            }
        }

        /// <summary>
        /// Применение фильтра к списку мероприятий. На этот метод ссылается только текстовое поле поиска по содержанию.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        private void search_textChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is SearchBar textBox)) return;

            string searchText = textBox.SearchText;
            string lowerSearchText = searchText.ToLower().Trim();

            //Наличие активного состояния любого фильтра
            bool isFilterActive = filterStates.Any(s => s.Value == true);

            foreach (event_element child in stack_events.Children)
            {
                ///Если мероприятие было уже скрыто с помощью кнопок фильтрации, то пропустим данный event_element
                ///И если применён хотя бы один из фильтров с помощью кнопок.
                ///Изначально все value из словаря filterStates равны false.
                ///Русским языком, если не добавить условие isFilterActive, то он будет "скипать" все мероприятия
                ///при ВЫКЛЮЧЕННЫХ "кнопочных" фильтрах, этот обработчик не будет ничего фильтровать
                if (filterStates[child.Tag.ToString()] == false && isFilterActive) continue;

                //Определяем, есть ли в плашке мероприятия инфа, которую мы ввели в TextBox
                bool containsSearchingText = child.text_name.Text.ToLower().Contains(lowerSearchText) || child.text_organizer_name.Text.ToLower().Contains(lowerSearchText) || child.text_start_time.Text.ToLower().Contains(lowerSearchText) || child.text_end_time.Text.ToLower().Contains(lowerSearchText) || child.text_date.Text.ToLower().Contains(lowerSearchText) || child.text_direction.Text.ToLower().Contains(lowerSearchText) || child.text_place.Text.ToLower().Contains(lowerSearchText);

                //При условии, что текст из TextBox всё таки присутствует в плашке мероприятия, оставим его видимым в списке, иначе убираем визуальное отображение
                child.Visibility = containsSearchingText ? Visibility.Visible : Visibility.Collapsed;
            }
            UpdateBottomCounters();

            //Если мы отчистили поисковое поле, то выведем все мероприятия в соответствии с включенными "кнопочными" фильтрами
            if (string.IsNullOrEmpty(searchText.Trim()))
            {
                foreach (event_element child in stack_events.Children)
                {
                    if (filterStates[child.Tag.ToString()] == false) { continue; }
                    child.Visibility = Visibility.Visible;
                }
            }
        }

        private async void update_page_Click(object sender, RoutedEventArgs e)
        {
            await Page_EventList_Loaded();
        }
        private void go_EventListHelp(object sender, RoutedEventArgs e)
        {
            mainWindow.mainframe.Navigate(new page_EventListHelp(mainWindow));
        }


        /// <summary>
        /// Метод перекрашивания кнопки. Может принадлежать только кнопкам с двумя состояниями: включена, выключена.
        /// </summary>
        /// <param name="button">Объект, требующий анимирования</param>
        /// <param name="statusSwitch">Указатель, в какой цвет перекрашивать кнопку: true = из белого в синий; false - из синего в белый</param>
        /// <returns>Асинхронное выполнение метода</returns>
        private async Task AnimateButtonColorsAsync(Button button, bool statusSwitch)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {

                ColorAnimation backgroundAnim = new ColorAnimation()
                {
                    Duration = TimeSpan.FromMilliseconds(200),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                };
                ColorAnimation textAnim = new ColorAnimation()
                {
                    Duration = TimeSpan.FromMilliseconds(200),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                };

                if (statusSwitch)
                {
                    backgroundAnim.From = Colors.White;
                    backgroundAnim.To = Color.FromArgb(255, 0, 140, 255);

                    textAnim.From = Color.FromArgb(0xFF, 0x66, 0x66, 0x66);
                    textAnim.To = Colors.White;

                    button.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x60, 0xBC));
                }
                else
                {
                    backgroundAnim.To = Colors.White;
                    backgroundAnim.From = Color.FromArgb(255, 0, 140, 255);

                    textAnim.To = Color.FromArgb(0xFF, 0x66, 0x66, 0x66);
                    textAnim.From = Colors.White;

                    button.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x66, 0x66, 0x66));
                }

                button.Background = new SolidColorBrush();
                button.Background.BeginAnimation(SolidColorBrush.ColorProperty, backgroundAnim);

                var stack = button.Content as StackPanel;
                var text = stack.Children[0] as TextBlock;
                text.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, textAnim);
            });
        }

        /// <summary>
        /// Метод применения фильтра статуса мероприятия. Работает в коопе с btn_any_filter_click()
        /// </summary>
        /// <returns>Асинхронное выполнение метода</returns>
        private async Task apply_filter()
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                //TODO: изменить summary, раскомментировать код ниже, добавить фильтрацию по тексту и комбобоксу сюда
                //foreach (event_element child in stack_events.Children)
                //{
                //    child.Visibility = Visibility.Visible;
                //}
                if (filterStates.Any(b => b.Value == true))
                {
                    foreach (event_element child in stack_events.Children)
                    {
                        bool isVisible = filterStates[child.Tag.ToString()];
                        child.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
                else
                {
                    foreach (event_element child in stack_events.Children)
                    {
                        child.Visibility = Visibility.Visible;
                    }
                }
               UpdateBottomCounters();
            });
        }

        private void UpdateBottomCounters()
        {
            var all_event_element = stack_events.Children.OfType<event_element>().Where(e => e.Visibility == Visibility.Visible).ToList();
            text_amount_of_events.Text = all_event_element.Count().ToString();
            text_amount_of_gray.Text = all_event_element.Count(g => (int)g.Tag == 4).ToString();
            text_amount_of_blue.Text = all_event_element.Count(bl => (int)bl.Tag == 3).ToString();
            text_amount_of_green.Text = all_event_element.Count(gr => (int)gr.Tag == 2 || (int)gr.Tag == 5).ToString();
            text_amount_of_yellow.Text = all_event_element.Count(y => (int)y.Tag == 1).ToString();
            text_amount_of_red.Text = all_event_element.Count(r => (int)r.Tag == -1).ToString();
        }
    }
}