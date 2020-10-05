using DocumentsWeb.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentsWeb.Code.Storage
{
    public interface IDocumentsStorage
    {
        public IEnumerable<Document> GetDocuments();
        public DocumentsLoadingInfo GetLoadingInfo();
    }
}
