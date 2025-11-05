namespace YourCompany.YourProject.Modules.YourFeature.Tests.UnitTests;

public class SampleTest
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var expected = 2;

        // Act
        var actual = 1 + 1;

        // Assert
        Assert.Equal(expected, actual);
    }
}
