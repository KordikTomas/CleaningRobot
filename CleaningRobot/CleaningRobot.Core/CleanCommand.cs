namespace CleaningRobot.Core
{
    class CleanCommand : CommandBase
    {
        public CleanCommand() 
            : base(5)
        {
        }

        protected override void ExecuteCore(CleaningRobot robot)
        {
            robot.CleanCurrentPosition();
        }
    }
}
