using DocumentsWeb.Code.Parser;
using DocumentsWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentsWeb.Code.Storage
{
    public class DocumentsFileStorage : IDocumentsStorage
    {
        private readonly DocumentsFileStorageOption _option;
        private readonly IFileParser _fileParser;
        private object _addResultLockObj = new object();
        private List<Document> _documents = new List<Document>();
        private DocumentsLoadingInfo _documentsLoadingInfo = new DocumentsLoadingInfo()
        {
            Status = DocumentsLoadingStatus.NotStart
        };

        public DocumentsFileStorage(DocumentsFileStorageOption option, IFileParser fileParser)
        {
            _option = option;
            _fileParser = fileParser;
            Task.Run(InitStore);
        }

        
        private void InitStore()
        {
            _documentsLoadingInfo.Status = DocumentsLoadingStatus.InProgress;
            DirectoryInfo directoryInfo = new DirectoryInfo(_option.Directory);
            FileInfo[] filesInfo = directoryInfo.GetFiles();
            Dictionary<int, string> fileDictionary = new Dictionary<int, string>();
            int i = 0;
            foreach (var fileInfo in filesInfo)
            {
                fileDictionary.Add(i, fileInfo.FullName);
                i++;
            }
            _documentsLoadingInfo.AllItems = i;

            foreach (var fileItem in fileDictionary)
            {
                _ = _fileParser
                    .Parse(fileItem.Value)
                    .ContinueWith(x => AddResult(x.Result, fileItem.Value, fileItem.Key));
            }
        }

        private void AddResult(DocumentContent documentContent, string fileName, int id)
        {
            lock (_addResultLockObj)
            {
                _documents.Add(new Document() { 
                    DocumentContent = documentContent,
                    Name = fileName,
                    Id = id
                });
                _documentsLoadingInfo.LoadingItems++;
                if(_documentsLoadingInfo.AllItems == _documentsLoadingInfo.LoadingItems)
                {
                    _documentsLoadingInfo.Status = DocumentsLoadingStatus.Complited;
                }
            }
        }

        public IEnumerable<Document> GetDocuments()
        {
            return _documents;
        }

        public DocumentsLoadingInfo GetLoadingInfo()
        {
            return _documentsLoadingInfo;
        }
    }
}
