using DocumentsWeb.Models;
using System.Threading.Tasks;

namespace DocumentsWeb.Code.Parser
{
    public interface IFileParser
    {
        Task<DocumentContent> Parse(string fileName);
    }
}