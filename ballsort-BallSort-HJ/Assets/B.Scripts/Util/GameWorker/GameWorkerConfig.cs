using System.Collections.Generic;

namespace Soybean.GameWorker
{
    public class GameWorkerConfig : ScriptableSingleton<GameWorkerConfig>
    {
        public List<GameWorkerUnit> All;

        public List<GameWorkerUnit> GetCopys()
        {
            List<GameWorkerUnit> result = new List<GameWorkerUnit>();
            for (int i = 0; i < All.Count; i++)
            {
                GameWorkerUnit unit = All[i].Clone();
                result.Add(unit);
            }

            return result;
        }
    }
}