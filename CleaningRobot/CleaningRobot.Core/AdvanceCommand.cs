namespace CleaningRobot.Core
{
    class AdvanceCommand : MoveCommand
    {
        public AdvanceCommand() 
            : base(2, advance: true)
        {
        }
    }
}
