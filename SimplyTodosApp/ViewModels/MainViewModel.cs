using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Alerts;
using SimplyTodosApp.Services;
using SimplyTodosApp.Views;
using System.Collections.ObjectModel;
using Task = SimplyTodosApp.Models.Task;

namespace SimplyTodosApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        readonly DatabaseService _dbService;

        private List<Task> _allTasksCache = new();  //Cache tasks in memory

        List<Task> TasksToDeleteList = new();   //List of selected completed-tasks to be removed in Completions View

        [ObservableProperty]
        ObservableCollection<Task> _tasksList = new();  //List of tasks to bind in MainPage

        [ObservableProperty]
        bool _showOnlyCompleted;    //For view control (Todos view or Completions view)

        [ObservableProperty]
        string _searchKeyword = string.Empty;   //Keyword in searchbar to filter tasks

        public MainViewModel(DatabaseService dbService)
        {
            _dbService = dbService;
            _ = LoadTasksFromDbAsync();
        }

        //For changing tasks to display when the Views change
        partial void OnShowOnlyCompletedChanged(bool value)
        {
            SearchKeyword = string.Empty;   //Clear search when switching views
            TasksToDeleteList.Clear();      //Clear selected task history
            _ = ApplyFilterAsync();
        }

        //Re-filter live as the user types
        partial void OnSearchKeywordChanged(string value)
        {
            _ = ApplyFilterAsync();
        }

        //Load tasks from database and cache them in memory
        async System.Threading.Tasks.Task LoadTasksFromDbAsync()
        {
            _allTasksCache = await _dbService.GetTasksAsync();
            await ApplyFilterAsync();
        }

        async System.Threading.Tasks.Task ApplyFilterAsync()
        {
            var keyword = SearchKeyword?.Trim() ?? string.Empty;

            var filtered = await System.Threading.Tasks.Task.Run(() =>
            {
                //Filter by current view (Todos or Completions)
                var byView = _showOnlyCompleted
                    ? _allTasksCache.Where(t => t.IsCompleted)
                    : _allTasksCache.Where(t => !t.IsCompleted);

                //Filter by keyword against Task heading and description (case-insensitive)
                //If keyword is empty, skip the string comparison
                if (!string.IsNullOrEmpty(keyword))
                    byView = byView.Where(t =>
                        (t.Heading?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (t.Description?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false));

                return byView.ToList();
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

            string textForToast = task.IsCompleted ? "Task has been marked as incomplete." : "Task has been marked as complete.";

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

            await _dbService.DeleteAllAsync(TasksToDeleteList); //remove from database

            foreach (var task in TasksToDeleteList)
                _allTasksCache.Remove(task);  //Sync cache

            TasksToDeleteList.Clear();
            await Toast.Make("Selected tasks have been removed successfully!").Show();
            await ApplyFilterAsync();
        }
    }
}