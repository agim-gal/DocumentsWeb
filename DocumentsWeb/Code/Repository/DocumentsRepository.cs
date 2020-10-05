using DocumentsWeb.Code.Storage;
using DocumentsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentsWeb.Code.Repository
{
    public class DocumentsRepository : IDocumentsRepository
    {
        private readonly IDocumentsStorage _documentsStorage;

        public DocumentsRepository(IDocumentsStorage documentsStorage)
        {
            _documentsStorage = documentsStorage;
        }

        public ListContainer<Document> Get(int skip, int take)
        {
            var all = _documentsStorage
                .GetDocuments()
                .Where(x => !String.IsNullOrEmpty(x?.DocumentContent?.DecisionOnCase));
            return new ListContainer<Document>( 
                all 
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(take),
                all.Count());
        }

        public ListContainer<Document> GetByTerm(int skip, int take, string term)
        {
            var all = _documentsStorage
               .GetDocuments()
               .Where(x => !String.IsNullOrEmpty(x.DocumentContent.DecisionOnCase) && x.Name.Contains(term));
            return new ListContainer<Document>(
                all
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(take),
                all.Count());
        }

        public Document GetById(int id)
        {
            return _documentsStorage
                .GetDocuments()
                .FirstOrDefault(x => x.Id == id);
        }

    }
}
