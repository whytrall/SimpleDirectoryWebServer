using System;

namespace SimpleDirectoryWebServer.Header {
    public class InvalidHttpHeaderException: Exception {
        public InvalidHttpHeaderException(string msg): base(msg) {}
    }
}