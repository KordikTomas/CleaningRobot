using System;
using System.Collections.Generic;

namespace CleaningRobot.Core
{
    /// <summary>
    /// Represents cleaning robot.
    /// </summary>
    class CleaningRobot
    {
        private readonly IDictionary<CommandType, ICommand> _commands;
        private readonly HashSet<Point> _visitedCells;
        private readonly HashSet<Point> _cleanedCells;
        private int _backOffStrategy;

        /// <summary>
        /// Initializes new instance of CleaningRobot class.
        /// </summary>
        /// <param name="battery">Initial battery state.</param>
        /// <param name="map">Map of the house.</param>
        /// <param name="position">Initial position in map.</param>
        /// <param name="facing">Initial facing of cleaning robot.</param>
        public CleaningRobot(int battery, CellType[,] map, Point position, Facing facing)
        {
            if (battery < 0) throw new ArgumentOutOfRangeException(nameof(battery));
            if (map == null) throw new ArgumentNullException(nameof(map));
            if (position == null) throw new ArgumentNullException(nameof(position));

            this.Battery = battery;
            this.Map = map;
            this.Position = position;
            this.Facing = facing;

            _commands = new Dictionary<CommandType, ICommand>
            {
                { CommandType.TurnLeft, new TurnLeftCommand() },
                { CommandType.TurnRight, new TurnRightCommand() },
                { CommandType.Advance, new AdvanceCommand() },
                { CommandType.Back, new BackCommand() },
                { CommandType.Clean, new CleanCommand() }
            };
            _visitedCells = new HashSet<Point>();
            _visitedCells.Add(position);
            _cleanedCells = new HashSet<Point>();
            _backOffStrategy = 0;
        }

        internal int Battery { get; private set; }

        internal CellType[,] Map { get; private set; }

        internal Point Position { get; set; }

        internal Facing Facing { get; set; }

        internal IEnumerable<Point> VisitedCells
        {
            get { return _visitedCells; }
        }

        internal IEnumerable<Point> CleanedCells
        {
            get { return _cleanedCells; }
        }

        internal void Move(int directionX, int directionY)
        {
            int diffX = Math.Sign(directionX);
            int diffY = Math.Sign(directionY);
            var newPosition = new Point(this.Position.X + diffX, this.Position.Y+ diffY);
            _visitedCells.Add(newPosition);
            this.Position = newPosition;
        }

        internal void CleanCurrentPosition()
        {
            _cleanedCells.Add(this.Position);
        }

        /// <summary>
        /// Executes sequence of commands.
        /// </summary>
        /// <param name="commandTypes">Sequence of commands.</param>
        internal void Run(IEnumerable<CommandType> commandTypes)
        {
            if (commandTypes == null) throw new ArgumentNullException(nameof(commandTypes));

            Queue<CommandType> commandTypesQueue = new Queue<CommandType>(commandTypes);
            RunCore(commandTypesQueue);
        }

        private void RunCore(Queue<CommandType> commandTypes)
        { 
            while (commandTypes.Count != 0)
            {
                var commandType = commandTypes.Dequeue();
                ICommand command = GetCommand(commandType);

                if (this.Battery < command.BatteryUnits) return;

                this.Battery -= command.BatteryUnits;
                if (command.Validate(this))
                {
                    command.Execute(this);
                }
                else
                {
                    if (_backOffStrategy != 0)
                    {
                        // clear previous backoff strategy if failed
                        commandTypes.Clear();
                    }

                    var backOffQueue = GetBackOffStrategy();
                    if (backOffQueue.Count != 0)
                    {
                        _backOffStrategy++;
                        RunCore(backOffQueue);
                        _backOffStrategy--;
                    }
                }
            }
        }

        private ICommand GetCommand(CommandType commandType)
        {
            ICommand result;
            if (_commands.TryGetValue(commandType, out result))
            {
                return result;
            }

            throw new ArgumentException($"Unknown command type: {commandType}");
        }

        private Queue<CommandType> GetBackOffStrategy()
        {
            Queue<CommandType> result = new Queue<CommandType>();

            switch (_backOffStrategy)
            {
                case 0:
                    result.Enqueue(CommandType.TurnRight);
                    result.Enqueue(CommandType.Advance);
                    break;

                case 1:
                    result.Enqueue(CommandType.TurnLeft);
                    result.Enqueue(CommandType.Back);
                    result.Enqueue(CommandType.TurnRight);
                    result.Enqueue(CommandType.Advance);
                    break;

                case 2:
                    result.Enqueue(CommandType.TurnLeft);
                    result.Enqueue(CommandType.TurnLeft);
                    result.Enqueue(CommandType.Advance);
                    break;

                case 3:
                    result.Enqueue(CommandType.TurnRight);
                    result.Enqueue(CommandType.Back);
                    result.Enqueue(CommandType.TurnRight);
                    result.Enqueue(CommandType.Advance);
                    break;

                case 4:
                    result.Enqueue(CommandType.TurnLeft);
                    result.Enqueue(CommandType.TurnLeft);
                    result.Enqueue(CommandType.Advance);
                    break;

                default:
                    break;
            }

            return result;
        }
    }
}
