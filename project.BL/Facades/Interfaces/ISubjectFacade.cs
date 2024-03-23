using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Facades;

public interface ISubjectFacade : IFacade<SubjectEntity, SubjectListModel, SubjectListModel>
{ 
    Task<IEnumerable<SubjectListModel>> GetAsync(Guid? studentId);
    Task RegisterStudent(SubjectListModel subject, Guid studentId);
    Task UnregisterStudent(SubjectListModel subject, Guid studentId);
}
