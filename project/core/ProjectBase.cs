using System.IO;
using Exortech.NetReflector;
using ThoughtWorks.CruiseControl.Core.Triggers;
using ThoughtWorks.CruiseControl.Remote;

namespace ThoughtWorks.CruiseControl.Core
{
	public abstract class ProjectBase
	{
		public static readonly string DefaultWorkingSubDirectory = "WorkingDirectory";
		public static readonly string DefaultArtifactSubDirectory = "Artifacts";

		private string _name;
		private string _configuredWorkingDirectory;
		private string _configuredArtifactDirectory;
		private ITrigger[] triggers = new ITrigger[] { new IntervalTrigger() };

		[ReflectorProperty("name")]
		public virtual string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		[ReflectorArray("triggers", Required=false)]
		public ITrigger[] Triggers
		{
			get { return triggers; }
			set { triggers = value; }
		}

		[ReflectorProperty("workingDirectory", Required=false)]
		public string ConfiguredWorkingDirectory
		{
			get { return _configuredWorkingDirectory; }
			set { _configuredWorkingDirectory = value; }
		}

		[ReflectorProperty("artifactDirectory", Required=false)]
		public string ConfiguredArtifactDirectory
		{
			get { return _configuredArtifactDirectory; }
			set { _configuredArtifactDirectory = value; }
		}

		public string WorkingDirectory
		{
			get
			{
				if (_configuredWorkingDirectory == null || _configuredWorkingDirectory == string.Empty)
				{
					return new DirectoryInfo(Path.Combine(Name, DefaultWorkingSubDirectory)).FullName;
				}
				else
				{
					return _configuredWorkingDirectory;
				}
			}
		}

		public string ArtifactDirectory
		{
			get
			{
				if (_configuredArtifactDirectory == null || _configuredArtifactDirectory == string.Empty)
				{
					return new DirectoryInfo(Path.Combine(Name, DefaultArtifactSubDirectory)).FullName;
				}
				else
				{
					return _configuredArtifactDirectory;
				}
			}
		}
	}
}
