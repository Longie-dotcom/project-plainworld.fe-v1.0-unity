using Assets.Core;
using System;
using System.Collections;
using UnityEngine;
using Assets.Service.Interface;

public abstract class ComponentBinder : MonoBehaviour
{
    #region Attributes
    #endregion

    #region Properties
    public virtual string StepName 
    {
        get { return name; }
    }
    #endregion

    #region Methods
    protected IEnumerator BindWhenReady<T>(
        Action<T> onReady) where T : class, IService
    {
        // 1. Wait for registration
        while (!ServiceLocator.IsRegistered<T>())
            yield return null;

        var service = ServiceLocator.Get<T>();

        // 2. Wait for initialization
        while (!service.IsInitialized)
            yield return null;

        // 3. Safe to bind
        onReady(service);
    }

    /// <summary>
    /// Each binder overrides this to bind its services.
    /// Called by LoadingUI to orchestrate all binders.
    /// </summary>
    public abstract IEnumerator BindAllServices();
    #endregion
}
