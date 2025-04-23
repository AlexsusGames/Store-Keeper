public abstract class DataProvider 
{
    public abstract void Load();
    public abstract void Save();
    public virtual void OnCreate() { }
    public virtual void OnStart() { }
}
