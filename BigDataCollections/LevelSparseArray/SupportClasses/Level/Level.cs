namespace BigDataCollections.LevelSparseArray.SupportClasses.Level
{
    internal class Level
    {
        //API
        public Level(int size)
        {
            _nextLevels = new Level[size];
        }

        public virtual Level this[int index]
        {
            get
            {
                return _nextLevels[index];
            }
            set
            {
                _nextLevels[index] = value;
            }
        }

        //Data
        private readonly Level[] _nextLevels;
    }
}