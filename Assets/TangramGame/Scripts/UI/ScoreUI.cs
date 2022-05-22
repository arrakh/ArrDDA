using System;
using TMPro;
using UnityEngine;

namespace TangramGame.Scripts.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        private int lastScore;
        private LTDescr valueTween = null;
        
        private void OnEnable() => Events.OnScoreChanged += OnScoreChanged;

        private void OnScoreChanged(int newScore)
        {
            if (valueTween != null) LeanTween.cancel(valueTween.uniqueId); 
            
            valueTween = LeanTween.value(lastScore, newScore, 0.5f)
                .setOnUpdate(f => scoreText.text = Mathf.CeilToInt(f).ToString())
                .setOnComplete(
                delegate()
                {
                    lastScore = newScore;
                    valueTween = null;
                });
        }
    }
}