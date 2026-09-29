using DirectoryService.Domain.Utilit;

namespace DirectoryService.Domain.ValueObject;

// Объект для названий (подразделения, локации, позиции)
public record Name
{
    public const int maxLength = 100;

    // NotEmptyString гарантирует, что значение всегда валидное
    public NotEmptyString Value { get; }

    private Name (NotEmptyString value)
    {
        Value = value;
    }

    // Фабричный метод - принимает строку, проверяет и создает Name
    public static Name Create(string value)
    {
        return new Name (NotEmptyString.Create(value, maxLength));
    }
}