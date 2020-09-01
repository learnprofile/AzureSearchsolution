using System;
using System.Collections.Generic;
using System.Text;

namespace AzureSearch
{

    using System;
    using Microsoft.Azure.Search;
    using Microsoft.Azure.Search.Models;
    using Microsoft.Spatial;
    using Newtonsoft.Json;
    public class index2
    {
       
        [System.ComponentModel.DataAnnotations.Key]
        [IsRetrievable(true), IsFilterable, IsSortable]
        public string id { get; set; }

        [Analyzer(AnalyzerName.AsString.StandardLucene)]
        [IsRetrievable(true), IsFilterable, IsSortable, IsSearchable]
        public string code { get; set; }

   
        [Analyzer(AnalyzerName.AsString.StandardLucene)]
        [IsRetrievable(true), IsFilterable, IsSortable, IsSearchable]
        public string name { get; set; }

        [Analyzer(AnalyzerName.AsString.StandardLucene)]
        [IsRetrievable(true), IsFilterable, IsSortable, IsSearchable]
        public string city { get; set; }

                 
        [IsRetrievable(true), IsFilterable, IsSortable]
        public string countryCode { get; set; }
      
    }
}
