using AYellowpaper.SerializedCollections;
using Resources.Type;
using UnityEngine;

namespace Resources.Data
{
    [CreateAssetMenu(fileName = "ResourceData", menuName = "Data / Resource / Resource List")]
    public class ResourceListSo : ScriptableObject
    {
        [SerializeField, SerializedDictionary] private SerializedDictionary<ResourceType, ResourceSo> resourceList;

        public SerializedDictionary<ResourceType, ResourceSo> ResourceList => resourceList;
    }
}
