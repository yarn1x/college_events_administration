using college_events_desktop.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace college_events_desktop.View.Layers.Tables
{
    public partial class table_tuple_EventGroup_edit : UserControl
    {
        page_EventList_edit _page;
        EventGroups _group;
        page_table_EventGroup_edit _table;
        public table_tuple_EventGroup_edit(page_EventList_edit page, EventGroups group, object table)
        {
            InitializeComponent();
            _page = page;
            _group = group;
            _table = table as page_table_EventGroup_edit;
            Loaded += Table_tuple_EventGroup_Loaded;
        }

        private void Table_tuple_EventGroup_Loaded(object sender, RoutedEventArgs e)
        {
            if (_group != null)
            {
                combobox_group.Text = _group.name;
                text_supervisor_name.Text = $"{_group.supervisorSurname} {_group.supervisorName} {_group.supervisorLastname}";
                edit_expectedListenersCount.Text = _group.expectedListenersCount.ToString();
                edit_expectedParticipantsCount.Text = _group.expectedParticipantsCount.ToString();
                edit_expectedSuperParticipantsCount.Text = _group.expectedSuperParticipantsCount.ToString();
            }

            LoadGroups();
        }

        private void combobox_group_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (text_supervisor_name.Text == "-")
            {
                edit_expectedListenersCount.Text = "0";
                edit_expectedParticipantsCount.Text = "0";
                edit_expectedSuperParticipantsCount.Text = "0";

                _table.stack_table_rows.Children.Add(new table_tuple_EventGroup_edit(_page, null, _table));
            }

            if (combobox_group.SelectedItem == null) return;

            string selectedGroup = combobox_group.SelectedItem.ToString();
            text_supervisor_name.Text = GetSupervisor(selectedGroup);

        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation()
            {
                From = btn_delete_group.Width,
                To = 20,
                Duration = TimeSpan.FromMilliseconds(200),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            btn_delete_group.BeginAnimation(WidthProperty, anim);
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation()
            {
                From = btn_delete_group.Width,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(200),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };
            btn_delete_group.BeginAnimation(WidthProperty, anim);
        }


        //__Методы класса
        private void LoadGroups()
        {
            //проходимся по списку всех учебных групп для добавления списка в combobox
            foreach (var group in _table._groups)
            {
                bool already_added = false;
                //Для каждой добавляемой в список группы, мы проходимся по уже созданным таким же combobox-ам
                foreach (var element in _table.stack_table_rows.Children)
                {
                    if (element is table_tuple_EventGroup_edit tuple)
                    {
                        //и смотрим совпадает ли текст в combobox с названием группы, которую хотим добавить в новый combobox
                        if (tuple.combobox_group.Text == group.groupName)
                        {
                            //если совпадают, то мы сообщаем, что группа уже в списке на участие в мероприятии и прерываем проверять уже созданные combobox-ы
                            already_added = true;
                            break;
                        }
                    }
                }

                //Если значение отрицательно, добавляем группу в комбобокс
                if (!already_added) combobox_group.Items.Add(group.groupName);
            }

            //ЕСЛИ ВЫБОРКА ГРУПП НЕ ТРЕБУЕТСЯ, УДАЛИТЬ ИЛИ ЗАКОМЕНТИРОВАТЬ ЦИКЛ ВЫШЕ И РАСКОМЕНТИРОВАТЬ ЦИКЛ НИЖЕ
            //foreach (var group in _table._groups)
            //{
            //    combobox_group.Items.Add(group.groupName);
            //}
        }

        public string GetSupervisor(string groupName)
        {
            if (string.IsNullOrEmpty(groupName)) return "Куратор не указан";

            return _table.GroupSupervisor.TryGetValue(groupName, out string supervisor)
                ? supervisor
                : "Куратор не назначен";
        }

        private void btn_delete_group_Click(object sender, RoutedEventArgs e)
        {
            if (text_supervisor_name.Text == "-")
            {
                MessageBox.Show("Это строка для добавления новой группы. Её нельзя удалить.");
                return;
            }
            var callback = MessageBox.Show(
                $"Хотите удалить группу {combobox_group.Text} из списка?", 
                "Удаление", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Question, 
                MessageBoxResult.No);

            if (callback == MessageBoxResult.Yes)
            {
                _table.stack_table_rows.Children.Remove(this);
            }
        }

        private void edit_expectedCount_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                _page.validation_errors_count++;
            }
            else if (e.Action == ValidationErrorEventAction.Removed)
            {
                _page.validation_errors_count--;
            }
        }
    }
}
