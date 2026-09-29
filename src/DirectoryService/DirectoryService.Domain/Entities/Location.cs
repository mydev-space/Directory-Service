using System;
using System.Collections.Generic;
using System.Text;
using DirectoryService.Domain.ValueObject;

namespace DirectoryService.Domain.Entities;

public class Location
{
    public LocationId Id { get; private set; }
    public Name Name { get; private set; }
    public Address Address { get; private set; }
    // Код локации
    public IanaTimeZone TimeZone { get; private set; }
    // Дата создания и удаления
    public EntityLifeTime EntityLifeTime { get; private set; }

    // Закрытый конструктор
    private Location(LocationId id, Name name, Address address, IanaTimeZone timezone, EntityLifeTime entityLifeTime)
    {
        Id = id;
        Name = name;
        Address = address;
        TimeZone = timezone;
        EntityLifeTime = entityLifeTime;
    }
    // Фабричный метод - единственный метод создать локацию
    public static Location Create (Name name, Address address, IanaTimeZone timezone)
    {
        return new Location (LocationId.New(), name, address, timezone, EntityLifeTime.CreatedNew());
    }
    // Изменение адреса локации
    public void UpdateAddress (Address newAddress)
    {
        Address = newAddress;
        EntityLifeTime = EntityLifeTime.Update();
    }
}
