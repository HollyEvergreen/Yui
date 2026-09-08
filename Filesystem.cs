using System.Reflection;
using Tomlyn;

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
		FileObjects?.Add("Window", new FileInfo(@""));
	}
	private Filesystem(string path)
	{
		BaseDir = new DirectoryInfo(Path.GetFullPath(path));
	}
	public DirectoryInfo BaseDir { get; private set; }
	private readonly Dictionary<string, FileObject> FileObjects = [];

    internal T? GetConfig<T>(string FileObject) => 
		TomlSerializer.
			Deserialize<T>(
				File.ReadAllText(
					GetPath(FileObject)
				)
			);
	private string GetPath(string fileObject)
	{
		var Path = FileObjects[fileObject].Obj.FullName;
		return BaseDir + Path;
	}
}
record FileObject(bool IsFile, FileSystemInfo Obj)
{
	public static implicit operator FileObject(FileInfo finfo)
	{
		return new(true, finfo);
	}
	public static implicit operator FileObject(DirectoryInfo dinfo)
	{
		return new(false, dinfo);
	}
	public static implicit operator FileInfo?(FileObject fobj)
	{
		return fobj.Obj as FileInfo;
	}
	public static implicit operator DirectoryInfo?(FileObject fobj)
	{
		return fobj.Obj as DirectoryInfo;
	}
}