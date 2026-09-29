using DirectoryService.Domain.ValueObject;

namespace DirectoryService.Domain.Entities;

public class Department
{
    // Идентификатор
    public DepartmentId Id { get; private set; }
    // Название подразделения
    public Name Name { get; private set; }
    // Псевдоним (только латиница)
    public Identifier Identifier { get; private set; }
    // ParentId = null означает, что это корневое подразделение
    public DepartmentId? ParentId { get; private set; }
    // Путь подразделения в иерархии
    public DepartmentPath Path { get; private set; }
    // Глубина подразделения на уровне иерархии
    public short Depth { get; private set; }
    // Активно или неактивно подразделение
    public bool IsActive { get; private set; }
    // Даты создания и обновления
    public EntityLifeTime EntityLifeTime { get; private set; }

    // Приватный конструктор - снаружи нельзя создать через new
    private Department(DepartmentId id, Name name, Identifier identifier, DepartmentId? parentId, DepartmentPath path, short depth, bool isActive, EntityLifeTime entityLifeTime)
    {
        Id = id;
        Name = name;
        Identifier = identifier;
        ParentId = parentId;
        Path = path;
        Depth = depth;
        IsActive = isActive;
        EntityLifeTime = entityLifeTime;
    }

    // Создание корневого подразделения
    // У него нет родителя, Path = Identifier, Depth = 0
    public static Department CreateRoot(Name name, Identifier identifier)
    {
        return new Department(DepartmentId.New(), name, identifier, parentId: null, DepartmentPath.CreateRoot(identifier), depth: 0, isActive: true, EntityLifeTime.CreatedNew());
    }

    // Создание дочернего подразделения
    // Path и Depth вычисляются автоматически от родителя
    public static Department CreateChild(Name name, Identifier identifier, Department parent)
    {
        if (parent is null)
        {
            throw new ArgumentException("Родитель не может быть пустым");
        }

        return new Department(DepartmentId.New(), name, identifier, parentId: parent.Id, DepartmentPath.CreateChild(parent.Path, identifier), depth: (short)(parent.Depth + 1), isActive: true, EntityLifeTime.CreatedNew());
    }

    // Присоединение существующего подразделения к родителю
    // Обновляет ParentId, Path, Depth
    public void AttachToParent(Department parent)
    {
        if (parent is null)
        {
            throw new ArgumentException("Родитель не может быть пустым");
        }

        if (parent.Id == Id)
        {
            throw new InvalidOperationException("Нельзя быть родителем самому себе");
        }

        ParentId = parent.Id;
        Path = DepartmentPath.CreateChild(parent.Path, Identifier);
        Depth = (short)(parent.Depth + 1);
        EntityLifeTime = EntityLifeTime.Update();
    }

    // Деактивация подразделения
    public void Deactivate()
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("Уже неактивно");
        }

        IsActive = false;
        EntityLifeTime = EntityLifeTime.Update();
    }
}