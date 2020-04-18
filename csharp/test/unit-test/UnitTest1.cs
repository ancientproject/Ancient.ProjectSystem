using NUnit.Framework;

namespace unit_test
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Threading.Tasks;
    using Ancient.ProjectSystem;

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            var bytes = File.ReadAllBytes("./example-package.zip");
            var pkg = await RunePackage.Unwrap(bytes);


            Assert.AreEqual("terminal", pkg.ID);
            Assert.AreEqual("2.0.0", pkg.Version.ToString());
            Assert.AreEqual(bytes.Length, pkg.Content.ToArray().Length);
        }
    }
}