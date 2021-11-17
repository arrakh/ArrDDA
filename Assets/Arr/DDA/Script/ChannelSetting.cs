using System;

namespace Arr.DDA
{
    [Serializable]
    public class ChannelSetting
    {
        public float AnxietyThreshold;
        public float BoredomThreshold;
        public float FlowOffset;
        public float Width;
        public float Slant;

        public ChannelSetting()
        {
            AnxietyThreshold = 1f;
            BoredomThreshold = 1f;
            FlowOffset = 0f;
            Width = 0.5f;
            Slant = 1;
        }
        
        public ChannelSetting(float anxietyThreshold, float boredomThreshold, float flowOffset, float width, float slant)
        {
            AnxietyThreshold = anxietyThreshold;
            BoredomThreshold = boredomThreshold;
            FlowOffset = flowOffset;
            Width = width;
            Slant = slant;
        }

        public float GetAnxietyThreshold(float progression) => (Slant * progression) + (AnxietyThreshold * Width) + FlowOffset;
        public float GetBoredomThreshold(float progression) => (Slant * progression) - (BoredomThreshold * Width) + FlowOffset;
    }
}