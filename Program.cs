using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest.Azure;

namespace AzureSearch
{
    public sealed class Program
    {


        public static void Main(string[] args)
        {

            IConfiguration configuration = null;

            var builder = new ConfigurationBuilder().AddJsonFile("appSettings.json");
            configuration = builder.Build();


            if (configuration["SearchServiceName"] == "Put your search service name here")
            {
                Console.Error.WriteLine("Specify SearchServiceName in appsettings.json");
                Environment.Exit(-1);
            }

            if (configuration["SearchServiceAdminApiKey"] == "Put your primary or secondary API key here")
            {
                Console.Error.WriteLine("Specify SearchServiceAdminApiKey in appsettings.json");
                Environment.Exit(-1);
            }

            if (configuration["AzureSQLConnectionString"] == "Put your Azure SQL database connection string here")
            {
                Console.Error.WriteLine("Specify AzureSQLConnectionString in appsettings.json");
                Environment.Exit(-1);
            }

            SearchServiceClient searchService = new SearchServiceClient(searchServiceName: configuration["SearchServiceName"],  credentials: new SearchCredentials(configuration["SearchServiceAdminApiKey"]));


            Console.WriteLine("Creating Index Begin ... 'index1'");
            var index = new Index
                (
                name: "index1",
                fields: FieldBuilder.BuildForType<index1>(),
                suggesters: new List<Suggester> { new Suggester { Name = "sg", SourceFields = new List<string> { "code", "site2ndCode", "siteTypeName", "name", "city", "street" } } },
                corsOptions: new CorsOptions(new List<string> { "*" }, 300)
                );

            bool exists = searchService.Indexes.Exists(index.Name);

            if (exists)
            {
                searchService.Indexes.Delete(index.Name);
            }

            searchService.Indexes.Create(index);
            Console.WriteLine("Creating Index End ... 'index1'");



            Console.WriteLine("Creating Datasource Begin ...'dsource1'");
            DataSource dataSource = DataSource.AzureSql(
                name: "firstdatasource",
                sqlConnectionString: configuration["AzureSQLConnectionString"],
                tableOrViewName: "INDEX_FIRST_VIEW", description: null);

            searchService.DataSources.CreateOrUpdate(dataSource);
            Console.WriteLine("Creating Datasource End ...'dsource1'");


            Console.WriteLine("Creating Azure SQL Indexer Begin ...'indexer1'");
            Indexer indexer = new Indexer(
                name: "indexer1",
                dataSourceName: dataSource.Name,
                targetIndexName: index.Name);

            exists = searchService.Indexers.Exists(indexer.Name);
            if (exists)
            {
                searchService.Indexers.Reset(indexer.Name);
            }

            searchService.Indexers.CreateOrUpdate(indexer);
            Console.WriteLine("Creating Azure SQL Indexer End ...'indexer1'");


            Console.WriteLine("Running Azure SQL Indexer Begin ... 'indexer1'");
            try
            {
                searchService.Indexers.Run(indexer.Name);
            }
            catch (CloudException e) when (e.Response.StatusCode == (HttpStatusCode)429)
            {
                Console.WriteLine("Failed to run indexer: {0}", e.Response.Content);
            }
            Console.WriteLine("Running Azure SQL Indexer End ... 'indexer1'");

            Console.WriteLine("");
            Console.WriteLine("");

            /////////////////////////////////////////////////////////////////////

            Console.WriteLine("Creating Index Begin ... 'index2'");
            var index1 = new Index
                (
                name: "index2",
                fields: FieldBuilder.BuildForType<index2>(),
                suggesters: new List<Suggester> { new Suggester { Name = "sg", SourceFields = new List<string> { "code", "gsapCode", "brandCode", "name", "city", "street", "currencyCode"} } },
                corsOptions: new CorsOptions(new List<string> { "*" }, 300)
                );

            bool exists1 = searchService.Indexes.Exists(index1.Name);

            if (exists1)
            {
                searchService.Indexes.Delete(index1.Name);
            }

            searchService.Indexes.Create(index1);

            Console.WriteLine("Creating Index End ... 'index2'");

            Console.WriteLine("Creating Datasource Begin ...'dsource2'");


            DataSource dataSource1 = DataSource.AzureSql(
                name: "seconddsource",
                sqlConnectionString: configuration["AzureSQLConnectionString"],
                tableOrViewName: "INDEX_SECOND_VIEW", description: null);

            searchService.DataSources.CreateOrUpdate(dataSource1);

            Console.WriteLine("Creating Datasource End ...'dsource2'");


            Console.WriteLine("Creating Azure SQL Indexer Begin ...'indexer2'");
            Indexer indexer1 = new Indexer(
                name: "indexer2",
                dataSourceName: dataSource1.Name,
                targetIndexName: index1.Name);

            exists1 = searchService.Indexers.Exists(indexer1.Name);
            if (exists1)
            {
                searchService.Indexers.Reset(indexer1.Name);
            }

            searchService.Indexers.CreateOrUpdate(indexer1);

            Console.WriteLine("Creating Azure SQL Indexer End ... 'indexer2'");

            Console.WriteLine("Running Azure SQL Indexer Begin ... 'indexer2'");

            try
            {
                searchService.Indexers.Run(indexer1.Name);
            }
            catch (CloudException e) when (e.Response.StatusCode == (HttpStatusCode)429)
            {
                Console.WriteLine("Failed to run indexer: {0}", e.Response.Content);
            }

            Console.WriteLine("Running Azure SQL Indexer End ... 'indexer2'");


            //    Console.WriteLine("Press any key to continue...");

            Environment.Exit(0);
        }
    }
}
