using SimplyTodosApp.ViewModels;

namespace SimplyTodosApp
{
    public partial class MainPage : ContentPage
    {
        string activated = "todos";

        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
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

        //private void AddTaskBtn_Clicked(object sender, EventArgs e)
        //{
        //    DisplayAlertAsync("Notification", "A new task has been added successfully.", "Okay");
        //}

        private void Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlertAsync("Update","Edit button pop-up here!","Close");
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlertAsync("How to...", "How to use this app pop-up here!", "Close");
        }
    }
}
