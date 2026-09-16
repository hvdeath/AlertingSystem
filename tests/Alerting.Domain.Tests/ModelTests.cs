using System;
using Alerting.Domain.Entities;
using Xunit;

namespace Alerting.Domain.Tests
{
    public class ModelTests
    {
        [Fact]
        public void AlertRule_Ctor_ValidSetsProperties()
        {
            var id = Guid.NewGuid();
            var rule = new AlertRule(id, "High Cpu");

            Assert.Equal(id, rule.Id);
            Assert.Equal("High Cpu", rule.Name);
            Assert.True(rule.IsActive);
        }

        [Fact]
        public void AlertRule_Ctor_InvalidName_Throws()
        {
            Assert.Throws<ArgumentException>(() => new AlertRule(Guid.NewGuid(), " "));
        }

        [Fact]
        public void Subscription_Ctor_ValidLinksEntities()
        {
            var rule = new AlertRule(Guid.NewGuid(), "Rule1");
            var channel = new Channel(Guid.NewGuid(), "Email", "email", "test@example.com");
            var sub = new Subscription(Guid.NewGuid(), rule, channel, "test@example.com");

            Assert.Equal(rule.Id, sub.AlertRuleId);
            Assert.Equal(channel.Id, sub.ChannelId);
            Assert.Equal("test@example.com", sub.Recipient);
            Assert.True(sub.IsEnabled);
        }

        [Fact]
        public void DeliveryLog_Ctor_SetsProperties()
        {
            var rule = new AlertRule(Guid.NewGuid(), "Rule1");
            var channel = new Channel(Guid.NewGuid(), "Email", "email", "test@example.com");
            var sub = new Subscription(Guid.NewGuid(), rule, channel, "test@example.com");

            var log = new DeliveryLog(Guid.NewGuid(), sub, DateTimeOffset.UtcNow, false, "Failed to deliver");

            Assert.Equal(sub.Id, log.SubscriptionId);
            Assert.False(log.Success);
            Assert.Equal("Failed to deliver", log.Message);
        }
    }
}
