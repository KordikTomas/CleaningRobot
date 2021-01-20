namespace CleaningRobot.Core
{
    class TurnRightCommand : TurnCommand
    {
        public TurnRightCommand() 
            : base(1, toRight: true)
        {
        }
    }
}
