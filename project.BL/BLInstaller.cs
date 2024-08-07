﻿using project.BL.Facades;
using project.BL.Mappers;
using project.DAL.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using project.BL.Filters;

namespace project.BL;

public static class BLInstaller
{
    public static IServiceCollection AddBLServices(this IServiceCollection services)
    {
        services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();

        services.AddSingleton<StudentModelMapper>();
        services.AddSingleton<SubjectModelMapper>();
        services.AddSingleton<ActivityModelMapper>();
        services.AddSingleton<RatingModelMapper>();

        services.AddSingleton<StudentModelFilter>();
        services.AddSingleton<SubjectModelFilter>();
        services.AddSingleton<ActivityModelFilter>();
        services.AddSingleton<RatingModelFilter>();

        services.AddSingleton<IStudentFacade, StudentFacade>();
        services.AddSingleton<ISubjectFacade, SubjectFacade>();
        services.AddSingleton<IActivityFacade, ActivityFacade>();
        services.AddSingleton<IRatingFacade, RatingFacade>();

        return services;
    }
}
