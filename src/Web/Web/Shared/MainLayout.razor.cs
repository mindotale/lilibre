using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using Radzen;

namespace Lilibre.Web.Shared;

public partial class MainLayout
{
    private bool sidebarExpanded = true;

    [Inject]
    protected IJSRuntime JSRuntime { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    protected DialogService DialogService { get; set; }

    [Inject]
    protected TooltipService TooltipService { get; set; }

    [Inject]
    protected ContextMenuService ContextMenuService { get; set; }

    [Inject]
    protected NotificationService NotificationService { get; set; }

    private void SidebarToggleClick()
    {
        sidebarExpanded = !sidebarExpanded;
    }
}
