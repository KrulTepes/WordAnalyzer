using System;
using System.Collections.Generic;

namespace WordAnalyzer.IO
{
    internal class MenuResponse
    {
        public Func<Dictionary<string, object>?, MenuResponse>? Next { get; set; }
        public Dictionary<string, object>? Parameters { get; set; }
    }
}
