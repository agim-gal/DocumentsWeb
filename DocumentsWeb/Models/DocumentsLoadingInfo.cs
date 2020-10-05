using System;

namespace DocumentsWeb.Models
{
    public class DocumentsLoadingInfo
    {
        public int Percent { get => (int)Math.Round((double)LoadingItems / AllItems * 100.0); }
        public int AllItems { get; set; }
        public int LoadingItems { get; set; }
        public DocumentsLoadingStatus Status { get;set;}
    }
}