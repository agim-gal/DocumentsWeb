using DocumentsWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsWeb.Code.Parser
{
    public class FileParser : IFileParser
    {
        public async Task<DocumentContent> Parse(string fileName)
        {
            try
            {
                using Stream fileStream = new FileStream(fileName, FileMode.Open);
                using StreamReader sr = new StreamReader(fileStream);
                while (!sr.EndOfStream)
                {
                    var line = await sr.ReadLineAsync();
                    if (line.StartsWith("$$9 "))
                    {
                        var decisionOnCaseContent = new StringBuilder();
                        while (!sr.EndOfStream)
                        {
                            var contentLine = await sr.ReadLineAsync();
                            if (contentLine.StartsWith("$$"))
                            {
                                break;
                            }
                            decisionOnCaseContent.AppendLine(contentLine);
                        }
                        return new DocumentContent()
                        {
                            DecisionOnCase = decisionOnCaseContent.ToString(),
                        };
                    }
                }
                return new DocumentContent();
            }
            catch
            {
                return null;
            }

        }
    }
}
