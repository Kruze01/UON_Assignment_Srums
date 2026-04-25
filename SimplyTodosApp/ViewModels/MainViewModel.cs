using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Alerts;
using SimplyTodosApp.Services;
using SimplyTodosApp.Views;
using System.Collections.ObjectModel;
using Task = SimplyTodosApp.Models.Task;
using System.Diagnostics;

namespace SimplyTodosApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        readonly DatabaseService _dbService;
        private List<Task> _allTasksCache = new();  //Cache tasks in memory

        [ObservableProperty]
        ObservableCollection<Task> _tasksList = new();  //List of tasks to bind in MainPage

        //List of selected completed-tasks to be removed in Completions View
        List<Task> TasksToDeleteList = new();

        [ObservableProperty]
        bool _showOnlyCompleted;    //For task display control

        //For changing tasks to display when the Views change
        partial void OnShowOnlyCompletedChanged(bool value)
        {
            _=ApplyFilterAsync();
            TasksToDeleteList.Clear();
        }

        public MainViewModel(DatabaseService dbService)
        {
            _dbService = dbService;
            _ = LoadTasksFromDbAsync();
        }

        async System.Threading.Tasks.Task LoadTasksFromDbAsync()
        {
            _allTasksCache = await _dbService.GetTasksAsync();
            await ApplyFilterAsync();
        }

        async System.Threading.Tasks.Task ApplyFilterAsync()
        {
            var filtered = await System.Threading.Tasks.Task.Run(() =>
            {
                return _showOnlyCompleted ? _allTasksCache.Where(t => t.IsCompleted).ToList() : _allTasksCache.Where(t => !t.IsCompleted).ToList();
            });

            MainThread.BeginInvokeOnMainThread(() =>
                TasksList = new ObservableCollection<Task>(filtered)
            );
        }

        //For adding a new task
        //This will show ModifyTaskPopup
        [RelayCommand]
        async System.Threading.Tasks.Task AddTask()
        {
            var popup = new ModifyTaskPopup();

            var result = await Shell.Current.CurrentPage.ShowPopupAsync<Task>(popup);

            if (!result.WasDismissedByTappingOutsideOfPopup && result.Result is Task newTask && !string.IsNullOrWhiteSpace(newTask.Heading))
            {
                await _dbService.SaveTaskAsync(newTask);
                await Toast.Make("A new task has been created successfully!").Show();
                _allTasksCache.Add(newTask);  //Sync cache
                await ApplyFilterAsync();
            }
        }

        //For editing a new Task
        //This will show ModifyTaskPopup
        [RelayCommand]
        async System.Threading.Tasks.Task EditTask(Task taskToEdit)
        {
            var popup = new ModifyTaskPopup(taskToEdit);
            var result = await Shell.Current.CurrentPage.ShowPopupAsync<Task>(popup);

            if (!result.WasDismissedByTappingOutsideOfPopup && result.Result is Task updatedTask)
            {
                taskToEdit.Heading = updatedTask.Heading;
                taskToEdit.Description = updatedTask.Description;
                taskToEdit.Priority = updatedTask.Priority;

                await _dbService.SaveTaskAsync(taskToEdit);
                await Toast.Make("Task has been updated successfully!").Show();

                //Task object is mutated in-place inside the cache — just re-filter
                await ApplyFilterAsync();
            }
        }

        //For removing a new Task
        //This will show ConfirmationPopup
        [RelayCommand]
        async void RemoveTask(Task task)
        {
            if (task == null)
                return;

            var popup = new ConfirmationPopup("Remove Task", "Are you sure you want to remove the task ?");
            var result = await Shell.Current.CurrentPage.ShowPopupAsync<bool>(popup);
            if (result == null || result.WasDismissedByTappingOutsideOfPopup || !result.Result)
                return;

            await _dbService.DeleteTaskAsync(task);
            await Toast.Make("Task has been removed successfully!").Show();

            _allTasksCache.Remove(task);  //Sync cache
            await ApplyFilterAsync();
        }

        //For changing a task's IsCompleted attribute
        //This method is called if a task's checkbox is clicked
        [RelayCommand]
        async void ToggleComplete(Task task)
        {
            if (task == null)
                return;

            string textForToast = task.IsCompleted ? "Task has been marked as incompleted." : "Task has been marked as completed.";

            task.IsCompleted = !task.IsCompleted;
            await _dbService.SaveTaskAsync(task);
            await Toast.Make(textForToast).Show();
            await ApplyFilterAsync();
        }

        //For reviewing a Task
        //This will show ReviewTaskPopup
        [RelayCommand]
        async System.Threading.Tasks.Task ReviewTask(Task task)
        {
            if (task == null)
                return;

            await Shell.Current.CurrentPage.ShowPopupAsync(new ReviewTaskPopup(task));
        }

        //Selecting and Deselecting completed task to be removed

        [RelayCommand]
        void SelectCompletedTask(Task task)
        {
            if (task == null)
                return;

            if (TasksToDeleteList.Contains(task))
                TasksToDeleteList.Remove(task);
            else
                TasksToDeleteList.Add(task);
        }

        //For removing all selected tasks history in Completions view
        //Will call ConfirmationPopup
        [RelayCommand]
        async void RemoveSelectedTasksHistory(List<Task> TaskToDeleteList)
        {
            if (TasksToDeleteList.Count == 0)
            {
                //Notify user how it works
                await Toast.Make("Please select at least one task to remove.").Show();
                return;
            }

            var popup = new ConfirmationPopup("Clear Tasks", "Are you sure you want to remove all selected tasks that are completed?");
            var result = await Shell.Current.CurrentPage.ShowPopupAsync<bool>(popup);
            if (result == null || result.WasDismissedByTappingOutsideOfPopup || !result.Result)
                return;

            await _dbService.DeleteAllAsync(TasksToDeleteList);

            foreach (var task in TasksToDeleteList)
                _allTasksCache.Remove(task);  //Sync cache

            TasksToDeleteList.Clear();
            await Toast.Make("Selected tasks have been removed successfully!").Show();
            await ApplyFilterAsync();
        }
    }
}
