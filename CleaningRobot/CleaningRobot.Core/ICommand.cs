namespace CleaningRobot.Core
{
    /// <summary>
    /// Represents a command which can be executed on cleaning robot.
    /// </summary>
    interface ICommand
    {
        /// <summary>
        /// Gets the number of battery units that are needed to execute the command.
        /// </summary>
        int BatteryUnits { get; }

        /// <summary>
        /// Validates if execution of this command is possible for current robot position.
        /// </summary>
        /// <param name="robot">Cleaning robot.</param>
        /// <returns><c>true</c> if the command can be executed; otherwise, <c>false</c>.</returns>
        bool Validate(CleaningRobot robot);

        /// <summary>
        /// Executes command.
        /// </summary>
        /// <param name="robot">Cleaning robot on which is the command executed.</param>
        void Execute(CleaningRobot robot);
    }
}
