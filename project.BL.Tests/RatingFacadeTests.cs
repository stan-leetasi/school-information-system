using project.BL.Models;
using project.BL.Facades;
using Xunit.Abstractions;
using project.DAL.Entities;
using project.Common.Tests.Seeds;
using project.BL.Filters;

namespace project.BL.Tests;

public sealed class RatingFacadeTests : FacadeTestsBase
{
    private readonly IRatingFacade _ratingFacadeSUT;
    private readonly RatingDetailModel _local;

    public RatingFacadeTests(ITestOutputHelper output) : base(output)
    {
        _ratingFacadeSUT = new RatingFacade(UnitOfWorkFactory, RatingModelMapper, RatingModelFilter);
        _local = new RatingDetailModel
        {
            Points = 5,
            Notes = "Great job!",
            StudentId = StudentSeeds.Terry.Id,
            StudentName = StudentSeeds.Terry.Name,
            ActivityId = ActivitiesSeeds.ICSCviko.Id,
            StudentSurname = StudentSeeds.Terry.Surname,
            ActivityName = Enum.GetName(RatingsSeeds.ICSCvikoRatingJohnL.Activity!.Type)!
        };
    }

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

    // Tests

    [Fact]
    public async Task Get_RatingDetailModel_ICSRating()
    {
        //Act
        var ratingDetailModel = await _ratingFacadeSUT.GetAsync(RatingsSeeds.ICSCvikoRatingJohnL.Id);

        //Assert
        AssertRatingDetailModel(RatingsSeeds.ICSCvikoRatingJohnL, ratingDetailModel);
    }

    [Fact]
    public async Task Get_RatingDetailModel_IOSRating()
    {
        //Act
        var ratingDetailModel = await _ratingFacadeSUT.GetAsync(RatingsSeeds.IOSPolsemkaRatingTerry.Id);

        //Assert
        AssertRatingDetailModel(RatingsSeeds.IOSPolsemkaRatingTerry, ratingDetailModel);
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
        var ratingListModel = await _ratingFacadeSUT.GetAsync();
        var ratingList = ratingListModel.ToList();
        var ICSRating = ratingList.SingleOrDefault(r => r.Id == RatingsSeeds.ICSCvikoRatingJohnL.Id);
        var IOSRating = ratingList.SingleOrDefault(r => r.Id == RatingsSeeds.IOSPolsemkaRatingTerry.Id);

        // Assert
        Assert.NotNull(ratingListModel);
        Assert.Equal(9, ratingList.Count);

        AssertRating(RatingsSeeds.ICSCvikoRatingJohnL, ICSRating);
        AssertRating(RatingsSeeds.IOSPolsemkaRatingTerry, IOSRating);
    }

    [Fact]
    public async Task Create_RatingDetailModel()
    {
        //Act

        var rating = await _ratingFacadeSUT.SaveAsync(_local);
        _local.Id = rating.Id;


        var remote = await _ratingFacadeSUT.GetAsync(rating.Id);

        //Assert
        Assert.NotNull(rating);
        AssertRatingDetailModel(_local, remote);
    }

    [Fact]
    public async Task Update_RatingDetailModel()
    {
        //Act
        _local.Id = RatingsSeeds.ICSCvikoRatingJohnL.Id;
        var rating = await _ratingFacadeSUT.SaveAsync(_local);
        _local.Id = rating.Id;

        var remote = await _ratingFacadeSUT.GetAsync(rating.Id);

        //Assert
        Assert.NotNull(rating);
        AssertRatingDetailModel(_local, remote);
    }

    [Fact]
    public async Task Delete_Existing_RatingDetailModel()
    {
        //Act
        await _ratingFacadeSUT.DeleteAsync(RatingsSeeds.ICSCvikoRatingJohnL.Id);

        var removed = await _ratingFacadeSUT.GetAsync(RatingsSeeds.ICSCvikoRatingJohnL.Id);

        //Assert
        Assert.Null(removed);
    }

    [Fact]
    public async Task Delete_NonExisting_RatingDetailModel()
    {
        //Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _ratingFacadeSUT.DeleteAsync(new Guid());
        });

        //Assert
        Assert.Equal($"Sequence contains no elements", exception.Message);
    }

    [Fact]
    public async Task Filter_Ratings_By_Student_Name_Terry()
    {
        // Arrange
        FilterPreferences preferences = FilterPreferences.Default with { SearchedTerm = "terry" };

        // Act
        IEnumerable<RatingListModel> listModels = await _ratingFacadeSUT.GetAsync(preferences);

        // Assert
        Assert.Contains(listModels, s => s.StudentName == StudentSeeds.Terry.Name);
    }

    [Fact]
    public async Task Filter_Ratings_By_Full_Student_Name_Surname_First()
    {
        // Arrange
        FilterPreferences preferences = FilterPreferences.Default with { SearchedTerm = "Alderson Elliot" };

        // Act
        IEnumerable<RatingListModel> listModels = await _ratingFacadeSUT.GetAsync(preferences);
        

        // Assert
        Assert.Contains(listModels, s => s.StudentName == StudentSeeds.Elliot.Name);
    }

    [Fact]
    public async Task Filter_Ratings_By_Student_Name_NonExisting()
    {
        // Arrange
        FilterPreferences preferences = FilterPreferences.Default with { SearchedTerm = "Larry" };

        // Act
        IEnumerable<RatingListModel> listModels = await _ratingFacadeSUT.GetAsync(preferences);

        // Assert
        Assert.Empty(listModels);
    }
}