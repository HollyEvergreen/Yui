using Silk.NET.Maths;
using Silk.NET.Windowing;
using Tomlyn.Serialization;

namespace Yui.Serial;

public class WindowConfig
{
	public static implicit operator WindowConfig(WindowOptions opts)
	{
		WindowConfig cfg = new()
		{
			API = opts.API,
			FramesPerSecond = opts.FramesPerSecond,
			IsContextControlDisabled = opts.IsContextControlDisabled,
			IsEventDriven = opts.IsEventDriven,
			IsVisible = opts.IsVisible,
			PreferredBitDepth = opts.PreferredBitDepth,
			PreferredDepthBufferBits = opts.PreferredDepthBufferBits,
			PreferredStencilBufferBits = opts.PreferredStencilBufferBits,
			Samples = opts.Samples,
			ShouldSwapAutomatically = opts.ShouldSwapAutomatically,
			Title = opts.Title,
			TransparentFramebuffer = opts.TransparentFramebuffer,
			UpdatesPerSecond = opts.UpdatesPerSecond,
			VSync = opts.VSync,
			Position = opts.Position,
			Size = opts.Size,
			VideoMode = opts.VideoMode,
			WindowBorder = opts.WindowBorder,
			TopMost = opts.TopMost,
			WindowClass = opts.WindowClass,
			WindowState = opts.WindowState
		};
		return cfg;
	}
	public static implicit operator WindowOptions(WindowConfig cfg)
	{
		var def = WindowOptions.Default;
		return WindowOptions.Default with // or 'new WindowOptions' if it's a struct/record
		{
			Title = cfg.Title ?? def.Title,
			WindowClass = cfg.WindowClass ?? def.WindowClass,
			API = cfg.API ?? def.API,
			Position = cfg.Position ?? def.Position,
			Size = cfg.Size ?? def.Size,
			VideoMode = cfg.VideoMode ?? def.VideoMode,
			WindowState = cfg.WindowState,
			WindowBorder = cfg.WindowBorder,
			FramesPerSecond = cfg.FramesPerSecond ?? def.FramesPerSecond,
			VSync = cfg.VSync ?? def.VSync,
			UpdatesPerSecond = cfg.UpdatesPerSecond ?? def.UpdatesPerSecond,
			PreferredDepthBufferBits = cfg.PreferredDepthBufferBits ?? def.PreferredDepthBufferBits,
			PreferredStencilBufferBits = cfg.PreferredStencilBufferBits ?? def.PreferredStencilBufferBits,
			PreferredBitDepth = cfg.PreferredBitDepth ?? new Vector4D<int>(8,8,8,8),
			Samples = cfg.Samples ?? def.Samples,
			IsVisible = cfg.IsVisible ?? def.IsVisible,
			ShouldSwapAutomatically = cfg.ShouldSwapAutomatically ?? def.ShouldSwapAutomatically,
			IsEventDriven = cfg.IsEventDriven ?? def.IsEventDriven,
			IsContextControlDisabled = cfg.IsContextControlDisabled ?? def.IsContextControlDisabled,
			TransparentFramebuffer = cfg.TransparentFramebuffer ?? def.TransparentFramebuffer
		};
	}
	public string? Title { get; init; }
	public string? WindowClass { get; init; }
	public GraphicsAPIConfig? API { get; init; }
	public Vector2DToml<int>? Position { get; init; }
	public Vector2DToml<int>? Size { get; init; }
	public VideoModeConfig? VideoMode { get; init; }
	[TomlConverter(typeof(TomlEnumConv<WindowState>))]
	public WindowState WindowState { get; init; }
	[TomlConverter(typeof(TomlEnumConv<WindowBorder>))]
	public WindowBorder WindowBorder { get; init; }
	public double? FramesPerSecond { get; init; }
	public bool? VSync { get; init; }
	public double? UpdatesPerSecond { get; init; }
	public int? PreferredDepthBufferBits { get; init; }
	public int? PreferredStencilBufferBits { get; init; }
	public Vector4DToml<int>? PreferredBitDepth { get; init; }
	public int? Samples { get; init; }
	public bool? IsVisible { get; init; }
	public bool? ShouldSwapAutomatically { get; init; }
	public bool? IsEventDriven { get; init; }
	public bool? IsContextControlDisabled { get; init; }
	public bool? TransparentFramebuffer { get; init; }
	public bool? TopMost { get; init; }
}

public class VideoModeConfig 
{
	public Vector2DToml<int>? Resolution { get; init; }
	public int? RefreshRate { get; init; }

	public static implicit operator VideoMode(VideoModeConfig cfg) => new(cfg.Resolution ?? new Vector2DToml<int>(1280, 720), cfg.RefreshRate ?? 144);
	public static implicit operator VideoModeConfig(VideoMode cfg) => new() {Resolution = cfg.Resolution, RefreshRate = cfg.RefreshRate};
}

public class GraphicsAPIConfig
{
	[TomlConverter(typeof(TomlEnumConv<ContextAPI>))]
	public ContextAPI API { get; init; }
	[TomlConverter(typeof(TomlEnumConv<ContextProfile>))]
	public ContextProfile Profile { get; init; }
	[TomlConverter(typeof(TomlEnumConv<ContextFlags>))]
	public ContextFlags Flags { get; init; }
	public APIVersionConfig Version { get; init; }

	public static implicit operator GraphicsAPIConfig(GraphicsAPI api) =>
		new() {
			API = api.API,
			Flags = api.Flags,
			Profile = api.Profile,
			Version = api.Version
		};
	public static implicit operator GraphicsAPI(GraphicsAPIConfig cfg) =>
		new() {
			API = cfg.API,
			Profile = cfg.Profile,
			Flags = cfg.Flags,
			Version = cfg.Version
		};
}

public class APIVersionConfig
{
	public int MajorVersion { get; init; }
	public int MinorVersion { get; init; }
	public static implicit operator APIVersion(APIVersionConfig cfg) => new()
		{ MajorVersion = cfg.MajorVersion, MinorVersion = cfg.MinorVersion };
	public static implicit operator APIVersionConfig(APIVersion cfg) => new()
		{ MajorVersion = cfg.MajorVersion, MinorVersion = cfg.MinorVersion };
}

public class Vector2DToml<T>(T X, T Y) where T : unmanaged, IFormattable, IEquatable<T>, IComparable<T>
{
	public T X { get; init; } = X;
	public T Y { get; init; } = Y;
	public static implicit operator Vector2D<T>(Vector2DToml<T> cfg) => new(cfg.X, cfg.Y);
	public static implicit operator Vector2DToml<T>(Vector2D<T> cfg) => new(cfg.X, cfg.Y);
}
public class Vector4DToml<T>(T X, T Y, T Z, T W)
	where T : unmanaged, IFormattable, IEquatable<T>, IComparable<T>
{
	public T X { get; init; } = X;
	public T Y { get; init; } = Y;
	public T Z { get; init; } = Z;
	public T W { get; init; } = W;
	public static implicit operator Vector4D<T>(Vector4DToml<T> cfg) => new(cfg.X, cfg.Y, cfg.Z, cfg.W);
	public static implicit operator Vector4DToml<T>(Vector4D<T> cfg) => new(cfg.X, cfg.Y, cfg.Z, cfg.W);
}