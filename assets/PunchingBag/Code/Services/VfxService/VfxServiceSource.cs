namespace PunchingBag.Code.Services.VfxService
{
    using MageSurvivor.Code.Core.Abstract.Service;
    using UnityEngine;

    [CreateAssetMenu(fileName = "VfxSpawnerService", menuName = "PGD/Services/VfxSpawnerService")]
    public class VfxServiceSource : ServiceSource<VfxService>
    {
        protected override VfxService CreateServiceInstance()
        {
            throw new System.NotImplementedException();
        }
    }
}