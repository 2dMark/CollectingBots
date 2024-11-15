using UnityEngine;

public class BotPicker : MonoBehaviour
{
    private Resource _target;

    public bool IsTargetReached => _target != null;

    public void PickUp(Resource resource)
    {
        if (_target != null)
            return;

        _target = resource;
        _target.transform.parent = transform;
        _target.transform.localPosition = Vector3.up;
    }

    public void PutIn(Base homeBase)
    {
        if (_target == null)
            return;

        homeBase.PutResourceOnWarehouse(_target);

        _target = null;
    }
}