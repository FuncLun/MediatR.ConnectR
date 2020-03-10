namespace MediatR.ConnectR
{
    public interface IConnectorProvider
    {
        IConnectorEntry TryGetEntry(
            string path
        );

    }
}