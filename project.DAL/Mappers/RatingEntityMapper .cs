using project.DAL.Entities;

namespace project.DAL.Mappers;

public class RatingEntityMapper : IEntityMapper<RatingEntity>
{
    public void MapToExistingEntity(RatingEntity existingEntity, RatingEntity newEntity)
    {
        existingEntity.Points = newEntity.Points;
        existingEntity.Notes = newEntity.Notes;
        existingEntity.ActivityId = newEntity.ActivityId;
        existingEntity.StudentId = newEntity.StudentId;
    }
}