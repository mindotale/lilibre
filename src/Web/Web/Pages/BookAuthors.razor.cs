using Lilibre.Web.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

using Radzen;
using Radzen.Blazor;

namespace Lilibre.Web.Pages;

public partial class BookAuthors
{
    protected IEnumerable<BookAuthor> bookAuthors;

    protected RadzenDataGrid<BookAuthor> grid0;

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
        bookAuthors = await DataService.GetBookAuthors(new Query { Expand = "Author,Book" });
    }

    protected async Task AddButtonClick(MouseEventArgs args)
    {
        await DialogService.OpenAsync<AddBookAuthor>("Add Book Author");
        await grid0.Reload();
    }

    protected async Task EditRow(BookAuthor args)
    {
        await DialogService.OpenAsync<EditBookAuthor>(
            "Edit Book Author",
            new Dictionary<string, object>
            {
                { "AuthorsId", args.AuthorsId },
                { "BookId", args.BookId }
            });
    }

    protected async Task GridDeleteButtonClick(MouseEventArgs args, BookAuthor bookAuthor)
    {
        try
        {
            if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
            {
                var deleteResult = await DataService.DeleteBookAuthor(bookAuthor.AuthorsId, bookAuthor.BookId);

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
                    Detail = "Unable to delete Book Author"
                });
        }
    }
}
