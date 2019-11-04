using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WebMVC.Infrastructure;
using WebMVC.Models;

namespace WebMVC.Services
{
    public class EventService : IEventService
    {
        private readonly IHttpClient _client;
        private readonly string _baseUri;
        public EventService(IConfiguration config,
            IHttpClient client)
        {
            _baseUri = $"{config["EventUrl"]}/api/event/";
            _client = client;
        }

        public async Task<IEnumerable<SelectListItem>> GetLocationAsync()
        {
            var locationUri = ApiPaths.Catalog.GetAllLocations(_baseUri);
            var dataString = await _client.GetStringAsync(locationUri);
            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = null,
                    Text = "All",
                    Selected = true
                }
            };

            var locations = JArray.Parse(dataString);
            foreach (var location in locations)
            {
                items.Add(
                    new SelectListItem
                    {
                        Value = location.Value<string>("id"),
                        Text = location.Value<string>("location")
                    }
                );
            }

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetPriceAsync()
        {
            var priceUri = ApiPaths.Catalog.GetAllPrices(_baseUri);
            var dataString = await _client.GetStringAsync(priceUri);
            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = null,
                    Text = "All",
                    Selected = true
                }
            };

            var prices = JArray.Parse(dataString);
            foreach (var price in prices)
            {
                items.Add(
                    new SelectListItem
                    {
                        Value = price.Value<string>("id"),
                        Text = price.Value<string>("price")
                    }
                );
            }

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetTypesAsync()
        {
            var typeUri = ApiPaths.Catalog.GetAllTypes(_baseUri);
            var dataString = await _client.GetStringAsync(typeUri);
            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = null,
                    Text = "All",
                    Selected = true
                }
            };

            var types = JArray.Parse(dataString);
            foreach (var type in types)
            {
                items.Add(
                    new SelectListItem
                    {
                        Value = type.Value<string>("id"),
                        Text = type.Value<string>("type")
                    }
                );
            }

            return items;
        }

        public async Task<Catalog> GetEventItemsAsync(int page, int size, int? location, int? price, int? type)
        {
            var eventItemsUri = ApiPaths.Catalog.GetAllEventItems(_baseUri, page, size, location, price, type);
            var dataString = await _client.GetStringAsync(eventItemsUri);
            var response = JsonConvert.DeserializeObject<Catalog>(dataString);
            return response;
        }


    }
}
