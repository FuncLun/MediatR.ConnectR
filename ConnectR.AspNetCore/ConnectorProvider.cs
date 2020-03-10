using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.ConnectR
{
    public class ConnectorProvider : IConnectorProvider
    {
        public ConnectorProvider(
            IEnumerable<IConnectorCollection> collections
        )
        {
            Entries = collections
                .SelectMany(c => c)
                .SelectMany(c => c.Keys.Select(k => (Key: k, Entry: c)))
                .GroupBy(e => e.Key, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(vt => vt.Entry)
                        .ToList()
                );
        }

        private IDictionary<string, List<IConnectorEntry>> Entries { get; }

        public IConnectorEntry TryGetEntry(
            string path
        )
        {
            if (!Entries.TryGetValue(path, out var entries))
                return default;

            //TODO: Add case sensitive check here
            var entry = entries.FirstOrDefault();
            return entry;
        }
    }
}