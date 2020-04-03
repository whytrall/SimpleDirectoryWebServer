using System;
using System.Text;
using System.Net.Sockets;
using SimpleDirectoryWebServer.Header;
using SimpleDirectoryWebServer.Response;
using SimpleDirectoryWebServer.Response.Page;

namespace SimpleDirectoryWebServer {
    
    public class Client: IDisposable {
        private readonly TcpClient _client;
        private string ReadRequest() {
            var buffer = new byte[1024];
            int count;
            var req = new StringBuilder();
            while ((count = _client.GetStream().Read(buffer, 0, buffer.Length)) > 0) {
                var bufToS = Encoding.ASCII.GetString(buffer, 0, count);
                req.Append(bufToS.Split("\r\n\r\n")[0]);
                
                if (bufToS.IndexOf("\r\n\r\n", StringComparison.Ordinal) >= 0 || req.Length > 4096) break;
            }

            return req.ToString();
        }
 
        public Client(TcpClient client) {
            _client = client;
        }

        public void Dispose() {
            if (_client.Connected) {
                _client.Close();
            }
        }

        public void Process() {
            PageFacade page;

            try {
                var reqParse = HttpHeader.Parse(ReadRequest());
                var headerDec = new HttpHeaderDecorator(reqParse);
                
                page = new PageFacade(headerDec.GetUrl());
            }
            catch (Exception e) {
                page = new PageFacade(e);
            }

            var header = new HttpHeader()
                .SetHttpStatus(page.StatusCode)
                .SetKey("Content-Type", page.MimeType);
            
            new ResponseBuilder()
                .SetDest(_client.GetStream())
                .SetOrig(page.GetStream())
                .SetHeader(header)
                .StreamResponse();
        }
    }
}