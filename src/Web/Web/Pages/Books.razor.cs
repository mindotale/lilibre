using Lilibre.Web.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

using Radzen;
using Radzen.Blazor;

namespace Lilibre.Web.Pages;

public partial class Books
{
    protected IEnumerable<Book> books;

    protected RadzenDataGrid<Book> grid0;

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
        books = await DataService.GetBooks();
    }

    protected async Task AddButtonClick(MouseEventArgs args)
    {
        await DialogService.OpenAsync<AddBook>("Add Book");
        await grid0.Reload();
    }

    protected async Task EditRow(Book args)
    {
        await DialogService.OpenAsync<EditBook>("Edit Book", new Dictionary<string, object> { { "Id", args.Id } });
    }

    protected async Task GridDeleteButtonClick(MouseEventArgs args, Book book)
    {
        try
        {
            if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
            {
                var deleteResult = await DataService.DeleteBook(book.Id);

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
                    Detail = "Unable to delete Book"
                });
        }
    }
}
