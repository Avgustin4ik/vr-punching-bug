namespace PunchingBag.Code.Services.InputService
{
    using MageSurvivor.Code.Core.Abstract.Service;
    using UnityEngine;

    [CreateAssetMenu(fileName = "InputServiceSource", menuName = "PGD/Services/InputServiceSource")]
    public class InputServiceSource : ServiceSource<InputService>
    {
        public KeyBinding KeyBinding;
        protected override InputService CreateServiceInstance()
        {
            return new InputService(KeyBinding);
        }
    }
}