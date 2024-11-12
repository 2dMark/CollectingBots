using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceDatabase : MonoBehaviour
{
    private Dictionary<Resource, ResourceStates> _resourcesStates;

    public bool IsResourceUnassigned => _resourcesStates.ContainsValue(ResourceStates.Unassigned);

    private void Awake()
    {
        _resourcesStates = new();
    }

    public bool TryGetCloserResource(Bot bot, out Resource closerResource)
    {
        closerResource = _resourcesStates.Keys.
            OrderBy(resource => (bot.transform.position - resource.transform.position).sqrMagnitude).
            FirstOrDefault(resource => _resourcesStates[resource] == ResourceStates.Unassigned);

        if (closerResource != null)
            _resourcesStates[closerResource] = ResourceStates.Assigned;

        return closerResource != null;
    }

    public void AddResourceData(List<Resource> resources)
    {
        foreach (Resource resource in resources)
            if (_resourcesStates.TryAdd(resource, ResourceStates.Unassigned))
                resource.Destroyed += RemoveResourceData;
    }

    private void RemoveResourceData(Resource resource) => _resourcesStates.Remove(resource);
}