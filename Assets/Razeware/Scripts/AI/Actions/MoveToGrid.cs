using Apex.AI;

namespace AI
{
    public class MoveToGrid : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (AIContext)context;
            c.Enemy.MoveTo(Enemies.AIManager.Instance.Grid.transform.position);
        }
    }
}