using Asana.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Services
{
    public class ToDoServiceProxy
    {
        private List<ToDo> _toDoList;
        public List<ToDo> ToDos
        {
            get
            {
                return _toDoList.Take(100).ToList();
            }

            private set
            {
                if (value != _toDoList)
                {
                    _toDoList = value;
                }
            }
        }

        private ToDoServiceProxy()
        {
            ToDos = new List<ToDo>
            {
                new ToDo{Id = 1, Name = "Task 1", Description = "My Task 1", IsCompleted=true}
            };
        }

        private static ToDoServiceProxy? instance;

        private int nextKey
        {
            get
            {
                if (ToDos.Any())
                {
                    return ToDos.Select(t => t.Id).Max() + 1;
                }
                return 1;
            }
        }

        public static ToDoServiceProxy Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new ToDoServiceProxy();
                }

                return instance;
            }
        }

        public ToDo? AddOrUpdate(ToDo? toDo)
        {
            if (toDo != null && toDo.Id == 0)
            {
                toDo.Id = nextKey;
                _toDoList.Add(toDo);
            }

            return toDo;
        }

        public void DisplayToDos(bool isShowCompleted = false)
        {
            if (isShowCompleted)
            {
                ToDos.ForEach(Console.WriteLine);
            }
            else
            {
                ToDos.Where(t => (t != null) && !(t.IsCompleted))
                     .ToList()
                     .ForEach(Console.WriteLine);
            }
        }

        public ToDo? GetById(int id)
        {
            return ToDos.FirstOrDefault(t => t.Id == id);
        }

        public void DeleteToDo(ToDo? toDo)
        {
            if (toDo == null)
            {
                return;
            }
            _toDoList.Remove(toDo);
        }

        public bool DeleteToDo(int id)
        {
            var todo = _toDoList.FirstOrDefault(t => t.Id == id);
            if (todo != null)
            {
                _toDoList.Remove(todo);
                var project = _projects.FirstOrDefault(p => p.Id == todo.ProjectId);
                project?.ToDoIds.Remove(id);
                UpdateProjectCompletion(project);
                return true;
            }
            return false;
        }

        // === Project Functionality Below ===

        private List<Project> _projects = new();
        private int nextProjectKey => _projects.Any() ? _projects.Max(p => p.Id) + 1 : 1;

        public Project AddProject(string name, string description)
        {
            var project = new Project
            {
                Id = nextProjectKey,
                Name = name,
                Description = description
            };
            _projects.Add(project);
            return project;
        }

        public bool DeleteProject(int id)
        {
            var project = _projects.FirstOrDefault(p => p.Id == id);
            if (project != null)
            {
                _projects.Remove(project);
                _toDoList.RemoveAll(t => t.ProjectId == id);
                return true;
            }
            return false;
        }

        public List<Project> GetAllProjects() => _projects;

        public bool UpdateProject(Project updatedProject)
        {
            var project = _projects.FirstOrDefault(p => p.Id == updatedProject.Id);
            if (project != null)
            {
                project.Name = updatedProject.Name;
                project.Description = updatedProject.Description;
                return true;
            }
            return false;
        }

        public List<ToDo> GetToDosByProject(int projectId)
        {
            return _toDoList.Where(t => t.ProjectId == projectId).ToList();
        }

        public void UpdateProjectCompletion(Project? project)
        {
            if (project == null || project.ToDoIds.Count == 0)
            {
                if (project != null) project.CompletePercent = 0;
                return;
            }

            var todos = _toDoList.Where(t => project.ToDoIds.Contains(t.Id)).ToList();
            double completed = todos.Count(t => t.IsCompleted);
            project.CompletePercent = (completed / project.ToDoIds.Count) * 100;
        }
    }
}