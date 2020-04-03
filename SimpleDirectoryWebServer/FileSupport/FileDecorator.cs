using System.IO;

namespace SimpleDirectoryWebServer.FileSupport {
    public class FileDecorator {
        private FileStream _file;

        public FileDecorator(FileStream fileStream) {
            _file = fileStream;
        }

        public string MimeType() {
            return MimeMapping.MimeUtility.GetMimeMapping(_file.Name);
        }

        public string FileEncoding() {
            var code = new StreamReader(_file).CurrentEncoding;
            return code.ToString();
        }
    }
}