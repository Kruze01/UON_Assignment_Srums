using CommunityToolkit.Maui.Views;

namespace SimplyTodosApp.Views;

public partial class ConfirmationPopup : Popup<bool>
{
    public ConfirmationPopup(string HeadingText, string BodyText)
    {
        InitializeComponent();
        PopupHeading.Text = HeadingText;
        PopupBody.Text = BodyText;

    }
    async void OnCancelClicked(object sender, EventArgs e)
    {
        await CloseAsync(false);
    }

    async void OnConfirmClicked(object sender, EventArgs e)
    {
        await CloseAsync(true);
    }
}