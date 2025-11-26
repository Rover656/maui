namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 30565, "[Android] Bottom navigation bar not updating correctly after tab visibility changes", PlatformAffected.Android)]
public class Issue30565 : Shell
{
	// Tabs representing the "logged in" state - 5 tabs visible when logged in
	readonly ShellContent _homeTab;
	readonly ShellContent _searchTab;
	readonly ShellContent _favoritesTab;
	readonly ShellContent _profileTab;
	readonly ShellContent _settingsTab;

	// Login tab - visible only when logged out
	readonly ShellContent _loginTab;

	readonly Label _statusLabel;

	public Issue30565()
	{
		_statusLabel = new Label
		{
			AutomationId = "StatusLabel",
			Text = "Not logged in"
		};

		var loginButton = new Button
		{
			AutomationId = "LoginButton",
			Text = "Login"
		};
		loginButton.Clicked += OnLoginClicked;

		var logoutButton = new Button
		{
			AutomationId = "LogoutButton",
			Text = "Logout"
		};
		logoutButton.Clicked += OnLogoutClicked;

		// Create the login tab content
		var loginPage = new ContentPage
		{
			Title = "Login",
			Content = new StackLayout
			{
				Children =
				{
					_statusLabel,
					loginButton
				}
			}
		};

		_loginTab = new ShellContent
		{
			Title = "Login",
			AutomationId = "LoginTab",
			Content = loginPage
		};

		// Create 5 tabs for the "logged in" state - this matches the Android bottom navigation bar limit
		_homeTab = CreateTab("Home", "HomeTab", logoutButton);
		_searchTab = CreateTab("Search", "SearchTab");
		_favoritesTab = CreateTab("Favorites", "FavoritesTab");
		_profileTab = CreateTab("Profile", "ProfileTab");
		_settingsTab = CreateTab("Settings", "SettingsTab");

		var tabBar = new TabBar();
		tabBar.Items.Add(_loginTab);
		tabBar.Items.Add(_homeTab);
		tabBar.Items.Add(_searchTab);
		tabBar.Items.Add(_favoritesTab);
		tabBar.Items.Add(_profileTab);
		tabBar.Items.Add(_settingsTab);

		Items.Add(tabBar);

		// Start in "logged out" state - only show login tab
		SetLoggedOutState();
	}

	ShellContent CreateTab(string title, string automationId, Button logoutButton = null)
	{
		var content = new StackLayout
		{
			Children =
			{
				new Label
				{
					AutomationId = $"{automationId}Label",
					Text = $"This is the {title} tab"
				}
			}
		};

		if (logoutButton != null)
		{
			content.Children.Add(logoutButton);
		}

		return new ShellContent
		{
			Title = title,
			AutomationId = automationId,
			Content = new ContentPage
			{
				Title = title,
				Content = content
			}
		};
	}

	void OnLoginClicked(object sender, EventArgs e)
	{
		SetLoggedInState();
	}

	void OnLogoutClicked(object sender, EventArgs e)
	{
		SetLoggedOutState();
	}

	void SetLoggedInState()
	{
		// Hide login tab, show all 5 main tabs
		_loginTab.IsVisible = false;
		_homeTab.IsVisible = true;
		_searchTab.IsVisible = true;
		_favoritesTab.IsVisible = true;
		_profileTab.IsVisible = true;
		_settingsTab.IsVisible = true;
		_statusLabel.Text = "Logged in";

		// Navigate to home tab
		CurrentItem = _homeTab;
	}

	void SetLoggedOutState()
	{
		// Show login tab, hide all main tabs
		_loginTab.IsVisible = true;
		_homeTab.IsVisible = false;
		_searchTab.IsVisible = false;
		_favoritesTab.IsVisible = false;
		_profileTab.IsVisible = false;
		_settingsTab.IsVisible = false;
		_statusLabel.Text = "Not logged in";

		// Navigate to login tab
		CurrentItem = _loginTab;
	}
}
