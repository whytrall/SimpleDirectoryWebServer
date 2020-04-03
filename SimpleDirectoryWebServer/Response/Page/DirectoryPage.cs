using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Stubble.Core.Builders;

namespace SimpleDirectoryWebServer.Response.Page {
    public class DirectoryPage: Page {
        public DirectoryPage(string filePath): base(filePath) {}
        public override string MimeType => "text/html";
        public override HttpStatusCode StatusCode => HttpStatusCode.OK;
        public override string ClassName => "DirectoryPage";

        public override Stream GetStream() {
            var stubble = new StubbleBuilder()
                .Configure(settings => {
                    settings.SetIgnoreCaseOnKeyLookup(true);
                    settings.SetMaxRecursionDepth(512);
                })
                .Build();

            string backUrl = null;
            if (_filePath != "/") {
                backUrl = Path.GetDirectoryName(_filePath);
            }

            var encoded = Encoding.ASCII.GetBytes(
                stubble.Render(new StreamReader($"{ProjectPath}/Templates/main.html", Encoding.UTF8).ReadToEnd(), new {
                    IsRoot = _filePath == "/",
                    BackUrl = backUrl,
                    WithListing = true,
                    Title = new DirectoryInfo(_filePath).Name,
                    Files = Directory.GetFiles(_filePath).Select(dir => {
                        dir = dir.Trim();
                        var a = new Dictionary<string, string>();
                        a.Add("BaseName", new FileInfo(dir).Name);
                        a.Add("FullPath", dir);
                        return a;
                    }).ToList(),
                    Directories = Directory.GetDirectories(_filePath).Select(dir => {
                        var a = new Dictionary<string, string>();
                        a.Add("BaseName", new DirectoryInfo(dir).Name);
                        a.Add("FullPath", dir);
                        return a;
                    }).ToList()
                })
                );
            return new MemoryStream(encoded);
        }
        
    }
}