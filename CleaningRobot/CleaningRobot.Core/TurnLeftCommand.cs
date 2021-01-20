namespace CleaningRobot.Core
{
    class TurnLeftCommand : TurnCommand
    {
        public TurnLeftCommand() 
            : base(1, toRight: false)
        {
        }
    }
}
