using System.IO;
using SimpleDirectoryWebServer.BasicClasses;

namespace SimpleDirectoryWebServer.RequestChecks {
    public class ExistenceCheck: ChainOfResponsibility<string, bool> {
        public override object Handle(object request) {
            if (!File.Exists((string)request) && !Directory.Exists((string)request)) {
                throw new FileNotFoundException();
            }
            
            return base.Handle(request);
        }
    }
}