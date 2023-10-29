using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Lilibre.Web.Models;

namespace Lilibre.Web.Pages
{
    public partial class BookGenres
    {
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

        protected IEnumerable<BookGenre> bookGenres;

        protected RadzenDataGrid<BookGenre> grid0;
        protected override async Task OnInitializedAsync()
        {
            bookGenres = await DataService.GetBookGenres(new Query { Expand = "Book,Genre" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddBookGenre>("Add BookGenre", null);
            await grid0.Reload();
        }

        protected async Task EditRow(BookGenre args)
        {
            await DialogService.OpenAsync<EditBookGenre>("Edit BookGenre", new Dictionary<string, object> { {"BookId", args.BookId}, {"GenresId", args.GenresId} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, BookGenre bookGenre)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await DataService.DeleteBookGenre(bookGenre.BookId, bookGenre.GenresId);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete BookGenre"
                });
            }
        }
    }
}