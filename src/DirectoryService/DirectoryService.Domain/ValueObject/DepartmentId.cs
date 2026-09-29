namespace DirectoryService.Domain.ValueObject;

// Типизированный идентификатор подразделения
public record DepartmentId
{
    public Guid Value { get; }

    private DepartmentId (Guid value)
    {
        Value = value;
    }

    public static DepartmentId Create (Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Идентификатор подразделения не может быть пустым");
        }
        return new DepartmentId (value);
    }
    public static DepartmentId New()
    {
        return new DepartmentId(Guid.NewGuid());
    }
}