namespace project.App.Services;

public class AlertService : IAlertService
{
    public async Task DisplayAsync(string title, string message)
    {
        var displayAlert = Application.Current?.MainPage?.DisplayAlert(title, message, "OK");
        
        if (displayAlert is not null)
        {
            await displayAlert;
        }
    }

    public async Task<bool> ConfirmAsync(string title, string message)
    {
        var displayAlert = Application.Current?.MainPage?.DisplayAlert(title, message, "Confirm", "Cancel");

        if (displayAlert is not null)
        {
            return await displayAlert;
        }

        return await Task.FromResult(false);
    }
}
