using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace ThoughtWorks.CruiseControl.Web
{
	public class ServerLog : Page
	{
		protected HtmlTableCell LogData;

		private void Page_Load(object sender, EventArgs e)
		{
			string serverLogFilename = GetServerLogFilenameFromConfig();
			string logData = ReadLinesFromLog(serverLogFilename, GetServerLogLinesFromConfig());
			LogData.InnerHtml += logData;
		}

		private string GetServerLogFilenameFromConfig()
		{
			return ConfigurationSettings.AppSettings["ServerLogFilePath"];
		}

		private int GetServerLogLinesFromConfig()
		{
			string configValue = ConfigurationSettings.AppSettings["ServerLogFileLines"];
			return (configValue != null) ? int.Parse(ConfigurationSettings.AppSettings["ServerLogFileLines"]) : 80;
		}

		private string ReadLinesFromLog(string filename, int lines)
		{
			string logFileData = new ServerLogFileReader(filename, lines).Read();
			return new HtmlLogFormatter().Format(logFileData);
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{    
			this.Load += new EventHandler(this.Page_Load);

		}
		#endregion
	}
}
