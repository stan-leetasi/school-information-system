using project.DAL.Entities;

namespace project.DAL.Mappers;

public class StudentSubjectEntityMapper : IEntityMapper<StudentSubjectEntity>
{
    public void MapToExistingEntity(StudentSubjectEntity existingEntity, StudentSubjectEntity newEntity)
    {
        existingEntity.StudentId = newEntity.StudentId;
        existingEntity.SubjectId = newEntity.SubjectId;
    }
}