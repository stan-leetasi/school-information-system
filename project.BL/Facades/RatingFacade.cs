using project.BL.Mappers;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.UnitOfWork;

namespace project.BL.Facades;

public class RatingFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    RatingModelMapper modelMapper)
    : FacadeBase<RatingEntity, RatingListModel, RatingDetailModel, RatingEntityMapper>(unitOfWorkFactory, modelMapper),
        IRatingFacade
{
    protected override List<string> IncludesNavigationPathDetails =>
        [$"{nameof(RatingEntity.Student)}", $"{nameof(RatingEntity.Activity)}"];
    protected override List<string> IncludesNavigationPathDetailsListModels =>
        [$"{nameof(RatingEntity.Student)}"];
}
