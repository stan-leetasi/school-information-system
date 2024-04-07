using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Facades;

public interface IStudentFacade : IFacade<StudentEntity, StudentListModel, StudentDetailModel>
{
}
