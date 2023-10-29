using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace Lilibre.Web.Pages
{
    public partial class Books
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

        protected IEnumerable<Lilibre.Web.Models.Data.Book> books;

        protected RadzenDataGrid<Lilibre.Web.Models.Data.Book> grid0;
        protected override async Task OnInitializedAsync()
        {
            books = await DataService.GetBooks();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddBook>("Add Book", null);
            await grid0.Reload();
        }

        protected async Task EditRow(Lilibre.Web.Models.Data.Book args)
        {
            await DialogService.OpenAsync<EditBook>("Edit Book", new Dictionary<string, object> { {"Id", args.Id} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, Lilibre.Web.Models.Data.Book book)
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
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete Book"
                });
            }
        }
    }
}