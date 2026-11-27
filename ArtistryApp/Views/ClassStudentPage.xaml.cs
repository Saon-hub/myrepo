
using ArtClass.Mobile.Services;
using ArtistryApp.Model;
using ArtistryApp.Models;

namespace ArtistryApp.Views;

public partial class ClassStudentPage : ContentPage
{
    private readonly ClassStudentService _studentService;
    private readonly int _sessionId;

    private bool _isLoadingStudents;

    private readonly HashSet<int> _initializedCheckboxes = new();

    public ClassStudentPage(
        ClassStudentService studentService,
        int sessionId,
        string className)
    {
        InitializeComponent();

        _studentService = studentService;
        _sessionId = sessionId;

        ClassNameLabel.Text = className;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadStudents();
    }

    // Load registered students
    private async Task LoadStudents()
    {
        try
        {
            _isLoadingStudents = true;

            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            EmptyMessage.IsVisible = false;

            var students = await _studentService
                .GetStudentsBySessionAsync(_sessionId);

            StudentCollection.ItemsSource = students;

            StudentCountLabel.Text =
                $"{students.Count} Students";

            EmptyMessage.IsVisible = students.Count == 0;
            StudentCollection.IsVisible = students.Count > 0;

            await LoadAvailableStudents();
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Student Error",
                ex.Message,
                "OK");
        }
        finally
        {
            _isLoadingStudents = false;

            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
        }
    }

    // Initialize attendance checkbox
    private void AttendanceCheckBox_Loaded(
        object sender,
        EventArgs e)
    {
        if (sender is CheckBox checkBox &&
            checkBox.BindingContext is ClassStudent student)
        {
            _initializedCheckboxes.Add(student.AssociationId);
        }
    }

    // Update attendance
    private async void Attendance_CheckedChanged(
        object sender,
        CheckedChangedEventArgs e)
    {
        if (_isLoadingStudents)
            return;

        if (sender is not CheckBox checkBox ||
            checkBox.BindingContext is not ClassStudent student)
            return;

        if (!_initializedCheckboxes.Contains(student.AssociationId))
            return;

        bool newAttendance = e.Value;
        bool previousAttendance = !newAttendance;

        try
        {
            checkBox.IsEnabled = false;

            await _studentService.UpdateAttendanceAsync(
                student.AssociationId,
                newAttendance);

            student.Attendance = newAttendance;

            await DisplayAlert(
                "Attendance Saved",
                $"{student.StudentName} marked as {(newAttendance ? "Present" : "Absent")}.",
                "OK");
        }
        catch (Exception ex)
        {
            student.Attendance = previousAttendance;
            checkBox.IsChecked = previousAttendance;

            await DisplayAlert(
                "Attendance Error",
                $"Unable to update attendance.\n{ex.Message}",
                "OK");
        }
        finally
        {
            checkBox.IsEnabled = true;
        }
    }

    // Load students not yet associated with this session
    private async Task LoadAvailableStudents()
    {
        try
        {
            var students = await _studentService
                .GetAvailableStudentsBySessionAsync(_sessionId);

            AvailableStudentsPicker.ItemsSource = students;
            AvailableStudentsPicker.SelectedItem = null;

            AddStudentButton.IsEnabled = students.Count > 0;

            AvailableStudentsPicker.Title =
                students.Count > 0
                    ? "Select Student"
                    : "No available students";
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                $"Unable to load available students.\n{ex.Message}",
                "OK");
        }
    }

    // Add existing student to current session
    private async void AddStudent_Clicked(
        object sender,
        EventArgs e)
    {
        if (AvailableStudentsPicker.SelectedItem
            is not AvailableStudent selectedStudent)
        {
            await DisplayAlert(
                "Select Student",
                "Please select a student first.",
                "OK");

            return;
        }

        try
        {
            AddStudentButton.IsEnabled = false;
            AddStudentButton.Text = "Adding...";

            await _studentService.AddStudentToSessionAsync(
                _sessionId,
                selectedStudent.StudentId);

            await LoadStudents();

            await DisplayAlert(
                "Success",
                $"{selectedStudent.StudentName} added successfully.",
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                $"Unable to add student.\n{ex.Message}",
                "OK");
        }
        finally
        {
            AddStudentButton.Text = "+ Add Student";
            AddStudentButton.IsEnabled = true;
        }
    }

    // Create new student
    private async void CreateStudent_Clicked(object sender, EventArgs e)
    {
        string firstName = FirstNameEntry.Text?.Trim() ?? "";
        string lastName = LastNameEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(firstName))
        {
            await DisplayAlert("Validation", "Please enter first name.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            await DisplayAlert("Validation", "Please enter last name.", "OK");
            return;
        }

        try
        {
            CreateStudentButton.IsEnabled = false;
            CreateStudentButton.Text = "Creating...";

            int studentEntityId =
                await _studentService.GetStudentEntityIdAsync();

            var request = new UserCreateRequest
            {
                Id = 0,
                FirstName = firstName,
                LastName = lastName,
                MiddleName = MiddleNameEntry.Text?.Trim(),
                Comments = CommentsEditor.Text?.Trim(),
                PhoneNumber = PhoneNumberEntry.Text?.Trim(),
                Email = EmailEntry.Text?.Trim(),
                LoginId = Guid.NewGuid().ToString(),
                UserTypeId = studentEntityId,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = 0,
                ModifiedDate = DateTime.UtcNow
            };

            // Create student only
            await _studentService.CreateStudentAsync(request);

            // Clear form
            FirstNameEntry.Text = string.Empty;
            MiddleNameEntry.Text = string.Empty;
            LastNameEntry.Text = string.Empty;
            PhoneNumberEntry.Text = string.Empty;
            EmailEntry.Text = string.Empty;
            CommentsEditor.Text = string.Empty;

            // Refresh available students dropdown
            await LoadAvailableStudents();

            // Refresh existing class student list
            await LoadStudents();

            await DisplayAlert(
                "Success",
                $"{firstName} {lastName} created successfully. You can now add the student to this class using the Add Existing Student section.",
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Create Student Error",
                ex.Message,
                "OK");
        }
        finally
        {
            CreateStudentButton.Text = "+ Create New Student";
            CreateStudentButton.IsEnabled = true;
        }
    }
}
