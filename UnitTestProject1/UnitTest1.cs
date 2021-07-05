using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Критический_Путь_Экзамен_Болдин;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        CrithWay crw = new CrithWay();
        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsInstanceOfType(crw.Input(),typeof(List<>));
        }
    }
}
