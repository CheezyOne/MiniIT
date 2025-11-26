using DG.Tweening;
using TMPro;
using UnityEngine;
using MiniIT.Managers;

namespace MiniIT
{
    public class ProfitText : MonoBehaviour
    {
        [SerializeField] private TMP_Text text = null;
        [SerializeField] private float lifetime = 1;
        [SerializeField] private float yDistance = 0.1f;

        private float initialAlpha = 0;

        private void Awake()
        {
            initialAlpha = text.alpha;
        }

        private void OnEnable()
        {
            text.alpha = initialAlpha;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOMoveY(transform.position.y + yDistance, lifetime));
            sequence.Join(text.DOFade(0, lifetime).OnComplete(() => PoolManager.Instance.DestroyObject(gameObject)));
        }

        public void SetText(string text)
        {
            this.text.text = text;
        }
    }
}