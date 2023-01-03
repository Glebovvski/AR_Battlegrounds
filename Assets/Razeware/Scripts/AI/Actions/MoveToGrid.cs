using Apex.AI;

namespace AI
{
    public class MoveToGrid : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (AIContext)context;
            var position = Enemies.AIManager.Instance.Grid.transform.position;
            c.Enemy.MoveTo(position);
        }
    }
}