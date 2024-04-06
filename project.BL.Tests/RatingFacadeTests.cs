using project.BL.Models;
using project.BL.Facades;
using Xunit.Abstractions;
using project.DAL.Entities;
using project.Common.Tests.Seeds;

namespace project.BL.Tests;

public sealed class RatingFacadeTests : FacadeTestsBase
{
    private readonly IRatingFacade _ratingFacadeSut;
    private readonly RatingDetailModel _local;

    private static void AssertRatingDetailModel(RatingDetailModel expected, RatingDetailModel? actual)
    {
        Assert.NotNull(actual);
        Assert.NotNull(expected);
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.Notes, actual.Notes);
        Assert.Equal(expected.Points, actual.Points);
        Assert.Equal(expected.StudentId, actual.StudentId);
        Assert.Equal(expected.ActivityId, actual.ActivityId);
        Assert.Equal(expected.StudentName, actual.StudentName);
        Assert.Equal(expected.ActivityName, actual.ActivityName);
        Assert.Equal(expected.StudentSurname, actual.StudentSurname);
    }

    private static void AssertRatingDetailModel(RatingEntity expected, RatingDetailModel? actual)
    {
        Assert.NotNull(actual);
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.Notes, actual.Notes);
        Assert.Equal(expected.Points, actual.Points);
        Assert.Equal(expected.StudentId, actual.StudentId);
        Assert.Equal(expected.ActivityId, actual.ActivityId);
        Assert.Equal(expected.Student!.Name, actual.StudentName);
        Assert.Equal(expected.Student!.Surname, actual.StudentSurname);
        Assert.Equal(Enum.GetName(expected.Activity!.Type), actual.ActivityName);
    }

    public RatingFacadeTests(ITestOutputHelper output) : base(output)
    {
        _ratingFacadeSut = new RatingFacade(UnitOfWorkFactory, RatingModelMapper);
        _local = new RatingDetailModel
        {
            Points = 5,
            Notes = "Great job!",
            StudentId = StudentSeeds.Terry.Id,
            StudentName = StudentSeeds.Terry.Name,
            ActivityId = ActivitiesSeeds.ICSCviko.Id,
            StudentSurname = StudentSeeds.Terry.Surname,
            ActivityName = Enum.GetName(RatingsSeeds.ICSRating.Activity!.Type)!
        };
    }

    // Tests

    [Fact]
    public async Task Get_RatingDetailModel_ICSRating()
    {
        //Act
        var ratingDetailModel = await _ratingFacadeSut.GetAsync(RatingsSeeds.ICSRating.Id);

        //Assert
        AssertRatingDetailModel(RatingsSeeds.ICSRating, ratingDetailModel);
    }

    [Fact]
    public async Task Get_RatingDetailModel_IOSRating()
    {
        //Act
        var ratingDetailModel = await _ratingFacadeSut.GetAsync(RatingsSeeds.IOSRating.Id);

        //Assert
        AssertRatingDetailModel(RatingsSeeds.IOSRating, ratingDetailModel);
    }

    [Fact]
    public async Task Get_RatingListModel()
    {
        // Arrange
        void AssertRating(RatingEntity expected, RatingListModel? actual)
        {
            Assert.NotNull(actual);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Points, actual.Points);
            Assert.Equal(expected.StudentId, actual.StudentId);
            Assert.Equal(expected.Student!.Name, actual.StudentName);
            Assert.Equal(expected.Student!.Surname, actual.StudentSurname);
        }

        // Act
        var ratingListModel = await _ratingFacadeSut.GetAsync();
        var ratingList = ratingListModel.ToList();
        var ICSRating = ratingList.SingleOrDefault(r => r.Id == RatingsSeeds.ICSRating.Id);
        var IOSRating = ratingList.SingleOrDefault(r => r.Id == RatingsSeeds.IOSRating.Id);

        // Assert
        Assert.NotNull(ratingListModel);
        Assert.Equal(2, ratingList.Count);

        AssertRating(RatingsSeeds.ICSRating, ICSRating);
        AssertRating(RatingsSeeds.IOSRating, IOSRating);
    }

    [Fact]
    public async Task Create_RatingDetailModel()
    {
        //Act
        var rating = await _ratingFacadeSut.SaveAsync(_local);
        _local.Id = rating.Id;


        var remote = await _ratingFacadeSut.GetAsync(rating.Id);

        //Assert
        Assert.NotNull(rating);
        AssertRatingDetailModel(_local, remote);
    }

    [Fact]
    public async Task Update_RatingDetailModel()
    {
        //Act
        var rating = await _ratingFacadeSut.SaveAsync(_local);
        _local.Id = rating.Id;

        var remote = await _ratingFacadeSut.GetAsync(rating.Id);

        //Assert
        Assert.NotNull(rating);
        AssertRatingDetailModel(_local, remote);
    }

    [Fact]
    public async Task Delete_Existing_RatingDetailModel()
    {
        //Act
        await _ratingFacadeSut.DeleteAsync(RatingsSeeds.ICSRating.Id);

        var removed = await _ratingFacadeSut.GetAsync(RatingsSeeds.ICSRating.Id);

        //Assert
        Assert.Null(removed);
    }

    [Fact]
    public async Task Delete_NonExisting_RatingDetailModel()
    {
        //Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _ratingFacadeSut.DeleteAsync(new Guid());
        });

        //Assert
        Assert.Equal($"Sequence contains no elements", exception.Message);
    }
}