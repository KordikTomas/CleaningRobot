using System;
using System.Linq;
using System.Collections.Generic;
using CleaningRobot.Core;
using Newtonsoft.Json;

namespace CleaningRobot.Json
{
    public class JsonController
    {
        private readonly Dictionary<string, CellType> _cellTypesMap;
        private readonly Dictionary<string, CommandType> _commandTypesMap;
        private readonly Dictionary<string, Facing> _facingMap;

        public JsonController()
        {
            _cellTypesMap = new Dictionary<string, CellType>
            {
                { "C", CellType.Column },
                { "S", CellType.Space },
                { "null", CellType.Wall }
            };
            _commandTypesMap = new Dictionary<string, CommandType>
            {
                { "TR", CommandType.TurnRight },
                { "TL", CommandType.TurnLeft },
                { "A", CommandType.Advance },
                { "B", CommandType.Back },
                { "C", CommandType.Clean }
            };
            _facingMap = new Dictionary<string, Facing>
            {
                { "N", Facing.North },
                { "E", Facing.East },
                { "S", Facing.South },
                { "W", Facing.West }
            };
        }

        public string Process(string json)
        {
            if (string.IsNullOrEmpty(json)) throw new ArgumentNullException(nameof(json));

            var input = JsonConvert.DeserializeObject<JsonInput>(json);
            var result = Execute(input);
            return JsonConvert.SerializeObject(result);
        }

        public JsonResult Execute(JsonInput input)
        {
            var map = ConvertMap(input.Map);
            var commandTypes = ConvertCommands(input.Commands);
            var facing = ConvertToFacing(input.Start.Facing);
            var start = new Point(input.Start.X, input.Start.Y);

            var robot = new CleaningRobot.Core.CleaningRobot(input.Battery, map, start, facing);
            robot.Run(commandTypes);

            var result = new JsonResult();
            result.Visited = robot.VisitedCells.OrderBy(p => p.X).ThenBy(p => p.Y);
            result.Cleaned = robot.CleanedCells.OrderBy(p => p.X).ThenBy(p => p.Y);
            result.Final = new Position
            {
                X = robot.Position.X,
                Y = robot.Position.Y,
                Facing = ConvertToString(robot.Facing)
            };
            result.Battery = robot.Battery;

            return result;
        }

        private CellType[,] ConvertMap(string[,] map)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));

            CellType[,] result = new CellType[map.GetLength(0), map.GetLength(1)];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    CellType cell;
                    if (_cellTypesMap.TryGetValue(map[i, j], out cell))
                    {
                        result[i, j] = cell;
                    }
                    else
                    {
                        throw new Exception($"Unknown cell type: {map[i, j]}");
                    }
                }
            }

            return result;
        }

        private CommandType[] ConvertCommands(string[] commands)
        {
            if (commands == null) throw new ArgumentNullException(nameof(commands));

            CommandType[] result = new CommandType[commands.Length];

            for (int i = 0; i < commands.Length; i++)
            {
                CommandType commandType;
                if (_commandTypesMap.TryGetValue(commands[i], out commandType))
                {
                    result[i] = commandType;
                }
                else
                {
                    throw new Exception($"Unknown command type: {commands[i]}");
                }
            }

            return result;
        }

        private Facing ConvertToFacing(string facing)
        {
            Facing result;
            if (_facingMap.TryGetValue(facing, out result))
            {
                return result;
            }
            else
            {
                throw new Exception($"Unknown facing: {facing}");
            }
        }

        private string ConvertToString(Facing facing)
        {
            foreach (var kvp in _facingMap)
            {
                if (kvp.Value == facing)
                {
                    return kvp.Key;
                }
            }
            throw new Exception($"Unknown facing: {facing}");
        }
    }
}
