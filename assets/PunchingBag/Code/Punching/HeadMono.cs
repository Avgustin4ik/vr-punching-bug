namespace PunchingBag.Code.Punching
{
    using MoreMountains.Feedbacks;
    using UnityEngine;

    public class HeadMono : Damagable
    {
        [SerializeField] private MMF_Player hitFeedback;
        public override void TakeDamage(float force)
        {
            hitFeedback?.PlayFeedbacks();
        }
    }
}