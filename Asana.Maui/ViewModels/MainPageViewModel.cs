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

        public ObservableCollection<ToDo> ToDos
        {
            get
            {
                var toDos = _toDoSvc.ToDos;
                if (!IsShowCompleted)
                {
                    toDos = _toDoSvc.ToDos.Where(t => !t?.IsCompleted ?? false).ToList();
                }
                return new ObservableCollection<ToDo>(toDos);
            }
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
                    NotifyPropertyChanged(nameof(ToDos));
                }
            }
        }

        public void DeleteToDo()
        {
            if (SelectedToDo == null)
                return;

            _toDoSvc.DeleteToDo(SelectedToDo);
            SelectedToDo = null;
            NotifyPropertyChanged(nameof(ToDos));
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
                DueDate = dueDate
            };

            _toDoSvc.AddOrUpdate(newToDo);
            NotifyPropertyChanged(nameof(ToDos));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
