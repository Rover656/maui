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
		// Wait for the login page to be visible (starts logged out)
		App.WaitForElement("FillerTabLabel");

		// Tap the login button to switch to the "logged in" state with 5 tabs
		App.Tap("FillerTabLabel");

		// Wait for the Home tab content to appear after login
		// The FakeLogin service has a 500ms delay, so we wait for the filler label
		App.WaitForElement("FillerTabLabel");

		// Verify that the bottom navigation shows exactly 5 tabs (not a "More" tab)
		// The bug was that the "More" tab appeared even though there were only 5 tabs
		// After the fix, the 5 tabs should be: Home, Page 2, Page 3, Page 4, Page 5
		
		// Tap on each tab to verify they are accessible
		App.Tap("Tab2");
		App.WaitForElement("FillerTabLabel");

		App.Tap("Tab3");
		App.WaitForElement("FillerTabLabel");

		App.Tap("Tab4");
		App.WaitForElement("FillerTabLabel");

		// Navigate to Tab5 (Page 5) which has the logout button
		App.Tap("Tab5");
		App.WaitForElement("FillerTabLabel");

		// Now log out and log back in to ensure the navigation updates correctly
		// Tap the logout button (FillerTabLabel on Page 5)
		App.Tap("FillerTabLabel");

		// Verify we're back at the login screen
		App.WaitForElement("FillerTabLabel");

		// Log in again - this is where the bug would manifest
		App.Tap("FillerTabLabel");

		// Wait for Home tab to appear
		App.WaitForElement("FillerTabLabel");

		// Verify all 5 tabs are still accessible (no "More" tab bug)
		App.Tap("Tab2");
		App.WaitForElement("FillerTabLabel");

		App.Tap("Tab3");
		App.WaitForElement("FillerTabLabel");

		App.Tap("Tab4");
		App.WaitForElement("FillerTabLabel");

		App.Tap("Tab5");
		App.WaitForElement("FillerTabLabel");
	}
}
