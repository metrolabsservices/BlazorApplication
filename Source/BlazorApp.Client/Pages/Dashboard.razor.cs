﻿using BlazorApp.Client.ApiClient;
using BlazorApp.Client.Dashboard;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace BlazorApp.Client.Pages;

public partial class Dashboard
{
    [Inject]
    private IStatsClient StatsClient { get; set; } = default!;

    [Inject]
    private HubConnection HubConnection { get; set; } = default!;

    [Parameter]
    public int UserCount { get; set; }

    [Parameter]
    public int RoleCount { get; set; }

    private bool _loaded = false;

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
        _loaded = true;
        HubConnection.On<StatsChangedNotification>(nameof(StatsChangedNotification), async _ =>
        {
            await LoadDataAsync();
            StateHasChanged();
        });
    }

    private async Task LoadDataAsync()
    {
        var response = await StatsClient.GetAsync();
        if (response.Succeeded)
        {
            if (response.Data is not null)
            {
                UserCount = response.Data.UserCount;
                RoleCount = response.Data.RoleCount;
            }
        }
        else if (response.Messages is not null)
        {
            foreach (string message in response.Messages)
            {
                _snackBar.Add(message, Severity.Error);
            }
        }
    }
}