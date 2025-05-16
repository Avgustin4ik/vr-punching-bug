namespace PunchingBag.Code.Installers
{
    using MageSurvivor.Code.Core.Abstract.Service;
    using Reflex.Core;
    using UnityEngine;

    public class ServicesInstaller : MonoBehaviour, IInstaller
    {
        public ServiceSourceBase[] serviceSources;
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
#if DEBUG
            Debug.Log("ServicesInstaller InstallBindings");
#endif
            foreach (var sourceBase in serviceSources)
            {
                var service = sourceBase.CreateService();
                Debug.Log("sourceBase: " + service.GetType());
                containerBuilder.AddSingleton(service);
                var serviceType = service.GetType();
                var interfaceType = serviceType.GetInterface($"I{serviceType.Name}");

                if (interfaceType != null)
                {
                    containerBuilder.AddSingleton(service, interfaceType);
                }
                else
                {
                    containerBuilder.AddSingleton(service);
                    Debug.LogWarning($"No interface found for service type {serviceType.Name}");
                }
            }
        }
    }
}