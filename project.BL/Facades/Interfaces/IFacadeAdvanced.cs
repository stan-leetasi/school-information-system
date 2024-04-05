using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Facades;

public interface IFacadeAdvanced<TEntity, TListModel,  TAdminDetailModel, TStudentDetailModel>
    : IFacade<TEntity, TListModel, TAdminDetailModel>
    where TEntity : class, IEntity
    where TListModel : IModel
    where TStudentDetailModel : class, IModel
    where TAdminDetailModel : class, IModel
{
    Task<TStudentDetailModel?> GetAsyncStudentDetail(Guid entityId, Guid? studentId);
    Task RegisterStudent(Guid targetId, Guid studentId);
    Task UnregisterStudent(Guid targetId, Guid studentId);
}
