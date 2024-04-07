using project.BL.Facades;
using project.BL.Models;
using project.Common.Tests;
using project.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Diagnostics;

namespace project.BL.Tests;

public sealed class StudentFacadeTests : FacadeTestsBase
{
    private readonly IStudentFacade _studentFacadeSUT;

    public StudentFacadeTests(ITestOutputHelper output) : base(output)
    {
        _studentFacadeSUT = new StudentFacade(UnitOfWorkFactory,StudentModelMapper);
    }

    [Fact]
    public async Task Get_StudentListModel_For_John()
    {
        //Act
        IEnumerable<StudentListModel> listModels = await _studentFacadeSUT.GetAsync();
        Assert.NotNull(listModels);
        var John = listModels.SingleOrDefault(s => s.Id == StudentSeeds.John.Id);

        //Assert
        Assert.NotNull(John);
        Assert.Equal("John", John!.Name);
        Assert.Equal("Lark", John!.Surname);
        Assert.Equal(John.Id, StudentSeeds.John.Id);
    }

    [Fact]
    public async Task Get_StudentDetailModel_For_Terry() 
    {
        // Act
        var Terry = await _studentFacadeSUT.GetAsync(StudentSeeds.Terry.Id);
        
        // Assert
        Assert.NotNull(Terry);
        Assert.Equal("Terry",Terry.Name);
        Assert.Equal("Davis", Terry.Surname);
        Assert.Equal(StudentSeeds.Terry.Id, Terry.Id);
        Assert.Equal(StudentSeeds.Terry.ImageUrl, Terry.ImageUrl);
    }

    [Fact]
    public async Task Edit_Student_Details_Terry()
    {
        // Arrange
        var Student = await _studentFacadeSUT.GetAsync(StudentSeeds.Terry.Id);

        Student.Name = "Maurice";
        Student.Surname = "Moss";
        Student.ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ6X-H8hmtdTRXhVZCMLWI6JS6H4uuaRvLfLMUFoEl9LA&s";

        // Act
        var SavedStudent = await _studentFacadeSUT.SaveAsync(Student);
        var Maurice = await _studentFacadeSUT.GetAsync(StudentSeeds.Terry.Id);

        // Assert
        Assert.NotNull(Maurice);
        Assert.Equal(SavedStudent, Maurice);
        Assert.Equal(Student.Name, Maurice.Name);
        Assert.Equal(Student.Surname, Maurice.Surname);
        Assert.Equal(Student.ImageUrl, Maurice.ImageUrl);
        Assert.Equal(Student.Id, Maurice.Id);
    }

    [Fact]
    public async Task Add_A_New_Student()
    {
        // Arrange

        var NewStudentName = "Jen";
        var NewStudentSurname = "Barber";
        var NewStudentImageUrl = "https://static.wikia.nocookie.net/offandonagain/images/7/76/The_it_crowd_jen.jpg/revision/latest/scale-to-width-down/250?cb=20091123145816";
        
        var NewStudent = new StudentDetailModel()
        {
            Name = NewStudentName,
            Surname = NewStudentSurname,
            ImageUrl = NewStudentImageUrl
        };


        // Act
        var Jen = await _studentFacadeSUT.SaveAsync(NewStudent);
        Assert.NotNull(Jen);

        var JenRetrieved = await _studentFacadeSUT.GetAsync(Jen.Id);

        IEnumerable<StudentListModel> listModels = await _studentFacadeSUT.GetAsync();
        Assert.NotNull(listModels);
        var Jen_ListModel = listModels.SingleOrDefault(s => s.Id == Jen.Id);

        // Assert
        Assert.NotNull(Jen_ListModel);
        Assert.NotNull(JenRetrieved);

        Assert.Equal(Jen, JenRetrieved);
        Assert.Equal(NewStudentName, Jen.Name);
        Assert.Equal(NewStudentSurname, Jen.Surname);
        Assert.Equal(NewStudentImageUrl, Jen.ImageUrl);

        Assert.Equal(Jen_ListModel.Name, Jen.Name);
        Assert.Equal(Jen_ListModel.Surname, Jen.Surname);
        Assert.Equal(Jen_ListModel.Id, Jen.Id);
    }

    [Fact]
    public async Task Remove_Terry()
    {
        // Act
        var Terry = await _studentFacadeSUT.GetAsync(StudentSeeds.Terry.Id);
        await _studentFacadeSUT.DeleteAsync(StudentSeeds.Terry.Id);

        var TerryOznuk = await _studentFacadeSUT.GetAsync(StudentSeeds.Terry.Id);

        // Assert
        Assert.NotNull(Terry);
        Assert.Null(TerryOznuk);
    }

    [Fact]
    public async Task Remove_Non_Existing_Student()
    {
        // Arrange
        var NonExistingStudentID = new Guid();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _studentFacadeSUT.DeleteAsync(NonExistingStudentID);
        });

        Assert.Equal($"Sequence contains no elements", exception.Message);
        
    }
}