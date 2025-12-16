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
    private readonly ReactiveProperty<IReadOnlyDictionary<uint, int>> staggers = new(new Dictionary<uint, int>());
    private readonly ReactiveProperty<IReadOnlyDictionary<uint, float>> iframes = new(new Dictionary<uint, float>());

    public ReadOnlyReactiveProperty<IReadOnlyDictionary<uint, int>> Hps => hps;
    public ReadOnlyReactiveProperty<IReadOnlyDictionary<uint, int>> Staggers => staggers;
    public ReadOnlyReactiveProperty<IReadOnlyDictionary<uint, float>> Iframes => iframes;


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

    [Route]
    private UniTask UpdateStagger(StaggerCommand command)
    {
        var dict = new Dictionary<uint, int>(staggers.Value)
        {
            [command.NetId] = command.StaggerTime
        };

        staggers.Value = dict;

        return UniTask.CompletedTask;
    }

    [Route]
    private UniTask UpdateStagger(EndStaggerCommand command)
    {
        var dict = new Dictionary<uint, int>(staggers.Value)
        {
            [command.NetId] = -1
        };

        staggers.Value = dict;

        return UniTask.CompletedTask;
    }

    [Route]
    private UniTask UpdateIFrame(IFrameCommand command)
    {
        var dict = new Dictionary<uint, float>(iframes.Value)
        {
            [command.NetId] = command.IFrameTime
        };

        iframes.Value = dict;

        return UniTask.CompletedTask;
    }

    [Route]
    private UniTask UpdateIFrame(EndIFrameCommand command)
    {
        var dict = new Dictionary<uint, int>(staggers.Value)
        {
            [command.NetId] = -1
        };

        staggers.Value = dict;

        return UniTask.CompletedTask;
    }
}