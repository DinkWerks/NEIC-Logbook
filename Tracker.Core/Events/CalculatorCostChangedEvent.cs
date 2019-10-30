﻿using Prism.Events;
using Tracker.Core.Events.Payloads;

namespace Tracker.Core.Events
{
    public class CalculatorCostChangedEvent : PubSubEvent<ChargePayload>
    {
    }
}
