﻿namespace BlazorApp.Shared.Notifications;

public interface INotificationMessage
{
    public string MessageType { get; set; }

    public string? Message { get; set; }
}