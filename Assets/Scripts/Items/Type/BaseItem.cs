using System;
using DG.Tweening;
using Grid;
using Items.Data;
using UnityEngine;

namespace Items.Type
{
    public abstract class BaseItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Collider2D boxCollider;
        [SerializeField] private ItemParticlePlayer mergeParticle;
        
        [SerializeField] private Cell cell;
        [SerializeField] private ItemData data;
        
        private Camera _mainCamera;
        private Vector3 _offset;
        private Tween _scaleTween;
        private Tween _moveTween;
        private Sequence _popSequence;
        private int _spriteOrder;

        public ItemData Data => data;
        public Cell Cell => cell;

        public Action<BaseItem> OnInputDown;
        public Action<BaseItem> OnInputDrag;
        public Action<BaseItem> OnInputUp;
        public Action<BaseItem> OnRemoveItem;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _spriteOrder = spriteRenderer.sortingOrder;
        }

        protected virtual void OnDestroy()
        {
            _scaleTween?.Kill();
            _moveTween?.Kill();
            _popSequence?.Kill();
        }

        public virtual void OnSpawned(ItemData itemData, Cell newCell)
        {
            data = itemData;
            cell = newCell;
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            spriteRenderer.sprite = data.LevelSo.Sprite;
        }

        public abstract void OnTap();

        protected void DestroyItem()
        {
            OnRemoveItem?.Invoke(this);
        }

        public virtual void OnMerge()
        {
            Data.UpgradeLevel();
            UpdateVisuals();
            var duration = 0.2f;
            Scale(Vector3.zero, duration, Ease.OutCubic, () =>
            {
                transform.localScale = Vector2.zero;
                Scale(Vector3.one, duration, Ease.OutBack);
            });
            mergeParticle.PlayParticle();
        }
        
        public void ReturnToCell()
        {
            Move(cell);
        }

        public void Move(Cell targetCell)
        {
            cell = targetCell;
            _moveTween?.Kill();
            _moveTween = transform.DOMove(GetCellPosition(targetCell), 0.1f).SetEase(Ease.OutCubic);
        }

        public void MoveInstant(Cell targetCell)
        {
            cell = targetCell;
            transform.position = GetCellPosition(targetCell);
        }

        public void Pop(Cell from, Cell to)
        {
            cell = to;
            
            transform.position = from.transform.position;
            transform.localScale = Vector3.zero;
            
            spriteRenderer.sortingOrder = _spriteOrder + 10;
            boxCollider.enabled = false;

            CreatePopSequence(from.transform.position, GetCellPosition(to));
        }
        
        private void CreatePopSequence(Vector3 start, Vector3 end)
        {
            var duration = 0.4f;

            _popSequence = DOTween.Sequence();

            _popSequence.Append(transform.DOMove(Vector3.Lerp(start, end, 0.5f), duration).SetEase(Ease.OutQuad));
            _popSequence.Join(transform.DOScale(Vector3.one * 1.5f, duration).SetEase(Ease.OutQuad));

            _popSequence.Append(transform.DOMove(end, duration).SetEase(Ease.InQuad));
            _popSequence.Join(transform.DOScale(Vector3.one, duration).SetEase(Ease.InQuad));
            
            _popSequence.OnComplete(() =>
            {
                spriteRenderer.sortingOrder = _spriteOrder;
                boxCollider.enabled = true;
            }).OnKill(() =>
            {
                spriteRenderer.sortingOrder = _spriteOrder;
                boxCollider.enabled = true;
                transform.localScale = Vector3.one;
            });
        }

        private Vector3 GetCellPosition(Cell targetCell)
        {
            var targetPosition = targetCell.transform.position;
            targetPosition.z = -0.1f;
            return targetPosition;
        }

        private void Scale(Vector2 size, float duration, Ease ease, Action onComplete = null)
        {
            _scaleTween?.Kill();
            _scaleTween = transform.DOScale(size, duration).SetEase(ease).OnComplete(() => { onComplete?.Invoke(); }).OnKill(() => { transform.localScale = Vector3.one; });
        }
        
        private void OnMouseDown()
        {
            _offset = transform.position - GetMouseWorldPos();
            boxCollider.enabled = false;
            spriteRenderer.sortingOrder = _spriteOrder + 1;
            
            OnInputDown?.Invoke(this);
        }

        private void OnMouseDrag()
        {
            var newPosition = GetMouseWorldPos() + _offset;
            if (transform.position != newPosition)
            {
                transform.position = newPosition;
                OnInputDrag?.Invoke(this);
            }
        }

        private void OnMouseUp()
        {
            OnInputUp?.Invoke(this);

            boxCollider.enabled = true;
            spriteRenderer.sortingOrder = _spriteOrder;
        }

        public void PlayTapAnimation()
        {
            transform.localScale = Vector2.one * 0.75f;
            Scale(Vector2.one, 1f, Ease.OutBounce);
        }
        
        private Vector3 GetMouseWorldPos()
        {
            var mousePos = Input.mousePosition;
            mousePos.z = 10f;
            return _mainCamera.ScreenToWorldPoint(mousePos);
        }
    }
}
