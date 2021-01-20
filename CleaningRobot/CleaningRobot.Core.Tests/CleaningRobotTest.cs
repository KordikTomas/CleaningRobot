using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CleaningRobot.Core.Tests
{
    [TestClass]
    public class CleaningRobotTest
    {
        [TestMethod]
        public void Ctor_InitialPositionIsVisitedCell_Test()
        {
            var start = new Point(1, 2);
            var target = GetTarget(start);

            Assert.AreEqual(1, target.VisitedCells.Count());
            Assert.AreEqual(start.X, target.VisitedCells.First().X);
            Assert.AreEqual(start.Y, target.VisitedCells.First().Y);
        }

        [TestMethod]
        public void Run_TurnLeftFromWest_Test()
        {
            var start = new Point(0, 0);
            var target = GetTarget(start, facing: Facing.West);
            var commands = new CommandType[]
            {
                CommandType.TurnLeft
            };

            target.Run(commands);

            VerifyState(target, 99, start, Facing.South);
        }

        [TestMethod]
        public void Run_TurnLeftFromNorth_Test()
        {
            var start = new Point(0, 0);
            var target = GetTarget(start);
            var commands = new CommandType[]
            {
                CommandType.TurnLeft
            };

            target.Run(commands);

            VerifyState(target, 99, start, Facing.West);
        }

        [TestMethod]
        public void Run_TurnRightFromWest_Test()
        {
            var start = new Point(0, 0);
            var target = GetTarget(start, facing: Facing.West);
            var commands = new CommandType[]
            {
                CommandType.TurnRight
            };

            target.Run(commands);

            VerifyState(target, 99, start, Facing.North);
        }

        [TestMethod]
        public void Run_TurnRightFromNorth_Test()
        {
            var start = new Point(0, 0);
            var target = GetTarget(start);
            var commands = new CommandType[]
            {
                CommandType.TurnRight
            };

            target.Run(commands);

            VerifyState(target, 99, start, Facing.East);
        }

        [TestMethod]
        public void Run_CleanCell_Test()
        {
            var start = new Point(1, 1);
            var target = GetTarget(start);
            var commands = new CommandType[]
            {
                CommandType.Clean
            };

            target.Run(commands);

            VerifyState(target, 95, start, Facing.North);
            Assert.AreEqual(1, target.CleanedCells.Count());
            var currentCell = target.CleanedCells.First();
            Assert.AreEqual(start.X, currentCell.X);
            Assert.AreEqual(start.Y, currentCell.Y);
        }

        [TestMethod]
        public void Run_AdvanceToNorth_Test()
        {
            Run_Advance_Test(new Point(1, 1), new Point(1, 0), Facing.North);
        }

        [TestMethod]
        public void Run_AdvanceToEast_Test()
        {
            Run_Advance_Test(new Point(0, 1), new Point(1, 1), Facing.East);
        }

        [TestMethod]
        public void Run_AdvanceToSouth_Test()
        {
            Run_Advance_Test(new Point(1, 0), new Point(1, 1), Facing.South);
        }

        [TestMethod]
        public void Run_AdvanceToWest_Test()
        {
            Run_Advance_Test(new Point(1, 1), new Point(0, 1), Facing.West);
        }

        [TestMethod]
        public void Run_BackToNorth_Test()
        {
            Run_Back_Test(new Point(1, 1), new Point(1, 0), Facing.South);
        }

        [TestMethod]
        public void Run_BackToEast_Test()
        {
            Run_Back_Test(new Point(0, 1), new Point(1, 1), Facing.West);
        }

        [TestMethod]
        public void Run_BackToSouth_Test()
        {
            Run_Back_Test(new Point(1, 0), new Point(1, 1), Facing.North);
        }

        [TestMethod]
        public void Run_BackToWest_Test()
        {
            Run_Back_Test(new Point(1, 1), new Point(0, 1), Facing.East);
        }

        [TestMethod]
        public void Run_FirstBackoffStrategy_Test()
        {
            CellType[,] map = new CellType[,]
            {
                { CellType.Space, CellType.Space },
                { CellType.Space, CellType.Space }
            };
            var target = GetTarget(new Point(0, 1), facing: Facing.West, map: map);

            target.Run(new CommandType[] { CommandType.Advance });

            Assert.AreEqual(95, target.Battery);
            Assert.AreEqual(0, target.Position.X);
            Assert.AreEqual(0, target.Position.Y);
            Assert.AreEqual(Facing.North, target.Facing);
            Assert.AreEqual(2, target.VisitedCells.Count());
        }

        [TestMethod]
        public void Run_AllBackoffStrategies_Test()
        {
            CellType[,] map = new CellType[,]
            {
                { CellType.Space, CellType.Space, CellType.Space, CellType.Space }
            };
            var target = GetTarget(new Point(0, 0), facing: Facing.West, map: map);

            target.Run(new CommandType[] { CommandType.Advance });

            Assert.AreEqual(73, target.Battery);
            Assert.AreEqual(2, target.Position.X);
            Assert.AreEqual(0, target.Position.Y);
            Assert.AreEqual(Facing.South, target.Facing);
            Assert.AreEqual(3, target.VisitedCells.Count());
        }

        [TestMethod]
        public void Run_OutOfBattery_Test()
        {
            var target = GetTarget(new Point(1, 0), battery: 10, facing: Facing.East);

            target.Run(new CommandType[] 
            {
                CommandType.Clean,
                CommandType.Advance,
                CommandType.Clean // will not execute due to almost drained battery
            });

            Assert.AreEqual(3, target.Battery);
            Assert.AreEqual(2, target.Position.X);
            Assert.AreEqual(0, target.Position.Y);
            Assert.AreEqual(1, target.CleanedCells.Count());
            var cleanedCell = target.CleanedCells.First();
            Assert.AreEqual(1, cleanedCell.X);
            Assert.AreEqual(0, cleanedCell.Y);
        }

        private void Run_Advance_Test(Point start, Point end, Facing facing)
        {
            Run_Move_Test(start, end, facing, CommandType.Advance, 98);
        }

        private void Run_Back_Test(Point start, Point end, Facing facing)
        {
            Run_Move_Test(start, end, facing, CommandType.Back, 97);
        }

        private void Run_Move_Test(Point start, Point end, Facing facing, 
            CommandType commandType, int battery)
        {
            var target = GetTarget(start, facing: facing);
            var commands = new CommandType[]
            {
                commandType
            };

            target.Run(commands);

            VerifyState(target, battery, end, facing);
            Assert.AreEqual(2, target.VisitedCells.Count());
            Assert.AreEqual(end.X, target.Position.X);
            Assert.AreEqual(end.Y, target.Position.Y);
        }

        [TestMethod]
        public void Run_Example1_Test()
        {
            var start = new Point(3, 0);
            var target = GetTarget(start, 80, Facing.North);
            var commands = new CommandType[]
            {
                CommandType.TurnLeft,
                CommandType.Advance,
                CommandType.Clean,
                CommandType.Advance,
                CommandType.Clean,
                CommandType.TurnRight,
                CommandType.Advance,
                CommandType.Clean
            };

            target.Run(commands);

            VerifyState(target, 54, new Point(2, 0), Facing.East);
            Assert.AreEqual(3, target.VisitedCells.Count());
            VerifyVisitedCell(target, 1, 0);
            VerifyVisitedCell(target, 2, 0);
            VerifyVisitedCell(target, 3, 0);
            Assert.AreEqual(2, target.CleanedCells.Count());
            VerifyCleanedCell(target, 1, 0);
            VerifyCleanedCell(target, 2, 0);
        }

        [TestMethod]
        public void Run_Example2_Test()
        {
            var start = new Point(3, 1);
            var target = GetTarget(start, 1094, Facing.South);
            var commands = new CommandType[]
            {
                CommandType.TurnRight,
                CommandType.Advance,
                CommandType.Clean,
                CommandType.Advance,
                CommandType.Clean,
                CommandType.TurnRight,
                CommandType.Advance,
                CommandType.Clean
            };

            target.Run(commands);

            VerifyState(target, 1040, new Point(3, 2), Facing.East);
            Assert.AreEqual(4, target.VisitedCells.Count());
            VerifyVisitedCell(target, 2, 2);
            VerifyVisitedCell(target, 3, 0);
            VerifyVisitedCell(target, 3, 1);
            VerifyVisitedCell(target, 3, 2);
            Assert.AreEqual(3, target.CleanedCells.Count());
            VerifyCleanedCell(target, 2, 2);
            VerifyCleanedCell(target, 3, 0);
            VerifyCleanedCell(target, 3, 2); 
        }

        private void VerifyVisitedCell(CleaningRobot target, int x, int y)
        {
            foreach (var cell in target.VisitedCells)
            {
                if (cell.X == x && cell.Y == y)
                {
                    return;
                }
            }
            Assert.Fail($"Cell {x}, {y} was not visited.");
        }

        private void VerifyCleanedCell(CleaningRobot target, int x, int y)
        {
            foreach (var cell in target.CleanedCells)
            {
                if (cell.X == x && cell.Y == y)
                {
                    return;
                }
            }
            Assert.Fail($"Cell {x}, {y} was not cleaned.");
        }

        private CleaningRobot GetTarget(Point position, int battery = 100, 
            Facing? facing = null, CellType[,] map = null)
        {
            if (!facing.HasValue)
            {
                facing = Facing.North;
            }
            if (map == null)
            {
                map = new CellType[,]
                {
                { CellType.Space, CellType.Space, CellType.Space, CellType.Space },
                { CellType.Space, CellType.Space, CellType.Column, CellType.Space },
                { CellType.Space, CellType.Space, CellType.Space, CellType.Space },
                { CellType.Space, CellType.Wall, CellType.Space, CellType.Space }
                };
            }
            return new CleaningRobot(battery, map, position, facing.Value);
        }

        private void VerifyState(CleaningRobot target, int battery, Point position, Facing facing)
        {
            Assert.AreEqual(battery, target.Battery);
            Assert.AreEqual(position.X, target.Position.X);
            Assert.AreEqual(position.Y, target.Position.Y);
            Assert.AreEqual(facing, target.Facing);
        }
    }
}
