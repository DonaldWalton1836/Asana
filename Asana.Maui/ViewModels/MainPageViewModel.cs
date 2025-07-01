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

            ToDos = new ObservableCollection<ToDo>(result);
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

        public void DeleteToDo()
        {
            if (SelectedToDo == null)
                return;

            _toDoSvc.DeleteToDo(SelectedToDo);
            SelectedToDo = null;
            LoadToDos(); // this will refresh the new list.
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

        public void DeleteProject(int id)
        {
            _toDoSvc.DeleteProject(id);

            NotifyPropertyChanged(nameof(Projects));

            LoadToDos();

        }

    }
}
