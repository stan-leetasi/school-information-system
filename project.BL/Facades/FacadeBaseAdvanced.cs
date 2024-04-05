﻿using project.BL.Mappers;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.Repositories;
using project.DAL.UnitOfWork;

namespace project.BL.Facades;

public abstract class
    FacadeBaseAdvanced<TEntity, TListModel, TAdminDetailModel, TStudentDetailModel, TEntityMapper, TRegistrationEntity, TRegistrationEntityMapper>(
        IUnitOfWorkFactory unitOfWorkFactory,
        IModelMapper<TEntity, TListModel, TAdminDetailModel> modelMapper)
    : FacadeBase<TEntity, TListModel, TAdminDetailModel, TEntityMapper>(unitOfWorkFactory, modelMapper),
    IFacadeAdvanced<TEntity, TListModel, TAdminDetailModel, TStudentDetailModel>
    where TEntity : class, IEntity
    where TListModel : IModel
    where TStudentDetailModel : class, IModel
    where TAdminDetailModel : class, IModel
    where TEntityMapper : IEntityMapper<TEntity>, new()
    where TRegistrationEntity : class, IEntity
    where TRegistrationEntityMapper : IEntityMapper<TRegistrationEntity>, new()
{
    protected bool ExistsStudent(IUnitOfWork uow, Guid studentId)
    {
        return uow.GetRepository<StudentEntity, StudentEntityMapper>().Get().Any(s => s.Id == studentId);
    }
    public abstract Task<TStudentDetailModel?> GetAsyncStudentDetail(Guid entityId, Guid? studentId);
    public async Task RegisterStudent(Guid targetId, Guid studentId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        if (!ExistsStudent(uow, studentId)) throw new ArgumentException($"Student with {studentId} does not exist.");

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

        if (!ExistsStudent(uow, studentId)) throw new ArgumentException($"Student with {studentId} does not exist.");

        IRepository<TRegistrationEntity> repositoryRegistrations = uow.GetRepository<TRegistrationEntity, TRegistrationEntityMapper>();
        var registration = await GetRegistrationEntity(targetId, studentId, repositoryRegistrations.Get())
                           ?? throw new InvalidOperationException("Registration does not exist.");

        repositoryRegistrations.Delete(registration.Id);
        await uow.CommitAsync();
    }

    protected abstract Task<TRegistrationEntity?> GetRegistrationEntity(Guid targetId, Guid studentId, IQueryable<TRegistrationEntity> registrationEntities);
    protected abstract TRegistrationEntity CreateRegistrationEntity(Guid targetId, Guid studentId);
}
