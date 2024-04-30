﻿using project.BL.Mappers;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using project.BL.Filters;

namespace project.BL.Facades;

public class ActivityFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    ActivityModelMapper modelMapper,
    ActivityModelFilter activityModelFilter,
    RatingModelFilter ratingModelFilter)
    : FacadeBaseAdvanced<ActivityEntity, ActivityListModel, ActivityAdminDetailModel, ActivityStudentDetailModel,
            ActivityEntityMapper, RatingEntity, RatingEntityMapper, RatingListModel>(unitOfWorkFactory, modelMapper, activityModelFilter, ratingModelFilter),
        IActivityFacade
{
    protected override List<string> IncludesNavigationPathDetails =>
        [$"{nameof(ActivityEntity.Ratings)}.{nameof(RatingEntity.Student)}", $"{nameof(ActivityEntity.Subject)}"];

    protected override IEnumerable<RatingListModel> GetListModelsInDetailModel(ActivityAdminDetailModel detailModel)
    {
        return detailModel.Ratings;
    }

    protected override ActivityAdminDetailModel SetListModelsInDetailModel(ActivityAdminDetailModel detailModel, IEnumerable<RatingListModel> newListModels)
    {
        detailModel.Ratings = newListModels.ToObservableCollection();
        return detailModel;
    }

    public override Task<IEnumerable<ActivityListModel>> GetAsync(FilterPreferences? filterPreferences = null)
        => throw new NotImplementedException("This method is unsupported. Use ISubjectFacade.GetAsyncStudentDetail() to retrieve ActivityListModels of a certain subject.");

    protected override async Task<RatingEntity?> GetRegistrationEntity(Guid targetId, Guid studentId, IQueryable<RatingEntity> registrationEntities)
    {
        return await registrationEntities.FirstOrDefaultAsync(r => r.StudentId == studentId && r.ActivityId == targetId);
    }

    protected override RatingEntity CreateRegistrationEntity(Guid targetId, Guid studentId)
    {
        return new RatingEntity()
        {
            Id = Guid.NewGuid(),
            Points = 0,
            Notes = String.Empty,
            ActivityId = targetId,
            Activity = null,
            StudentId = studentId,
            Student = null
        };
    }

    public override async Task<ActivityStudentDetailModel?> GetAsyncStudentDetail(Guid entityId, Guid? studentId, FilterPreferences? filterPreferences = null)
    {
        if (studentId is null)
            throw new ArgumentNullException(
                $"ActivityFacade.GetAsyncStudentDetail() shouldn't be used with studentId={studentId}. Use GetAsyncAdminDetail() instead.");

        if (filterPreferences is not null)
            throw new NotImplementedException("Filter preferences should not be used with ActivityStudentDetailModel.");

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<ActivityEntity> query = uow.GetRepository<ActivityEntity, ActivityEntityMapper>().Get();

        query = query.Include(a => a.Ratings).Include(a => a.Subject);

        ActivityEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == entityId);

        if (entity is null) return null;
        var detailModel = modelMapper.MapToStudentDetailModel(entity);

        var studentEntity = await uow.GetRepository<StudentEntity, StudentEntityMapper>().Get()
            .Include(i => i.Ratings)
            .SingleAsync(s => s.Id == studentId) ?? throw new ArgumentException($"Student with {studentId} does not exist.");

        var isRegistered = studentEntity.Ratings.Any(r => r.ActivityId == detailModel.Id);
        var points = studentEntity.Ratings.Single(r => r.ActivityId == detailModel.Id).Points;
        var notes = studentEntity.Ratings.Single(r => r.ActivityId == detailModel.Id).Notes;

        return detailModel with { IsRegistered = isRegistered, Points = points, Notes = notes };
    }

    public static IEnumerable<ActivityListModel> FilterActivityListModels(IEnumerable<ActivityListModel> listModels, string searchedTerm)
    {
        searchedTerm = searchedTerm.ToLower();
        return listModels.Where(a =>
            a.BeginTime.ToShortDateString().Replace(" ", "").Contains(searchedTerm) ||
            a.BeginTime.ToShortTimeString().Contains(searchedTerm) ||
            a.EndTime.ToShortDateString().Replace(" ", "").Contains(searchedTerm) ||
            a.EndTime.ToShortTimeString().Contains(searchedTerm)
        );
    }
}