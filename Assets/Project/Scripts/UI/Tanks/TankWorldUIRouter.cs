using System.Collections.Generic;
using BC.Gameplay.Tanks;
using Cysharp.Threading.Tasks;
using R3;
using VitalRouter;

namespace BC.UI;

[Routes]
public partial class TankWorldUIRouter
{
    private readonly ReactiveProperty<IReadOnlyDictionary<uint, int>> hps = new(new Dictionary<uint, int>());

    public ReadOnlyReactiveProperty<IReadOnlyDictionary<uint, int>> Hps => hps;


    [Route]
    private UniTask UpdateHp(UpdateHpCommand command)
    {
        var dict = new Dictionary<uint, int>(hps.Value)
        {
            [command.ID] = command.Hp
        };

        hps.Value = dict;

        return UniTask.CompletedTask;
    }
}