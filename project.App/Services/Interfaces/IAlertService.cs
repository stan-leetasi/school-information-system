﻿namespace project.App.Services;

public interface IAlertService
{
    Task DisplayAsync(string title, string message);
    Task<bool> ConfirmAsync(string title, string message);
}
