using Cart.API.Entities;
using Common.EventBus.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Cart.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IDistributedCache _redisCache;
        private readonly IPublishEndpoint _publishEndpoint;
        public BasketController(ILogger<WeatherForecastController> logger, IDistributedCache cache, IPublishEndpoint publishEndpoint)
        {
            _redisCache = cache;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            return Ok(JsonConvert.DeserializeObject<ShoppingCart>(basket));
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
            return Ok();
        }

        [HttpPost]
        [Route("checkout")]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout checkout)
        {
            var basket = JsonConvert.DeserializeObject<ShoppingCart>(await _redisCache.GetStringAsync(checkout.UserName));
            BasketCheckoutEvent checkoutEvent = new BasketCheckoutEvent
            {
                Address = checkout.Address,
                UserName = checkout.UserName,
                Item = basket.Items.Select(s => s.PackageName).ToList(),
                TotalPrice = basket.TotalPrice.ToString(),
            };
            await _publishEndpoint.Publish(checkoutEvent);

            return Ok("Checkout Successful.");
        }
    }
}