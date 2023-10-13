using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public abstract void OnInteractorEnter(GameObject newInteractor);

    public abstract void OnInteractorExit(GameObject oldInteractor);

    public abstract void OnInteractorActivate();

    public abstract void OnInteractorGlowActivate(GlowEffectManager.GlowType glowType);
}
