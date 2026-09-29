using System.Text.RegularExpressions;
using DirectoryService.Domain.Utilit;

namespace DirectoryService.Domain.ValueObject;

// Объект для псевдонима подразделения
public record Identifier
{
    public const int maxLength = 50;

    public NotEmptyString Value { get; }

    private Identifier (NotEmptyString value)
    {
        Value = value;
    }

    public static Identifier Create (string value)
    {
        // Сначала общая проверка через NotEmptyString
        var formatted = NotEmptyString.Create (value, maxLength);

        // Дополнительная проверка - только латинские буквы
        if (!Regex.IsMatch(formatted.Value, "^[a-zA-Z]+$"))
        {
            throw new ArgumentException("Идентификатор может содержать только латиницу");
        }
        return new Identifier(formatted);
    }
}