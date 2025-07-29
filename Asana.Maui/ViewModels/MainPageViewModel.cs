using Asana.Library.Models;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Maui.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private ToDoServiceProxy _toDoSvc;

        public ObservableCollection<Project> Projects => new(_toDoSvc.GetAllProjects());

        private Project selectedProject;
        public Project SelectedProject
        {
            get => selectedProject;
            set
            {
                selectedProject = value;
                NotifyPropertyChanged();
                LoadToDos();
            }
        }

        public MainPageViewModel()
        {
            _toDoSvc = ToDoServiceProxy.Current;
        }

        private ToDo selectedToDo;
        public ToDo SelectedToDo
        {
            get => selectedToDo;
            set
            {
                if (selectedToDo != value)
                {
                    selectedToDo = value;


                    NotifyPropertyChanged();


                    NotifyPropertyChanged(nameof(SelectedToDoId));
                }
            }
        }

        private ObservableCollection<ToDo> toDos = new();

        public ObservableCollection<ToDo> ToDos
        {
            get => toDos;

            set
            {
                toDos = value;

                NotifyPropertyChanged();
            }
        }
        private void LoadToDos()
        {
            var result = SelectedProject == null
                ? _toDoSvc.ToDos

                : _toDoSvc.GetToDosByProject(SelectedProject.Id);

            if (!IsShowCompleted){result = result.Where(t => !t?.IsCompleted ?? false).ToList();}

            FilterToDos();
        }

        public int SelectedToDoId => SelectedToDo?.Id ?? 0;

        private bool isShowCompleted;
        public bool IsShowCompleted
        {
            get => isShowCompleted;

            set
            {
                if (isShowCompleted != value)
                {

                    isShowCompleted = value;

                    LoadToDos();
                }
            }
        }
        private bool isSortedByName;
        public bool IsSortedByName
        {
            get => isSortedByName;
            set
            {
                if (isSortedByName != value)
                {
                    isSortedByName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                if (searchText != value)
                {
                    searchText = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public void DeleteToDo()
        {
            if (SelectedToDo == null)
                return;

            _toDoSvc.DeleteToDo(SelectedToDo);
            SelectedToDo = null;
            LoadToDos(); // this will help refresh the new list.
        }
        public void RefreshPage()
        {

            NotifyPropertyChanged(nameof(ToDos));

        }

        public void AddNewToDo(string name, string description, int? priority, bool isCompleted, DateTime dueDate)
        {
            var newToDo = new ToDo
            {
                Name = name,
                Description = description,
                Priority = priority,
                IsCompleted = isCompleted,
                DueDate = dueDate,
                ProjectId = SelectedProject != null ? SelectedProject.Id : 0
            };

            _toDoSvc.AddOrUpdate(newToDo);

            LoadToDos();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public void AddProject(string name, string description)
        {

            _toDoSvc.AddProject(name, description);

            NotifyPropertyChanged(nameof(Projects));

        }
        public void FilterToDos()
        {
            var baseList = SelectedProject == null
                ? _toDoSvc.ToDos
                : _toDoSvc.GetToDosByProject(SelectedProject.Id);

            if (!IsShowCompleted)
                baseList = baseList.Where(t => !t.IsCompleted).ToList();

            if (!string.IsNullOrWhiteSpace(SearchText))
                baseList = baseList.Where(t =>
                    t.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    t.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();
            if (IsSortedByName)
                baseList = baseList.OrderBy(t => t.Name).ToList();

            ToDos = new ObservableCollection<ToDo>(baseList);

        }

        public void DeleteProject(int id)
        {
            _toDoSvc.DeleteProject(id);

            NotifyPropertyChanged(nameof(Projects));

            LoadToDos();

        }

    }
}
