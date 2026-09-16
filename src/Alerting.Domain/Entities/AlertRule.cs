using System;
using System.Collections.Generic;

namespace Alerting.Domain.Entities
{
    public class AlertRule
    {
        protected AlertRule() { }
        private readonly List<Subscription> _subscriptions = new();

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public IReadOnlyCollection<Subscription> Subscriptions => _subscriptions.AsReadOnly();

        public AlertRule(Guid id, string name, bool isActive = true)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id must not be empty", nameof(id));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name must not be empty", nameof(name));

            Id = id;
            Name = name;
            IsActive = isActive;
        }

        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName)) throw new ArgumentException("Name must not be empty", nameof(newName));
            Name = newName;
        }

        public void AddSubscription(Subscription subscription)
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));
            if (!_subscriptions.Contains(subscription))
            {
                _subscriptions.Add(subscription);
            }
        }
    }
}
