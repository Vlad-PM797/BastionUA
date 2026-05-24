using System.Collections.Generic;
using BastionUA.Core;

namespace BastionUA.Services
{
    public sealed class EventLogService
    {
        private readonly List<string> _entries = new List<string>();

        public void AddEntry(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            _entries.Insert(0, message);
            while (_entries.Count > GameConstants.EventLogMaxEntries)
            {
                _entries.RemoveAt(_entries.Count - 1);
            }
        }

        public IReadOnlyList<string> GetRecentEntries()
        {
            return _entries;
        }

        public void Clear()
        {
            _entries.Clear();
        }
    }
}
