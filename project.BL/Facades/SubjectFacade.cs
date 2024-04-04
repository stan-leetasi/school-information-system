﻿using project.BL.Mappers;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using project.DAL.Repositories;

namespace project.BL.Facades;

public class SubjectFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    SubjectModelMapper modelMapper)
    : FacadeBase<SubjectEntity, SubjectListModel, SubjectListModel, SubjectEntityMapper>(unitOfWorkFactory, modelMapper),
        ISubjectFacade
{
    protected override string IncludesNavigationPathDetail =>
        $"{nameof(SubjectEntity.Activities)}";

    public override Task<IEnumerable<SubjectListModel>> GetAsync()
        => throw new NotImplementedException("This method is unsupported. Use the overload with Guid studentID.");

    public async Task<IEnumerable<SubjectListModel>> GetAsync(Guid? studentId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<SubjectEntity> entities = await uow
            .GetRepository<SubjectEntity, SubjectEntityMapper>()
            .Get()
            .ToListAsync().ConfigureAwait(false);

        var listModels = ModelMapper.MapToListModel(entities);

        if (studentId is not null) // null indicates model for admin
        {
            var studentEntity = await uow.GetRepository<StudentEntity, StudentEntityMapper>()
                .Get()
                .Include(i => i.Subjects)
                .SingleOrDefaultAsync(s => s.Id == studentId);

            var updatedList = listModels.Select(listModel =>
            {
                var isRegistered = studentEntity is not null && studentEntity.Subjects.Any(studentSubject => studentSubject.SubjectId == listModel.Id);
                return listModel with { IsRegistered = isRegistered };
            }).ToList();
            listModels = updatedList;
        }

        return listModels;
    }

    public async Task RegisterStudent(SubjectListModel subject, Guid studentId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        var studentEntity = await uow.GetRepository<StudentEntity, StudentEntityMapper>().Get()
            .SingleAsync(s => s.Id == studentId);
        if (studentEntity is null) throw new ArgumentException($"Student with {studentId} does not exist.");

        IRepository<StudentSubjectEntity> repository =
            uow.GetRepository<StudentSubjectEntity, StudentSubjectEntityMapper>();

        if (repository.Get().Any(reg => reg.StudentId == studentId && reg.SubjectId == subject.Id))
            throw new InvalidOperationException("Student is already registered.");

        StudentSubjectEntity registration = new()
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            SubjectId = subject.Id,
            Student = null,
            Subject = null
        };

        await repository.InsertAsync(registration);
        await uow.CommitAsync();
    }

    public async Task UnregisterStudent(SubjectListModel subject, Guid studentId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        var studentEntity = await uow.GetRepository<StudentEntity, StudentEntityMapper>().Get()
            .SingleAsync(s => s.Id == studentId);
        if (studentEntity is null) throw new ArgumentException($"Student with {studentId} does not exist.");

        IRepository<StudentSubjectEntity> repository =
            uow.GetRepository<StudentSubjectEntity, StudentSubjectEntityMapper>();

        var registration = await repository.Get().SingleAsync(reg => reg.StudentId == studentId && reg.SubjectId == subject.Id);

        if (registration is null) throw new InvalidOperationException("Registration does not exist.");

        repository.Delete(registration.Id);
        await uow.CommitAsync();
    }
}