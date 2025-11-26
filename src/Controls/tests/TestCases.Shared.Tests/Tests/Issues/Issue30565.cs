using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue30565 : _IssuesUITest
{
	public Issue30565(TestDevice device) : base(device)
	{
	}

	public override string Issue => "[Android] Bottom navigation bar not updating correctly after tab visibility changes";

	[Test]
	[Category(UITestCategories.Shell)]
	public void BottomNavigationBarUpdatesCorrectlyAfterTabRemoval()
	{
		// Wait for the login tab to be visible
		App.WaitForElement("LoginButton");

		// Tap the login button to switch to the "logged in" state with 5 tabs
		App.Tap("LoginButton");

		// Wait for the Home tab content to appear
		App.WaitForElement("HomeTabLabel");

		// Verify we can see the Home tab content
		var homeLabel = App.FindElement("HomeTabLabel");
		Assert.That(homeLabel.GetText(), Is.EqualTo("This is the Home tab"));

		// Verify that the bottom navigation shows exactly 5 tabs (not a "More" tab)
		// The bug was that the "More" tab appeared even though there were only 5 tabs
		// After the fix, the 5 tabs should be: Home, Search, Favorites, Profile, Settings
		
		// Tap on each tab to verify they are accessible
		App.Tap("SearchTab");
		App.WaitForElement("SearchTabLabel");

		App.Tap("FavoritesTab");
		App.WaitForElement("FavoritesTabLabel");

		App.Tap("ProfileTab");
		App.WaitForElement("ProfileTabLabel");

		App.Tap("SettingsTab");
		App.WaitForElement("SettingsTabLabel");

		// Now log out and log back in to ensure the navigation updates correctly
		// Navigate back to Home tab since logout button is only on that tab
		App.Tap("HomeTab");
		App.WaitForElement("LogoutButton");
		App.Tap("LogoutButton");

		// Verify we're back at the login screen
		App.WaitForElement("LoginButton");

		// Log in again - this is where the bug would manifest
		App.Tap("LoginButton");

		// Wait for Home tab to appear
		App.WaitForElement("HomeTabLabel");

		// Verify all 5 tabs are still accessible (no "More" tab bug)
		App.Tap("SearchTab");
		App.WaitForElement("SearchTabLabel");

		App.Tap("FavoritesTab");
		App.WaitForElement("FavoritesTabLabel");

		App.Tap("ProfileTab");
		App.WaitForElement("ProfileTabLabel");

		App.Tap("SettingsTab");
		App.WaitForElement("SettingsTabLabel");
	}
}
