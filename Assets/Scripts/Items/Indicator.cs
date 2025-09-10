using UnityEngine;

namespace Items
{
    public class Indicator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private const string propertyName = "_FillAmount";

        public void Show(Sprite sprite)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = sprite;
        }

        public void Hide()
        {
            spriteRenderer.enabled = false;
        }

        public void SetFillAmount(float fillAmount)
        {
            var material = spriteRenderer.material;
            material.SetFloat(propertyName, fillAmount);
        }
    }
}
