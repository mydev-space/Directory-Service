using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObject;

namespace DirectoryService.Domain.Abstractions
{
    public interface IDepartmentRepository
    {
        // Найти подразделение по идентификатору
        Department? GetById(DepartmentId id);

        // Проверить существует ли подразделение с таким названием
        bool ExistsByName(Name name);

        // Проверить, название при переименовании, исключив текущее подразделение
        bool ExistsByNameExceptId(Name name, DepartmentId exceptId);

        // Добавить новое подразделение
        void Add(Department department);

        // Сохранить изменения подразделения
        void Update(Department department);
    }
}