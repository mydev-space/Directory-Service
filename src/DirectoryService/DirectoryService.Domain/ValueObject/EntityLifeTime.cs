namespace DirectoryService.Domain.ValueObject;

// Объект для хранения дат создания, обновления и удаления
public readonly record struct EntityLifeTime
{
    public DateTime CreatedAt { get; }
    public DateTime? DeletedAt { get; }
    public DateTime UpdatedAt { get; }

    private EntityLifeTime(DateTime createdAt, DateTime? deletedAt, DateTime updatedAt)
    {
        CreatedAt = createdAt;
        DeletedAt = deletedAt;
        UpdatedAt = updatedAt;
    }

    // Создание нового объекта - дата создания и обновления = текущее время
    public static EntityLifeTime CreatedNew()
    {
        var now = DateTime.UtcNow;
        return new EntityLifeTime(now, null, now);
    }

    // Создание с явными датами (например, при загрузке из БД)
    public static EntityLifeTime Create(DateTime createdAt, DateTime? deletedAt, DateTime updatedAt)
    {
        if (createdAt == DateTime.MinValue)
            throw new ArgumentException("Некорректная дата создания");

        if (updatedAt == DateTime.MinValue)
            throw new ArgumentException("Некорректная дата обновления");

        if (updatedAt < createdAt)
            throw new ArgumentException("Дата обновления должна быть позже даты создания");

        if (deletedAt.HasValue)
        {
            if (deletedAt.Value == DateTime.MinValue)
                throw new ArgumentException("Некорректная дата удаления");

            if (deletedAt.Value < createdAt)
                throw new ArgumentException("Дата удаления должна быть позже даты создания");
        }

        return new EntityLifeTime(createdAt, deletedAt, updatedAt);
    }

    // Обновление даты изменения — возвращает новое значение с текущим временем
    public EntityLifeTime Update()
    {
        return new EntityLifeTime(CreatedAt, DeletedAt, DateTime.UtcNow);
    }

    // Удаление — фиксируем дату удаления
    public EntityLifeTime Delete()
    {
        return new EntityLifeTime(CreatedAt, DateTime.UtcNow, UpdatedAt);
    }

    public bool IsDeleted()
    {
        return DeletedAt.HasValue;
    }
}