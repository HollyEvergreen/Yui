using System.Reflection;

namespace Yui;

public class Filesystem
{
	public Filesystem()
	{
		string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/{Assembly.GetEntryAssembly()?.GetName().Name}";
		BaseDir = new DirectoryInfo(Path.GetFullPath(path));
		if (!BaseDir.Exists)
		{
			BaseDir.Create();
		}
	}
	private Filesystem(string path)
	{
		BaseDir = new DirectoryInfo(Path.GetFullPath(path));
	}
	public DirectoryInfo BaseDir { get; private set; }
}