using Items.Data.Common;
using Resources;
using Resources.Type;
using VContainer;

namespace Items.Type.Coin
{
    public class CoinItem : BaseItem
    {
        private ResourceManager _resourceManager;
        
        [Inject]
        private void Construct(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }
        
        public override void OnTap()
        {
            _resourceManager.Add(ResourceType.Coin, ((ResourceLevelSo)Data.LevelSo).RewardCount);
            DestroyItem();
        }
    }
}
