using System;
using Xunit;
using Sample.MyLib;

namespace Sample.MyLib.Tests
{
    public class Tests
    {
        private readonly MathLib _sut;

        public Tests() => _sut = new MathLib();

        [Fact]
        public void Add()
        {
            var result = _sut.Add(2, 3);

            Assert.Equal(5, result);
        }

        [Fact]
        public void Sub()
        {
            var result = _sut.Sub(2, 3);

            Assert.Equal(-1, result);
        }

        [Fact]
        public void Mul()
        {
            var result = _sut.Mul(2, 3);

            Assert.Equal(6, result);
        }
    }
}
