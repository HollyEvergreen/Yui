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
	private readonly Dictionary<string, FileInfo> FileObjects = [];
	private readonly Dictionary<string, FileInfo> DirectoryObjects = [];

	public Target? GetConfig<Target>(string FileObject, TomlSerializerOptions? opt = null) =>
		TomlSerializer.Deserialize<Target>(
			File.ReadAllText(GetFileObjectPath(FileObject)), 
			opt
		);
	public void WriteConfig<T>(string name, T val, TomlSerializerOptions? opt = null)
    {
	    TomlSerializer.Serialize(
		    GetFileObject(name),
		    val,
		    opt
	    );
    }
	private string GetFileObjectPath(string fileObject) => FileObjects[fileObject].FullName;
	private string GetPath(string path) => BaseDir + path;
    private FileStream GetFileObject(string fileObject) => (FileObjects[fileObject] as FileInfo)?.Open(FileMode.OpenOrCreate)!;
    public BinaryWriter OpenBin(string path) => new(GetFile(GetPath(path)));
    private static FileStream GetFile(string Path) => File.Open(Path, FileMode.OpenOrCreate);
    public void DeclareConfig(string name, string path)
    {
	    var file = new FileInfo(BaseDir +"/"+ path);
	    FileObjects.Add(name, file);
	    if (file.Directory is { Exists: true }) file.Directory.Create();
	    if (!file.Exists)
	    {
		    var f = file.CreateText();
		    f.Dispose();
		    f.Close();
	    }
    }
    public void Foreach(string Path, string pattern, Action<FileInfo> action) => new DirectoryInfo(BaseDir + Path).EnumerateFiles(pattern).ToList().ForEach(action);
    public void PrintConfig<T>(T val, TomlSerializerOptions opts) =>
	    TomlSerializer.Serialize(
		    Console.OpenStandardOutput(),
		    val,
		    opts
	    );
    public void WriteConfigAt<T>(string path, T config, TomlSerializerOptions opts)
    {
	    using var file = File.OpenWrite(GetPath(path));
	    TomlSerializer.Serialize(
		    file,
		    config,
		    opts
	    );
	    file.Flush(true);
    }
}
