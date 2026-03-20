using CommunityToolkit.Mvvm.ComponentModel;

namespace SimplyTodosApp.Models
{
    public partial class Task : ObservableObject
    {
        [ObservableProperty]
        private string _heading;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private string _priority;

        [ObservableProperty]
        private Boolean _isCompleted;

    }
//    public enum TaskPriority
//    {
//        High = 1,
//        Medium = 2,
//        Low = 3,
//    }
}
