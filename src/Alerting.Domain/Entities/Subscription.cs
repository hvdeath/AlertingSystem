using System;

namespace Alerting.Domain.Entities
{
    public class Subscription
    {
        protected Subscription() { }
        public Guid Id { get; private set; }

        public Guid AlertRuleId { get; private set; }
        public AlertRule AlertRule { get; private set; } = null!;

        public Guid ChannelId { get; private set; }
        public Channel Channel { get; private set; } = null!;

        public string Recipient { get; private set; } = null!;
        public bool IsEnabled { get; private set; }

        public Subscription(Guid id, AlertRule alertRule, Channel channel, string recipient, bool isEnabled = true)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id must not be empty", nameof(id));
            if (alertRule == null) throw new ArgumentNullException(nameof(alertRule));
            if (channel == null) throw new ArgumentNullException(nameof(channel));
            if (string.IsNullOrWhiteSpace(recipient)) throw new ArgumentException("Recipient must not be empty", nameof(recipient));

            Id = id;
            AlertRule = alertRule;
            AlertRuleId = alertRule.Id;
            Channel = channel;
            ChannelId = channel.Id;
            Recipient = recipient;
            IsEnabled = isEnabled;
        }

        public void Disable() => IsEnabled = false;
        public void Enable() => IsEnabled = true;
    }
}
