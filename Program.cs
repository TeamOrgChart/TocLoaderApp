using CommandLine;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Teamorgchart;

namespace TocLoaderApp
{
    class Program
    {
        public class Options
        {
            [Option('u', "uniqueid", Required = false, HelpText = "Unique ID field for a person.", Default = "UniqueId" )]
            public string UniqueId { get; set; }
            [Option('i', "input", Required = true, HelpText = "Input file name.")]
            public string InputFileName { get; set; }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunProgram);
        }

        static void RunProgram(Options options)
        {
            Console.WriteLine($"Opening file {options.InputFileName}");
            var rows = ReadRowsFromCsv(options.InputFileName);
            
            foreach (IDictionary<String, Object> row in rows)
            {
                var uniqueId = (string)row[options.UniqueId];
                if (string.IsNullOrWhiteSpace(uniqueId))
                {
                    Console.WriteLine($"Item does not have a valid Unique Id field in '{options.UniqueId}' column");
                }
                else
                {
                    Console.WriteLine($"Unique Id is {uniqueId}");

                }
            }
            var client = new TeamOrgChartApi(new ApiKeyHandler("hvfqgL.0pf7TJbKu2ugoZQV"));
            var chart = client.ChartsApi.CreateChart(new Teamorgchart.Models.CreateChartModel { IsPublic = false, Name = $"API Loader ${DateTime.Now.ToShortTimeString()}" }, "1");
            var uploaded = client.ChartDataApi.CreateChartItems(chart.OrgChartDefinitionId, rows.ToList(), "1");
            Console.WriteLine($"Total number of items created: {uploaded.Count}");
        }

        static IEnumerable<dynamic> ReadRowsFromCsv(string filePath)
        {
            List<dynamic> results = new List<dynamic>();
            using (TextReader tr = File.OpenText(filePath))
            {
                var conf = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    //HasHeaderRecord = false
                };
                using (var reader = new CsvReader(tr, conf))
                {
                    results.AddRange(reader.GetRecords<dynamic>().ToList());
                }
            }
            return results;
        }
    }
}
