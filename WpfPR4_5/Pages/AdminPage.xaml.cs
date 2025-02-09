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
using WpfPR4_5.ModelClasses;

namespace WpfPR4_5.Pages
{

    public partial class AdminPage : Page
    {
        // Приватное свойство для работы с моделью данных EF.
        private ModelEF model { get; set; }
        public AdminPage(ModelEF _model)
        {
            InitializeComponent();
            // Присвоение значения свойству model.
            model = _model;

        }
        // Обработчик события нажатия кнопки добавления.
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу добавления/редактирования учетных записей.
            this.NavigationService.Navigate(new AddUpdateAccountsPage(model));
        }
        // Обработчик события нажатия кнопки обновления.
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка наличия выделенной строки в DataGrid.
            if (accountsDataGrid.SelectedItems.Count > 0)
            {
                // Преобразование выделенной строки в объект типа Accounts.
                Accounts selectedAcount = accountsDataGrid.SelectedItem as Accounts;
                // Проверка на null.
                if (selectedAcount != null)
                {
                    // Переход на страницу добавления/редактирования учетных записей с передачей выбранной учетной записи.
 this.NavigationService.Navigate(new AddUpdateAccountsPage(model, selectedAcount));
                }
            }
        }
        // Обработчик события нажатия кнопки выхода.
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Возврат на предыдущую страницу.
            this.NavigationService.GoBack();
        }
        // Обработчик события нажатия кнопки удаления.
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Преобразование выделенной строки в объект типа Accounts.
            Accounts selectedAcount = accountsDataGrid.SelectedItem as Accounts;
            // Запрос подтверждения удаления у пользователя.
            var result = MessageBox.Show($"Вы точно хотите удалить элемент c ID {selectedAcount.ID}?",
           "Подтверждение",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
            // Если пользователь подтвердил удаление.
            if (result == MessageBoxResult.Yes)
            {
                // Удаление учетной записи из контекста базы данных.
                model.Accounts.Remove(selectedAcount);
                try
                {
                    // Сохранение изменений в базу данных.
                    model.SaveChanges();
                    // Уведомление об успешном удалении.
                    MessageBox.Show($"Пользователь с ID - {selectedAcount.ID} удалён");
                }
                catch (Exception ex)
                {
                    // Вывод сообщения об ошибке, если удаление прошло неуспешно.
                    MessageBox.Show(ex.Message);
                }
                // Перезагрузка данных в DataGrid.
                LoadDataInCollectionViewSource();
            }
        }
        // Обработчик события загрузки страницы.
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Загрузка данных в CollectionViewSource.
            LoadDataInCollectionViewSource();
        }
        // Метод загрузки данных в CollectionViewSource.
        private void LoadDataInCollectionViewSource()
        {
            // Присвоение списка учетных записей источнику данных CollectionViewSource.
            (Resources["accountsViewSource"] as CollectionViewSource).Source = model.Accounts.ToList();
        }

    }
}
