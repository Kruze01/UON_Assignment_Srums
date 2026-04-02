using CommunityToolkit.Maui.Views;
using Task = SimplyTodosApp.Models.Task;

namespace SimplyTodosApp.Views;

public partial class ModifyTaskPopup : Popup<Task>
{
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
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        await CloseAsync(BindingContext as Task);
    }


    async void OnCancelClicked(object sender, EventArgs e)
    {
        await CloseAsync(null);
    }

}