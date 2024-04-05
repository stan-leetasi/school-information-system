using project.BL.Mappers;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace project.BL.Facades;

public class SubjectFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    SubjectModelMapper modelMapper)
    : FacadeBaseAdvanced<SubjectEntity, SubjectListModel, SubjectAdminDetailModel, SubjectStudentDetailModel, SubjectEntityMapper, StudentSubjectEntity, StudentSubjectEntityMapper>(unitOfWorkFactory, modelMapper),
        ISubjectFacade
{
    protected override List<string> IncludesNavigationPathDetails =>
        [$"{nameof(SubjectEntity.Students)}.{nameof(StudentSubjectEntity.Student)}"];

    public override Task<IEnumerable<SubjectListModel>> GetAsync()
        => throw new NotImplementedException("This method is unsupported. Use the overload with Guid studentID.");

    public async Task<IEnumerable<SubjectListModel>> GetAsyncListModels(Guid? studentId)
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

    public override async Task<SubjectStudentDetailModel?> GetAsyncStudentDetail(Guid entityId, Guid? studentId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<SubjectEntity> query = uow.GetRepository<SubjectEntity, SubjectEntityMapper>().Get();

        query = query.Include(s => s.Activities).ThenInclude(a => a.Ratings);

        SubjectEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == entityId);
        if (entity is null) return null;

        SubjectStudentDetailModel detailModel = modelMapper.MapToStudentDetailModel(entity);

        var updatedList = detailModel.Activities.Select(listModel =>
        {
            ActivityEntity activity = entity.Activities.Single(a => a.Id == listModel.Id);
            var registeredStudents = activity.Ratings.Count;
            var rating = studentId is null ? null : activity.Ratings.SingleOrDefault(r => r.StudentId == studentId);
            var isRegistered = rating != null;
            var points = rating?.Points ?? 0;
            return listModel with { RegisteredStudents = registeredStudents, IsRegistered = isRegistered, Points = points };
        }).ToObservableCollection();
        detailModel.Activities = updatedList;

        return detailModel;
    }
}
