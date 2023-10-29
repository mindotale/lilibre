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
    public partial class BookAuthors
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

        protected IEnumerable<BookAuthor> bookAuthors;

        protected RadzenDataGrid<BookAuthor> grid0;
        protected override async Task OnInitializedAsync()
        {
            bookAuthors = await DataService.GetBookAuthors(new Query { Expand = "Author,Book" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddBookAuthor>("Add BookAuthor", null);
            await grid0.Reload();
        }

        protected async Task EditRow(BookAuthor args)
        {
            await DialogService.OpenAsync<EditBookAuthor>("Edit BookAuthor", new Dictionary<string, object> { {"AuthorsId", args.AuthorsId}, {"BookId", args.BookId} });
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
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete BookAuthor"
                });
            }
        }
    }
}