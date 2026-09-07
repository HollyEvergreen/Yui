namespace Yui;

public abstract class App
{
	private int run()
	{
		Run();
		return 0;
	}
	private void on_load(Filesystem fs)
	{
		OnLoad(fs);
	}
	private void update(double dt)
	{
		Update(dt);
	}
	private void render(double dt)
	{
		Render(dt);
	}
	protected virtual void Run(){}
	protected virtual void OnLoad(Filesystem fs){}
	protected virtual void Update(double dt){}
	protected virtual void Render(double dt){}
}