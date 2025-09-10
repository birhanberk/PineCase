using DG.Tweening;
using Items;
using Items.Type;
using UnityEngine;
using VContainer;

namespace Grid
{
    public class SelectionHighlight : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private ItemManager _itemManager;
        
        private Tween _scaleTween;
        private Tween _alphaTween;
        
        [Inject]
        private void Construct(ItemManager itemManager)
        {
            _itemManager = itemManager;
            
            AddListeners();
            Hide();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            _itemManager.OnItemSelectionStarted += Show;
            _itemManager.OnItemSelected += OnItemSelected;
        }

        private void RemoveListeners()
        {
            _itemManager.OnItemSelectionStarted -= Show;
            _itemManager.OnItemSelected -= OnItemSelected;
        }

        private void Show(BaseItem item)
        {
            spriteRenderer.enabled = true;
            transform.position = item.Cell.transform.position;
        }

        private void OnItemSelected(BaseItem item)
        {
            if (item != null)
            {
                Show(item);
                Animate();
            }
            else
            {
                Hide();
            }
        }
        
        
        private void Animate()
        {
            transform.localScale = Vector3.one * 0.5f;
            _scaleTween?.Kill();
            _scaleTween = transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
                    
            SetAlpha(0f);
            _alphaTween?.Kill();
            _alphaTween = spriteRenderer.DOFade(1f, 0.5f).SetEase(Ease.OutSine);
        }

        private void Hide()
        {
            _scaleTween?.Kill();
            _alphaTween?.Kill();

            spriteRenderer.enabled = false;
            transform.localScale = Vector3.one;
        }

        private void SetAlpha(float a)
        {
            var color = spriteRenderer.color;
            color.a = a;
            spriteRenderer.color = color;
        }
    }
}
