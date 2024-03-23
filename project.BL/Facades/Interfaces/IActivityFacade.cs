using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Facades;

public interface IActivityFacade : IFacade<ActivityEntity, ActivityListModel, ActivityStudentDetailModel>
{
    Task<IEnumerable<ActivityListModel>> GetAsync(Guid subjectId, Guid? studentId);
    Task RegisterStudent(ActivityListModel activity, Guid studentId);
    Task UnregisterStudent(ActivityListModel activity, Guid studentId);

    Task<ActivityStudentDetailModel?> GetAsyncStudentDetail(Guid activityId, Guid studentId);
    Task<ActivityAdminDetailModel?> GetAsyncAdminDetail(Guid activityId);
}
