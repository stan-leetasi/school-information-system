using project.BL.Filters;
using project.BL.Mappers;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.Repositories;
using project.DAL.UnitOfWork;

namespace project.BL.Facades;

/// <summary>
/// Facade that works with two different detail models (for admin and for student).
/// The <c>TAdminDetailModel</c> is used as a substitute for the regular <c>TDetailModel</c> in <c>FacadeBase</c>.
/// The facade is also capable of creating/deleting relations between students and <c>TEntity</c> (student is registered to <c>TEntity</c>)
/// using <c>TRegistrationEntity</c>.
/// </summary>
public abstract class
    FacadeBaseAdvanced<TEntity, TListModel, TAdminDetailModel, TStudentDetailModel, TEntityMapper,
        TRegistrationEntity, TRegistrationEntityMapper, TListModelInDetail>(
        IUnitOfWorkFactory unitOfWorkFactory,
        IModelMapper<TEntity, TListModel, TAdminDetailModel> modelMapper,
        IListModelFilter<TListModel> listModelFilter,
        IListModelFilter<TListModelInDetail> detailModelFilter)
    : FacadeBase<TEntity, TListModel, TAdminDetailModel, TEntityMapper, TListModelInDetail>(unitOfWorkFactory, modelMapper, listModelFilter, detailModelFilter),
    IFacadeAdvanced<TEntity, TListModel, TAdminDetailModel, TStudentDetailModel>
    where TEntity : class, IEntity
    where TListModel : IModel
    where TStudentDetailModel : class, IModel
    where TAdminDetailModel : class, IModel
    where TEntityMapper : IEntityMapper<TEntity>, new()
    where TRegistrationEntity : class, IEntity
    where TRegistrationEntityMapper : IEntityMapper<TRegistrationEntity>, new()
    where TListModelInDetail : IModel
{
    protected bool ExistsStudent(IUnitOfWork uow, Guid studentId)
    {
        return uow.GetRepository<StudentEntity, StudentEntityMapper>().Get().Any(s => s.Id == studentId);
    }

    protected abstract Task<bool> CanBeRegisteredFor(Guid targetId, Guid studentId, IUnitOfWork? uow);

    /// <summary>
    /// Gets <c>TStudentDetailModel</c> from the perspective of a certain student.
    /// </summary>
    /// <param name="entityId">ID of the entity.</param>
    /// <param name="studentId">ID of the student whose perspective we are looking from. NULL if we want a general perspective.</param>
    /// <param name="filterPreferences">Filtration of items based on the given search term and sorting style.</param>
    public abstract Task<TStudentDetailModel?> GetAsyncStudentDetail(Guid entityId, Guid? studentId, FilterPreferences? filterPreferences = null);

    public async Task RegisterStudent(Guid targetId, Guid studentId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        if (!ExistsStudent(uow, studentId)) throw new ArgumentException($"Student with ID {studentId} does not exist.");

        if (!(await CanBeRegisteredFor(targetId, studentId, uow)))
            throw new InvalidOperationException($"Student with ID {studentId} doesn't meet the requirements to be registered.");

        IRepository<TRegistrationEntity> repositoryRegistrations = uow.GetRepository<TRegistrationEntity, TRegistrationEntityMapper>();
        if (await GetRegistrationEntity(targetId, studentId, repositoryRegistrations.Get()) is not null)
            throw new InvalidOperationException("Student is already registered.");
        TRegistrationEntity registrationEntity = CreateRegistrationEntity(targetId, studentId);

        await repositoryRegistrations.InsertAsync(registrationEntity);
        await uow.CommitAsync();
    }

    public async Task UnregisterStudent(Guid targetId, Guid studentId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        if (!ExistsStudent(uow, studentId)) throw new ArgumentException($"Student with ID {studentId} does not exist.");

        IRepository<TRegistrationEntity> repositoryRegistrations = uow.GetRepository<TRegistrationEntity, TRegistrationEntityMapper>();
        var registration = await GetRegistrationEntity(targetId, studentId, repositoryRegistrations.Get())
                           ?? throw new InvalidOperationException("Registration does not exist.");

        repositoryRegistrations.Delete(registration.Id);
        await uow.CommitAsync();
    }

    protected abstract Task<TRegistrationEntity?> GetRegistrationEntity(Guid targetId, Guid studentId, IQueryable<TRegistrationEntity> registrationEntities);
    protected abstract TRegistrationEntity CreateRegistrationEntity(Guid targetId, Guid studentId);
}
