using ArtClass.Mobile.Services;
using ArtistryApp.Model;
using ArtistryApp.Models;
using System.Globalization;

namespace ArtistryApp.Views;

public partial class CalendarPage : ContentPage
{
    private readonly CalendarService _calendarService;
    private readonly ClassStudentService _classStudentService;
    public CalendarPage(CalendarService calendarService,
        ClassStudentService classStudentService)
	{
		InitializeComponent();
        _calendarService = calendarService;
        _classStudentService = classStudentService;
        SessionDatePicker.Date = DateTime.Today;
        SessionStartTimePicker.Time = new TimeSpan(9, 0, 0);
        SessionEndTimePicker.Time = new TimeSpan(10, 0, 0);
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadCalendarData();
    }

    private async Task LoadCalendarData()
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            EmptyContainer.IsVisible = false;

            var events = await _calendarService.GetCalendarDataAsync();

            CalendarCollection.ItemsSource = events;

            EmptyContainer.IsVisible = events.Count == 0;
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Calendar Error",
                ex.Message,
                "OK");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
        }
    }
    private async void OnClassSelected(
        object sender,
        SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not CalendarEvent selectedClass)
            return;

        // Clear selection so the same class can be clicked again.
        CalendarCollection.SelectedItem = null;

        var sessionId = selectedClass.Id;

        var studentPage = new ClassStudentPage(
            _classStudentService,
            sessionId,
            selectedClass.ClassName ?? "Class Students");

        await Navigation.PushAsync(studentPage);
    }

    private async void AddSession_Clicked(object sender, EventArgs e)
    {
        try
        {
            CalendarCollection.IsVisible = false;
            EmptyContainer.IsVisible = false;
            AddSessionForm.IsVisible = true;

            TeacherPicker.ItemsSource =
                await _calendarService.GetTeachersAsync();

            TeacherPicker.SelectedItem = null;
        }
        catch (Exception ex)
        {
            AddSessionForm.IsVisible = false;
            CalendarCollection.IsVisible = true;

            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void CancelSession_Clicked(object sender, EventArgs e)
    {
        AddSessionForm.IsVisible = false;
        CalendarCollection.IsVisible = true;
    }

    private async void SaveSession_Clicked(object sender, EventArgs e)
    {
        string name = SessionNameEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(name))
        {
            await DisplayAlert("Validation",
                "Please enter session name.", "OK");
            return;
        }

        if (TeacherPicker.SelectedItem is not TeacherResponse teacher)
        {
            await DisplayAlert("Validation","Please select a teacher.", "OK");
            return;
        }

        if (SessionEndTimePicker.Time <= SessionStartTimePicker.Time)
        {
            await DisplayAlert("Validation","End time must be later than start time.", "OK");
            return;
        }

        try
        {
            SaveSessionButton.IsEnabled = false;
            SaveSessionButton.Text = "Saving...";
            var request = new SessionCreateRequest
            {
                Name = name,
                Description = SessionDescriptionEditor.Text?.Trim(),
                TeacherId = teacher.Id,
                Date = SessionDatePicker.Date.Value.ToString("yyyy-MM-dd",CultureInfo.InvariantCulture),
                StartTime = SessionStartTimePicker.Time.Value.ToString(@"hh\:mm"),
                EndTime = SessionEndTimePicker.Time.Value.ToString(@"hh\:mm"), 
                CreatedBy = teacher.Id
            };

            await _calendarService.CreateSessionAsync(request);
            await DisplayAlert("Success","Session created successfully.","OK");

            // Clear the form
            SessionNameEntry.Text = "";
            SessionDescriptionEditor.Text = "";
            TeacherPicker.SelectedItem = null;
            SessionDatePicker.Date = DateTime.Today;
            SessionStartTimePicker.Time = new TimeSpan(9, 0, 0);
            SessionEndTimePicker.Time = new TimeSpan(10, 0, 0);

            AddSessionForm.IsVisible = false;

            // Show calendar again
            CalendarCollection.IsVisible = true;

            // Refresh calendar data
            await LoadCalendarData();            
        }
        catch (Exception ex)
        {
            await DisplayAlert("Session Error",ex.Message,"OK");
        }
        finally
        {
            SaveSessionButton.IsEnabled = true;
            SaveSessionButton.Text = "Save Session";
        }
    }
}