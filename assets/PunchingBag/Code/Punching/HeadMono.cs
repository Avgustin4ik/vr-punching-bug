namespace PunchingBag.Code.Punching
{
    using MoreMountains.Feedbacks;
    using UnityEngine;

    public class HeadMono : Damagable
    {
        [SerializeField] private Animator faceAnimator;
        [SerializeField] private string[] animationsTriggers;
        [SerializeField] private MMF_Player hitFeedback;
        [SerializeField] private MMF_Player strongHitFeedback;
        [SerializeField] private float strongHitLowLimit = 450f;
        public override void TakeDamage(float force)
        {
            hitFeedback?.PlayFeedbacks();
            PlayAnimation(Random.Range(0,3));
            if (force > strongHitLowLimit)
            {
                strongHitFeedback?.PlayFeedbacks();
            }
            base.TakeDamage(force);
        }

        private void PlayAnimation(int range)
        {
            if (faceAnimator == null)
            {
                Debug.LogError("Face animator is not assigned.");
                return;
            }
            if (animationsTriggers == null || animationsTriggers.Length == 0)
            {
                Debug.LogError("Animation triggers are not assigned.");
                return;
            }
            
            if (range < 0 || range >= animationsTriggers.Length)
            {
                Debug.LogError("Invalid animation trigger index.");
                return;
            }
            string trigger = animationsTriggers[range];
            faceAnimator.SetTrigger(trigger);
        }
    }
}