
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Newtonsoft.Json.Linq;
using System.Runtime.Intrinsics;
using System.Text;

namespace Data.Test
{
    [TestClass]
    public sealed class LoggerTests
    {
        [TestMethod]
        public async Task loggerWriteTest()
        {
            string fileName = "test.txt";
            deleteFile(fileName);
            Logger logger = new Logger(fileName);
            var sb = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                string msg = i.ToString();
                await logger.logAsync(msg);
                sb.AppendLine(msg);           
            }
            await Task.Delay(100);
            await logger.DisposeAsync();
            string text = File.ReadAllText(fileName);
            Assert.AreEqual(sb.ToString(), text);
            deleteFile(fileName);
        }

        [TestMethod]
        public async Task loggerWriteTest3()
        {
            string fileName = "test2.txt";
            deleteFile(fileName);
            Logger logger = new Logger(fileName);
            await logger.logAsync("A");
            await logger.logAsync("B");
            await logger.DisposeAsync();
            string text = File.ReadAllText(fileName);
            Assert.Contains("A", text);
            Assert.Contains("B", text);
            File.WriteAllText(fileName, string.Empty);

            Logger logger2 = new Logger(fileName);
            logger2.logAsync("A");
            logger2.logAsync("B");
            logger2.logAsync("C");
            await logger2.DisposeAsync();
            await Task.Delay(100);
            deleteFile(fileName);
        }

        private void deleteFile(string fileName)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);
        }
    }
}
