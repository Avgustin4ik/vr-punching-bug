namespace PunchingBag.Code.Punching
{
    using MoreMountains.Feedbacks;
    using UnityEngine;

    public class HeadMono : Damagable
    {
        [SerializeField] private MMF_Player hitFeedback;
        [SerializeField] private MMF_Player strongHitFeedback;
        [SerializeField] private float strongHitLowLimit = 450f;
        public override void TakeDamage(float force)
        {
            hitFeedback?.PlayFeedbacks();
            if (force > strongHitLowLimit)
            {
                strongHitFeedback?.PlayFeedbacks();
            }
        }
    }
}