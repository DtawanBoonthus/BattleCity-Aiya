using R3;

namespace BC.Shared.Networks;

public interface INetworkConfig
{
    ReadOnlyReactiveProperty<string> Address { get; }
    ReadOnlyReactiveProperty<int> Port { get; }
    ReadOnlyReactiveProperty<int> MaxConnections { get; }

    void SetAddress(string address);
    void SetPort(int port);
    void SetMaxConnections(int maxConnections);
}