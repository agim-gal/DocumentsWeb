using DocumentsWeb.Models;
using System.Collections.Generic;

namespace DocumentsWeb.Code.Repository
{
    public interface IDocumentsRepository
    {
        ListContainer<Document> Get(int skip, int take);
        Document GetById(int id);
        ListContainer<Document> GetByTerm(int skip, int take, string term);
    }
}