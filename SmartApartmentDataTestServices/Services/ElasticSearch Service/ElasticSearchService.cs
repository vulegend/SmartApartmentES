using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json;
using System.Configuration;
using Newtonsoft.Json.Linq;
using SmartApartmentDataTestDataModule.DTOs;
using SmartApartmentDataTestServices.Const;
using SmartApartmentDataTestServices.Services.ElasticSearch_Service.Models;

namespace SmartApartmentDataTestServices.Services.ElasticSearch_Service
{
    public class ElasticSearchService : IElasticSearchService
    {
        #region Private Fields and Properties

        private ElasticClient _elasticClient;
        public string SmartApartmentAutocompleteFilter;

        private ElasticClient ElasticClient
        {
            get
            {
                if (_elasticClient != null) return _elasticClient;

                var node = new Uri("https://search-smartapartment-qmapfidkpgiv3a55l2krmfnuzy.eu-central-1.es.amazonaws.com");
                var settings = new ConnectionSettings(node);
                settings.DisableDirectStreaming(true);
                _elasticClient = new ElasticClient(settings);

                return _elasticClient;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates property index with mapping.
        /// </summary>
        /// <param name="indexName"></param>
        private void CreatePropertyIndex(string indexName)
        {
            

            ElasticClient.CreateIndex(indexName,
                s => s.Settings(st => st.Analysis(an => an.
                        TokenFilters(tf => tf.
                            EdgeNGram(Consts.SMART_APARTMENT_AUTOCOMPLETE_FILTER, eg => eg.
                                MinGram(1).
                                MaxGram(20)
                            )).
                        Analyzers(anz => anz.
                            Custom(Consts.SMART_APARTMENT_AUTOCOMPLETE_ANALYZER, c => c.
                                Tokenizer("standard").
                                Filters(Consts.SMART_APARTMENT_AUTOCOMPLETE_FILTER, "lowercase"))))).
                    Mappings(mp => mp.Map<PropertySearchModel>(map => map.Properties(p => p.
                            Text(tt => tt.
                                Name(n => n.Market).
                                Fields(fs => fs.
                                    Keyword(kw => kw.Name("raw")))).
                            Text(tt => tt.
                                Name(n => n.FullAddress).
                                Analyzer(Consts.SMART_APARTMENT_AUTOCOMPLETE_ANALYZER).
                                Fields(fs => fs.Keyword(kw => kw.
                                    Name("raw")))).
                            Text(tt => tt.
                                Name(n => n.FormerName).
                                Analyzer(Consts.SMART_APARTMENT_AUTOCOMPLETE_ANALYZER).
                                Fields(fs => fs.Keyword(kw => kw.
                                    Name("raw")))).
                            Text(tt => tt.
                                Name(n => n.Name).
                                Analyzer(Consts.SMART_APARTMENT_AUTOCOMPLETE_ANALYZER).
                                Fields(fs => fs.
                                    Keyword(kw => kw.Name("raw"))))).
                        AutoMap<PropertySearchModel>())));
        }

        /// <summary>
        /// Creates management index with mapping.
        /// </summary>
        /// <param name="indexName"></param>
        private void CreateManagementIndex(string indexName)
        {
            ElasticClient.CreateIndex(indexName,
                s => s.Settings(st => st.Analysis(an => an.
                        TokenFilters(tf => tf.
                            EdgeNGram((Consts.SMART_APARTMENT_AUTOCOMPLETE_FILTER), eg => eg.
                                MinGram(1).
                                MaxGram(20)
                            )).
                        Analyzers(anz => anz.
                            Custom(Consts.SMART_APARTMENT_AUTOCOMPLETE_ANALYZER, c => c.
                                Tokenizer("standard").
                                Filters(Consts.SMART_APARTMENT_AUTOCOMPLETE_FILTER, "lowercase"))))).
                    Mappings(mp => mp.Map<ManagementSearchModel>(map => map.Properties(p => p.
                            Text(tt => tt.
                                Name(n => n.Market).
                                Fields(fs => fs.
                                    Keyword(kw => kw.Name("raw")))).
                            Text(tt => tt.
                                Name(n => n.Name).
                                Analyzer(Consts.SMART_APARTMENT_AUTOCOMPLETE_ANALYZER).
                                Fields(fs => fs.Keyword(kw => kw.
                                    Name("raw")))).
                            Text(tt => tt.
                                Name(n => n.State).
                                Analyzer(Consts.SMART_APARTMENT_AUTOCOMPLETE_ANALYZER).
                                Fields(fs => fs.Keyword(kw => kw.
                                    Name("raw"))))).
                        AutoMap<ManagementSearchModel>())));
        }

        /// <summary>
        /// Creates the list of PropertySearchModel from the given json file
        /// </summary>
        /// <param name="jsonFilePath"></param>
        /// <returns></returns>
        private List<PropertySearchModel> GeneratePropertySearchModels(string jsonFilePath)
        {
            var propertiesJArray = JArray.Parse(File.ReadAllText("properties.json"));
            var propertySearchModels = new List<PropertySearchModel>();

            foreach (var jToken in propertiesJArray)
            {
                var getRoot = jToken.First;
                var rootProperty = (JProperty)getRoot;
                var test = jToken[rootProperty.Name];

                var toAdd = new JObject();
                foreach (var childToken in test)
                {
                    var childProperty = (JProperty)childToken;
                    toAdd.Add(childProperty.Name.Replace(" ", ""), childProperty.Value);
                }

                var propertySearchModel = toAdd.ToObject<PropertySearchModel>();
                propertySearchModel.FullAddress =
                    $"{propertySearchModel.StreetAddress} {propertySearchModel.City} {propertySearchModel.State}";
                propertySearchModel.SetupAddress();
                propertySearchModels.Add(propertySearchModel);
            }

            return propertySearchModels;
        }

        /// <summary>
        /// Indexes the property objects from the json file in the ElasticSearch
        /// </summary>
        /// <param name="propertyJsonPath"></param>
        /// <param name="propertyIndexName"></param>
        private void IndexPropertyObjects(string propertyJsonPath, string propertyIndexName)
        {
            ElasticClient.BulkAll(GeneratePropertySearchModels(propertyJsonPath), b => b
                    .Index(propertyIndexName)
                    .BackOffTime("30s")
                    .BackOffRetries(2)
                    .RefreshOnCompleted()
                    .MaxDegreeOfParallelism(Environment.ProcessorCount)
                    .Size(1000)
                )
                .Wait(TimeSpan.FromMinutes(15), next =>
                {
                    //Blocking call
                });
        }

        /// <summary>
        /// Creates the list of ManagementSearchModel from the given json file
        /// </summary>
        /// <param name="jsonFilePath"></param>
        /// <returns></returns>
        private List<ManagementSearchModel> GenerateManagementSearchModels(string jsonFilePath)
        {
            var managementsJArray = JArray.Parse(File.ReadAllText(jsonFilePath));
            var managementSearchModels = new List<ManagementSearchModel>();

            foreach (var jToken in managementsJArray)
            {
                var getRoot = jToken.First;
                var rootProperty = (JProperty)getRoot;
                var test = jToken[rootProperty.Name];

                JObject toAdd = new JObject();
                foreach (var childToken in test)
                {
                    var childProperty = (JProperty)childToken;
                    toAdd.Add(childProperty.Name.Replace(" ", ""), childProperty.Value);
                }

                if (managementSearchModels.Find(x => x.Id == long.Parse(toAdd["mgmtID"].ToString())) == null)
                    managementSearchModels.Add(toAdd.ToObject<ManagementSearchModel>());
            }

            return managementSearchModels;
        }

        /// <summary>
        /// Indexes the management objects from the json file in the ElasticSearch
        /// </summary>
        /// <param name="managementJsonPath"></param>
        /// <param name="managementIndexName"></param>
        private void IndexManagementObjects(string managementJsonPath, string managementIndexName)
        {
            ElasticClient.BulkAll(GenerateManagementSearchModels(managementJsonPath), b => b
                    .Index(managementIndexName)
                    .BackOffTime("30s")
                    .BackOffRetries(2)
                    .RefreshOnCompleted()
                    .MaxDegreeOfParallelism(Environment.ProcessorCount)
                    .Size(1000)
                )
                .Wait(TimeSpan.FromMinutes(15), next =>
                {
                    //Blocking call
                });
        }

        /// <summary>
        /// Generates our search response from the ElasticSearch raw multi search response
        /// </summary>
        /// <param name="rawResponse"></param>
        /// <param name="returnSize"></param>
        /// <returns></returns>
        private SearchResponse GenerateSearchResponse(IMultiSearchResponse rawResponse, int returnSize)
        {
            var baseResponsesList = new List<BaseSearchResponse>();
            var propertySearchResponse = rawResponse.GetResponse<PropertySearchModel>("properties");
            var propertySearchModels = propertySearchResponse.Documents.ToList();

            propertySearchModels.ForEach(x =>
            {
                baseResponsesList.Add(new PropertySearchResponse
                {
                    PropertyModel = x,
                    Score = propertySearchResponse.Hits.FirstOrDefault(y => y.Id.Equals(x.Id.ToString()))?.Score ?? 0
                });
            });

            var managementSearchResponse = rawResponse.GetResponse<ManagementSearchModel>("managements");
            var managementSearchModels = managementSearchResponse.Documents.ToList();

            managementSearchModels.ForEach(x =>
            {
                baseResponsesList.Add(new ManagementSearchResponse()
                {
                    ManagementModel = x,
                    Score = managementSearchResponse.Hits.FirstOrDefault(y => y.Id.Equals(x.Id.ToString()))?.Score ?? 0
                });
            });

            var orderedList = baseResponsesList.OrderByDescending(s => s.Score).ToList();

            return new SearchResponse
            {
                SearchResponses = orderedList.Count > returnSize ? orderedList.GetRange(0, returnSize) : orderedList
            };
        }

        #endregion

        #region Search

        /// <summary>
        /// Executes global search in the ElasticSearch.
        /// </summary>
        /// <param name="phrase"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public SearchResponse GlobalSearch(string phrase, int size = 25)
        {
            #region Global Search Query

            //Executes multi search against property and management index. Setting the fetch size to double the response size to enable response variety
            var searchResponse = ElasticClient.MultiSearch(ms => ms.Search<PropertySearchModel>("properties", s => s
                    .IndicesBoost(ib => ib.Add(ConfigurationManager.AppSettings["ESPropertyIndex"], 5)).Size(size * 2).Index(ConfigurationManager.AppSettings["ESPropertyIndex"])
                    .Type(typeof(PropertySearchModel)).Source(sf => sf.Includes(i => i.Fields(f => f.Name,
                        f => f.StreetAddress,
                        f => f.State,
                        f => f.City,
                        f => f.Id,
                        f => f.Lat,
                        f => f.Lng,
                        f => f.FormerName,
                        f => f.Market))).Query(q =>
                        q.Match(m => m.Field(p => p.Name.Suffix("raw")).Boost(1500).Query(phrase)) ||
                        q.Match(m => m.Field(p => p.FullAddress.Suffix("raw")).Boost(1500).Query(phrase)) ||
                        q.Match(m => m.Field(p => p.FormerName.Suffix("raw")).Boost(1500).Query(phrase)))
                        .Query(q => q.Match(m => m.Field(p => p.Id.Suffix("keyword")).Boost(2000).Query(phrase))).Query(q =>
                            q.Match(m => m.Field(f => f.Name).Analyzer("standard").Query(phrase).Boost(5.2)) ||
                            q.Match(m => m.Field(f => f.FormerName).Analyzer("standard").Boost(4.9)) ||
                            q.Match(m => m.Field(f => f.FullAddress).Analyzer("standard").Boost(4))))
                    .Search<ManagementSearchModel>("managements", s => s.IndicesBoost(ib => ib.Add(ConfigurationManager.AppSettings["ESManagementIndex"], 4.9))
                        .Index(ConfigurationManager.AppSettings["ESManagementIndex"]).Type(typeof(ManagementSearchModel)).Size(size * 2).Source(sf => sf.Includes(i =>
                            i.Fields(f => f.Name,
                                f => f.State,
                                f => f.Id,
                                f => f.Market))).Query(q =>
                            q.Match(m => m.Field(f => f.Name.Suffix("raw")).Boost(1500).Query(phrase))).Query(q =>
                            q.Match(m => m.Field(f => f.Name).Analyzer("standard").Boost(5).Query(phrase)) ||
                            q.Match(m => m.Field(f => f.State).Analyzer("standard").Query(phrase)))));
            #endregion

            return GenerateSearchResponse(searchResponse, size);
        }

        /// <summary>
        /// Executes the markets specific search in the ElasticSearch
        /// </summary>
        /// <param name="phrase"></param>
        /// <param name="market"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public SearchResponse MarketSearch(string phrase, string market, int size = 25)
        {
            #region Market Search Query

            var searchResponse = ElasticClient.MultiSearch(ms => ms.Search<PropertySearchModel>("properties", s =>
                    s.Size(size * 2).Index(ConfigurationManager.AppSettings["ESPropertyIndex"])
                        .IndicesBoost(ib => ib.Add(ConfigurationManager.AppSettings["ESPropertyIndex"], 5)).Source(sf =>
                            sf.Includes(i =>
                                i.Fields(f => f.Name,
                                    f => f.StreetAddress,
                                    f => f.State,
                                    f => f.City,
                                    f => f.Id,
                                    f => f.Lat,
                                    f => f.Lng,
                                    f => f.FormerName,
                                    f => f.Market))).Query(q =>
                            q.Bool(b => b
                                .Must(must => must.QueryString(m =>
                                    m.Fields(fs => fs.Field(f => f.Market.Suffix("raw"))).Query(market.GenerateMultiQueryString())))
                                .Should(sh =>
                                    sh.Match(m => m.Field(f => f.Name).Analyzer("standard").Boost(5.2).Query(phrase)) ||
                                    sh.Match(m =>
                                        m.Field(f => f.FormerName).Analyzer("standard").Boost(4.9).Query(phrase)) ||
                                    sh.Match(m =>
                                        m.Field(f => f.FullAddress).Analyzer("standard").Boost(4).Query(phrase))))))
                .Search<ManagementSearchModel>("managements", s => s.Size(size * 2)
                    .Index(ConfigurationManager.AppSettings["ESManagementIndex"]).Type(typeof(ManagementSearchModel))
                    .IndicesBoost(ib => ib.Add(ConfigurationManager.AppSettings["ESManagementIndex"], 4.9)).Source(sr =>
                        sr.Includes(i =>
                            i.Fields(f => f.Name,
                                f => f.Id,
                                f => f.Market,
                                f => f.State))).Query(q =>
                        q.Bool(b => b.Must(must => must.QueryString(m => m.Fields(fs => fs.Field(f => f.Market.Suffix("raw"))).Query(market.GenerateMultiQueryString())))
                            .Should(sh =>
                                sh.Match(m => m.Field(f => f.Name).Analyzer("standard").Boost(6).Query(phrase)))))));

            #endregion

            return GenerateSearchResponse(searchResponse, size);
        }


        #endregion

        #region Index Management

        /// <summary>
        /// Sets up the indexes and indexes the data
        /// </summary>
        public void Setup()
        {
            DeleteIndex(ConfigurationManager.AppSettings["ESPropertyIndex"]);
            DeleteIndex(ConfigurationManager.AppSettings["ESManagementIndex"]);
            CreatePropertyIndex(ConfigurationManager.AppSettings["ESPropertyIndex"]);
            CreateManagementIndex(ConfigurationManager.AppSettings["ESManagementIndex"]);
            IndexPropertyObjects(ConfigurationManager.AppSettings["ESPropertyJsonPath"], ConfigurationManager.AppSettings["ESPropertyIndex"]);
            IndexManagementObjects(ConfigurationManager.AppSettings["ESManagementJsonPath"], ConfigurationManager.AppSettings["ESManagementIndex"]);
        }

        /// <summary>
        /// Deletes an index in ElasticSearch
        /// </summary>
        /// <param name="indexName"></param>
        public void DeleteIndex(string indexName)
        {
            ElasticClient.DeleteIndex(indexName);
        }

        /// <summary>
        /// Indexes property model
        /// </summary>
        /// <param name="propertyDto"></param>
        /// <returns></returns>
        public string IndexProperty(PropertyDto propertyDto)
        {
            var propertyElasticModel = PropertySearchModel.FromDto(propertyDto);
            var response = ElasticClient.Index(propertyElasticModel, i => i.Index(ConfigurationManager.AppSettings["ESPropertyIndex"]));
            return response.ToString();
        }

        /// <summary>
        /// Indexes management model
        /// </summary>
        /// <param name="managementDto"></param>
        /// <returns></returns>
        public string IndexManagement(ManagementDto managementDto)
        {
            var managementElasticModel = ManagementSearchModel.FromDto(managementDto);
            var response = ElasticClient.Index(managementElasticModel, i => i.Index(ConfigurationManager.AppSettings["ESManagementIndex"]));
            return response.ToString();
        }

        #endregion
    }
}
