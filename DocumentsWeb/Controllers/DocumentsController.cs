using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentsWeb.Code.Repository;
using DocumentsWeb.Code.Storage;
using DocumentsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DocumentsWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentsStorage _documentsStorage;
        private readonly IDocumentsRepository _documentsRepository;

        public DocumentsController(IDocumentsStorage documentsStorage, IDocumentsRepository documentsRepository)
        {
            _documentsStorage = documentsStorage;
            _documentsRepository = documentsRepository;
        }

        [HttpGet]
        [Route("[action]")]
        public DocumentsLoadingInfo GetDocumentsLoadingInfo()
        {
            return _documentsStorage.GetLoadingInfo();
        }

        [HttpGet]
        public ListContainer<Document> Get(int page, int size, string term)
        {
            var result = String.IsNullOrEmpty(term) ?
                _documentsRepository.Get((page - 1) * size, size) :
                _documentsRepository.GetByTerm((page - 1) * size, size, term);
            return result;
        }
    }
}
