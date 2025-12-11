using BC.Shared.Injections;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace BC.Core.Injections;

public class InjectionService : IInjectionService
{
    [Inject] private readonly IObjectResolver resolver = null!;

    public void Inject(GameObject gameObject)
    {
        resolver.InjectGameObject(gameObject);
    }
}