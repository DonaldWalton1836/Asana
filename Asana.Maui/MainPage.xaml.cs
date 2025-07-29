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

            /*This will help parse the priority*/
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
        private void AddProjectClicked(object sender, EventArgs e)
        {
            var name = ProjectNameEntry.Text ?? "";

            var desc = ProjectDescriptionEntry.Text ?? "";

            if (!string.IsNullOrWhiteSpace(name))
            {
                (BindingContext as MainPageViewModel)?.AddProject(name, desc);

                ProjectNameEntry.Text = "";

                ProjectDescriptionEntry.Text = "";

            }
        }

        private void DeleteProjectClicked(object sender, EventArgs e)
        {
            var selected = (BindingContext as MainPageViewModel)?.SelectedProject;
            if (selected != null)
            {

                (BindingContext as MainPageViewModel)?.DeleteProject(selected.Id);

            }
        }
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            (BindingContext as MainPageViewModel)?.FilterToDos();
        }
        private void OnSortChanged(object sender, CheckedChangedEventArgs e)
        {
            (BindingContext as MainPageViewModel)?.FilterToDos();
        }





    }

}
