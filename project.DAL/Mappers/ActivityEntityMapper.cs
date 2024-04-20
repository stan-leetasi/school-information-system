using project.DAL.Entities;

namespace project.DAL.Mappers;

public class ActivityEntityMapper : IEntityMapper<ActivityEntity>
{
    public void MapToExistingEntity(ActivityEntity existingEntity, ActivityEntity newEntity)
    {
        existingEntity.BeginTime = newEntity.BeginTime;
        existingEntity.EndTime = newEntity.EndTime;
        existingEntity.Area = newEntity.Area;
        existingEntity.Type = newEntity.Type;
        existingEntity.Description = newEntity.Description;
        existingEntity.SubjectId = newEntity.SubjectId;
    }
}