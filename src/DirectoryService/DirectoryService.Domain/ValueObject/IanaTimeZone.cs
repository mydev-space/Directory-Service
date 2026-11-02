using System;

namespace DirectoryService.Domain.ValueObject;

// Объект для часового пояса в формате IANA
// Например: Europe/London, Asia/Tokyo
public record IanaTimeZone
{
    public string Value { get; }

    private IanaTimeZone (string value)
    {
        Value = value;
    }

    public static IanaTimeZone Create (string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Часовой пояс не может быть пустым", nameof(value));
        }

        var formatted = value.Trim();

        // Проверка на соответствие шаблону Страна/Город
        if (!formatted.Contains('/'))
        {
            throw new ArgumentException("Укажите часовой пояс в формате IANA, например Europe/London", nameof(value));
        }

        TimeZoneInfo timeZoneInfo;

        try
        {
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(formatted);
        }
        catch (TimeZoneNotFoundException ex)
        {
            throw new ArgumentException($"Часовой пояс {formatted} не найден", nameof(value), ex);
        }
        catch (InvalidTimeZoneException ex)
        {
            throw new ArgumentException($"Часовой пояс {formatted} имеет некорректные данные", nameof(value), ex);
        }
        return new IanaTimeZone (timeZoneInfo.Id);
    }
}