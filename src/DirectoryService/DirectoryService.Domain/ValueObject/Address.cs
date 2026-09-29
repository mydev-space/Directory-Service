using DirectoryService.Domain.Utilit;

namespace DirectoryService.Domain.ValueObject;

// Объект для адреса локации
public record Address
{
    public const int maxLength = 200;

    public NotEmptyString Value { get; }

    private Address (NotEmptyString value)
    {
        Value = value;
    }

    public static Address Create(string value)
    {
        return new Address (NotEmptyString.Create(value, maxLength));
    }
}