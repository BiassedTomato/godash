public interface IDataSource
{
    public bool InitializeDeferred { get; set; }
    public void Initialize();
}