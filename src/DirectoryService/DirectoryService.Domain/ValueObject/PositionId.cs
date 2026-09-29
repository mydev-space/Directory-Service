namespace DirectoryService.Domain.ValueObject;

// Типизированный идентификатор позиции
public record PositionId
{
    public Guid Value { get; }

    private PositionId(Guid value)
    {
        Value = value;
    }

    public static PositionId Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Идентификатор позиции не может быть пустым");
        }
        return new PositionId(value);
    }
    public static PositionId New()
    {
        return new PositionId(Guid.NewGuid());
    }
}
