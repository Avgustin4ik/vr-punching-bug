namespace MageSurvivor.Code.Core.Abstract.Service
{
    using System;
    using UnityEngine;

    [Serializable]
    public class Service : IDisposable
    {
        public void Dispose()
        {
            Debug.Log("Service Dispose" + GetType());
        }
    }
}