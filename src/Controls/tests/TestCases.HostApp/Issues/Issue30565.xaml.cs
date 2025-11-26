using System.Globalization;

namespace Controls.TestCases.HostApp.Issues;

// Fake "login service"
internal static class FakeLogin {
	public static bool IsLoggedIn { get; private set; }

	public static event EventHandler LoggedIn;
	public static event EventHandler LoggedOut;

	public static async Task LoginAsync() {
		await Task.Delay(500);
		IsLoggedIn = true;
		LoggedIn?.Invoke(null, EventArgs.Empty);
	}

	public static async Task LogoutAsync() {
		await Task.Delay(500);
		IsLoggedIn = false;
		LoggedOut?.Invoke(null, EventArgs.Empty);
	}
}

[Issue(IssueTracker.Github, 30565,
	"[Android] Bottom navigation bar not updating correctly after tab visibility changes", PlatformAffected.Android)]
public partial class Issue30565 : Shell
{
	public Issue30565()
	{
		InitializeComponent();
		
		FakeLogin.LoggedIn += (_, _) => UpdateTabVisibility();
		FakeLogin.LoggedOut += (_, _) => UpdateTabVisibility();
	}
	
	public bool IsLoggedIn => FakeLogin.IsLoggedIn;
	
	private void UpdateTabVisibility() {
		OnPropertyChanged(nameof(IsLoggedIn));
	}
}

public class Issue30565InvertedBoolConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool b)
		{
			return !b;
		}

		return value;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}

public class Issue30565FillerPage : ContentPage {
	public Issue30565FillerPage()
	{
		Content = new Label {
			AutomationId = "FillerTabLabel",
			Text = "This is a filler tab"
		};
	}
}

public class Issue30565LoginPage : ContentPage {
	public Issue30565LoginPage()
	{
		var button = new Button() {
			AutomationId = "FillerTabLabel",
			Text = "Login",
		};

		button.Clicked += async (_, _) => await FakeLogin.LoginAsync();
		
		Content = button;
	}
}

public class Issue30565LogoutPage : ContentPage {
	public Issue30565LogoutPage()
	{
		var button = new Button() {
			AutomationId = "FillerTabLabel",
			Text = "Logout",
		};

		button.Clicked += async (_, _) =>
		{
			await FakeLogin.LogoutAsync();
			await Shell.Current.GoToAsync("///PretendLoginPage");
		};
		
		Content = button;
	}
}