using R3;

namespace BC.Shared.Networks;

public interface INetworkRoomService
{
    ReadOnlyReactiveProperty<int> PlayerCount { get; }
}