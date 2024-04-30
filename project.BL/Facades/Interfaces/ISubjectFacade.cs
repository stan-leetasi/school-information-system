using project.BL.Filters;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Facades;

public interface ISubjectFacade : IFacadeAdvanced<SubjectEntity, SubjectListModel, SubjectAdminDetailModel, SubjectStudentDetailModel>
{
    Task<IEnumerable<SubjectListModel>> GetAsyncListModels(Guid? studentId, FilterPreferences? filterPreferences = null);
}
