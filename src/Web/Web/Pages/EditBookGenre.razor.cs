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
    public partial class EditBookGenre
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
        public int BookId { get; set; }

        [Parameter]
        public int GenresId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            bookGenre = await DataService.GetBookGenreByBookIdAndGenresId(BookId, GenresId);

            booksForBookId = await DataService.GetBooks();

            genresForGenresId = await DataService.GetGenres();
        }
        protected bool errorVisible;
        protected BookGenre bookGenre;

        protected IEnumerable<Book> booksForBookId;

        protected IEnumerable<Genre> genresForGenresId;

        protected async Task FormSubmit()
        {
            try
            {
                await DataService.UpdateBookGenre(BookId, GenresId, bookGenre);
                DialogService.Close(bookGenre);
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