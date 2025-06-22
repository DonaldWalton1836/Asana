using Asana.Maui.ViewModels;

namespace Asana.Maui
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }

        private void AddNewClicked(object sender, EventArgs e)
        {
            var vm = BindingContext as MainPageViewModel;
            if (vm == null) return;

            var name = NameEntry.Text ?? "";
            var description = DescriptionEntry.Text ?? "";
            var isCompleted = IsCompletedCheckBox.IsChecked;
            var dueDate = DueDatePicker.Date;

            // Parse priority from Picker selection
            var priorityText = PriorityPicker.SelectedItem as string;
            int priority = 1;
            if (!string.IsNullOrEmpty(priorityText) && int.TryParse(priorityText.Split('-')[0].Trim(), out int parsed))
            {
                priority = parsed;
            }

            vm.AddNewToDo(name, description, priority, isCompleted, dueDate);
        }


        private void EditClicked(object sender, EventArgs e)
        {
            var selectedId = (BindingContext as MainPageViewModel)?.SelectedToDoId ?? 0;
            Shell.Current.GoToAsync($"//ToDoDetails?toDoId={selectedId}");
        }

        private void DeleteClicked(object sender, EventArgs e)
        {
            (BindingContext as MainPageViewModel)?.DeleteToDo();
        }

        private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            (BindingContext as MainPageViewModel)?.RefreshPage();
        }

        private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
        {

        }


    }

}
