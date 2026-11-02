using System;
using DirectoryService.Domain.ValueObject;

namespace DirectoryService.Domain.Entities;

public class Location
{
    public LocationId Id { get; private set; }
    public Name Name { get; private set; }
    public Address Address { get; private set; }
    // Часовой пояс локации
    public IanaTimeZone TimeZone { get; private set; }
    // Активна локация или нет
    public bool IsActive { get; private set; }
    // Дата создания и обновления
    public EntityLifeTime EntityLifeTime { get; private set; }

    // Закрытый конструктор
    private Location(LocationId id, Name name, Address address, IanaTimeZone timezone, bool isActive, EntityLifeTime entityLifeTime)
    {
        Id = id;
        Name = name;
        Address = address;
        TimeZone = timezone;
        IsActive = isActive;
        EntityLifeTime = entityLifeTime;
    }
    // Фабричный метод - единственный метод создать локацию
    public static Location Create (Name name, Address address, IanaTimeZone timezone)
    {
        return new Location (LocationId.New(), name, address, timezone, isActive: true, EntityLifeTime.CreatedNew());
    }

    // Изменение названия локации
    public void Rename (Name newName)
    {
        EnsureIsActive();
        Name = newName;
        EntityLifeTime = EntityLifeTime.Update();
    }

    // Изменение временной зоны локации
    public void UpdateTimeZone (IanaTimeZone newTimeZone)
    {
        EnsureIsActive();
        TimeZone = newTimeZone;
        EntityLifeTime = EntityLifeTime.Update();
    }
    // Изменение адреса локации
    public void UpdateAddress (Address newAddress)
    {
        EnsureIsActive();
        Address = newAddress;
        EntityLifeTime = EntityLifeTime.Update();
    }
    // Деактивация локации
    public void Deactivate()
    {
        EnsureIsActive();
        IsActive = false;
        EntityLifeTime = EntityLifeTime.Update();
    }
    private void EnsureIsActive()
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("Нельзя изменить данные архивированной локации");
        }
    }
}
