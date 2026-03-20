using SimplyTodosApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Task = SimplyTodosApp.Models.Task;
using CommunityToolkit.Mvvm.Input;

namespace SimplyTodosApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Task> _tasksList = new ObservableCollection<Task>();

        public MainViewModel() {
            TasksList.Add(new Task { Heading = "Task1", Description = "NewDescription", Priority = "High", IsCompleted = false });
            TasksList.Add(new Task { Heading = "Task1", Description = "NewDescription", Priority = "High", IsCompleted = false });
            TasksList.Add(new Task { Heading = "Task1", Description = "NewDescription", Priority = "High", IsCompleted = false });
            TasksList.Add(new Task { Heading = "Task1", Description = "NewDescription", Priority = "High", IsCompleted = false });
            TasksList.Add(new Task { Heading = "Task1", Description = "NewDescription", Priority = "High", IsCompleted = false });
            TasksList.Add(new Task { Heading = "Task1", Description = "NewDescription", Priority = "High", IsCompleted = false });
            TasksList.Add(new Task { Heading = "Task1", Description = "NewDescription", Priority = "High", IsCompleted = false });
            TasksList.Add(new Task { Heading = "Task1", Description = "NewDescription", Priority = "High", IsCompleted = false });
            TasksList.Add(new Task { Heading = "Task1", Description = "NewDescription", Priority = "High", IsCompleted = false });
            TasksList.Add(new Task { Heading = "Task1", Description = "NewDescription", Priority = "High", IsCompleted = false });
            TasksList.Add(new Task { Heading = "Task1", Description = "NewDescription", Priority = "High", IsCompleted = false });
        }

        [RelayCommand]
        private void AddTask()
        {
            TasksList.Add(new Task { Heading = "New Task", Description = "Description hello how are you nice to meet you thanks haha nice ok great well played great well fafewbuefwfaefbbjbea beffabjef kafeabjebjfafkjefjbefaw", Priority = "High", IsCompleted = false });
        }
        
    }
};


