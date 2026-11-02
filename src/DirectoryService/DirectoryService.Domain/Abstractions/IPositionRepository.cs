using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObject;

namespace DirectoryService.Domain.Abstractions;

public interface IPositionRepository
{
    // Найти должность по ID
    Position? GetById (PositionId Id);

    // Проверять название при создании
    bool ExistsByName(Name name);

    // Проверять название при переименовании,
    // исключив текущую должность
    bool ExistsByNameExceptId(Name name, PositionId exceptId);

    // Добавить новую должность
    void Add(Position position);

    // Сохранить изменения существующей должности
    void Update(Position position);
}