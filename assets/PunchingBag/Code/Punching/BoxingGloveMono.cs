namespace PunchingBag.Code.Punching
{
    using MageSurvivor.Code.Core.Pool;
    using UnityEngine;

    public class BoxingGloveMono : PooledMonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;
        public Rigidbody rigidBody => _rigidBody;
    }
}