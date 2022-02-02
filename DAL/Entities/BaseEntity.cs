namespace DAL.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public bool MarkToDelete { get; set; } = false;
}