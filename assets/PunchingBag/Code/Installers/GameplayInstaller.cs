namespace PunchingBag.Code.Installers
{
    using Punching;
    using Reflex.Core;
    using UnityEngine;

    public class GameplayInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder containerBuilder)
        { 
            Debug.Log("GameplayInstaller InstallBindings");
        }
    }
}