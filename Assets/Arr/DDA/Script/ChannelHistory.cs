using System.Collections.Generic;

namespace Arr.DDA.Script
{
    public struct ChannelHistory
    {
        private readonly List<ChannelData> records;

        public List<ChannelData> Records => records;

        public ChannelHistory(ChannelData startingData)
        {
            records = new List<ChannelData>();
            records.Add(startingData);
        }

        public ChannelData LatestData => records.Count > 0 ? records[records.Count - 1] : ChannelData.Default;

        public void AddRecord(ChannelData data) => records.Add(data);
    }
}