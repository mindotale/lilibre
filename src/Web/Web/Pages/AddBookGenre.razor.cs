using Lilibre.Web.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

using Radzen;

namespace Lilibre.Web.Pages;

public partial class AddBookGenre
{
    protected bool errorVisible;
    protected BookGenre bookGenre;

    protected IEnumerable<Book> booksForBookId;

    protected IEnumerable<Genre> genresForGenresId;

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
        bookGenre = new BookGenre();

        booksForBookId = await DataService.GetBooks();

        genresForGenresId = await DataService.GetGenres();
    }

    protected async Task FormSubmit()
    {
        try
        {
            await DataService.CreateBookGenre(bookGenre);
            DialogService.Close(bookGenre);
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