using MainViewModel = SimplyTodosApp.ViewModels.MainViewModel;

namespace SimplyTodosApp
{
    public partial class MainPage : ContentPage
    {
        readonly string HowToUseInstructions= @"
1. Navigation: 
    - Use the buttons at the top to toggle between your 'To-dos' (ongoing tasks) and 'Completions' (completed tasks).

2. Creating a New Task: 
    - Tap the '+' button. 
    - Enter a Heading, Description, and select a Priority.
    - Tap 'Save'.

3. Reviewing Task: 
    - Tap any task in the list to review details.

4. Editing Task:
    - Use the Edit button (pen icon) to modify tasks.

5. Remove Task: 
    - Use the Remove button (trashbin icon) to modify tasks.

6. Marking Task as Completed: 
    - Tap the square box in the task row to mark it as done. 
    - It will automatically move to the Completions view.

7. Marking Task as Incomplete:
    - In the Completions view, tap the square box in the task row to mark it as undone.
    - It will automatically move to the To-dos view.

8. Removing Completed Tasks: 
    - In the Completions view, tap the circle on the right to select a completed task.
    - Use the Remove button (trashbin icon) to delete selected completed tasks.

Helpful tip: The task heading's color change according to task's priority. (i.e., Red for high proiority, Orange for medium priority, Black for low prioity.)";
    
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
            DisplayAlertAsync("User Guide", HowToUseInstructions, "Close");
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
