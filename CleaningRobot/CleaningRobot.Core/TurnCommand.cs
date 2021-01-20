using System;

namespace CleaningRobot.Core
{
    abstract class TurnCommand : CommandBase
    {
        private readonly bool _toRight;

        public TurnCommand(int batteryUnits, bool toRight) 
            : base(batteryUnits)
        {
            _toRight = toRight;
        }

        protected override void ExecuteCore(CleaningRobot robot)
        {
            var facing = robot.Facing + (_toRight ? 1 : -1);
            if (!Enum.IsDefined(typeof(Facing), facing))
            {
                facing = _toRight ? Facing.North : Facing.West;
            }
            robot.Facing = facing;
        }
    }
}
