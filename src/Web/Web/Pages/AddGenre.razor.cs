using Lilibre.Web.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

using Radzen;

namespace Lilibre.Web.Pages;

public partial class AddGenre
{
    protected bool errorVisible;
    protected Genre genre;

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

    [Inject]
    public DataService DataService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        genre = new Genre();
    }

    protected async Task FormSubmit()
    {
        try
        {
            await DataService.CreateGenre(genre);
            DialogService.Close(genre);
        }
        catch (Exception ex)
        {
            errorVisible = true;
        }
    }

    protected async Task CancelButtonClick(MouseEventArgs args)
    {
        DialogService.Close();
    }
}