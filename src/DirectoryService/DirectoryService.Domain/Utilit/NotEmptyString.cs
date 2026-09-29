namespace DirectoryService.Domain.Utilit;

// Общий объект для непустых строк
// Используется внутри Name, Description, Address, Identifier, Path —
// чтобы не писать одни и те же проверки в каждом из них
public record NotEmptyString
{
    public string Value { get; }

    // Приватный конструктор - объект можно будет создать только через фабричный метод Create
    private NotEmptyString(string value)
    {
        Value = value;
    }

    // Фабричный метод - единственный способ создать NotEmptyString
    public static NotEmptyString Create (string value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Строка не может быть пустой");
        }
        // Убираем лишние пробелы
        var formatted = StringUtils.Format(value);

        if (formatted.Length > maxLength)
        {
            throw new ArgumentException($"Строка не может быть длиннее {maxLength} символов");
        }
        return new NotEmptyString(formatted);
    }
}