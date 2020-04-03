using System.IO;
using System.Text;
using SimpleDirectoryWebServer.Header;

namespace SimpleDirectoryWebServer.Response {
    public class ResponseBuilder {
        private HttpHeader _header;
        private Stream input;
        private Stream output;

        public ResponseBuilder SetDest(Stream to) {
            output = to;
            return this;
        }

        public ResponseBuilder SetOrig(Stream from) {
            input = from;
            return this;
        }

        public ResponseBuilder SetOrigFromObj<T>(T obj) {
            var encoded = Encoding.ASCII.GetBytes(obj.ToString());
            input = new MemoryStream(encoded);
            
            return this;
        }

        public ResponseBuilder SetHeader(HttpHeader header) {
            _header = header;
            return this;
        }

        public void StreamResponse() {
            _header["Content-Length"] = input.Length.ToString();
            var headerBytes = Encoding.ASCII.GetBytes($"{_header}\n");
            output.Write(headerBytes, 0, headerBytes.Length);
            
            var buffer = new byte[1024];

            while (input.Position < input.Length) {
                var readCount = input.Read(buffer, 0, buffer.Length);
                output.Write(buffer, 0, readCount);
            }
            
            input.Close();
            output.Close();
        }
    }
}