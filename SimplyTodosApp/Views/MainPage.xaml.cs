using MainViewModel = SimplyTodosApp.ViewModels.MainViewModel;

namespace SimplyTodosApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            TodayDate.Text = DateTime.Now.ToString("MMM dd, yyyy");
        }

        private void TodosBtn_Clicked(object sender, EventArgs e)
        {
            if (BindingContext is MainViewModel vm)
            {
                vm.ShowOnlyCompleted = false;
                TodosBtn.BackgroundColor = Colors.Gray;
                TodosBtn.TextColor = Colors.White;
                CompletionsBtn.BackgroundColor = Colors.GhostWhite;
                CompletionsBtn.TextColor = Colors.Black;
                AddTaskBtn.IsVisible = true;
                DeleteSelectedTasksBtn.IsVisible = false;
            }
        }

        private void CompletionsBtn_Clicked(object sender, EventArgs e)
        {
            if (BindingContext is MainViewModel vm)
            {
                vm.ShowOnlyCompleted = true;
                TodosBtn.BackgroundColor = Colors.GhostWhite;
                TodosBtn.TextColor = Colors.Black;
                CompletionsBtn.BackgroundColor = Colors.Gray;
                CompletionsBtn.TextColor = Colors.White;
                AddTaskBtn.IsVisible = false;
                DeleteSelectedTasksBtn.IsVisible = true;
            }
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlertAsync("How to...", "How to use this app pop-up here!", "Close");
        }

        private void TickIcon_Tapped(object sender, TappedEventArgs e)
        {
            if (sender is Border border)
            {
                var tick = border.FindByName("TickIcon");

                if (tick is Image img)
                {
                    img.Scale = img.Scale == 0 ? 1 : 0; //If Image scale is 0 -> change 1, and vice versa.. (Scale 0 = disappear)
                }
            }
        }
    }
}
