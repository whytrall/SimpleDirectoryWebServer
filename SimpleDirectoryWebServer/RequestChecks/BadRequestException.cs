using System;

namespace SimpleDirectoryWebServer.RequestChecks {
    public class BadRequestException: Exception {
        public BadRequestException(string str): base(str) {}
    }
}