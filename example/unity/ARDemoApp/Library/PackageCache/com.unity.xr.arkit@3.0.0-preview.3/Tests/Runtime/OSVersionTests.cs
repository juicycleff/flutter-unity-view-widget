using NUnit.Framework;

namespace UnityEngine.XR.ARKit.Tests
{
    [TestFixture]
    class OSVersionTestFixture
    {
        void GreaterThan(int major, int minor, int point)
        {
            Assert.That(new OSVersion(major + 1, minor, point) > new OSVersion(major, minor, point));
            Assert.That(!(new OSVersion(major, minor, point) > new OSVersion(major + 1, minor, point)));

            Assert.That(new OSVersion(major, minor + 1, point) > new OSVersion(major, minor, point));
            Assert.That(!(new OSVersion(major, minor, point) > new OSVersion(major, minor + 1, point)));

            Assert.That(new OSVersion(major, minor, point + 1) > new OSVersion(major, minor, point));
            Assert.That(!(new OSVersion(major, minor, point) > new OSVersion(major, minor, point + 1)));
        }

        void LessThan(int major, int minor, int point)
        {
            Assert.That(new OSVersion(major - 1, minor, point) < new OSVersion(major, minor, point));
            Assert.That(!(new OSVersion(major, minor, point) < new OSVersion(major - 1, minor, point)));

            Assert.That(new OSVersion(major, minor - 1, point) < new OSVersion(major, minor, point));
            Assert.That(!(new OSVersion(major, minor, point) < new OSVersion(major, minor - 1, point)));

            Assert.That(new OSVersion(major, minor, point - 1) < new OSVersion(major, minor, point));
            Assert.That(!(new OSVersion(major, minor, point) < new OSVersion(major, minor, point - 1)));
        }

        void LessThanOrEqualTo(int major, int minor, int point)
        {
            Assert.That(new OSVersion(major, minor, point) <= new OSVersion(major, minor, point));
            LessThan(major, minor, point);
        }

        void GreaterThanOrEqualTo(int major, int minor, int point)
        {
            Assert.That(new OSVersion(major, minor, point) >= new OSVersion(major, minor, point));
            GreaterThan(major, minor, point);
        }

        void EqualTo(int major, int minor, int point)
        {
            Assert.That(new OSVersion(major, minor, point) == new OSVersion(major, minor, point));
            Assert.That(!(new OSVersion(major + 1, minor, point) == new OSVersion(major, minor, point)));
            Assert.That(!(new OSVersion(major, minor + 1, point) == new OSVersion(major, minor, point)));
            Assert.That(!(new OSVersion(major, minor, point + 1) == new OSVersion(major, minor, point)));
        }

        void NotEqualTo(int major, int minor, int point)
        {
            Assert.That(!(new OSVersion(major, minor, point) != new OSVersion(major, minor, point)));
            Assert.That(new OSVersion(major + 1, minor, point) != new OSVersion(major, minor, point));
            Assert.That(new OSVersion(major, minor + 1, point) != new OSVersion(major, minor, point));
            Assert.That(new OSVersion(major, minor, point + 1) != new OSVersion(major, minor, point));
        }

        void TestStringParser(int major, int minor, int point)
        {
            var version = new OSVersion(major, minor, point);
            var versionString = version.ToString();
            Assert.That(OSVersion.Parse(versionString).Equals(version));

            // Add unicode characters
            Assert.That(OSVersion.Parse("iPhone version 中文" + versionString + "عربى -/@!@#$23").Equals(version));
        }

        [Test]
        public void GreaterThan()
        {
            GreaterThan(1, 2, 3);
        }

        [Test]
        public void GreaterThanOrEqualTo()
        {
            GreaterThanOrEqualTo(1, 2, 3);
        }

        [Test]
        public void LessThan()
        {
            LessThan(2, 3, 4);
        }

        [Test]
        public void LessThanOrEqualTo()
        {
            LessThanOrEqualTo(2, 3, 4);
        }

        [Test]
        public void EqualTo()
        {
            EqualTo(1, 2, 3);
        }

        [Test]
        public void NotEqualTo()
        {
            NotEqualTo(1, 2, 3);
        }

        [Test]
        public void StringParser()
        {
            for (int point = 0; point < 10; ++point)
            {
                TestStringParser(0, 1, point);
                TestStringParser(12, 0, point);
                TestStringParser(9, 0, point);
                TestStringParser(0, 99, point);
                TestStringParser(999, 999, point);
                TestStringParser(0, 0, point);
            }

            Assert.That(OSVersion.Parse("iOS 12.2") == new OSVersion(12, 2));
            Assert.That(OSVersion.Parse("iOS 12.3.1") == new OSVersion(12, 3, 1));

            // We do a lot of tests against iOS 12, so let's actually check that.
            Assert.That(OSVersion.Parse("12") >= new OSVersion(12));
            Assert.That(OSVersion.Parse("12.0") >= new OSVersion(12));
            Assert.That(OSVersion.Parse("12.1") >= new OSVersion(12));
            Assert.That(OSVersion.Parse("12.1.1") >= new OSVersion(12));
        }

        [Test]
        public void IgnoresLeadingZeroes()
        {
            Assert.That(OSVersion.Parse("0012.02.004") == new OSVersion(12, 2, 4));
        }

        [Test]
        public void StopsParsingAtFirstInvalidCharacter()
        {
            Assert.That(OSVersion.Parse("12.2 .4") == new OSVersion(12, 2));
        }

        [Test]
        public void HandlesNull()
        {
            Assert.That(OSVersion.Parse(null) == new OSVersion(0));
        }

        [Test]
        public void HandlesEmptyString()
        {
            Assert.That(OSVersion.Parse("") == new OSVersion(0));
        }
    }
}
