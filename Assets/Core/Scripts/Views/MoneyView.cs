using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Core.Scripts.Views
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private Transform _lastPoint;
        [SerializeField] private TextMeshProUGUI _moneyUIText;
        [SerializeField] private List<Transform> _moneyListUI;

        private Camera camera;

        void Start()
        {
            camera = Camera.main;
        }

        public void MoveMoneyUI(Transform startPoint)
        {
            var item = _moneyListUI.Find(x => x.gameObject.activeSelf == false);
            item.gameObject.SetActive(true);
            item.position = camera.WorldToScreenPoint(startPoint.position);
            item.transform.localScale = Vector3.zero;

            var seq = DOTween.Sequence();
            seq.Append(item.DOScale(.3f, 0.3f).SetEase(Ease.OutBack));
            seq.Insert(0.15f, item.DOMove(_lastPoint.position, .8f).SetEase(Ease.InBack));
            seq.OnComplete(() => { item.gameObject.SetActive(false); });
        }
    }
}