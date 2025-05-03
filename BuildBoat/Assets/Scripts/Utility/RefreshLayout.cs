using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Goossyaa
{
    public class RefreshLayout : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private bool _isInUpdate;
        
        private void Start()
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            
            Refresh();
        }

        public void Refresh()
        {
            if (_rectTransform.gameObject.activeSelf)
                StartCoroutine(RefreshRoutine());
        }

        public IEnumerator RefreshRoutine()
        {
            yield return null;
            Active();
        }

        public void Active()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
        }

        private void Update()
        {
            if (_isInUpdate)
            {
                Active();
            }
        }
    }
}