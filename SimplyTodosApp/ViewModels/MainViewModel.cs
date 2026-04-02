using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimplyTodosApp.Services;
using SimplyTodosApp.Views;
using System.Collections.ObjectModel;
using Task = SimplyTodosApp.Models.Task;

namespace SimplyTodosApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        readonly DatabaseService _dbService;

        [ObservableProperty]
        ObservableCollection<Task> _tasksList = new();  //List of tasks to bind in MainPage

        [ObservableProperty]
        bool _showOnlyCompleted;    //For task display control

        //For changing tasks to display when the Views change
        partial void OnShowOnlyCompletedChanged(bool value)
        {
            LoadTasks();
        }

        public MainViewModel(DatabaseService dbService)
        {
            _dbService = dbService;
            LoadTasks();
        }

        //For tasks filtering between Todos View and Completion View
        async void LoadTasks()
        {
            var allTasks = await _dbService.GetTasksAsync();
            TasksList.Clear();

            var filtered = _showOnlyCompleted ? allTasks.Where(t => t.IsCompleted) : allTasks.Where(t => !t.IsCompleted);
            foreach (var task in filtered)
                TasksList.Add(task);
        }

        //For adding a new task
        //Will call ModifyTaskPopup
        [RelayCommand]
        async System.Threading.Tasks.Task AddTask()
        {
            var popup = new ModifyTaskPopup();

            var result = await Shell.Current.CurrentPage.ShowPopupAsync<Task>(popup);

            if (!result.WasDismissedByTappingOutsideOfPopup && result.Result is Task newTask && !string.IsNullOrWhiteSpace(newTask.Heading))
            {
                //Save to SQLite
                await _dbService.SaveTaskAsync(newTask);

                //Add to UI list
                TasksList.Add(newTask);
            }
        }

        //For editing a new Task
        //Will call ModifyTaskPopup
        [RelayCommand]
        async System.Threading.Tasks.Task EditTask(Task taskToEdit)
        {
            var popup = new ModifyTaskPopup(taskToEdit);

            var result = await Shell.Current.CurrentPage.ShowPopupAsync(new ModifyTaskPopup(taskToEdit)) as IPopupResult<Task>;

            if (!result.WasDismissedByTappingOutsideOfPopup && result != null && result.Result != null)
            {
                var updatedTask = result.Result;
                taskToEdit.Heading = updatedTask.Heading;
                taskToEdit.Description = updatedTask.Description;
                taskToEdit.Priority = updatedTask.Priority;

                //Update in SQLite
                await _dbService.SaveTaskAsync(taskToEdit);

                //Refresh UI list
                LoadTasks();
            }
        }

        //For removing a new Task
        //Will call ConfirmationPopup
        [RelayCommand]
        async void DeleteTask(Task task)
        {
            if (task == null)
                return;

            var popup = new ConfirmationPopup("Remove Task", "Are you sure you want to Remove this task ?");
            var result = await Shell.Current.CurrentPage.ShowPopupAsync<bool>(popup);
            bool confirmed = result != null && !result.WasDismissedByTappingOutsideOfPopup && result.Result;
            if (!confirmed)
                return;

            //Remove from SQLite
            await _dbService.DeleteTaskAsync(task);

            //Remove from UI list
            TasksList.Remove(task);
        }

        //For changing a task's IsCompleted attribute
        //Called if a task's checkbox is clicked
        [RelayCommand]
        async void ToggleComplete(Task task)
        {
            if (task == null)
                return;

            //Update in SQLite
            task.IsCompleted = !task.IsCompleted;
            await _dbService.SaveTaskAsync(task);

            //Update in UI list
            LoadTasks();
        }

        //For reviewing a Task
        //Will call ReviewTaskPopup
        [RelayCommand]
        async System.Threading.Tasks.Task ReviewTask(Task task)
        {
            if (task == null)
                return;

            var popup = new ReviewTaskPopup(task);
            await Shell.Current.CurrentPage.ShowPopupAsync(popup);
        }

        //List of selected completed-tasks to be deleted in Completions View
        List<Task> TasksToDeleteList = new();

        [RelayCommand]
        async void SelectCompletedTask(Task task)
        {
            if (task == null)
                return;

            //if (TasksToDeleteList.Count == 0)
            //    TasksToDeleteList.Add(task);
            //else
            //{
                if (TasksToDeleteList.Contains(task))
                    TasksToDeleteList.Remove(task);
                else
                    TasksToDeleteList.Add(task);
            //}
        }

        //For removing all selected completed-tasks in Completions view
        //Will call ConfirmationPopup
        [RelayCommand]
        async void DeleteSelectedTasks(List<Task> TaskToDeleteList)
        {
            if (TasksToDeleteList.Count == 0)
                return;

            var popup = new ConfirmationPopup("Clear Tasks", "Are you sure you want to remove all selected tasks that you have already completed?");
            var result = await Shell.Current.CurrentPage.ShowPopupAsync<bool>(popup);
            bool confirmed = result != null && !result.WasDismissedByTappingOutsideOfPopup && result.Result;
            if (!confirmed)
                return;

            foreach (var task in TasksToDeleteList)
                await _dbService.DeleteTaskAsync(task);

            LoadTasks();
        }

    }
}
