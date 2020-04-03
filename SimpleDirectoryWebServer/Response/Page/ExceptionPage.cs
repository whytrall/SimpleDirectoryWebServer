using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using SimpleDirectoryWebServer.BasicClasses;
using SimpleDirectoryWebServer.Header;
using Stubble.Core.Builders;

namespace SimpleDirectoryWebServer.Response.Page {
    public class ExceptionPage: Page {
        public override HttpStatusCode StatusCode { get; }

        public ExceptionPage(Exception e) : base(e) {
            StatusCode = GetHttpStatus();
        }
        public override string MimeType => "text/html";
        public override string ClassName => "ExceptionPage";
        
        public override Stream GetStream() {
            var stubble = new StubbleBuilder()
                .Configure(settings => {
                    settings.SetIgnoreCaseOnKeyLookup(true);
                    settings.SetMaxRecursionDepth(512);
                })
                .Build();
            
            var encoded = Encoding.ASCII.GetBytes(
                stubble.Render(new StreamReader($"{ProjectPath}/Templates/errors.html", Encoding.UTF8).ReadToEnd(), new {
                    Title = "Error",
                    ErrorText = StatusCode.ToString()
                })
            );
            return new MemoryStream(encoded);
        }

        private HttpStatusCode GetHttpStatus() {
            var code = HttpStatusCode.InternalServerError;
            
            var ts = new TypeSwitch()
                .Case((InvalidHttpHeaderException e) => code = HttpStatusCode.BadRequest)
                .Case((FileNotFoundException e) => code = HttpStatusCode.NotFound)
                .Case((UnauthorizedAccessException e) => code = HttpStatusCode.Forbidden)
                .DefaultAction((e) => code = HttpStatusCode.InternalServerError);
            ts.Switch(_exception);

            return code;
        }
    }
}