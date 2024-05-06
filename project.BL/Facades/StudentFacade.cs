using project.BL.Filters;
using project.BL.Mappers;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.UnitOfWork;

namespace project.BL.Facades;

public class StudentFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    StudentModelMapper modelMapper,
    StudentModelFilter studentModelFilter)
    : FacadeBase<StudentEntity, StudentListModel, StudentDetailModel, StudentEntityMapper, StudentListModel>(unitOfWorkFactory, modelMapper, studentModelFilter, studentModelFilter),
        IStudentFacade
{

    protected override IEnumerable<StudentListModel> GetListModelsInDetailModel(StudentDetailModel detailModel)
    {
        throw new NotImplementedException("Filtering in StudentDetailModel is not supported.");
    }

    protected override StudentDetailModel SetListModelsInDetailModel(StudentDetailModel detailModel, IEnumerable<StudentListModel> newListModels)
    {
        throw new NotImplementedException("Filtering in StudentDetailModel is not supported.");
    }
}
