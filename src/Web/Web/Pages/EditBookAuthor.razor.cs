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
    public partial class EditBookAuthor
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
        protected bool errorVisible;
        protected BookAuthor bookAuthor;

        protected IEnumerable<Author> authorsForAuthorsId;

        protected IEnumerable<Book> booksForBookId;

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
            DialogService.Close(null);
        }
    }
}