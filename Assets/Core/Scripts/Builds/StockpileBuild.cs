using Lean.Pool;

namespace Core.Scripts.Builds
{
    public class StockpileBuild : Build
    {
        public int countSword;

        public void DespawnItems()
        {
            items.ForEach(x => LeanPool.Despawn(x));
            countSword += items.Count;
            items.Clear();
        }
    }
}