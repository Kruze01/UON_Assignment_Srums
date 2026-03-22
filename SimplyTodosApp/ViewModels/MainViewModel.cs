using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimplyTodosApp.Models;
using SimplyTodosApp.Services;
using System.Collections.ObjectModel;
using Task = SimplyTodosApp.Models.Task;

namespace SimplyTodosApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly DatabaseService _dbService;

        [ObservableProperty]
        private ObservableCollection<Task> _tasksList = new();

        public MainViewModel(DatabaseService dbService)
        {
            _dbService = dbService;
            LoadTasks();
        }

        private async void LoadTasks()
        {
            var tasks = await _dbService.GetTasksAsync();
            foreach (var task in tasks)
                TasksList.Add(task);
        }

        [RelayCommand]
        private async void AddTask()
        {
            var newTask = new Task { Heading = "New Task 101", Description = "This is a testing.", Priority = "Medium" }; //Is a placeholder, to be updated by using user input

            //Save in sqlite
            await _dbService.SaveTaskAsync(newTask);

            //Update in UI list
            TasksList.Add(newTask);
        }

        [RelayCommand]
        private async void DeleteTask(Task task)
        {
            if (task == null) return;

            //1.Remove from SQLite
            await _dbService.DeleteTaskAsync(task);

            //2.Remove from UI list
            TasksList.Remove(task);
        }
    }
};


