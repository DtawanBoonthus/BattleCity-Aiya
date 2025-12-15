using BC.Shared.GameSessions;
using Cysharp.Threading.Tasks;
using R3;

namespace BC.UI;

public interface IGameModeViewModel
{
    Observable<Unit> OnConnectSuccess { get; }
    UniTask ConnectGameAsync(GameMode mode);
}