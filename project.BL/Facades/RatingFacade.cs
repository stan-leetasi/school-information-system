using project.BL.Mappers;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.Repositories;
using project.DAL.UnitOfWork;

namespace project.BL.Facades;

public class RatingFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    RatingModelMapper modelMapper)
    : FacadeBase<RatingEntity, RatingListModel, RatingDetailModel, RatingEntityMapper>(unitOfWorkFactory, modelMapper),
        IRatingFacade
{
    public override Task<RatingDetailModel> SaveAsync(RatingDetailModel model)
        => throw new NotImplementedException("This method is unsupported. Use the overload with Guid activityId and Guid studentID.");

    public async Task<RatingDetailModel> SaveAsync(RatingDetailModel model, Guid activityId, Guid studentId)
    {
        RatingDetailModel result;

        GuardCollectionsAreNotSet(model);

        RatingModelMapper ratingModelMapper = new();
        RatingEntity entity = ratingModelMapper.MapToEntity(model, activityId, studentId);

        IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<RatingEntity> repository = uow.GetRepository<RatingEntity, RatingEntityMapper>();

        if (await repository.ExistsAsync(entity))
        {
            RatingEntity updatedEntity = await repository.UpdateAsync(entity);
            result = ModelMapper.MapToDetailModel(updatedEntity);
        }
        else
        {
            entity.Id = Guid.NewGuid();
            RatingEntity insertedEntity = await repository.InsertAsync(entity);
            result = ModelMapper.MapToDetailModel(insertedEntity);
        }

        await uow.CommitAsync();

        return result;
    }
}
