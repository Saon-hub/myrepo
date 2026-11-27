using ArtistryApp.Views;

namespace ArtClass.Mobile.Views;

public partial class LoginPage : ContentPage
{
    private readonly CalendarPage _calendarPage;

    public LoginPage(CalendarPage calendarPage)
    {
        InitializeComponent();
        _calendarPage = calendarPage;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(_calendarPage);
    }

    private async void OnForgotPasswordClicked(
        object sender, TappedEventArgs e)
    {
        await DisplayAlert(
            "Forgot Password",
            "Password recovery will be implemented later.",
            "OK");
    }

    private async void OnGoogleLoginClicked(
        object sender, EventArgs e)
    {
        await DisplayAlert(
            "Google Login",
            "Google authentication will be integrated next.",
            "OK");
    }
}