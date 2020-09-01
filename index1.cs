using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
namespace AzureSearch
{
    public class index1
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

        [Analyzer(AnalyzerName.AsString.StandardLucene)]
        [IsRetrievable(true), IsFilterable, IsSortable, IsSearchable]
        public string street { get; set; }

        [IsRetrievable(true), IsFilterable, IsSortable]
        public string statusCode { get; set; }

        [IsRetrievable(true), IsFilterable, IsSortable]
        public string currencyCode { get; set; }
        
        [IsRetrievable(true), IsFilterable, IsSortable]
        public string countryCode { get; set; }


    }
}
