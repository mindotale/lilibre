using Lilibre.Web.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

using Radzen;

namespace Lilibre.Web.Pages;

public partial class EditBookAuthor
{
    protected bool errorVisible;
    protected BookAuthor bookAuthor;

    protected IEnumerable<Author> authorsForAuthorsId;

    protected IEnumerable<Book> booksForBookId;

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

    [Parameter]
    public int AuthorsId { get; set; }

    [Parameter]
    public int BookId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        bookAuthor = await DataService.GetBookAuthorByAuthorsIdAndBookId(AuthorsId, BookId);

        authorsForAuthorsId = await DataService.GetAuthors();

        booksForBookId = await DataService.GetBooks();
    }

    protected async Task FormSubmit()
    {
        try
        {
            await DataService.UpdateBookAuthor(AuthorsId, BookId, bookAuthor);
            DialogService.Close(bookAuthor);
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
