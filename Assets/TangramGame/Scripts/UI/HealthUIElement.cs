using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TangramGame.Scripts.UI
{
    public class HealthUIElement : MonoBehaviour
    {
        [SerializeField] private RectTransform visual;
        [SerializeField] private float fallTime = 2f;
        [SerializeField] private AnimationCurve yFallCurve;

        private Vector2 startPosition;

        private void Awake()
        {
            startPosition = visual.anchoredPosition;
        }

        public void ResetVisual()
        {
            visual.LeanAlpha(1f, 0f);
            visual.LeanRotateZ(0f, 0f);
            visual.anchoredPosition = startPosition;
        }

        public void AnimateVisual()
        {
            var randX = Random.Range(-100f, 200f);
            visual.LeanRotateZ(-randX / 5f, fallTime);
            visual.LeanMoveX(randX, fallTime);
            visual.LeanMoveY(-600f, fallTime).setEase(yFallCurve);
            visual.LeanAlpha(0f, fallTime);
        }
    }
}