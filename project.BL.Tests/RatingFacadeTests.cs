using project.BL.Facades;
using project.BL.Models;
using project.Common.Tests.Seeds;
using Xunit.Abstractions;

namespace project.BL.Tests;

public sealed class RatingFacadeTests : FacadeTestsBase
{
    private readonly IRatingFacade _ratingFacadeSUT;

    public RatingFacadeTests(ITestOutputHelper output) : base(output)
    {
        _ratingFacadeSUT = new RatingFacade(UnitOfWorkFactory, RatingModelMapper);
    }

    [Fact]
    public async Task Get_RatingDetailModel()
    {
        //Act
        var ICSratingDetailModel = await _ratingFacadeSUT.GetAsync(RatingsSeeds.ICSRating.Id);

        var IOSratingDetailModel = await _ratingFacadeSUT.GetAsync(RatingsSeeds.IOSRating.Id);

        //Assert
        Assert.NotNull(ICSratingDetailModel);
        Assert.Equal(RatingsSeeds.ICSRating.Id, ICSratingDetailModel.Id);
        Assert.Equal(RatingsSeeds.ICSRating.Notes, ICSratingDetailModel.Notes);
        Assert.Equal(RatingsSeeds.ICSRating.Points, ICSratingDetailModel.Points);

        Assert.NotNull(IOSratingDetailModel);
        Assert.Equal(RatingsSeeds.IOSRating.Id, IOSratingDetailModel.Id);
        Assert.Equal(RatingsSeeds.IOSRating.Notes, IOSratingDetailModel.Notes);
        Assert.Equal(RatingsSeeds.IOSRating.Points, IOSratingDetailModel.Points);
    }

    [Fact]
    public async Task Get_RatingListModel_For_ICSRating()
    {
        //Act
        var ratingListModel = await _ratingFacadeSUT.GetAsync();
        var ratingList = ratingListModel.ToList();
        var ICSRating = ratingList.SingleOrDefault(r => r.Id == RatingsSeeds.ICSRating.Id);
        var IOSRating = ratingList.SingleOrDefault(r => r.Id == RatingsSeeds.IOSRating.Id);

        //Assert
        Assert.NotNull(ratingListModel);
        Assert.Equal(2, ratingList.Count);

        Assert.NotNull(ICSRating);
        Assert.Equal(RatingsSeeds.ICSRating.Points, ICSRating.Points);
        Assert.Equal(RatingsSeeds.ICSRating.StudentId, ICSRating.StudentId);

        Assert.NotNull(IOSRating);
        Assert.Equal(RatingsSeeds.IOSRating.Points, IOSRating.Points);
        Assert.Equal(RatingsSeeds.IOSRating.StudentId, IOSRating.StudentId);
    }

    [Fact]
    public async Task Create_RatingDetailModel()
    {
        //Arrange
        var local = new RatingDetailModel
        {
            Points = 5,
            Notes = "Great job!",
            StudentName = "Terry",
            StudentSurname = "Test",
            ActivityName = "ICS",
            ActivityId = ActivitiesSeeds.ICSCviko.Id,
            StudentId = StudentSeeds.Terry.Id,
        };
        var rating = await _ratingFacadeSUT.SaveAsync(local);

        //Act
        var remote = await _ratingFacadeSUT.GetAsync(rating.Id);

        //Assert
        Assert.NotNull(rating);
        Assert.NotNull(remote);
        Assert.Equal(rating.Id, remote.Id);
        Assert.Equal(local.Points, remote.Points);
        Assert.Equal(local.Notes, remote.Notes);
        Assert.Equal(local.ActivityId, remote.ActivityId);
        Assert.Equal(local.StudentId, remote.StudentId);
    }

    [Fact]
    public async Task Update_RatingDetailModel()
    {
        //Arrange
        RatingDetailModel local = new RatingDetailModel
        {
            Points = 5,
            Notes = "Great job!",
            StudentName = "Terry",
            StudentSurname = "Test",
            ActivityName = "ICS",
            ActivityId = ActivitiesSeeds.ICSCviko.Id,
            StudentId = StudentSeeds.Terry.Id,
        };
        var rating = await _ratingFacadeSUT.SaveAsync(
            local);

        //Act
        var remote = await _ratingFacadeSUT.GetAsync(rating.Id);

        //Assert
        Assert.NotNull(rating);
        Assert.NotNull(remote);
        Assert.Equal(rating.Id, remote.Id);
        Assert.Equal(local.Points, remote.Points);
        Assert.Equal(local.Notes, remote.Notes);
        Assert.Equal(local.ActivityId, remote.ActivityId);
        Assert.Equal(local.StudentId, remote.StudentId);
    }

    [Fact]
    public async Task Delete_Existing_RatingDetailModel()
    {
        await _ratingFacadeSUT.DeleteAsync(RatingsSeeds.ICSRating.Id);

        var removed = await _ratingFacadeSUT.GetAsync(RatingsSeeds.ICSRating.Id);

        Assert.Null(removed);
    }

    [Fact]
    public async Task Delete_NonExisting_RatingDetailModel()
    {
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _ratingFacadeSUT.DeleteAsync(new Guid());
        });

        Assert.Equal($"Sequence contains no elements", exception.Message);
    }
}