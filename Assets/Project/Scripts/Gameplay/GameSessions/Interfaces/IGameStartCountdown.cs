using Cysharp.Threading.Tasks;

namespace BC.Gameplay;

public interface IGameStartCountdown
{
    UniTask StartCountdownAsync(int seconds);
}