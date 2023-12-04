using Desktop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desktop.Data;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAllAsync();

    Task<Author?> GetAsync(int id);

    Task<Author> CreateAsync(Author author);

    Task<Author> UpdateAsync(Author author);

    Task DeleteAsync(int id);
}
