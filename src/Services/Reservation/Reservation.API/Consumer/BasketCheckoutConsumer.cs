using Common.EventBus.Events;
using MassTransit;

namespace Reservation.API.Consumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly ILogger<BasketCheckoutConsumer> _logger;

        public BasketCheckoutConsumer(ILogger<BasketCheckoutConsumer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("BasketCheckout event consumed and reserved with " + message.ToString());
        }
    }
}
