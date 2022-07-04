using System;
using UnityEngine;

namespace Arr.DDA.Script
{
    [Serializable]
    public struct ChannelData
    {
        public float currentProgression;
        public float currentDifficulty;
        
        public float anxietyThreshold;
        public float boredomThreshold;
        public float flowOffset;
        public float width;
        public float slant;

        public ChannelData(float currentProgression, float currentDifficulty, float anxietyThreshold, float boredomThreshold, float flowOffset, float width, float slant)
        {
            this.currentProgression = currentProgression;
            this.currentDifficulty = currentDifficulty;
            this.anxietyThreshold = anxietyThreshold;
            this.boredomThreshold = boredomThreshold;
            this.flowOffset = flowOffset;
            this.width = width;
            this.slant = slant;
        }

        public float GetAnxietyThreshold(float progression) => (slant * progression) + (anxietyThreshold * width) + flowOffset;
        public float GetAnxietyThreshold() => (slant * currentProgression) + (anxietyThreshold * width) + flowOffset;
        public float GetBoredomThreshold(float progression) => (slant * progression) - (boredomThreshold * width) + flowOffset;
        public float GetBoredomThreshold() => (slant * currentProgression) - (boredomThreshold * width) + flowOffset;

        public static ChannelData Default => new ChannelData(0f, 0f, 1f, 1f, 0f, 0.5f, 1f);

        public override string ToString()
        {
            return $"Prog: {currentProgression.ToString("F2")} | " +
                   $"Diff: {currentDifficulty.ToString("F2")} | " +
                   $"Anx: {anxietyThreshold.ToString("F2")} | " +
                   $"Bor: {boredomThreshold.ToString("F2")} | " +
                   $"Offset: {flowOffset.ToString("F2")} | " +
                   $"Width: {width.ToString("F2")} | " +
                   $"Slant: {slant.ToString("F2")}";
        }
    }

}