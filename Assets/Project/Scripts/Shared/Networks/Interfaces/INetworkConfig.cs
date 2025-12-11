namespace BC.Shared.Networks;

public interface INetworkConfig
{
    string Address { get; }
    int Port { get; }
    int MaxConnections { get; }

    void SetAddress(string address);
    void SetPort(int port);
    void SetMaxConnections(int maxConnections);
}