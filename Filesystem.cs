namespace Yui;

public class Filesystem
{
	private Filesystem(string path)
	{
		BaseDir = new DirectoryInfo(Path.GetFullPath(path));
	}
	public DirectoryInfo BaseDir { get; private set; }
}