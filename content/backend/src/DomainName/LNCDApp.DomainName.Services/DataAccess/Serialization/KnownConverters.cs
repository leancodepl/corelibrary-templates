using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LNCDApp.DomainName.Services.DataAccess.Serialization
{
    public static class KnownConverters
    {
        public static IEnumerable<JsonConverter> All { get; } = Array.Empty<JsonConverter>();

        public static JsonSerializerSettings AddAll(JsonSerializerSettings settings)
        {
            // MassTransit uses static configuration (singleton) so we need not to stomp on our feet
            lock (settings.Converters)
            {
                foreach (var c in All)
                {
                    if (!settings.Converters.Contains(c))
                    {
                        settings.Converters.Add(c);
                    }
                }
            }

            return settings;
        }
    }
}
