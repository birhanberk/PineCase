using Items.Data.Common;
using Resources;
using Resources.Type;
using VContainer;

namespace Items.Type.Energy
{
    public class EnergyItem : BaseItem
    {
        private ResourceManager _resourceManager;
        
        [Inject]
        private void Construct(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }
        
        public override void OnTap()
        {
            _resourceManager.Add(ResourceType.Energy, ((ResourceLevelSo)Data.LevelSo).RewardCount);
            DestroyItem();
        }
    }
}
