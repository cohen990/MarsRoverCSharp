using NUnit.Framework;

namespace MarsRoverCSharp.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Should_move_a_rover_according_to_instructions()
    {
        const string instructions = @"
5 5
1 2 N
LMLMLMLMM
3 3 E
MMRMMRMRRM
        ";

        const string expectedOutput = "1 3 N\n5 1 E";

        var output = Transmitter.Send(instructions);

        Assert.That(output, Is.EqualTo(expectedOutput));
    }
}