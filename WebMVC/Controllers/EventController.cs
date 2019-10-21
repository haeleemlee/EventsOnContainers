using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Services;
using WebMVC.ViewModels;

namespace WebMVC.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _service;
        public EventController(IEventService service) =>
            _service = service;


        public async Task<IActionResult> About()
        {
            return View();
        }

        public async Task<IActionResult> Index(
            int? locationFilterApplied,
            int? priceFilterApplied,
            int? typesFilterApplied,
            int? page)
        {
            var itemsOnPage = 10;
            var catalog = await _service.GetEventItemsAsync(page ?? 0,
                itemsOnPage, locationFilterApplied, priceFilterApplied, typesFilterApplied);

            var vm = new EventIndexViewModel
            {
                PaginationInfo = new PaginationInfo
                {
                    ActualPage = page ?? 0,
                    ItemsPerPage = itemsOnPage,
                    TotalItems = catalog.Count,
                    TotalPages = (int)Math.Ceiling((decimal)catalog.Count / itemsOnPage)
                },
                EventItems = catalog.Data,
                Locations = await _service.GetLocationAsync(),
                Prices = await _service.GetPriceAsync(),
                Types = await _service.GetTypesAsync(),
                LocationFilterApplied = locationFilterApplied ?? 0,
                PriceFilterApplied = priceFilterApplied ?? 0,
                TypesFilterApplied = typesFilterApplied ?? 0
            };

            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";
            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";

            return View(vm);
        }
    }
}