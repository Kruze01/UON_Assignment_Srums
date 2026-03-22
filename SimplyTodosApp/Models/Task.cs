using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace SimplyTodosApp.Models
{
    public partial class Task : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } // index for database

        [ObservableProperty]
        private string _heading;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private string _priority;

        [ObservableProperty]
        private bool _isCompleted;
    }
    //    public enum TaskPriority
    //    {
    //        High = 1,
    //        Medium = 2,
    //        Low = 3,
    //    }
}
