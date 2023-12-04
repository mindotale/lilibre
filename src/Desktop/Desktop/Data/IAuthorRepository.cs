using System.Collections.Generic;

using Desktop.Models;

namespace Desktop.Data;

public interface IAuthorRepository
{
    IEnumerable<Author> GetAll();

    Author? GetById(int id);

    Author Add(Author author);

    Author Update(Author author);

    void Remove(int id);
}
