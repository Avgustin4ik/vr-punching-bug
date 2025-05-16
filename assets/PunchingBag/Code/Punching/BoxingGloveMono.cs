namespace PunchingBag.Code.Punching
{
    using MageSurvivor.Code.Core.Pool;
    using UnityEngine;
    public class BoxingGloveMono : PooledMonoBehaviour
    {
        [SerializeField] private float Force = 10f;
        [SerializeField] private Rigidbody _rigidBody;
        public Rigidbody rigidBody => _rigidBody;

        public void Punch()
        {
            if (_rigidBody != null)
            {
                _rigidBody.AddForce(transform.forward * Force, ForceMode.Impulse);
            }
            else
            {
                Debug.LogWarning("Rigidbody is not assigned.");
            }
            
        }
    }
}