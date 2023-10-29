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
    public partial class AddBookGenre
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
            bookGenre = new Lilibre.Web.Models.Data.BookGenre();

            booksForBookId = await DataService.GetBooks();

            genresForGenresId = await DataService.GetGenres();
        }
        protected bool errorVisible;
        protected Lilibre.Web.Models.Data.BookGenre bookGenre;

        protected IEnumerable<Lilibre.Web.Models.Data.Book> booksForBookId;

        protected IEnumerable<Lilibre.Web.Models.Data.Genre> genresForGenresId;

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
            DialogService.Close(null);
        }
    }
}