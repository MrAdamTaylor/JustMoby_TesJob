using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RadialTrigger : MonoBehaviour
{
    public Action TriggerAction;
    public Action TriggerEndAction;
        
    [SerializeField]private Transform _goalTransform;
    [SerializeField] private float _radius;
    private bool _isTriggered;


    public void Construct(float radius, Transform goalTransform)
    {
        _radius = radius;
        _goalTransform = goalTransform;
    }

    private void OnDrawGizmos()
    {
        Handles.color = CheckTrigger(_goalTransform.position)? Color.green: Color.red;
        Handles.DrawWireDisc(this.transform.position, Vector3.up, _radius);
    }
       
    private void FixedUpdate()
    {
        _isTriggered = CheckTrigger(_goalTransform.position);
        if (_isTriggered)
            TriggerAction?.Invoke();
        else
        {
            TriggerEndAction?.Invoke();
        }
    }

    private bool CheckTrigger(Vector3 goalTransformPosition)
    {
        Vector3 dirToTargetWorld = (goalTransformPosition - transform.position);
        Vector3 flatDirToTarget = transform.InverseTransformVector(dirToTargetWorld);
        flatDirToTarget.y = 0;
        float flatDistance = flatDirToTarget.magnitude;
        if (flatDistance > _radius)
            return false;
        else
        {
            return true;
        }
    }
}
