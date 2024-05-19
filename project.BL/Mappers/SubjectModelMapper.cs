using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers
{
    public class SubjectModelMapper(ActivityModelMapper activityModelMapper, StudentModelMapper studentModelMapper)
        : ModelMapperBase<SubjectEntity, SubjectListModel, SubjectAdminDetailModel>
    {
        public override SubjectListModel MapToListModel(SubjectEntity? entity)
        {
            if (entity is null)
            {
                return SubjectListModel.Empty;
            }

            return new SubjectListModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Acronym = entity.Acronym,
                IsRegistered = false
            };
        }

        public SubjectStudentDetailModel MapToStudentDetailModel(SubjectEntity? entity)
        {
            return entity is null
                ? SubjectStudentDetailModel.Empty
                : new SubjectStudentDetailModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    IsRegistered = false,
                    Acronym = entity.Acronym,
                    Activities = activityModelMapper.MapToListModel(entity.Activities).ToObservableCollection()
                };
        }

        public SubjectAdminDetailModel MapToAdminDetailModel(SubjectEntity? entity)
        {
            return entity is null
                ? SubjectAdminDetailModel.Empty
                : new SubjectAdminDetailModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Acronym = entity.Acronym,
                    Students = studentModelMapper.MapToListModel(entity.Students.Select(s => s.Student)!)
                        .ToObservableCollection()
                };
        }

        public override SubjectAdminDetailModel MapToDetailModel(SubjectEntity entity)
        {
            return MapToAdminDetailModel(entity);
        }

        public override SubjectEntity MapToEntity(SubjectAdminDetailModel model)
        {
            return new SubjectEntity { Id = model.Id, Name = model.Name, Acronym = model.Acronym };
        }
    }
}