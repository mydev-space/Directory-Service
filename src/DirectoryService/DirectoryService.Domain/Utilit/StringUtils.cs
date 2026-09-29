using System.Linq;

namespace DirectoryService.Domain.Utilit;

// Статический класс для вспомогательных операций со строками
// Вынесен отдельно, чтобы не дублировать один и тот же код
// В разных объектах (Name, Description и т.д.)

public static class StringUtils
{
    // Обрезает пробелы по краям (Trim()),
    // Разбивает по пробелам (Split())
    // Убирает лишние пробелы внутри (RemoveEmptyEntries)
    // Собирает обратно в одну строку через пробел
    public static string Format (string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }
        var words = value.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return string.Join(" ", words.Select(word => word.Trim()));
    }
}