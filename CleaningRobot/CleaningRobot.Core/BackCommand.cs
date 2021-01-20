namespace CleaningRobot.Core
{
    class BackCommand : MoveCommand
    {
        public BackCommand() 
            : base(3, advance: false)
        {
        }
    }
}
