using Resources;
using Resources.Type;
using TMPro;
using UnityEngine;
using VContainer;

namespace UI.Resources
{
    public class EnergyTimerDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;
        
        private ResourceManager _resourceManager;
        private RechargeableResource _energyResource;

        [Inject]
        private void Construct(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        private void Start()
        {
            if (_resourceManager.TryGetResource(ResourceType.Energy, out var generic) && generic is RechargeableResource rechargeable)
            {
                _energyResource = rechargeable;
                _energyResource.OnTimerChanged += UpdateTimerText;
                _energyResource.OnValueChanged += OnEnergyChanged;
            }

            UpdateTimerText(Mathf.CeilToInt(_energyResource?.TimeUntilNextRecharge() ?? 0));
        }

        private void OnDestroy()
        {
            if (_energyResource != null)
            {
                _energyResource.OnTimerChanged -= UpdateTimerText;
                _energyResource.OnValueChanged -= OnEnergyChanged;
            }
        }

        private void OnEnergyChanged(int value)
        {
            if (_energyResource.IsFull())
            {
                timerText.text = "";
            }
        }

        private void UpdateTimerText(int seconds)
        {
            if (_energyResource == null || _energyResource.IsFull())
            {
                timerText.text = "";
                return;
            }

            var min = seconds / 60;
            var sec = seconds % 60;
            timerText.text = $"{min:D2}:{sec:D2}";
        }
    }
}