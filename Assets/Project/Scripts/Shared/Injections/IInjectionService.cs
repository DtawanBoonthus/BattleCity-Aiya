using UnityEngine;

namespace BC.Shared.Injections;

public interface IInjectionService
{
    void Inject(GameObject gameObject);
}