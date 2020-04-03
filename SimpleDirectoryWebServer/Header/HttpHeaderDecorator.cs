using System;
using System.Text.RegularExpressions;

namespace SimpleDirectoryWebServer.Header {
    public class HttpHeaderDecorator {
        private HttpHeader _header;

        public HttpHeaderDecorator(HttpHeader header) {
            _header = header;
        }

        public string GetUrl() {
            return Uri.UnescapeDataString(
                Regex.Match(_header.TopStr, @"^\w+\s+([^\s\?]+)[^\s]*\s+HTTP/.*|").Groups[1].ToString()
                );
        }
    }
}