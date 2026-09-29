using DirectoryService.Domain.Utilit;

namespace DirectoryService.Domain.ValueObject;

// Объект для описания позиции
public record Description
{
    public const int maxLength = 500;

    public NotEmptyString Value { get; }

    private Description (NotEmptyString value)
    {
        Value = value;
    }

    public static Description Create (string value)
    {
        return new Description (NotEmptyString.Create(value, maxLength));
    }
}