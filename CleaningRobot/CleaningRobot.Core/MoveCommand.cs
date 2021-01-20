using System;

namespace CleaningRobot.Core
{
    abstract class MoveCommand : CommandBase
    {
        private readonly int _correction;

        public MoveCommand(int batteryUnits, bool advance) 
            : base(batteryUnits)
        {
            _correction = advance ? 1 : -1;
        }

        public override bool Validate(CleaningRobot robot)
        {
            var moveDirections = GetMoveDirections(robot.Facing);
            var newPosition = new Point(robot.Position.X + moveDirections.X, robot.Position.Y + moveDirections.Y);
            var cellTypeAtNewPosition = GetCellType(robot.Map, newPosition);
            return cellTypeAtNewPosition == CellType.Space;
        }

        protected override void ExecuteCore(CleaningRobot robot)
        {
            var moveDirections = GetMoveDirections(robot.Facing);
            robot.Move(moveDirections.X, moveDirections.Y);
        }

        private Point GetMoveDirections(Facing facing)
        {
            switch (facing)
            {
                case Facing.North:
                    return new Point(0, -1 * _correction);

                case Facing.East:
                    return new Point(1 * _correction, 0);

                case Facing.South:
                    return new Point(0, 1 * _correction);

                case Facing.West:
                    return new Point(-1 * _correction, 0);

                default:
                    throw new ArgumentException($"Unknown facing: {facing}");
            }
        }

        private CellType GetCellType(CellType[,] map, Point position)
        {
            if ((position.X < 0) || (position.Y < 0) ||
                (position.X >= map.GetLength(1)) || (position.Y >= map.GetLength(0)))
            {
                return CellType.Wall; // outside of map
            }

            return map[position.Y, position.X];
        }
    }
}
