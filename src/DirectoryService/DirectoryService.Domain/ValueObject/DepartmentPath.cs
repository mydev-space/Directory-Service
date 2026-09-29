using DirectoryService.Domain.Utilit;

namespace DirectoryService.Domain.ValueObject;

// Объект для пути подразделения в иерархии
// Например: it.marketing.sales
public record DepartmentPath
{
    public const int maxLength = 500;

    public NotEmptyString Value { get; }

    private DepartmentPath(NotEmptyString value)
    {
        Value = value;
    }

    public static DepartmentPath Create(string value)
    {
        return new DepartmentPath(NotEmptyString.Create(value, maxLength));
    }

    public static DepartmentPath CreateRoot(Identifier identifier)
    {
        return new DepartmentPath(identifier.Value);
    }

    public static DepartmentPath CreateChild(DepartmentPath parentPath, Identifier identifier)
    {
        return new DepartmentPath(NotEmptyString.Create($"{parentPath.Value.Value}.{identifier.Value.Value}", maxLength));
    }
}