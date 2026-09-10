using System.Diagnostics;
using System.Reflection;
using Silk.NET.Windowing;
using Tomlyn;

namespace Yui;

public class Filesystem
{
	public Filesystem()
	{
		__init__($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/{Assembly.GetEntryAssembly()?.GetName().Name}");
	}
	private Filesystem __init__(string path)
	{
		Debug.Assert(FileObjects is not null, nameof(FileObjects) + " is not null");
		BaseDir = new DirectoryInfo(Path.GetFullPath(path));
		if (!BaseDir.Exists)
		{
			BaseDir.Create();
		}
		AddFileObject("Window", "config/window.toml");
		var files =
			from file in FileObjects.Values
			select file as FileInfo;
		foreach (var file in files)
		{
			Debug.Assert(file is not null, "f is not null");
			if (file.Exists) continue;
			File.WriteAllBytes(file.FullName, []);
		}
		return this;
	}
	private void AddFileObject(string name, string path)
	{
		path = BaseDir +"/"+path;
		Console.WriteLine(path);
		FileObjects.Add(name, new FileInfo(path));
		var parent = (FileObjects[name] as FileInfo)?.Directory!;
		if (!parent.Exists)
		{
			parent.Create();
		}
	}
	public DirectoryInfo BaseDir { get; private set; } = null!;
	private readonly Dictionary<string, FileSystemInfo> FileObjects = [];

	public Target? GetConfig<Target>(string FileObject, TomlSerializerOptions? opt = null) =>
		TomlSerializer.Deserialize<Target>(
			File.ReadAllText(GetPath(FileObject)), 
			opt
		);
	public void WriteConfig<T>(string name, T val, TomlSerializerOptions? opt = null)
    {
	    TomlSerializer.Serialize(
		    GetFile(name),
		    val,
		    opt
	    );
    }
    private string GetPath(string fileObject) => FileObjects[fileObject].FullName;
    private FileStream GetFile(string fileObject) => (FileObjects[fileObject] as FileInfo)?.Open(FileMode.OpenOrCreate)!;
}
