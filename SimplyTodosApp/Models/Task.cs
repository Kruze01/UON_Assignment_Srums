using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace SimplyTodosApp.Models
{
    public partial class Task : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } // Database needs a unique ID

        [ObservableProperty]
        private string _heading;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private string _priority;

        [ObservableProperty]
        private bool _isCompleted;
    }
}
