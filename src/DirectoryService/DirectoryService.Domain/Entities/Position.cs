using DirectoryService.Domain.ValueObject;

namespace DirectoryService.Domain.Entities;

public class Position
{
    // Идентификатор
    public PositionId Id { get; private set; }
    // Название позиции
    public Name Name { get; private set; }
    // Описание позиции
    public Description Description { get; private set; }
    // Активна позиция или нет
    public bool IsActive { get; private set; }
    // Даты создания и обновления
    public EntityLifeTime EntityLifeTime { get; private set; }

    private Position(PositionId id, Name name, Description description, bool isActive, EntityLifeTime entityLifeTime)
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
        EntityLifeTime = entityLifeTime;
    }

    // Фабричный метод - единственный способ создать позицию
    public static Position CreateNew(Name name, Description description)
    {
        return new Position(PositionId.New(), name, description, isActive: true, EntityLifeTime.CreatedNew());
    }

    // Деактивация позиции
    public void Deactivate()
    {
        EnsureIsActive();
        IsActive = false;
        EntityLifeTime = EntityLifeTime.Update();
    }

    // Изменение описания позиции
    public void UpdateDescription(Description newDescription)
    {
        EnsureIsActive();
        Description = newDescription;
        EntityLifeTime = EntityLifeTime.Update();
    }

    // Переименование позиции
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
            throw new InvalidOperationException("Нельзя изменить архивированную должность");
        }
    }
}
