using System;
using System.Collections.Generic;

namespace Arr.DDA.Script
{
    [Serializable]
    public struct ChannelHistory
    {
        public List<ChannelData> records;

        public ChannelHistory(ChannelData startingData)
        {
            records = new List<ChannelData>();
            records.Add(startingData);
        }

        public void Add(ChannelData newData, int limit = int.MaxValue)
        {
            if (records.Count > limit) records.RemoveRange(0, limit - records.Count);
            records.Add(newData);
        }

        public ChannelData LatestData => records.Count > 0 ? records[^1] : ChannelData.Default;

        public override string ToString()
        {
            var toPrint = "";
            for (var i = 0; i < records.Count; i++)
                toPrint += $"\n[{i}] {records[i]}";
            return toPrint;
        }
    }
}