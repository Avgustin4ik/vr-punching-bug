namespace PunchingBag.Code.Services.VfxService
{
    using System.Numerics;
    using MageSurvivor.Code.Core.Abstract.Service;

    public interface IVfxService
    {
        void PlayVfx(VfxType vfxName, Vector3 position);
    }

    public enum VfxType
    {
        Hit,
        Punch,
    }

    public class VfxService : Service
    {
        
    }
}