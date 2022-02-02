namespace BLLAdapter.Models;

public abstract class ModelBase
{
    public Guid Id { get; set; }
    public bool MarkToDelete { get; set; } = false;
}