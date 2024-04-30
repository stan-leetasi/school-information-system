using System.Collections;
using System.Reflection;
using project.BL.Mappers;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.Repositories;
using project.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using project.BL.Filters;

namespace project.BL.Facades;

public abstract class
    FacadeBase<TEntity, TListModel, TDetailModel, TEntityMapper, TListModelInDetail>(
        IUnitOfWorkFactory unitOfWorkFactory,
        IModelMapper<TEntity, TListModel, TDetailModel> modelMapper,
        IListModelFilter<TListModel> listModelFilter,
        IListModelFilter<TListModelInDetail> detailModelFilter)
    : IFacade<TEntity, TListModel, TDetailModel>
    where TEntity : class, IEntity
    where TListModel : IModel
    where TDetailModel : class, IModel
    where TEntityMapper : IEntityMapper<TEntity>, new()
    where TListModelInDetail : IModel
{
    protected readonly IModelMapper<TEntity, TListModel, TDetailModel> ModelMapper = modelMapper;
    protected readonly IUnitOfWorkFactory UnitOfWorkFactory = unitOfWorkFactory;

    protected virtual List<string> IncludesNavigationPathDetails => [];
    protected virtual List<string> IncludesNavigationPathDetailsListModels => [];

    protected abstract IEnumerable<TListModelInDetail> GetListModelsInDetailModel(TDetailModel detailModel);
    protected abstract TDetailModel SetListModelsInDetailModel(TDetailModel detailModel, IEnumerable<TListModelInDetail> newListModels);

    public async Task DeleteAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        try
        {
            uow.GetRepository<TEntity, TEntityMapper>().Delete(id);
            await uow.CommitAsync().ConfigureAwait(false);
        }
        catch (DbUpdateException e)
        {
            throw new InvalidOperationException("Entity deletion failed.", e);
        }
    }

    public virtual async Task<TDetailModel?> GetAsync(Guid id, FilterPreferences? filterPreferences = null)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<TEntity> query = GetEntityQuery(uow, IncludesNavigationPathDetails);

        TEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == id);

        if (entity is null) return null;
        TDetailModel detailModel = ModelMapper.MapToDetailModel(entity);

        if (filterPreferences is not null)
        {
            IEnumerable<TListModelInDetail> listModelsInside = detailModelFilter.ApplyFilter(GetListModelsInDetailModel(detailModel), filterPreferences);
            detailModel = SetListModelsInDetailModel(detailModel, listModelsInside);
        }

        return detailModel;
    }

    public virtual async Task<IEnumerable<TListModel>> GetAsync(FilterPreferences? filterPreferences = null)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<TEntity> query = GetEntityQuery(uow, IncludesNavigationPathDetailsListModels);

        var listModels = ModelMapper.MapToListModel(await query.ToListAsync()); ;

        if (filterPreferences is not null)
        {
            listModels = listModelFilter.ApplyFilter(listModels, filterPreferences);
        }

        return listModels;
    }

    public virtual async Task<TDetailModel> SaveAsync(TDetailModel model)
    {
        TDetailModel result;

        GuardCollectionsAreNotSet(model);

        TEntity entity = ModelMapper.MapToEntity(model);

        IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<TEntity> repository = uow.GetRepository<TEntity, TEntityMapper>();

        if (await repository.ExistsAsync(entity))
        {
            TEntity updatedEntity = await repository.UpdateAsync(entity);
            result = ModelMapper.MapToDetailModel(updatedEntity);
        }
        else
        {
            entity.Id = Guid.NewGuid();
            TEntity insertedEntity = await repository.InsertAsync(entity);
            result = ModelMapper.MapToDetailModel(insertedEntity);
        }

        await uow.CommitAsync();

        return result;
    }

    private IQueryable<TEntity> GetEntityQuery(IUnitOfWork uow, List<string> includesNavigationPath)
    {
        IQueryable<TEntity> query = uow.GetRepository<TEntity, TEntityMapper>().Get();

        foreach (var navInclude in includesNavigationPath)
        {
            query = query.Include(navInclude);
        }

        return query;
    }

    /// <summary>
    /// This Guard ensures that there is a clear understanding of current infrastructure limitations.
    /// This version of BL/DAL infrastructure does not support insertion or update of adjacent entities.
    /// WARN: Does not guard navigation properties.
    /// </summary>
    /// <param name="model">Model to be inserted or updated</param>
    /// <exception cref="InvalidOperationException"></exception>
    protected static void GuardCollectionsAreNotSet(TDetailModel model)
    {
        IEnumerable<PropertyInfo> collectionProperties = model
            .GetType()
            .GetProperties()
            .Where(i => typeof(ICollection).IsAssignableFrom(i.PropertyType));

        foreach (PropertyInfo collectionProperty in collectionProperties)
        {
            if (collectionProperty.GetValue(model) is ICollection { Count: > 0 })
            {
                throw new InvalidOperationException(
                    "Current BL and DAL infrastructure disallows insert or update of models with adjacent collections.");
            }
        }
    }
}
