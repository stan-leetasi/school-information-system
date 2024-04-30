using project.BL.Filters;
using project.BL.Mappers;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.UnitOfWork;

namespace project.BL.Facades;

public class RatingFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    RatingModelMapper modelMapper,
    RatingModelFilter ratingModelFilter)
    : FacadeBase<RatingEntity, RatingListModel, RatingDetailModel, RatingEntityMapper, RatingListModel>(unitOfWorkFactory, modelMapper, ratingModelFilter, ratingModelFilter),
        IRatingFacade
{
    protected override List<string> IncludesNavigationPathDetails =>
        [$"{nameof(RatingEntity.Student)}", $"{nameof(RatingEntity.Activity)}"];
    protected override List<string> IncludesNavigationPathDetailsListModels =>
        [$"{nameof(RatingEntity.Student)}"];

    protected override IEnumerable<RatingListModel> GetListModelsInDetailModel(RatingDetailModel detailModel)
    {
        throw new NotImplementedException("Filtering in RatingFacade is not supported.");
    }

    protected override RatingDetailModel SetListModelsInDetailModel(RatingDetailModel detailModel, IEnumerable<RatingListModel> newListModels)
    {
        throw new NotImplementedException("Filtering in RatingFacade is not supported.");
    }
}
