namespace SimplyTodosApp
{
    public partial class MainPage : ContentPage
    {
        string activated = "todos";

        public MainPage()
        {
            InitializeComponent();
        }

        private void todosBtn_Clicked(object sender, EventArgs e)
        {
            if (activated != "todos")
            {
                activated = "todos";
                todosBtn.BackgroundColor = Colors.Gray;
                todosBtn.TextColor = Colors.White;
                completionsBtn.BackgroundColor = Colors.GhostWhite;
                completionsBtn.TextColor = Colors.Black;
            }

        }

        private void completionsBtn_Clicked(object sender, EventArgs e)
        {
            if (activated != "completions")
            {
                activated = "completions";
                todosBtn.BackgroundColor = Colors.GhostWhite;
                todosBtn.TextColor = Colors.Black;
                completionsBtn.BackgroundColor = Colors.Gray;
                completionsBtn.TextColor = Colors.White;
            }
        }

        private void addTaskBtn_Clicked(object sender, EventArgs e)
        {
            DisplayAlertAsync("Notification", "A new task has been added", "Okay");
        }
    }
}
