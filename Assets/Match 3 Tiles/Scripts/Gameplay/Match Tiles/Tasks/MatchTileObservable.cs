using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3Tiles.Scripts.Gameplay.MatchTiles
{
    public class MatchTileObservable : IObservable<TileData>
    {
        private List<IObserver<TileData>> _tileObservers;

        public MatchTileObservable() => _tileObservers = new();

        public IDisposable Subscribe(IObserver<TileData> observer)
        {
            if (_tileObservers != null && !_tileObservers.Contains(observer))
                _tileObservers.Add(observer);

            return new MatchTileUnsubscriber(_tileObservers, observer);
        }

        public void ExecuteTile(TileData tileData)
        {
            if (_tileObservers == null)
                return;

            for (int i = 0; i < _tileObservers.Count; i++)
            {
                _tileObservers[i].OnNext(tileData);
            }
        }
    }

    public class MatchTileUnsubscriber : IDisposable
    {
        private IObserver<TileData> _observer;
        private List<IObserver<TileData>> _observers;

        public MatchTileUnsubscriber(List<IObserver<TileData>> observers, IObserver<TileData> observer)
            => (_observers, _observer) = (observers, observer);

        public void Dispose()
        {
            if (_observers == null)
                return;
            
            if (_observers.Count == 0)
                return;

            if (!_observers.Contains(_observer))
                return;

            _observers.Remove(_observer);
        }
    }
}
