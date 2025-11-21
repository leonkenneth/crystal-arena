namespace CrystalArena.Tests.Unit
{
    using Xunit;

    public class CardTypeFacts
    {
        [Fact]
        public void Is()
        {
            var type = new CardType("backup monster");
            Assert.True(type.Is("backup"));
            Assert.True(type.Is("monster"));
        }
    }
}
