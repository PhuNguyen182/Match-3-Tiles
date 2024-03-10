using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Match3Tiles.Scripts.Gameplay.MatchTiles.Tasks
{
    public class TileMatchRule : IDisposable
    {
        private List<int> _matchOrder;

        public TileMatchRule() => _matchOrder = new();

        public int Append(int value)
        {
            if (IsFull()) 
                return -1;

            return _matchOrder.Contains(value) ? AppendExited(value) 
                                               : AppendNew(value);
        }

        private int AppendExited(int value)
        {
            int index = -1;
            for (int i = _matchOrder.Count - 1; i >= 0; i--)
            {
                if (_matchOrder[i] == value)
                {
                    index = i + 1;
                    _matchOrder.Insert(index, value);
                    return index;
                }
            }

            return index;
        }

        private int AppendNew(int value)
        {
            _matchOrder.Add(value);
            return _matchOrder.Count - 1;
        }

        public int CheckMatch()
        {
            if (_matchOrder.Count < MatchConfig.MATCH_RANGE)
                return -1;

            for (int i = 0; i < _matchOrder.Count - (MatchConfig.MATCH_RANGE - 1); i++)
            {
                List<int> values = _matchOrder.GetRange(i, MatchConfig.MATCH_RANGE);

                if (CheckValues(values))
                {
                    _matchOrder.RemoveRange(i, MatchConfig.MATCH_RANGE);
                    return i;
                }
            }

            return -1;
        }

        private bool CheckValues(List<int> values)
        {
            return values.Distinct().Count() == 1;
        }

        public bool IsFull()
        {
            return _matchOrder.Count >= MatchConfig.MAX_ORDER_COUNT;
        }

        public override string ToString()
        {
            StringBuilder builder = new();
            
            for (int i = 0; i < _matchOrder.Count; i++)
            {
                builder.Append($"{_matchOrder[i]} ");
            }

            return builder.ToString();
        }

        public void Dispose()
        {
            _matchOrder.Clear();
        }
    }
}
