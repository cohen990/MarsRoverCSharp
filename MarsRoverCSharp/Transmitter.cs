namespace MarsRoverCSharp;

public static class Transmitter
{
    private record Position(int x, int y, string heading)
    {
        public override string ToString()
        {
            return $"{x} {y} {heading}";
        }
    };

    public static string Send(string instructions)
    {
        var instructionLines = instructions.Trim().Split("\n");

        var numberOfHeaderLines = 1;
        var bundleSize = 2;
        var numberOfInstructionBundles = (instructionLines.Length - numberOfHeaderLines) / bundleSize;
        if ((instructionLines.Length - numberOfHeaderLines) % bundleSize > 0) numberOfInstructionBundles++;
        
        var bundleResults = new string[numberOfInstructionBundles];

        int positionIndex(int bundleNumber) => (bundleNumber * 2) + 1;
        int commandsIndex(int bundleNumber) => (bundleNumber * 2) + 2;
        for (var i = 0; i < numberOfInstructionBundles; i++)
        {
            var position = instructionLines.Length >= positionIndex(i) + 1 ? instructionLines[positionIndex(i)].Trim() : "";
            var commands = instructionLines.Length >= commandsIndex (i) + 1? instructionLines[commandsIndex(i)].Trim() : "";
            bundleResults[i] = ExecuteInstructionBundle(position, commands);
        }

        return string.Join('\n', bundleResults);
    }

    private static string ExecuteInstructionBundle(params string[] instructionBundle)
    {
        var instructionsContainPosition = instructionBundle.Length >= 1;
        var roverPositionLine = instructionsContainPosition ? instructionBundle[0] : "";
        var roverPosition = roverPositionLine.Split(" ");

        return instructionBundle.Length switch
        {
            2 => ExecuteCommands(roverPosition, instructionBundle[1]),
            1 => roverPositionLine,
            _ => ""
        };
    }

    private static string ExecuteCommands(IReadOnlyList<string> rawPosition, string commands)
    {
        var position = new Position(int.Parse(rawPosition[0]), int.Parse(rawPosition[1]), rawPosition[2]);
        
        return commands.Aggregate(position, ExecuteCommand).ToString();
    }

    private static Position ExecuteCommand(Position position, char command)
    {
        Position RotateRight()
        {
            return position.heading switch
            {
                "N" => position with { heading = "E" },
                "E" => position with { heading = "S" },
                "S" => position with { heading = "W" },
                "W" => position with { heading = "N" },
            };
        }

        Position RotateLeft()
        {
            return position.heading switch
            {
                "N" => position with { heading = "W"},
                "E" => position with { heading = "N"},
                "S" => position with { heading = "E"},
                "W" => position with { heading = "S"},
            };
        }

        Position MoveRover()
        {
            return position.heading switch
            {
                "N" => position with { y = position.y + 1 },
                "S" => position with { y = position.y - 1 },
                "E" => position with { x = position.x + 1 },
                "W" => position with { x = position.x - 1 }
            };
        }

        return command switch
        {
            'M' => MoveRover(),
            'R' => RotateRight(),
            'L' => RotateLeft()
        };
    }
}