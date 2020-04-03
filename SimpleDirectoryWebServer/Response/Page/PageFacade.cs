using System;
using System.IO;
using System.Net;
using SimpleDirectoryWebServer.RequestChecks;

namespace SimpleDirectoryWebServer.Response.Page {
    public class PageFacade {
        private string _filePath;
        private Page _currPage;
        public readonly string MimeType;
        private Exception _exception;
        public readonly HttpStatusCode StatusCode;
        public readonly string ClassName;

        
        public PageFacade(string filePath) {
            _filePath = filePath;
            
            var fldCheck = new FolderSecurityCheck();
            fldCheck.SetNext(new ExistenceCheck());
            fldCheck.SetNext(new PermissionsCheck());
            fldCheck.Handle(filePath);
            
            _currPage = SelectPage();
            MimeType = _currPage.MimeType;
            StatusCode = _currPage.StatusCode;
            ClassName = _currPage.ClassName;
        }

        public PageFacade(Exception e) {
            _exception = e;
            
            _currPage = SelectPage();
            MimeType = _currPage.MimeType;
            StatusCode = _currPage.StatusCode;
            ClassName = _currPage.ClassName;
        }

        public Stream GetStream() {
            return _currPage.GetStream();
        }

        private Page SelectPage() {
            if (_filePath != null) {
                var attr = File.GetAttributes(_filePath);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory) {
                    return new DirectoryPage(_filePath);
                }
                return new FilePage(_filePath);
            }
            
            if (_exception != null) {
                return new ExceptionPage(_exception);
            }

            throw new Exception("bad type");
        }
    }
}