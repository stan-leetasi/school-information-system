using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Facades;

public interface ISubjectFacade : IFacadeAdvanced<SubjectEntity, SubjectListModel, SubjectStudentDetailModel, SubjectAdminDetailModel>
{
    Task<IEnumerable<SubjectListModel>> GetAsync(Guid? studentId);
}
