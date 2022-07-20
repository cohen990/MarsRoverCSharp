using NUnit.Framework;

namespace MarsRoverCSharp.Test;

public class Transmitter_Test
{
    [Test]
    public void Should_do_nothing_when_sent_no_instructions()
    {
        const string instructions = "";
        const string expectedOutput = "";

        var output = Transmitter.Send(instructions);

        Assert.That(output, Is.EqualTo(expectedOutput));
    }

    [Test]
    public void Should_return_a_rovers_position_if_given_no_movements()
    {
        const string instructions = @"
5 5
1 2 N
        ";
        const string expectedOutput = "1 2 N";

        var output = Transmitter.Send(instructions);

        Assert.That(output, Is.EqualTo(expectedOutput));
    }

    [Test]
    public void Should_return_a_different_rovers_position_if_given_no_movements()
    {
        const string instructions = @"
5 5
2 2 S
        ";
        const string expectedOutput = "2 2 S";

        var output = Transmitter.Send(instructions);

        Assert.That(output, Is.EqualTo(expectedOutput));
    }

    [TestCase("1 2 N", "M", "1 3 N")]
    [TestCase("2 2 N", "M", "2 3 N")]
    [TestCase("1 2 N", "R", "1 2 E")]
    [TestCase("2 3 N", "R", "2 3 E")]
    [TestCase("2 3 E", "R", "2 3 S")]
    [TestCase("2 3 S", "R", "2 3 W")]
    [TestCase("2 3 W", "R", "2 3 N")]
    [TestCase("2 3 N", "L", "2 3 W")]
    [TestCase("2 3 W", "L", "2 3 S")]
    [TestCase("2 3 S", "L", "2 3 E")]
    [TestCase("2 3 E", "L", "2 3 N")]
    [TestCase("2 2 S", "M", "2 1 S")]
    [TestCase("2 2 E", "M", "3 2 E")]
    [TestCase("2 2 W", "M", "1 2 W")]
    [TestCase("1 3 N", "MM", "1 5 N")]
    public void Should_move_a_rover(string startingPosition, string command, string expectedOutput)
    {
        var instructions = $"5 5\n{startingPosition}\n{command}";

        var output = Transmitter.Send(instructions);

        Assert.That(output, Is.EqualTo(expectedOutput));
    }

    [Test]
    public void Should_move_one_rover_and_locate_a_second_if_only_one_is_given_commands()
    {
        var instructions = "5 5\n1 2 N\nM\n4 4 E";
        var expectedOutput = "1 3 N\n4 4 E";
            
        var output = Transmitter.Send(instructions);
        
        Assert.That(output, Is.EqualTo(expectedOutput));
    }
    
    [Test]
    public void Should_move_two_rovers()
    {
        var instructions = "5 5\n1 2 N\nM\n4 4 E\nM";
        var expectedOutput = "1 3 N\n5 4 E";
            
        var output = Transmitter.Send(instructions);
        
        Assert.That(output, Is.EqualTo(expectedOutput));
    }
}