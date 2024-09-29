using DataFactory.Test;
using FluentAssertions;

namespace Honeywell.Application.Test;

public class ApplicationDbContextTest
{
    [Fact]
    public void Constructor_CreateInMemoryDb_Success()
    {
        var context = ContextFactory.Create();
        context.Database.EnsureCreated();
        context.Should().NotBeNull();
    }
}