using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using Lilibre.Web.Data;

namespace Lilibre.Web.Controllers
{
    public partial class ExportDataController : ExportController
    {
        private readonly DataContext context;
        private readonly DataService service;

        public ExportDataController(DataContext context, DataService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/Data/authors/csv")]
        [HttpGet("/export/Data/authors/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAuthorsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAuthors(), Request.Query), fileName);
        }

        [HttpGet("/export/Data/authors/excel")]
        [HttpGet("/export/Data/authors/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAuthorsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAuthors(), Request.Query), fileName);
        }

        [HttpGet("/export/Data/bookauthors/csv")]
        [HttpGet("/export/Data/bookauthors/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBookAuthorsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetBookAuthors(), Request.Query), fileName);
        }

        [HttpGet("/export/Data/bookauthors/excel")]
        [HttpGet("/export/Data/bookauthors/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBookAuthorsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetBookAuthors(), Request.Query), fileName);
        }

        [HttpGet("/export/Data/bookgenres/csv")]
        [HttpGet("/export/Data/bookgenres/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBookGenresToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetBookGenres(), Request.Query), fileName);
        }

        [HttpGet("/export/Data/bookgenres/excel")]
        [HttpGet("/export/Data/bookgenres/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBookGenresToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetBookGenres(), Request.Query), fileName);
        }

        [HttpGet("/export/Data/books/csv")]
        [HttpGet("/export/Data/books/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBooksToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetBooks(), Request.Query), fileName);
        }

        [HttpGet("/export/Data/books/excel")]
        [HttpGet("/export/Data/books/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBooksToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetBooks(), Request.Query), fileName);
        }

        [HttpGet("/export/Data/genres/csv")]
        [HttpGet("/export/Data/genres/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportGenresToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetGenres(), Request.Query), fileName);
        }

        [HttpGet("/export/Data/genres/excel")]
        [HttpGet("/export/Data/genres/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportGenresToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetGenres(), Request.Query), fileName);
        }
    }
}
