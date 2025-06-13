using UnityEngine;

namespace EnterpriceLogic
{
    public class EllipseTrigger : MonoBehaviour
    {
        [SerializeField] private Transform _goalTransform;
        [SerializeField] private float _radius;
        [SerializeField] private float _xCompression = 1f;
        private bool _isTriggered;

        public bool CheckTrigger()
        {
            Vector3 localPos = transform.InverseTransformPoint(_goalTransform.position);
            localPos.x /= _xCompression;
            return localPos.magnitude <= _radius;
        }

        private void OnDrawGizmos()
        {
            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, new Vector3( _xCompression,1, 1));
            Gizmos.color = CheckTrigger() ? Color.green : Color.red;
            Gizmos.DrawWireSphere(Vector3.zero, _radius);
            Gizmos.matrix = oldMatrix;
        }
    }
}
