using Resources;
using Resources.Type;
using TMPro;
using UnityEngine;
using VContainer;

namespace UI.Resources
{
    public class ResourceDisplay : MonoBehaviour
    {
        [SerializeField] private ResourceType type;
        [SerializeField] private TMP_Text valueText;

        private ResourceManager _resourceManager;

        [Inject]
        private void Construct(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        private void Start()
        {
            UpdateText(_resourceManager.GetAmount(type));
            _resourceManager.Subscribe(type, UpdateText);
        }

        private void UpdateText(int newAmount)
        {
            valueText.text = newAmount.ToString();
        }

        private void OnDestroy()
        {
            _resourceManager.Unsubscribe(type, UpdateText);
        }
    }
}