using DG.Tweening;
using UnityEngine;

namespace Items
{
    public class ItemParticlePlayer : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;
        
        private Tween _particleTween;
        
        protected void OnDestroy()
        {
            _particleTween?.Kill();
        }

        public void PlayParticle()
        {
            _particleTween?.Kill();
            particle.Play();
        }

        public void PlayParticle(float duration)
        {
            PlayParticle();
            _particleTween = DOVirtual.DelayedCall(duration, StopParticle);
        }

        public void StopParticle()
        {
            particle.Stop();
        }
    }
}
