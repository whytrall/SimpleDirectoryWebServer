using System;
using System.IO;
using SimpleDirectoryWebServer.BasicClasses;

namespace SimpleDirectoryWebServer.RequestChecks {
    public class PermissionsCheck: ChainOfResponsibility<string, bool> {
        public override object Handle(object request) {
            var req = (string) request;
            try { 
                var attr = File.GetAttributes(req);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory) {
                    Directory.GetDirectories(req);
                }
                else {
                    new FileStream(req, FileMode.Open, FileAccess.Read, FileShare.Read);
                }
            }
            catch (UnauthorizedAccessException) {
                // TODO: Handle somehow? Or not?
                throw;
            }
            return base.Handle(request);
        }
    }
}