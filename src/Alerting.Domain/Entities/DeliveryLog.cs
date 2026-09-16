using System;

namespace Alerting.Domain.Entities
{
    public class DeliveryLog
    {
        protected DeliveryLog() { }
        public Guid Id { get; private set; }
        public Guid SubscriptionId { get; private set; }
        public Subscription Subscription { get; private set; } = null!;
        public DateTimeOffset AttemptedAt { get; private set; }
        public bool Success { get; private set; }
        public string Message { get; private set; } = null!;

        public DeliveryLog(Guid id, Subscription subscription, DateTimeOffset attemptedAt, bool success, string message)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id must not be empty", nameof(id));
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));
            if (string.IsNullOrWhiteSpace(message)) message = string.Empty;

            Id = id;
            Subscription = subscription;
            SubscriptionId = subscription.Id;
            AttemptedAt = attemptedAt;
            Success = success;
            Message = message;
        }
    }
}
