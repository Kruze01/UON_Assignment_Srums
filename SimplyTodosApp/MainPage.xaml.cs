using System.Collections.ObjectModel;
using SimplyTodosApp.ViewModels;

namespace SimplyTodosApp
{
    public partial class MainPage : ContentPage
    {
        string activated = "todos";

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        private void TodosBtn_Clicked(object sender, EventArgs e)
        {
            if (activated != "todos")
            {
                activated = "todos";
                TodosBtn.BackgroundColor = Colors.Gray;
                TodosBtn.TextColor = Colors.White;
                CompletionsBtn.BackgroundColor = Colors.GhostWhite;
                CompletionsBtn.TextColor = Colors.Black;
            }

        }

        private void CompletionsBtn_Clicked(object sender, EventArgs e)
        {
            if (activated != "completions")
            {
                activated = "completions";
                TodosBtn.BackgroundColor = Colors.GhostWhite;
                TodosBtn.TextColor = Colors.Black;
                CompletionsBtn.BackgroundColor = Colors.Gray;
                CompletionsBtn.TextColor = Colors.White;
            }
        }

        private void AddTaskBtn_Clicked(object sender, EventArgs e)
        {
            DisplayAlertAsync("Notification", "A new task has been added successfully.", "Okay");
        }

    }
}
