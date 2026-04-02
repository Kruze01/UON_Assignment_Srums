using CommunityToolkit.Maui.Views;
using Task = SimplyTodosApp.Models.Task;

namespace SimplyTodosApp.Views;

public partial class ReviewTaskPopup : Popup
{
    public ReviewTaskPopup(Task task)
    {
        InitializeComponent();
        BindingContext = task;
    }

    async void OnCloseClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}