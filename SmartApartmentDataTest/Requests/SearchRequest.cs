using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartApartmentDataTest.Requests
{
    public class SearchRequest
    {
        public string Phrase { get; set; }
        public string Market { get; set; }
        public int Size { get; set; }
    }
}