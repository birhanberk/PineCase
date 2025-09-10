using Grid;
using Items;
using Orders;
using Resources;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scope
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private ResourceManager resourceManager;
        [SerializeField] private GridManager gridManager;
        [SerializeField] private ItemManager itemManager;
        [SerializeField] private OrderManager orderManager;
        [SerializeField] private GameObject ui;
        [SerializeField] private GameObject grid;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(resourceManager);
            builder.RegisterComponent(gridManager);
            builder.RegisterComponent(itemManager);
            builder.RegisterComponent(orderManager);
        }

        protected override void Awake()
        {
            base.Awake();
            Container.InjectGameObject(ui);
            Container.InjectGameObject(grid);
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
        }
    }
}
