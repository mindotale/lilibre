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
    public partial class Authors
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

        protected IEnumerable<Lilibre.Web.Models.Data.Author> authors;

        protected RadzenDataGrid<Lilibre.Web.Models.Data.Author> grid0;
        protected override async Task OnInitializedAsync()
        {
            authors = await DataService.GetAuthors();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddAuthor>("Add Author", null);
            await grid0.Reload();
        }

        protected async Task EditRow(Lilibre.Web.Models.Data.Author args)
        {
            await DialogService.OpenAsync<EditAuthor>("Edit Author", new Dictionary<string, object> { {"Id", args.Id} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, Lilibre.Web.Models.Data.Author author)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await DataService.DeleteAuthor(author.Id);

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
                    Detail = $"Unable to delete Author"
                });
            }
        }
    }
}