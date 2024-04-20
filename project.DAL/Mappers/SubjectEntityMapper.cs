using project.DAL.Entities;

namespace project.DAL.Mappers;

public class SubjectEntityMapper : IEntityMapper<SubjectEntity>
{
    public void MapToExistingEntity(SubjectEntity existingEntity, SubjectEntity newEntity)
    {
        existingEntity.Name = newEntity.Name;
        existingEntity.Acronym = newEntity.Acronym;
    }
}