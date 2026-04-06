using college_events_desktop.DataModels;
using college_events_desktop.Model;
using college_events_desktop.Model.ApiProvider;
using college_events_desktop.View.Controls;
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

namespace college_events_desktop.View.Layers.Tables
{
    /// <summary>
    /// Класс представляет собой контейнер для помещения внутрь элементов с информацией в виде строк. 
    /// </summary>
    public partial class page_table_EventGroup : Page
    {
        #region Поля класса
        private MainWindow mainWindow;
        private page_EventList_edit _page;
        DataService _dataService;
        private Event _Event;
        internal List<Group> _groups;
        internal Dictionary<string, string> GroupSupervisor = new Dictionary<string, string>();
        #endregion

        #region Конструктор
        public page_table_EventGroup(Window parentWindow, page_EventList_edit parentPage, DataService dataService, Event _event)
        {
            InitializeComponent();
            Loaded += Page_table_EventGroup_Loaded;
            mainWindow = parentWindow as MainWindow;
            _page = parentPage;
            _dataService = dataService;
            _Event = _event;
        }
        #endregion

        #region Обработчики событий
        private async void Page_table_EventGroup_Loaded(object sender, RoutedEventArgs e)
        {
            //Показываем иконку загрузки, пока не загрузим контент
            stack_table_rows.Children.Clear();
            var element = new loading_interface()
            {
                Margin = new Thickness(0, 10, 0, 0)
            };
            stack_table_rows.Children.Add(element);

            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                try
                {
                    //В отдельном потоке, совершаем запрос к API на получение групп, участвовавших в мероприятии
                    var eventGroupsList = await _dataService.apiClient.GetEventGroupsByEventId(_Event.eventId);
                    // и полный список групп (для выбора из выпадающего списка)
                    var groups = _dataService.groups;
                    _groups = groups;
                    foreach (var group in groups)
                    {
                        GroupSupervisor.Add(group.groupName, $"{group.supervisorSurname} {group.supervisorName} {group.supervisorMiddlename}");
                    }

                    stack_table_rows.Children.Clear();
                    //По каждой найденной записанной группе в мероприятие, выводим в контейнер таблицы строчку с информацией.
                    foreach (var group in eventGroupsList)
                    {
                        stack_table_rows.Children.Add(new table_tuple_EventGroup(_page, group, this));
                    }
                    stack_table_rows.Children.Add(new table_tuple_EventGroup(_page, null, this));
                }
                catch //При возникновении ошибки, добавим в контейнер таблицы информацию об ошибке
                {
                    TextBlock MessageText = new TextBlock()
                    {
                        Text = $"Ошибка получения списка групп.\nКод ошибки: AA003",
                        TextAlignment = TextAlignment.Center,
                        Margin = new Thickness(0, 10, 0, 0)
                    };
                    stack_table_rows.Children.Clear();
                    stack_table_rows.Children.Add(MessageText);
                }
            });
        }
        #endregion

    }
}
