using System;
using System.Net;
using System.Threading;
 
namespace SimpleDirectoryWebServer {
    internal static class Program {
        static void Main(string[] args) {
            var threads = 4*Environment.ProcessorCount;
            ThreadPool.SetMaxThreads(threads, threads);
            ThreadPool.SetMinThreads((threads/3)+1, (threads/3)+1);
            
            var srv = new Server(IPAddress.Any, 8080);
            srv.Listen();
        }
    }
}