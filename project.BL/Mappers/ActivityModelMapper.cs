using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers;

public class ActivityModelMapper(RatingModelMapper ratingModelMapper)
    : ModelMapperBase<ActivityEntity, ActivityListModel, ActivityStudentDetailModel>
{
    public override ActivityListModel MapToListModel(ActivityEntity? entity)
    {
        return entity is null
            ? ActivityListModel.Empty
            : new ActivityListModel
            {
                Id = entity.Id,
                BeginTime = entity.BeginTime,
                EndTime = entity.EndTime,
                Area = entity.Area,
                Type = entity.Type,
                RegisteredStudents = entity.Subject?.Students.Count ?? 0,
                Points = 0,
                IsRegistered = false
            };
    }

    public override ActivityStudentDetailModel MapToDetailModel(ActivityEntity? entity)
    {
        throw new NotImplementedException("Use specific method for mapping to detail model.");
    }

    public ActivityStudentDetailModel MapToStudentDetailModel(ActivityEntity? entity)
    {
        return entity is null
            ? ActivityStudentDetailModel.Empty
            : new ActivityStudentDetailModel
            {
                Id = entity.Id,
                BeginTime = entity.BeginTime,
                EndTime = entity.EndTime,
                Area = entity.Area,
                Type = entity.Type,
                Name = entity.Subject?.Name ?? string.Empty,
                Description = entity.Description,
                SubjectId = entity.Subject?.Id ?? Guid.Empty,
                SubjectName = entity.Subject?.Name ?? string.Empty,
                IsRegistered = false,
                Points = 0,
                Notes = string.Empty
            };
    }

    public ActivityAdminDetailModel MapToAdminDetailModel(ActivityEntity? entity)
    {
        return entity is null
            ? ActivityAdminDetailModel.Empty
            : new ActivityAdminDetailModel
            {
                Id = entity.Id,
                BeginTime = entity.BeginTime,
                EndTime = entity.EndTime,
                Area = entity.Area,
                Type = entity.Type,
                Name = entity.Subject?.Name ?? string.Empty,
                Description = entity.Description,
                SubjectId = entity.Subject?.Id ?? Guid.Empty,
                SubjectName = entity.Subject?.Name ?? string.Empty,
                Ratings = ratingModelMapper.MapToListModel(entity.Ratings).ToObservableCollection()
            };
    }

    public override ActivityEntity MapToEntity(ActivityStudentDetailModel model)
    {
        return new ActivityEntity
        {
            Id = model.Id,
            BeginTime = model.BeginTime,
            EndTime = model.EndTime,
            Area = model.Area,
            Type = model.Type,
            Description = model.Description,
            SubjectId = model.SubjectId,
            Subject = null
        };
    }

    public ActivityEntity MapToEntity(ActivityAdminDetailModel model)
    {
        return new ActivityEntity
        {
            Id = model.Id,
            BeginTime = model.BeginTime,
            EndTime = model.EndTime,
            Area = model.Area,
            Type = model.Type,
            Description = model.Description,
            SubjectId = model.SubjectId,
            Subject = null
        };
    }
}