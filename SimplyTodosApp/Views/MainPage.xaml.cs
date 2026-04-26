using MainViewModel = SimplyTodosApp.ViewModels.MainViewModel;

namespace SimplyTodosApp
{
    public partial class MainPage : ContentPage
    {
        readonly string HowToUseInstructions= @"
1. Navigation: 
    - Use the buttons at the top to toggle between your 'To-dos' (ongoing tasks) and 'Completions' (completed tasks).

2. Searching Task:
    - Write down the keywords in the searchbar(with magnifying glass icon) to search for the task you want.
    - Tap on the 'x' icon on the right to clean the searchbar.

3. Creating a New Task: 
    - Tap the Add button (plus icon). 
    - Enter a Heading, Description, and select a Priority.
    - Tap 'Save'.

4. Reviewing Task: 
    - Tap any task in the list to review details.

5. Editing Task:
    - Tap the Edit button (pen icon) to modify tasks.

6. Remove Task: 
    - Tap the Remove button (trashbin icon) to modify tasks.

7. Marking Task as Completed: 
    - Tap the square box in the task row to mark it as done. 
    - It will automatically move to the Completions view.

8. Marking Task as Incomplete:
    - In the Completions view, tap the square box in the task row to mark it as undone.
    - It will automatically move to the To-dos view.

9. Removing Completed Tasks: 
    - In the Completions view, tap the circle on the right to select a completed task.
    - Tap the Remove button (trashbin icon) to delete selected completed tasks.

Note: 
Character limitations for a task heading is 1 to 100 characters, and a task description is up to 1000 characters.

Helpful tip: 
The task heading's color change according to task's priority. (i.e., Red for high proiority, Orange for medium priority, Green for low priority, and Black for no prioity.)";
    
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
            }
        }

        private void UserGuideBtn_Clicked(object sender, EventArgs e)
        {
            DisplayAlertAsync("User Guide", HowToUseInstructions, "Close");
        }

        private void TickIcon_Tapped(object sender, TappedEventArgs e)
        {
            if (sender is Border border && border.Content is Image img)
            {
                img.ScaleToAsync(img.Scale == 0 ? 1 : 0, 150); //Animation of tick icon inside selection circle
            }
        }
    }
}
