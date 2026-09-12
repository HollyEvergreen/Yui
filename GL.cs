using System.Diagnostics;
using Silk.NET;
using Silk.NET.OpenGL;

namespace Yui;

public static class OGL
{
	public static GL? gl;

	public static void ClearFramebuffer()
	{
#if DEBUG
		Debug.Assert(gl != null, nameof(gl) + " != null");
#endif
		gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
	}
	
	public static uint CreateProgram(Span<uint> shaders)
	{
		Debug.Assert(gl != null, nameof(gl) + " != null");
		uint id = gl.CreateProgram();
		gl.AttachShader(id, shaders[0]);
		gl.AttachShader(id, shaders[1]);
		gl.LinkProgram(id);
		gl.DetachShader(id, shaders[0]);
		gl.DetachShader(id, shaders[1]);
		gl.DeleteShader(shaders[0]);
		gl.DeleteShader(shaders[1]);
		return id;
	}
	public static unsafe void SetEntryPoint(uint shader, ReadOnlySpan<byte> entrypoint)
	{
		gl?.SpecializeShader(shader, entrypoint, 0, (uint*)null, (uint*)null);
	}
	public static unsafe void SetEntryPoint(uint shader, string entrypoint)
	{
		gl?.SpecializeShader(shader, entrypoint, 0, null, null);
	}
	public static unsafe Span<uint> LoadShaderBinary(string path)
	{
		Span<byte> spirv = File.ReadAllBytes(path);
		Debug.Assert(gl != null);
		uint[] shaders = [gl.CreateShader(ShaderType.VertexShader), gl.CreateShader(ShaderType.FragmentShader)];
		fixed (void* spv = spirv)
		fixed (uint* _shaders = shaders)
			gl.ShaderBinary(2, _shaders, GLEnum.ShaderBinaryFormatSpirV, spv, (uint)spirv.Length);
		return shaders;
	}

	public static unsafe void DrawVAO(uint vao, uint ebo, uint count)
	{
		Debug.Assert(gl != null, nameof(gl) + " != null");
		gl.BindVertexArray(vao);
		gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, ebo);
		gl.DrawElements(PrimitiveType.Triangles, count, DrawElementsType.UnsignedInt, (void*)0);
		gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
		gl.BindVertexArray(0);
	}
	public static unsafe void VertexAttributePointer<T>(uint index, int vectorLen, uint TotalComponents, uint offset, bool enable = true) where T : unmanaged
	{
		Debug.Assert(gl != null, nameof(gl) + " != null");
		var glT = typeof(T) switch
		{
			{ Name: nameof(Byte) } => VertexAttribPointerType.Byte,
			{ Name: nameof(Int16) } => VertexAttribPointerType.Short,
			{ Name: nameof(Int32) } => VertexAttribPointerType.Int,
			{ Name: nameof(Half) } => VertexAttribPointerType.HalfFloat,
			{ Name: nameof(Single) } => VertexAttribPointerType.Float,
			{ Name: nameof(Double) } => VertexAttribPointerType.Double,
			_ => throw new($"Unsupported Type Param {nameof(T)}")
		};
		gl.VertexAttribPointer(index, vectorLen, glT, false, (uint)(TotalComponents * sizeof(T)), (nint)offset*sizeof(T));
		if (enable) gl.EnableVertexAttribArray(index);
	}
}