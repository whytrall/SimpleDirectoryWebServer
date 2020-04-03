using System.IO;
using System.Net;
using SimpleDirectoryWebServer.FileSupport;

namespace SimpleDirectoryWebServer.Response.Page {
    public class FilePage: Page {
        private FileStream _fileStream;
        public override HttpStatusCode StatusCode => HttpStatusCode.OK;
        public override string ClassName => "FilePage";

        public override string MimeType {
            get {
                var fileDec = new FileDecorator(_fileStream);
                return fileDec.MimeType();
            }
        }

        public FilePage(string filePath) : base(filePath) {
            _fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public override Stream GetStream() {
            return _fileStream;
        }
    }
}