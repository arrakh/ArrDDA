using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TangramGame.Scripts.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private List<HealthUIElement> healthElements;

        private void OnEnable() => Events.OnHealthChanged += SetAmount;

        private void OnDisable() => Events.OnHealthChanged -= SetAmount;

        private void SetAmount(int amount)
        {
            for (int i = 0; i < healthElements.Count; i++)
            {
                var element = healthElements[i];
                element.ResetVisual();
                element.gameObject.SetActive(i <= amount);
                if (i == amount) element.AnimateVisual();
            }
        }
    }
}