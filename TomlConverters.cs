using Silk.NET.Maths;
using Silk.NET.Windowing;
using Tomlyn.Serialization;

namespace Yui.Serial;

public class TomlEnumConv<E> : TomlConverter<E> where E : struct, Enum
{
	public override E Read(TomlReader reader) => Enum.Parse<E>(reader.GetString());
	public override void Write(TomlWriter writer, E value) => writer.WriteStringValue(value.ToString());
}

public class TomlVector2DConv : TomlConverter<Vector2D<int>>
{
	public override Vector2D<int> Read(TomlReader reader) =>
		new() {
			X = (int)reader.GetInt64(),
			Y = (int)reader.GetInt64()
		};
	public override void Write(TomlWriter writer, Vector2D<int> value)
	{
		writer.WriteStartInlineTable();
		writer.WritePropertyName("X");
		writer.WriteIntegerValue(value.X);
		writer.WritePropertyName("Y");
		writer.WriteIntegerValue(value.Y);
		writer.WriteEndInlineTable();
	}
}
public class TomlVector3DConv : TomlConverter<Vector3D<int>>
{
	public override Vector3D<int> Read(TomlReader reader) =>
		new(){
			X = (int)reader.GetInt64(),
			Y = (int)reader.GetInt64(),
			Z = (int)reader.GetInt64()
		};
	public override void Write(TomlWriter writer, Vector3D<int> value)
	{
		writer.WriteStartInlineTable();
		writer.WritePropertyName("X");
		writer.WriteIntegerValue(value.X);
		writer.WritePropertyName("Y");
		writer.WriteIntegerValue(value.Y);
		writer.WritePropertyName("Z");
		writer.WriteIntegerValue(value.Z);
		writer.WriteEndInlineTable();
	}
}
public class TomlVector4DConv : TomlConverter<Vector4D<int>>
{
	public override Vector4D<int> Read(TomlReader reader) =>
		new()
		{
			X = (int)reader.GetInt64(),
			Y = (int)reader.GetInt64(),
			Z = (int)reader.GetInt64(),
			W = (int)reader.GetInt64()
		};
	public override void Write(TomlWriter writer, Vector4D<int> value)
	{
		writer.WriteStartInlineTable();
		writer.WritePropertyName("X");
		writer.WriteIntegerValue(value.X);
		writer.WritePropertyName("Y");
		writer.WriteIntegerValue(value.Y);
		writer.WritePropertyName("Z");
		writer.WriteIntegerValue(value.Z);
		writer.WritePropertyName("W");
		writer.WriteIntegerValue(value.W);
		writer.WriteEndInlineTable();
	}
}
