namespace PunchingBag.Code.Core.Extentions
{
    using UnityEngine;

    public class ColliderDrawer : MonoBehaviour
    {
        public BoxCollider colliderToDraw;
        public Color gizmoColor = Color.cyan;
        private Collider _collider;
        private void OnDrawGizmos()
        {
            _collider ??= colliderToDraw ?? GetComponent<Collider>();
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireCube(_collider.bounds.center, _collider.bounds.size);
        }
    }
}