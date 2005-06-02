using System.Windows.Forms;
using NUnit.Framework;
using ThoughtWorks.CruiseControl.CCTrayLib.Monitoring;
using ThoughtWorks.CruiseControl.CCTrayLib.Presentation;
using ThoughtWorks.CruiseControl.Remote;

namespace ThoughtWorks.CruiseControl.UnitTests.CCTrayLib.Presentation
{
	[TestFixture]
	public class ProjectStatusListViewItemAdaptorTest
	{
		[Test]
		public void CanCreateListViewItem()
		{
			TestingProjectMonitor projectMonitor = new TestingProjectMonitor( "projectName" );

			ProjectStatusListViewItemAdaptor adaptor = new ProjectStatusListViewItemAdaptor();
			ListViewItem item = adaptor.Create( projectMonitor );

			Assert.AreEqual( "projectName", item.Text );
			Assert.AreEqual( 0, item.ImageIndex );
		}


		[Test]
		public void WhenTheStateOfTheProjectChangesTheIconIsUpdated()
		{
			TestingProjectMonitor projectMonitor = new TestingProjectMonitor( "projectName" );
			ProjectStatusListViewItemAdaptor adaptor = new ProjectStatusListViewItemAdaptor();
			ListViewItem item = adaptor.Create( projectMonitor );

			Assert.AreEqual( "projectName", item.Text );
			Assert.AreEqual( 0, item.ImageIndex );

			projectMonitor.ProjectState = ProjectState.Building;

			projectMonitor.OnPolled( new MonitorPolledEventArgs( projectMonitor ) );

			Assert.AreEqual( "projectName", item.Text );
			Assert.AreEqual( ProjectState.Building.ImageIndex, item.ImageIndex );
		}

		[Test]
		public void WhenTheStateOfTheProjectChangesTheStatusEntriesOnTheListViewItemAreUpdated()
		{
			TestingProjectMonitor projectMonitor = new TestingProjectMonitor( "projectName" );
			projectMonitor.ProjectState = ProjectState.Building;
			projectMonitor.ProjectStatus = null;

			ProjectStatusListViewItemAdaptor adaptor = new ProjectStatusListViewItemAdaptor();
			ListViewItem item = adaptor.Create( projectMonitor );

			Assert.AreEqual(3, item.SubItems.Count);
			ListViewItem.ListViewSubItem activity = item.SubItems[1];
			ListViewItem.ListViewSubItem label = item.SubItems[2];

			Assert.AreEqual("", activity.Text);
			Assert.AreEqual("", label.Text);

			ProjectStatus status = new ProjectStatus();
			status.Activity = ProjectActivity.Sleeping;
			status.LastBuildLabel = "lastLabel";
			projectMonitor.ProjectStatus = status;

			projectMonitor.OnPolled( new MonitorPolledEventArgs( projectMonitor ) );

			Assert.AreEqual("Sleeping", activity.Text);
			Assert.AreEqual("lastLabel", label.Text);
			
		}

	}
}