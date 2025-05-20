namespace PunchingBag.Code.Punching
{
    using UnityEngine;

    public abstract class Damagable : MonoBehaviour
    {
        // Placeholder for the Damagable class
        public virtual void TakeDamage(float force) {}

        public void TakeDamage()
        {
        }
    }
}