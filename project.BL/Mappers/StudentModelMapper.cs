using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers;

public class StudentModelMapper : ModelMapperBase<StudentEntity, StudentListModel, StudentDetailModel>
{
    public override StudentListModel MapToListModel(StudentEntity? entity)
    {
        if (entity is null)
        {
            return StudentListModel.Empty;
        }

        return new StudentListModel { Id = entity.Id, Surname = entity.Surname, Name = entity.Name };
    }

    public override StudentDetailModel MapToDetailModel(StudentEntity entity)
    {
        return new StudentDetailModel
        {
            Id = entity.Id, Surname = entity.Surname, Name = entity.Name, ImageUrl = entity.ImageUrl
        };
    }

    public override StudentEntity MapToEntity(StudentDetailModel model)
    {
        return new StudentEntity
        {
            Id = model.Id, Surname = model.Surname, Name = model.Name, ImageUrl = model.ImageUrl
        };
    }
}