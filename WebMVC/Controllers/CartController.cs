using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;
using WebMVC.Models;
using WebMVC.Models.CartModels;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IEventService _eventService;
        private readonly IIdentityService<ApplicationUser> _identityService;

        public CartController(IIdentityService<ApplicationUser> identityService,
                        ICartService cartService, IEventService eventService)
        {
            _identityService = identityService;
            _cartService = cartService;
            _eventService = eventService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(
             Dictionary<string, int> quantities,
             string action)
        {
            if (action == "[ Checkout ]")
            {
                return RedirectToAction("Create", "Order");
            }

            try
            {
                var user = _identityService.Get(HttpContext.User);
                var basket = await _cartService.SetQuantities(user, quantities);
                var vm = await _cartService.UpdateCart(basket);
            }
            catch (BrokenCircuitException)
            {
                // Catch error when CartApi is in open circuit mode
                HandleBrokenCircuitException();
            }

            return View();
        }

        public async Task<IActionResult> AddToCart(EventItem eventDetails)
        {
            try
            {
                if (eventDetails.Id != null)
                {
                    var user = _identityService.Get(HttpContext.User);
                    var product = new CartItem()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Quantity = 1,
                        EventName = eventDetails.Name,
                        PictureUrl = eventDetails.PictureUrl,
                        UnitPrice = eventDetails.EPrice,
                        EventId = eventDetails.Id
                    };
                    await _cartService.AddItemToCart(user, product);
                }
            }
            catch (BrokenCircuitException)
            {
                // Catch error when CartApi is in circuit-open mode
                HandleBrokenCircuitException();
            }

            return RedirectToAction("Index", "Event");
        }

        private void HandleBrokenCircuitException()
        {
            TempData["BasketInoperativeMsg"] = "Cart Service i inoperative, please try later on. (Business Msg Due to Circuit-Breaker)";
        }
    }
}