using GT.Game.Modules.Stats;
using GT.Game.Swarms;

namespace GT.Game.Modules
{
    public class SpeedModule : BaseModule
    {
        public new SpeedModuleStats Stats => (SpeedModuleStats)base.Stats;

        public void Initialize(SpeedModuleStats moduleStats)
        {
            base.Initialize(moduleStats);
        }

        protected override void AddEffectToSwarm(Swarm swarm)
        {
            swarm.IncreaseSpeed(Stats.SpeedIncrease);
        }

        protected override void RemoveEffectFromSwarm(Swarm swarm)
        {
            swarm.IncreaseSpeed(-Stats.SpeedIncrease);
        }
    }

}