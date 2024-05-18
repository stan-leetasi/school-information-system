using project.BL.Mappers;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using project.BL.Filters;

namespace project.BL.Facades;

public class SubjectFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    SubjectModelMapper modelMapper,
    SubjectModelFilter subjectModelFilter,
    StudentModelFilter studentModelFilter,
    ActivityModelFilter activityModelFilter)
    : FacadeBaseAdvanced<SubjectEntity, SubjectListModel, SubjectAdminDetailModel, SubjectStudentDetailModel,
            SubjectEntityMapper, StudentSubjectEntity, StudentSubjectEntityMapper, StudentListModel>(unitOfWorkFactory, modelMapper, subjectModelFilter, studentModelFilter),
        ISubjectFacade
{
    protected override List<string> IncludesNavigationPathDetails =>
        [$"{nameof(SubjectEntity.Students)}.{nameof(StudentSubjectEntity.Student)}"];

    protected override IEnumerable<StudentListModel> GetListModelsInDetailModel(SubjectAdminDetailModel detailModel)
    {
        return detailModel.Students;
    }

    protected override SubjectAdminDetailModel SetListModelsInDetailModel(SubjectAdminDetailModel detailModel, IEnumerable<StudentListModel> newListModels)
    {
        detailModel.Students = newListModels.ToObservableCollection();
        return detailModel;
    }

    public override Task<IEnumerable<SubjectListModel>> GetAsync(FilterPreferences? filterPreferences = null)
        => throw new NotImplementedException("This method is unsupported. Use the overload with Guid studentID.");

    /// <summary>
    /// Gets list of <c>SubjectListModel</c> from the perspective of a certain student.
    /// </summary>
    /// <param name="studentId">ID of the student whose perspective we are looking from. NULL if we want a general perspective (admin view of activities).</param>
    public async Task<IEnumerable<SubjectListModel>> GetAsyncListModels(Guid? studentId, FilterPreferences? filterPreferences = null)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<SubjectEntity> entities = await uow
            .GetRepository<SubjectEntity, SubjectEntityMapper>()
            .Get()
            .ToListAsync();

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

        if (filterPreferences is not null)
        {
            listModels = subjectModelFilter.ApplyFilter(listModels, filterPreferences);
        }

        return listModels;
    }

    protected override async Task<StudentSubjectEntity?> GetRegistrationEntity(Guid targetId, Guid studentId, IQueryable<StudentSubjectEntity> registrationEntities)
    {
        return await registrationEntities.FirstOrDefaultAsync(reg => reg.StudentId == studentId && reg.SubjectId == targetId);
    }

    protected override StudentSubjectEntity CreateRegistrationEntity(Guid targetId, Guid studentId)
    {
        return new StudentSubjectEntity()
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            SubjectId = targetId,
            Student = null,
            Subject = null
        };
    }

    public override async Task<SubjectStudentDetailModel?> GetAsyncStudentDetail(Guid entityId, Guid? studentId, FilterPreferences? filterPreferences = null)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<SubjectEntity> query = uow.GetRepository<SubjectEntity, SubjectEntityMapper>().Get();

        query = query.Include(s => s.Activities).ThenInclude(a => a.Ratings);

        SubjectEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == entityId);
        if (entity is null) return null;

        SubjectStudentDetailModel detailModel = modelMapper.MapToStudentDetailModel(entity);

        if (studentId is not null)
        {
            detailModel.IsRegistered = await uow.GetRepository<StudentSubjectEntity, StudentSubjectEntityMapper>().Get()
                .AnyAsync(reg => reg.SubjectId == entityId && reg.StudentId == studentId);
        }

        var updatedList = detailModel.Activities.Select(listModel =>
        {
            ActivityEntity activity = entity.Activities.Single(a => a.Id == listModel.Id);
            var registeredStudents = activity.Ratings.Count;
            var rating = studentId is null ? null : activity.Ratings.SingleOrDefault(r => r.StudentId == studentId);
            var isRegistered = rating != null;
            var points = rating?.Points ?? 0;
            return listModel with { RegisteredStudents = registeredStudents, IsRegistered = isRegistered, Points = points };
        });

        if (filterPreferences is not null)
        {
            updatedList = activityModelFilter.ApplyFilter(updatedList, filterPreferences);
        }

        detailModel.Activities = updatedList.ToObservableCollection();
        return detailModel;
    }

    protected override Task<bool> CanBeRegisteredFor(Guid targetId, Guid studentId, IUnitOfWork? uow) => Task.FromResult(true);
}
