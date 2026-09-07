namespace Yui;

public abstract class App
{
	protected Filesystem? fs;
	private double _dt;
	public int run()
	{
		on_load();

		Start();
		while (true)
		{
			var _s = DateTime.Now;
			update(_dt);
			render(_dt);
			_dt = (DateTime.Now - _s).Ticks;
		}
		return 0;
	}
	private void on_load()
	{
		fs = new Filesystem();
		OnLoad();
	}
	private void update(double dt)
	{
		Update(dt);
	}
	private void render(double dt)
	{
		Render(dt);
	}
	protected virtual void Start(){}
	protected virtual void OnLoad(){}
	protected virtual void Update(double dt){}
	protected virtual void Render(double dt){}
}