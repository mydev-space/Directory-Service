using System.Text.RegularExpressions;

namespace DirectoryService.Domain.ValueObject;

// Объект для часового пояса в формате IANA
// Обязательно должен быть 1 разделитель "/"
// Например: Россия/Москва
public record IanaTimeZone
{
    private static readonly Regex Pattern = new(@"^[А-Яа-яЁё]+/[А-Яа-яЁё]+$", RegexOptions.Compiled);
    public string Value { get; }

    private IanaTimeZone (string value)
    {
        Value = value;
    }

    public static IanaTimeZone Create (string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Часовой пояс не может быть пустым");
        }

        var formatted = value.Trim();

        // Проверка на соответствие шаблону Страна/Город
        if (!Pattern.IsMatch(formatted))
        {
            throw new ArgumentException("Часовой пояс должен быть в формате Страна/Город");
        }
        return new IanaTimeZone (formatted);
    }
}