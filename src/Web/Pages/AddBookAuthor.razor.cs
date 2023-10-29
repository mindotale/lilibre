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
    public partial class AddBookAuthor
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

        protected override async Task OnInitializedAsync()
        {
            bookAuthor = new Lilibre.Web.Models.Data.BookAuthor();

            authorsForAuthorsId = await DataService.GetAuthors();

            booksForBookId = await DataService.GetBooks();
        }
        protected bool errorVisible;
        protected Lilibre.Web.Models.Data.BookAuthor bookAuthor;

        protected IEnumerable<Lilibre.Web.Models.Data.Author> authorsForAuthorsId;

        protected IEnumerable<Lilibre.Web.Models.Data.Book> booksForBookId;

        protected async Task FormSubmit()
        {
            try
            {
                await DataService.CreateBookAuthor(bookAuthor);
                DialogService.Close(bookAuthor);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}