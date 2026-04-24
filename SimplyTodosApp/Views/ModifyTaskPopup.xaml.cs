using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using Task = SimplyTodosApp.Models.Task;

namespace SimplyTodosApp.Views;

public partial class ModifyTaskPopup : Popup<Task>
{
    private readonly int _maxHeadingChars = 100;
    private readonly int _maxDescriptionChars = 1000;

    public ModifyTaskPopup(Task existingTask = null)
    {
        InitializeComponent();

        if (existingTask == null)   //if no parameter, create new task
        {
            PopupHeading.Text = "Add New Task";
            BindingContext = new Task { Priority = "Low", IsCompleted = false };
        }
        else                        // else, clone and edit existing content
        {
            PopupHeading.Text = "Edit Task";
            BindingContext = new Task 
            { 
                Id = existingTask.Id, 
                Heading = existingTask.Heading, 
                Description = existingTask.Description, 
                Priority = existingTask.Priority,
                IsCompleted = existingTask.IsCompleted 
            };
        }

        UpdateHeadingCount(HeadingEntry.Text);
        UpdateDescriptionCount(DescriptionEditor.Text);
    }

    private void OnHeadingTextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateHeadingCount(e.NewTextValue);
    }

    private void OnDescriptionTextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateDescriptionCount(e.NewTextValue);
    }

    private void UpdateHeadingCount(string text)
    {
        int count = text?.Length ?? 0;
        HeadingCountLabel.Text = $"{count}/{_maxHeadingChars} characters";
        HeadingCountLabel.TextColor = count > _maxHeadingChars || count == 0 ? Colors.Red : Colors.Black;
    }

    private void UpdateDescriptionCount(string text)
    {
        int count = text?.Length ?? 0;
        DescriptionCountLabel.Text = $"{count}/{_maxDescriptionChars} characters";
        DescriptionCountLabel.TextColor = count > _maxDescriptionChars ? Colors.Red : Colors.Black;
    }


    async void OnSaveClicked(object sender, EventArgs e)
    {
        var task = BindingContext as Task;
        
        int headLen = task?.Heading?.Length ?? 0;
        int descLen = task?.Description?.Length ?? 0;

        //Validating inputs
        bool isHeadingValid = headLen > 0 && headLen <= _maxHeadingChars;
        bool isDescriptionValid = descLen <= _maxDescriptionChars;

        if (!isHeadingValid || !isDescriptionValid)
        {

            if (!isDescriptionValid)
            {
                //Notify user
                await Toast.Make("The task description should not be exceeding 1000 characters.").Show();
            }

            if (!isHeadingValid)
            { 
                //Notify user
                await Toast.Make("The task heading should be between 1 to 100 characters.").Show();
            }

            try
            {
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(500));
            }
            catch (FeatureNotSupportedException)
            {
                System.Diagnostics.Debug.WriteLine("Vibration not supported on this device.");
            }
            
            return;
        }

        //If valid, return the task to the ViewModel
        await CloseAsync(BindingContext as Task);
    }
    async void OnCancelClicked(object sender, EventArgs e)
    {
        await CloseAsync(null);
    }

}