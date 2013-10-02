using System;

namespace BigDataCollections.DistributedArray.Managers
{
    public class GarabeCollectorManager
    {
        //API
        public GarabeCollectorManager(int maxCountOfRemovedElements)
        {
            MaxCountOfRemovedElements = maxCountOfRemovedElements;
        }
        public void CallGC()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }

        //Data
        public int MaxCountOfRemovedElements
        {
            get
            {
                return _maxCountOfRemovedElements;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "MaxCountOfRemovedElements cant be less than 0.");
                }
                _maxCountOfRemovedElements = value;

                TryToCallGC();
            }
        }
        public int CurrentCountOfRemovedElements
        {
            get
            {
                return _currentCountOfRemovedElements;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "CurrentCountOfRemovedElements cant be less than 0.");
                }
                _currentCountOfRemovedElements = value;

                TryToCallGC();
            }
        }

        private int _maxCountOfRemovedElements;
        private int _currentCountOfRemovedElements;

        //Support
        private void TryToCallGC()
        {
            if (CurrentCountOfRemovedElements >= MaxCountOfRemovedElements)
            {
                CallGC();
            }
        }
    }
}
