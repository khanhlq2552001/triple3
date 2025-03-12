using Lean.Pool;
using UnityEngine;

namespace Game.MainGame
{
    public class FxSmoke : MonoBehaviour
    {
        private ParticleSystem _par;

        private void Awake()
        {
            _par = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            _par.Play();

            Invoke("DelayPool", 1f);
        }

        private void DelayPool()
        {
            LeanPool.Despawn(this);
        }
    }
}
