using System;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObject;
using DirectoryService.Domain.Abstractions;

namespace DirectoryService.Application.Departments;

public class DepartmentService
{
    private readonly IDepartmentRepository _repository;

    public DepartmentService(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    // Создание корневого подразделения
    public Department CreateRoot(string nameText, string identifierText)
    {
        var name = Name.Create(nameText);
        var identifier = Identifier.Create(identifierText);

        // Проверка уникальности названия по всей системе
        if (_repository.ExistsByName(name))
        {
            throw new InvalidOperationException( $"Подразделение '{name.Value.Value}' уже существует");
        }

        var department = Department.CreateRoot(name, identifier);

        _repository.Add(department);

        return department;
    }

    // Переименование подразделения
    public void Rename(DepartmentId id, string nameText)
    {
        var department = _repository.GetById(id);

        if (department == null)
        {
            throw new InvalidOperationException($"Подразделение с ID '{id}' не найдено");
        }

        var newName = Name.Create(nameText);

        // Проверка уникальности, исключая текущее подразделение
        if (_repository.ExistsByNameExceptId(newName, id))
        {
            throw new InvalidOperationException($"Подразделение '{newName.Value.Value}' уже существует");
        }
        // Метод сущности проверит, что подразделение активно
        department.Rename(newName);

        _repository.Update(department);
    }

    // Создание дочернего подразделения
    public Department CreateChild(DepartmentId parentId, string nameText, string identifierText)
    {
        // Находим родителя
        var parent = _repository.GetById(parentId);
        if (parent is null)
        {
            throw new InvalidOperationException($"Родитель подразделения с ID '{parentId}' не найден");
        }

        // Создаем объекты со значениями, прошедшими валидацию
        var name = Name.Create(nameText);
        var identifier = Identifier.Create(identifierText);

        // Проверяем уникальность названия по всей системе
        if (_repository.ExistsByName(name))
        {
            throw new InvalidOperationException($"Подразделение '{name.Value.Value}' уже существует");
        }

        // Доменная сущность проверит активность родителя
        var department = Department.CreateNewChildDepartment(name, identifier, parent);

        _repository.Add(department);

        return department;
    }

    // Присоединение существующего подразделения к родителю
    public void Attach(DepartmentId departmentId, DepartmentId parentId)
    {
        // Находим подразделение
        var department = _repository.GetById(departmentId);

        if (department is null)
        {
            throw new InvalidOperationException($"Подразделение с ID '{departmentId}' не найдено");
        }

        // Находим нового родителя
        var parent = _repository.GetById(parentId);

        if (parent is null)
        {
            throw new InvalidOperationException($"Родитель подразделения с ID '{parentId}' не найден");
        }

        // Доменная сущность проверит доступность родителя
        department.AttachToParent(parent);

        // Передаем измененное подразделение на сохранение
        _repository.Update(department);
    }
}