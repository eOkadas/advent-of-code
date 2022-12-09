using AoC2022.Days1to10;
using NFluent;

namespace Aoc2022Tests;

public class Day9Tests
{
    public class Tail
    {
        [Theory]
        [InlineData(0,0,0,0)]
        [InlineData(-1,0,0,0)]
        [InlineData(1,0,0,0)]
        [InlineData(0,-1,0,0)]
        [InlineData(0,1,0,0)]
        [InlineData(1,1,0,0)]
        public void ShouldNotMoveWhenTouching(int x, int y, int dx, int dy)
        {
            var positionHead = new Coordinates(x, y);
            var positionTail = new Coordinates(dx, dy);
            
            var result = new Day9().ComputeTailMovement(positionHead, positionTail);
            
            Check.That(result).IsEqualTo(new Coordinates(dx, dy));
        }
        
        [Theory]
        [InlineData(2,0,0,0, 1, 0)]
        [InlineData(-2,0,0,0, -1,0)]
        [InlineData(0,-2,0,0, 0, -1)]
        [InlineData(0,2,0,0, 0, 1)]
        public void ShouldMoveLaterallyWhenOnSameLine(int x, int y, int dx, int dy, int expectedX, int expectedY)
        {
            var positionHead = new Coordinates(x, y);
            var positionTail = new Coordinates(dx, dy);
            
            var result = new Day9().ComputeTailMovement(positionHead, positionTail);
            
            Check.That(result).IsEqualTo(new Coordinates(expectedX, expectedY));
        }
    }
}