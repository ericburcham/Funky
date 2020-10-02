using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Funky.UnitTests.Memoizers
{
    [TestFixture]
    public class FuncExtensionsTests
    {
        private const int InvocationCount = 2;

        [Test]
        public void FuncMemoizedWithUnExpirableMemoizerShouldOnlyBeCalledOnceFromParallelForLoop()
        {
            // Arrange
            var callCount = 0;
            var callCountLock = new object();

            Func<int, int> getValueSquared = i =>
            {
                lock (callCountLock)
                {
                    callCount++;
                }

                return i * i;
            };

            var memoized = getValueSquared.Memoize();

            // Act
            Parallel.For(0, InvocationCount, Assert2SquaredIs4(memoized));

            // Assert
            callCount.Should().Be(1);
        }

        [Test]
        public void FuncMemoizedWithExpirableMemoizerShouldBeCalledFewerTimesThanCallingThreadCount()
        {
            // Arrange
            var callCount = 0;
            var callCountLock = new object();

            Func<int, int> getValueSquared = i =>
            {
                lock (callCountLock)
                {
                    callCount++;
                }

                return i * i;
            };

            var memoized = getValueSquared.Memoize(true);

            // Act
            Parallel.For(0, InvocationCount, Assert2SquaredIs4(memoized));

            // Assert
            Console.WriteLine(callCount);
            callCount.Should().BeLessThan(InvocationCount);
        }

        [Test]
        public void FuncWith2InputsCanBeMemoized()
        {
            // Arrange
            var callCount = 0;
            var callCountLock = new object();

            Func<int, int, int> getProduct = (i, j) =>
                {
                    lock (callCountLock)
                    {
                        callCount++;
                    }

                    return i * j;
                };

            var memoized = getProduct.Memoize();

            // Act
            Parallel.For(0, InvocationCount, Assert2ToTheSecondIs4(memoized));

            // Assert
            callCount.Should().Be(1);
        }

        [Test]
        public void FuncWith3InputsCanBeMemoized()
        {
            // Arrange
            var callCount = 0;
            var callCountLock = new object();

            Func<int, int, int, int> getProduct = (i, j, k) =>
                {
                    lock (callCountLock)
                    {
                        callCount++;
                    }

                    return i * j * k;
                };

            var memoized = getProduct.Memoize();

            // Act
            Parallel.For(0, InvocationCount, Assert2ToTheThirdIs8(memoized));

            // Assert
            callCount.Should().Be(1);
        }

        [Test]
        public void FuncWith4InputsCanBeMemoized()
        {
            // Arrange
            var callCount = 0;
            var callCountLock = new object();

            Func<int, int, int, int, int> getProduct = (i, j, k, l) =>
                {
                    lock (callCountLock)
                    {
                        callCount++;
                    }

                    return i * j * k * l;
                };

            var memoized = getProduct.Memoize();

            // Act
            Parallel.For(0, InvocationCount, Assert2ToTheFourthIs16(memoized));

            // Assert
            callCount.Should().Be(1);
        }

        [Test]
        public void FuncWith5InputsCanBeMemoized()
        {
            // Arrange
            var callCount = 0;
            var callCountLock = new object();

            Func<int, int, int, int, int, int> getProduct = (i, j, k, l, m) =>
                {
                    lock (callCountLock)
                    {
                        callCount++;
                    }

                    return i * j * k * l * m;
                };

            var memoized = getProduct.Memoize();

            // Act
            Parallel.For(0, InvocationCount, Assert2ToTheFifthIs32(memoized));

            // Assert
            callCount.Should().Be(1);
        }

        [Test]
        public void FuncWith6InputsCanBeMemoized()
        {
            // Arrange
            var callCount = 0;
            var callCountLock = new object();

            Func<int, int, int, int, int, int, int> getProduct = (i, j, k, l, m, n) =>
                {
                    lock (callCountLock)
                    {
                        callCount++;
                    }

                    return i * j * k * l * m * n;
                };

            var memoized = getProduct.Memoize();

            // Act
            Parallel.For(0, InvocationCount, Assert2ToTheSixthIs64(memoized));

            // Assert
            callCount.Should().Be(1);
        }

        [Test]
        public void FuncWith7InputsCanBeMemoized()
        {
            // Arrange
            var callCount = 0;
            var callCountLock = new object();

            Func<int, int, int, int, int, int, int, int> getProduct = (i, j, k, l, m, n, o) =>
                {
                    lock (callCountLock)
                    {
                        callCount++;
                    }

                    return i * j * k * l * m * n * o;
                };

            var memoized = getProduct.Memoize();

            // Act
            Parallel.For(0, InvocationCount, Assert2ToTheSeventhIs128(memoized));

            // Assert
            callCount.Should().Be(1);
        }

        [Test]
        public void FuncWith8InputsCanBeMemoized()
        {
            // Arrange
            var callCount = 0;
            var callCountLock = new object();

            Func<int, int, int, int, int, int, int, int, int> getProduct = (i, j, k, l, m, n, o, p) =>
                {
                    lock (callCountLock)
                    {
                        callCount++;
                    }

                    return i * j * k * l * m * n * o * p;
                };

            var memoized = getProduct.Memoize();

            // Act
            Parallel.For(0, InvocationCount, Assert2ToTheEighthIs256(memoized));

            // Assert
            callCount.Should().Be(1);
        }

        [Test]
        public void FuncWith9InputsCanBeMemoized()
        {
            // Arrange
            var callCount = 0;
            var callCountLock = new object();

            Func<int, int, int, int, int, int, int, int, int, int> getProduct = (i, j, k, l, m, n, o, p, q) =>
                {
                    lock (callCountLock)
                    {
                        callCount++;
                    }

                    return i * j * k * l * m * n * o * p * q;
                };

            var memoized = getProduct.Memoize();

            // Act
            Parallel.For(0, InvocationCount, Assert2ToTheNinthIs512(memoized));

            // Assert
            callCount.Should().Be(1);
        }

        [Test]
        public void FuncWith10InputsCanBeMemoized()
        {
            // Arrange
            var callCount = 0;
            var callCountLock = new object();

            Func<int, int, int, int, int, int, int, int, int, int, int> getProduct = (i, j, k, l, m, n, o, p, q, r) =>
                {
                    lock (callCountLock)
                    {
                        callCount++;
                    }

                    return i * j * k * l * m * n * o * p * q * r;
                };

            var memoized = getProduct.Memoize();

            // Act
            Parallel.For(0, InvocationCount, Assert2ToTheTenthIs1024(memoized));

            // Assert
            callCount.Should().Be(1);
        }

        [Test]
        public void FuncWith11InputsCanBeMemoized()
        {
            // Arrange
            var callCount = 0;
            var callCountLock = new object();

            Func<int, int, int, int, int, int, int, int, int, int, int, int> getProduct = (i, j, k, l, m, n, o, p, q, r, s) =>
            {
                lock (callCountLock)
                {
                    callCount++;
                }

                return i * j * k * l * m * n * o * p * q * r * s;
            };

            var memoized = getProduct.Memoize();

            // Act
            Parallel.For(0, InvocationCount, Assert2ToTheEleventhIs2048(memoized));

            // Assert
            callCount.Should().Be(1);
        }

        [Test]
        public void FuncWith12InputsCanBeMemoized()
        {
            // Arrange
            var callCount = 0;
            var callCountLock = new object();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int> getProduct = (i, j, k, l, m, n, o, p, q, r, s, t) =>
            {
                lock (callCountLock)
                {
                    callCount++;
                }

                return i * j * k * l * m * n * o * p * q * r * s * t;
            };

            var memoized = getProduct.Memoize();

            // Act
            Parallel.For(0, InvocationCount, Assert2ToTheTwelfthIs4096(memoized));

            // Assert
            callCount.Should().Be(1);
        }

        [Test]
        public void FuncWith13InputsCanBeMemoized()
        {
            // Arrange
            var callCount = 0;
            var callCountLock = new object();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int> getProduct = (i, j, k, l, m, n, o, p, q, r, s, t, u) =>
            {
                lock (callCountLock)
                {
                    callCount++;
                }

                return i * j * k * l * m * n * o * p * q * r * s * t * u;
            };

            var memoized = getProduct.Memoize();

            // Act
            Parallel.For(0, InvocationCount, Assert2ToTheThirteenthIs8192(memoized));

            // Assert
            callCount.Should().Be(1);
        }

        [Test]
        public void FuncWith14InputsCanBeMemoized()
        {
            // Arrange
            var callCount = 0;
            var callCountLock = new object();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> getProduct = (i, j, k, l, m, n, o, p, q, r, s, t, u, v) =>
            {
                lock (callCountLock)
                {
                    callCount++;
                }

                return i * j * k * l * m * n * o * p * q * r * s * t * u * v;
            };

            var memoized = getProduct.Memoize();

            // Act
            Parallel.For(0, InvocationCount, Assert2ToTheFourteenthIs16384(memoized));

            // Assert
            callCount.Should().Be(1);
        }

        [Test]
        public void FuncWith15InputsCanBeMemoized()
        {
            // Arrange
            var callCount = 0;
            var callCountLock = new object();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> getProduct = (i, j, k, l, m, n, o, p, q, r, s, t, u, v, w) =>
            {
                lock (callCountLock)
                {
                    callCount++;
                }

                return i * j * k * l * m * n * o * p * q * r * s * t * u * v * w;
            };

            var memoized = getProduct.Memoize();

            // Act
            Parallel.For(0, InvocationCount, Assert2ToTheFifteenthIs32768(memoized));

            // Assert
            callCount.Should().Be(1);
        }

        [Test]
        public void FuncWith16InputsCanBeMemoized()
        {
            // Arrange
            var callCount = 0;
            var callCountLock = new object();

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> getProduct = (i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x) =>
            {
                lock (callCountLock)
                {
                    callCount++;
                }

                return i * j * k * l * m * n * o * p * q * r * s * t * u * v * w * x;
            };

            var memoized = getProduct.Memoize();

            // Act
            Parallel.For(0, InvocationCount, Assert2ToTheSixteenthIs65536(memoized));

            // Assert
            callCount.Should().Be(1);
        }

        private static Action<int> Assert2SquaredIs4(Func<int, int> func)
        {
            return delegate
            {
                var result = func(2);
                result.Should().Be(4);
            };
        }

        private static Action<int> Assert2ToTheSecondIs4(Func<int, int, int> func)
        {
            return delegate
            {
                var result = func(2, 2);
                result.Should().Be(4);
            };
        }

        private static Action<int> Assert2ToTheThirdIs8(Func<int, int, int, int> func)
        {
            return delegate
            {
                var result = func(2, 2, 2);
                result.Should().Be(8);
            };
        }

        private static Action<int> Assert2ToTheFourthIs16(Func<int, int, int, int, int> func)
        {
            return delegate
            {
                var result = func(2, 2, 2, 2);
                result.Should().Be(16);
            };
        }

        private static Action<int> Assert2ToTheFifthIs32(Func<int, int, int, int, int, int> func)
        {
            return delegate
            {
                var result = func(2, 2, 2, 2, 2);
                result.Should().Be(32);
            };
        }

        private static Action<int> Assert2ToTheSixthIs64(Func<int, int, int, int, int, int, int> func)
        {
            return delegate
            {
                var result = func(2, 2, 2, 2, 2, 2);
                result.Should().Be(64);
            };
        }

        private static Action<int> Assert2ToTheSeventhIs128(Func<int, int, int, int, int, int, int, int> func)
        {
            return delegate
            {
                var result = func(2, 2, 2, 2, 2, 2, 2);
                result.Should().Be(128);
            };
        }

        private static Action<int> Assert2ToTheEighthIs256(Func<int, int, int, int, int, int, int, int, int> func)
        {
            return delegate
            {
                var result = func(2, 2, 2, 2, 2, 2, 2, 2);
                result.Should().Be(256);
            };
        }

        private static Action<int> Assert2ToTheNinthIs512(Func<int, int, int, int, int, int, int, int, int, int> func)
        {
            return delegate
            {
                var result = func(2, 2, 2, 2, 2, 2, 2, 2, 2);
                result.Should().Be(512);
            };
        }

        private static Action<int> Assert2ToTheTenthIs1024(Func<int, int, int, int, int, int, int, int, int, int, int> func)
        {
            return delegate
            {
                var result = func(2, 2, 2, 2, 2, 2, 2, 2, 2, 2);
                result.Should().Be(1024);
            };
        }

        private static Action<int> Assert2ToTheEleventhIs2048(Func<int, int, int, int, int, int, int, int, int, int, int, int> func)
        {
            return delegate
            {
                var result = func(2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2);
                result.Should().Be(2048);
            };
        }

        private static Action<int> Assert2ToTheTwelfthIs4096(Func<int, int, int, int, int, int, int, int, int, int, int, int, int> func)
        {
            return delegate
            {
                var result = func(2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2);
                result.Should().Be(4096);
            };
        }

        private static Action<int> Assert2ToTheThirteenthIs8192(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int> func)
        {
            return delegate
            {
                var result = func(2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2);
                result.Should().Be(8192);
            };
        }

        private static Action<int> Assert2ToTheFourteenthIs16384(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func)
        {
            return delegate
            {
                var result = func(2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2);
                result.Should().Be(16384);
            };
        }

        private static Action<int> Assert2ToTheFifteenthIs32768(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func)
        {
            return delegate
            {
                var result = func(2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2);
                result.Should().Be(32768);
            };
        }

        private static Action<int> Assert2ToTheSixteenthIs65536(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func)
        {
            return delegate
            {
                var result = func(2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2);
                result.Should().Be(65536);
            };
        }
    }
}
