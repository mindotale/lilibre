using Lilibre.Web.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

using Radzen;
using Radzen.Blazor;

namespace Lilibre.Web.Pages;

public partial class Genres
{
    protected IEnumerable<Genre> genres;

    protected RadzenDataGrid<Genre> grid0;

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
        genres = await DataService.GetGenres();
    }

    protected async Task AddButtonClick(MouseEventArgs args)
    {
        await DialogService.OpenAsync<AddGenre>("Add Genre");
        await grid0.Reload();
    }

    protected async Task EditRow(Genre args)
    {
        await DialogService.OpenAsync<EditGenre>("Edit Genre", new Dictionary<string, object> { { "Id", args.Id } });
    }

    protected async Task GridDeleteButtonClick(MouseEventArgs args, Genre genre)
    {
        try
        {
            if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
            {
                var deleteResult = await DataService.DeleteGenre(genre.Id);

                if (deleteResult != null)
                {
                    await grid0.Reload();
                }
            }
        }
        catch (Exception ex)
        {
            NotificationService.Notify(
                new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = "Unable to delete Genre"
                });
        }
    }
}
