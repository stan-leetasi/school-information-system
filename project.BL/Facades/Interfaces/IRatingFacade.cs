using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Facades;

public interface IRatingFacade : IFacade<RatingEntity, RatingListModel, RatingDetailModel>
{
}
