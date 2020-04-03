using System;
using SimpleDirectoryWebServer.BasicClasses;

namespace SimpleDirectoryWebServer.RequestChecks {
    public class FolderSecurityCheck: ChainOfResponsibility<string, bool> {
        public override object Handle(object request) {
            if (((string)request).IndexOf("..", StringComparison.Ordinal) >= 0) {
                throw new BadRequestException("security");
            }

            return base.Handle(request);
        }
    }
}