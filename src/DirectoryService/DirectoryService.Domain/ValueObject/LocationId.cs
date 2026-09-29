namespace DirectoryService.Domain.ValueObject;

// Типизированный идентификатор локации
public record LocationId
{
    public Guid Value { get; }

    private LocationId(Guid value)
    {
        Value = value;
    }

    public static LocationId Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Идентификатор локации не может быть пустым");
        }
        return new LocationId(value);
    }

    public static LocationId New()
    {
        return new LocationId (Guid.NewGuid());
    }
}