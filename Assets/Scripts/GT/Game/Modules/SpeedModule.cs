using GT.Game.Modules.Stats;
using GT.Game.Swarms;

namespace GT.Game.Modules
{
    public class SpeedModule : BaseModule
    {
        public new SpeedModuleStats Stats => (SpeedModuleStats)base.Stats;

        protected override void AddEffectToSwarm(Swarm swarm)
        {
            if (swarm)
                swarm.IncreaseSpeed(
                Stats.LinearSpeedIncrease,
                Stats.AngularSpeedIncrease,
                Stats.LinearVelocityLimitIncrease,
                Stats.AngularVelocityLimitIncrease);
        }

        protected override void RemoveEffectFromSwarm(Swarm swarm)
        {
            if (swarm)
                swarm.IncreaseSpeed(
                -Stats.LinearSpeedIncrease,
                -Stats.AngularSpeedIncrease,
                -Stats.LinearVelocityLimitIncrease,
                -Stats.AngularVelocityLimitIncrease);
        }
    }

}