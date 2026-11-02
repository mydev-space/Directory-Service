using System;
using System.Collections.Generic;
using System.Linq;
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
    // Связанные должности
    private readonly List <Position> positions = new();

    public IReadOnlyCollection <Position> Positions()
    {
        return positions.AsReadOnly();
    }

    // Связанные локации
    private readonly List <Location> locations = new();

    public IReadOnlyCollection <Location> Locations()
    {
        return locations.AsReadOnly();
    }


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
    public static Department CreateNewChildDepartment(Name name, Identifier identifier, Department parent)
    {
        if (parent is null)
        {
            throw new ArgumentNullException(nameof(parent), "Родитель не может быть пустым");
        }
        if (!parent.IsActive)
        {
            throw new InvalidOperationException("Нельзя создать ребенка у неактивного родителя");
        }
        return new Department(DepartmentId.New(), name, identifier, parentId: parent.Id, DepartmentPath.CreateChild(parent.Path, identifier), depth: (short)(parent.Depth + 1), isActive: true, EntityLifeTime.CreatedNew());
    }
    public void Rename(Name newName)
    {
        EnsureIsActive();

        Name = newName;
        EntityLifeTime = EntityLifeTime.Update();
    }
    private void EnsureIsActive()
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("Нельзя менять архивированное подразделение");
        }
    }

    // Добавление должности в подразделение
    public void AddPosition(Position position)
    {
        if (position is null)
        {
            throw new ArgumentNullException(nameof(position), "Должность не может быть пустой");
        }

        EnsureIsActive();

        if (positions.Any(p => p.Id.Equals(position.Id) || p.Name.Equals(position.Name)))
        {
            throw new InvalidOperationException("Должность с таким идентификатором или названием уже существует в этом подразделении");
        }
        positions.Add(position);
        EntityLifeTime = EntityLifeTime.Update();
    }
    // Добавление локации в подразделение
    public void AddLocation(Location location)
    {
        if (location is null)
        {
            throw new ArgumentNullException(nameof(location), "Локация не может быть пустой");
        }

        EnsureIsActive();

        if (locations.Any(l => l.Id.Equals(location.Id) || l.Address.Equals(location.Address)))
        {
            throw new InvalidOperationException("Локация с таким идентификатором или адресом уже существует в этом подразделении");
        }
        locations.Add(location);
        EntityLifeTime = EntityLifeTime.Update();
    }
    // Присоединение существующего подразделения к родителю
    // Обновляет ParentId, Path, Depth
    public void AttachToParent(Department parent)
    {
        if (parent is null)
        {
            throw new ArgumentNullException(nameof(parent), "Родитель не может быть пустым");
        }

        if (parent.Id == Id)
        {
            throw new InvalidOperationException("Нельзя быть родителем самому себе");
        }
        if (Path.IsAncestorOf(parent.Path))
        {
            throw new InvalidOperationException("Нельзя присоеденить подразделение к самому себе или своему потомку");
        }
        if (ParentId == parent.Id)
        {
            throw new InvalidOperationException("Подразделение уже присоеденено к этому родителю");
        }
        if (!parent.IsActive)
        {
            throw new InvalidOperationException("Нельзя присоедениться к неактивному родителю");
        }
        EnsureIsActive();
        ParentId = parent.Id;
        Path = DepartmentPath.CreateChild(parent.Path, Identifier);
        Depth = (short)(parent.Depth + 1);
        EntityLifeTime = EntityLifeTime.Update();
    }

    // Деактивация подразделения
    public void Deactivate()
    {
        EnsureIsActive();
        IsActive = false;
        EntityLifeTime = EntityLifeTime.Update();
    }
}