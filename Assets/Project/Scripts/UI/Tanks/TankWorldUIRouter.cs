using BC.Gameplay.Tanks;
using Cysharp.Threading.Tasks;
using R3;
using VitalRouter;

namespace BC.UI;

[Routes]
public partial class TankWorldUIRouter
{
    private readonly ReactiveProperty<(uint id, int hp)> hp = new((0, 0));

    public ReadOnlyReactiveProperty<(uint id, int hp)> Hp => hp;

    [Route]
    private UniTask UpdateHp(UpdateHpCommand command)
    {
        hp.Value = (command.ID, command.Hp);
        return UniTask.CompletedTask;
    }
}