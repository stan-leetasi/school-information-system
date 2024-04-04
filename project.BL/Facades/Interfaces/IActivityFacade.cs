using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Facades;

public interface IActivityFacade : IFacadeAdvanced<ActivityEntity, ActivityListModel, ActivityStudentDetailModel, ActivityAdminDetailModel>
{
    Task<IEnumerable<ActivityListModel>> GetAsync(Guid subjectId, Guid? studentId);
}
