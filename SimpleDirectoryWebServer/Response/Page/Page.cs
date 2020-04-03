using System;
using System.IO;
using System.Net;

namespace SimpleDirectoryWebServer.Response.Page {
    public abstract class Page {
        protected string _filePath;
        protected Exception _exception;
        public abstract string MimeType { get; }
        public abstract HttpStatusCode StatusCode { get; }
        public abstract string ClassName { get; }

        protected readonly string ProjectPath =
            Path.GetDirectoryName(
                Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())));
        public Page(string filePath) {
            _filePath = filePath;
        }
        public Page(Exception e) {
            _exception = e;
        }
        
        public abstract Stream GetStream();
    }
}