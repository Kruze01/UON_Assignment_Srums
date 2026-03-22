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
            //A placeholder is used now, and this will be updated 
            var newTask = new Task { 
                Heading = "New Task 101", 
                Description = "This is a testing.", 
                Priority = "Medium" }; 

            //Save in SQLite
            await _dbService.SaveTaskAsync(newTask);

            //Update in UI list
            TasksList.Add(newTask);
        }

        [RelayCommand]
        private async void DeleteTask(Task task)
        {
            if (task == null) return;

            //Remove from SQLite
            await _dbService.DeleteTaskAsync(task);

            //Remove from UI list
            TasksList.Remove(task);
        }
    }
};


