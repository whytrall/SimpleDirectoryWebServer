using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace SimpleDirectoryWebServer.Header {
    public class HttpHeader {
        public string TopStr { get; private set; } = null;
        //private bool _isIncoming = false;
        private Dictionary<string, string> _headers = new Dictionary<string, string>();
        private readonly HashSet<string> _possibleFields = new HashSet<string>(new []{
            "A-IM", "Accept", "Accept-Charset", "Accept-Encoding", "Accept-Language", "Accept-Datetime",
            "Access-Control-Request-Method", "Access-Control-Request-Headers", "Authorization", "Cache-Control", 
            "Connection", "Content-Length", "Content-Type", "Cookie", "Date", "Expect", "Forwarded", "From",
            "Host", "If-Match", "If-Modified-Since", "If-None-Match", "If-Range", "If-Unmodified-Since", 
            "Max-Forwards", "Origin", "Pragma", "Proxy-Authorization", "Range", "Referer", "TE", 
            "User-Agent", "Upgrade", "Via", "Warning"});

        public HttpHeader SetHttpStatus(HttpStatusCode statusCode) {
            TopStr = $"HTTP/1.1 {(int)statusCode} {statusCode}";
            return this;
        }

        public HttpHeader SetTopStr(string topStr) {
            TopStr = topStr;
            return this;
        }

        public string this[string key] {
            get => _headers[key];
            set {
                if (!_possibleFields.Contains(key)) {
                    throw new ArgumentException($"Bad key name: {key}");
                }

                _headers[key] = value;
            }
        }

        public HttpHeader SetKey(string key, object value) {
            this[key] = value.ToString();
            return this;
        }

        public static HttpHeader Parse(string stuff) {
            var h = new HttpHeader();
            stuff = Regex.Replace(stuff, @"/[\n]+/", "\n");
            var request = Regex.Match(stuff, @"^\w+\s+([^\s\?]+)[^\s]*\s+HTTP/.*|");

            if (request == Match.Empty) {
                throw new InvalidHttpHeaderException("Bad headers!");
            }
            
            var header = new HttpHeader()
                .SetTopStr(request.Groups[0].ToString());
            
            var splitStuff = stuff.Split("\n");
            for (var i = 1; i < splitStuff.Length; i++) {
                if (splitStuff[i].Trim() == string.Empty) continue;
                
                var a = splitStuff[i].Split(new[] { ':' }, 2);
                try {
                    header[a[0].Trim()] = a[1].Trim();
                }
                catch (Exception) {
                    //Console.WriteLine($"httpheader error: bad key {a[0].Trim()}");
                }
            }

            return header;
        }

        public override string ToString() {
            if (TopStr == null) {
                throw new InvalidHttpHeaderException("Bad header");
            }
            var str = $"{TopStr}\n";
            foreach (var (key, value) in _headers) {
                str += $"{key}: {value}\n";
            }

            return str;
        }
    }
}