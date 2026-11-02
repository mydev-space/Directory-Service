using System;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObject;
using DirectoryService.Domain.Abstractions;

namespace DirectoryService.Application.Positions;

public class PositionService
{
    private readonly IPositionRepository _positionRepository;

    public PositionService(IPositionRepository positionRepository)
    {
        _positionRepository = positionRepository;
    }

    // Создание должности
    public Position Create(string nameText, string descriptionText)
    {
        var name = Name.Create(nameText);
        var description = Description.Create(descriptionText);

        // Проверяем уникальность названия
        if (_positionRepository.ExistsByName(name))
        {
            throw new InvalidOperationException($"Должность '{name.Value.Value}' уже существует");
        }

        var position = Position.CreateNew(name, description);

        _positionRepository.Add(position);

        return position;
    }

    // Переименование должности
    public void Rename(PositionId id, string nameText)
    {
        var position = _positionRepository.GetById(id);

        if (position is null)
        {
            throw new InvalidOperationException($"Должность с ID '{id.Value}' не найдена");
        }

        // Проверяем уникальность нового названия
        var newName = Name.Create(nameText);

        // Не допускаем дубликаты, исключая текущюю должность
        if (_positionRepository.ExistsByNameExceptId(newName, id))
        {
            throw new InvalidOperationException($"Должность '{newName.Value.Value}' уже существует");
        }

        // Сущность проверит, что должность не архивирована
        position.Rename(newName);

        // Передаем измененную должность на сохранение
        _positionRepository.Update(position);
    }
}