using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CoinTween : MonoBehaviour
{
    [SerializeField] private Transform uiCoinIcon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerTagTwo>(out PlayerTagTwo playerTagTwo))
        {
            RectTransform uiCoinRect = uiCoinIcon.GetComponent<RectTransform>();
            transform.DOMove(uiCoinRect.position, 10f).SetEase(Ease.InOutQuad);
        }
    }
}
