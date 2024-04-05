using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers;

public class RatingModelMapper : ModelMapperBase<RatingEntity, RatingListModel, RatingDetailModel>
{
    public override RatingListModel MapToListModel(RatingEntity? entity)
    {
        if (entity is null)
        {
            return RatingListModel.Empty;
        }

        return new RatingListModel
        {
            Id = entity.Id,
            StudentId = entity.StudentId,
            StudentSurname = entity.Student?.Surname ?? "Student is not set",
            StudentName = entity.Student?.Name ?? "Student is not set",
            Points = entity.Points
        };
    }

    public override RatingDetailModel MapToDetailModel(RatingEntity? entity)
    {
        return entity is null
            ? RatingDetailModel.Empty
            : new RatingDetailModel
            {
                Id = entity.Id,
                StudentSurname = entity.Student?.Surname ?? "Student is not set",
                StudentName = entity.Student?.Name ?? "Student is not set",
                ActivityName = entity.Activity is null
                    ? string.Empty
                    : Enum.GetName(entity.Activity.Type)!,
                Points = entity.Points,
                Notes = entity.Notes,
                ActivityId = entity.ActivityId,
                StudentId = entity.StudentId
            };
    }

    public override RatingEntity MapToEntity(RatingDetailModel model)
    {
        return new RatingEntity
        {
            Id = model.Id,
            Points = Convert.ToUInt16(model.Points),
            Notes = model.Notes,
            ActivityId = model.ActivityId,
            StudentId = model.StudentId,
            Activity = null,
            Student = null
        };
    }
}