using System.Collections.Generic;

namespace MediatR.ConnectR
{
    public interface IConnectorCollection : IReadOnlyList<IConnectorEntry>
    {
    }

    public class ConnectorCollection : List<IConnectorEntry>, IConnectorCollection
    {
        public ConnectorCollection()
        { }

        public ConnectorCollection(
            IEnumerable<IConnectorEntry> entries
        ) : base(entries)
        { }
    }
}