using UnityEngine;

namespace Resources.Data
{
    [CreateAssetMenu(fileName = "Resource Data", menuName = "Data / Resource / Resource")]
    public class ResourceSo : ScriptableObject
    {
        [SerializeField] private int initialAmount;

        public int InitialAmount => initialAmount;
    }
}
