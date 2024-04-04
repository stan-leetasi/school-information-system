using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Facades;

public interface IFacadeAdvanced<TEntity, TListModel, TStudentDetailModel, TAdminDetailModel>
    : IFacade<TEntity, TListModel, TStudentDetailModel>
    where TEntity : class, IEntity
    where TListModel : IModel
    where TStudentDetailModel : class, IModel
    where TAdminDetailModel : class, IModel
{
    Task<TStudentDetailModel?> GetAsyncStudentDetail(Guid entityId, Guid? studentId);
    Task<TAdminDetailModel?> GetAsyncAdminDetail(Guid entityId);
    Task RegisterStudent(TListModel listModel, Guid studentId);
    Task UnregisterStudent(TListModel listModel, Guid studentId);
}
