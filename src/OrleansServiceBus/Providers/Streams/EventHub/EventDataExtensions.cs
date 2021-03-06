﻿
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ServiceBus.Messaging;
using Orleans.Serialization;

namespace Orleans.ServiceBus.Providers
{
    /// <summary>
    /// Extends EventData to support streaming
    /// </summary>
    public static class EventDataExtensions
    {
        public const string EventDataPropertyStreamNamespaceKey = "StreamNamespace";
        private static readonly string[] SkipProperties = { nameof(EventData.Offset), nameof(EventData.SequenceNumber), nameof(EventData.EnqueuedTimeUtc), EventDataPropertyStreamNamespaceKey };

        public static void SetStreamNamespaceProperty(this EventData eventData, string streamNamespace)
        {
            eventData.Properties[EventDataPropertyStreamNamespaceKey] = streamNamespace;
        }

        public static string GetStreamNamespaceProperty(this EventData eventData)
        {
            object namespaceObj;
            if (eventData.Properties.TryGetValue(EventDataPropertyStreamNamespaceKey, out namespaceObj))
            {
                return (string)namespaceObj;
            }
            return null;
        }

        public static byte[] SerializeProperties(this IDictionary<string, object> properties)
        {
            var writeStream = new BinaryTokenStreamWriter();
            SerializationManager.Serialize(properties.Where(kvp => !SkipProperties.Contains(kvp.Key)).ToList(), writeStream);
            return writeStream.ToByteArray();
        }

        public static IDictionary<string, object> DeserializeProperties(this ArraySegment<byte> bytes)
        {
            var stream = new BinaryTokenStreamReader(bytes);
            return SerializationManager.Deserialize<List<KeyValuePair<string, object>>>(stream).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
