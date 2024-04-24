using project.BL.Filters;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Facades;

public interface IFacade<TEntity, TListModel, TDetailModel>
    where TEntity : class, IEntity
    where TListModel : IModel
    where TDetailModel : class, IModel
{
    Task DeleteAsync(Guid id);
    Task<TDetailModel?> GetAsync(Guid id, FilterPreferences? filterPreferences = null);
    Task<IEnumerable<TListModel>> GetAsync(FilterPreferences? filterPreferences = null);
    Task<TDetailModel> SaveAsync(TDetailModel model);
}
