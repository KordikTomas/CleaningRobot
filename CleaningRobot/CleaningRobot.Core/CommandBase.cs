using System;

namespace CleaningRobot.Core
{
    abstract class CommandBase : ICommand
    {
        public CommandBase(int batteryUnits)
        {
            if (batteryUnits < 0) throw new ArgumentOutOfRangeException(nameof(batteryUnits));

            this.BatteryUnits = batteryUnits;
        }

        public int BatteryUnits { get; private set; }

        public virtual bool Validate(CleaningRobot robot)
        {
            return true;
        }

        public void Execute(CleaningRobot robot)
        {
            if (Validate(robot))
            {
                ExecuteCore(robot);
            }
        }

        protected abstract void ExecuteCore(CleaningRobot robot);
    }
}
