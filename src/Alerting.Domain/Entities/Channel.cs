using System;
using System.Collections.Generic;

namespace Alerting.Domain.Entities
{
    public class Channel
    {
        private readonly List<Subscription> _subscriptions = new();

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string Address { get; private set; }
        public IReadOnlyCollection<Subscription> Subscriptions => _subscriptions.AsReadOnly();

        public Channel(Guid id, string name, string type, string address)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id must not be empty", nameof(id));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name must not be empty", nameof(name));
            if (string.IsNullOrWhiteSpace(type)) throw new ArgumentException("Type must not be empty", nameof(type));
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Address must not be empty", nameof(address));

            Id = id;
            Name = name;
            Type = type;
            Address = address;
        }

        public void AddSubscription(Subscription subscription)
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));
            if (!_subscriptions.Contains(subscription)) _subscriptions.Add(subscription);
        }
    }
}
