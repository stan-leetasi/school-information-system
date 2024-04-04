﻿using project.BL.Mappers;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using project.DAL.Repositories;

namespace project.BL.Facades;

public class ActivityFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    ActivityModelMapper modelMapper)
    : FacadeBase<ActivityEntity, ActivityListModel, ActivityStudentDetailModel, ActivityEntityMapper>(unitOfWorkFactory, modelMapper),
        IActivityFacade
{
    protected override string IncludesNavigationPathDetail =>
        $"{nameof(ActivityEntity.Ratings)}";

    public override Task<IEnumerable<ActivityListModel>> GetAsync()
        => throw new NotImplementedException("This method is unsupported. Use the overload with Guid studentID.");

    public async Task<IEnumerable<ActivityListModel>> GetAsync(Guid subjectId, Guid? studentId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<ActivityEntity> entities = await uow
            .GetRepository<ActivityEntity, ActivityEntityMapper>()
            .Get()
            .Include(a => a.Ratings)
            .Where(a => a.SubjectId == subjectId)
            .ToListAsync().ConfigureAwait(false);

        var listModels = ModelMapper.MapToListModel(entities);

        var studentEntity = await uow.GetRepository<StudentEntity, StudentEntityMapper>()
            .Get()
            .Include(i => i.Ratings)
            .ThenInclude(r => r.Activity)
            .SingleOrDefaultAsync(s => s.Id == studentId);

        var updatedList = listModels.Select(listModel =>
        {
            var registeredStudents = entities.Single(a => a.Id == listModel.Id).Ratings.Count;
            var isRegistered = studentEntity is not null && studentEntity.Ratings.Any(r => r.ActivityId == listModel.Id);
            var points = studentEntity is null ? 0 : studentEntity.Ratings.Single(r => r.ActivityId == listModel.Id).Points;
            return listModel with { RegisteredStudents = registeredStudents, IsRegistered = isRegistered, Points = points };
        }).ToList();
        listModels = updatedList;

        return listModels;
    }

    public async Task RegisterStudent(ActivityListModel activity, Guid studentId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        var studentEntity = await uow.GetRepository<StudentEntity, StudentEntityMapper>().Get()
            .SingleAsync(s => s.Id == studentId) ?? throw new ArgumentException($"Student with {studentId} does not exist.");
        IRepository<RatingEntity> repository = uow.GetRepository<RatingEntity, RatingEntityMapper>();

        if (repository.Get().Any(r => r.StudentId == studentId && r.ActivityId == activity.Id))
            throw new InvalidOperationException("Student is already registered.");

        RatingEntity rating = new()
        {
            Id = Guid.NewGuid(),
            Points = 0,
            Notes = String.Empty,
            ActivityId = activity.Id,
            Activity = null,
            StudentId = studentId,
            Student = null
        };

        await repository.InsertAsync(rating);
        await uow.CommitAsync();
    }

    public async Task UnregisterStudent(ActivityListModel activity, Guid studentId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        var studentEntity = await uow.GetRepository<StudentEntity, StudentEntityMapper>().Get()
            .SingleAsync(s => s.Id == studentId) ?? throw new ArgumentException($"Student with {studentId} does not exist.");
        IRepository<RatingEntity> repository = uow.GetRepository<RatingEntity, RatingEntityMapper>();

        var rating = await repository.Get().SingleAsync(r => r.StudentId == studentId && r.ActivityId == activity.Id)
                     ?? throw new InvalidOperationException("Registration does not exist.");

        repository.Delete(rating.Id);
        await uow.CommitAsync();
    }

    public override Task<ActivityStudentDetailModel?> GetAsync(Guid id)
        => throw new NotImplementedException("This method is unsupported. Use the overload with Guid studentID.");

    public async Task<ActivityStudentDetailModel?> GetAsyncStudentDetail(Guid activityId, Guid studentId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<ActivityEntity> query = uow.GetRepository<ActivityEntity, ActivityEntityMapper>().Get();

        if (string.IsNullOrWhiteSpace(IncludesNavigationPathDetail) is false)
        {
            query = query.Include(a => a.Ratings).Include(a => a.Subject);
        }

        ActivityEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == activityId);

        if (entity is null) return null;
        ActivityModelMapper activityModelMapper = new(new RatingModelMapper());
        var detailModel = activityModelMapper.MapToStudentDetailModel(entity);

        var studentEntity = await uow.GetRepository<StudentEntity, StudentEntityMapper>().Get()
            .Include(i => i.Ratings)
            .SingleAsync(s => s.Id == studentId) ?? throw new ArgumentException($"Student with {studentId} does not exist.");

        var isRegistered = studentEntity.Ratings.Any(r => r.ActivityId == detailModel.Id);
        var points = studentEntity.Ratings.Single(r => r.ActivityId == detailModel.Id).Points;
        var notes = studentEntity.Ratings.Single(r => r.ActivityId == detailModel.Id).Notes;

        return detailModel with { IsRegistered = isRegistered, Points = points, Notes = notes };
    }

    public async Task<ActivityAdminDetailModel?> GetAsyncAdminDetail(Guid activityId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<ActivityEntity> query = uow.GetRepository<ActivityEntity, ActivityEntityMapper>().Get();

        if (string.IsNullOrWhiteSpace(IncludesNavigationPathDetail) is false)
        {
            query = query.Include(a => a.Subject)
                .Include(a => a.Ratings)
                .ThenInclude(r => r.Student);
        }

        ActivityEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == activityId);

        if (entity is null) return null;
        ActivityModelMapper activityModelMapper = new(new RatingModelMapper());
        var detailModel = activityModelMapper.MapToAdminDetailModel(entity);

        return detailModel;
    }


}