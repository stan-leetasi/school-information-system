using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Facades;

public interface IActivityFacade : IFacadeAdvanced<ActivityEntity, ActivityListModel, ActivityAdminDetailModel, ActivityStudentDetailModel>
{
}
