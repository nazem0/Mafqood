﻿namespace Domain.Entities
{
    public class BaseModel
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}
