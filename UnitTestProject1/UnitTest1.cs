using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Критический_Путь_Экзамен_Болдин;
using Microsoft.Win32;


namespace UnitTestProject1
{

    [TestClass]
    public class UnitTest1
    {
        [STAThread]
        static string Dialog()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "Input.csv";
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV documents (.csv)|*.csv";
            dlg.ShowDialog();
            return dlg.FileName;
        }
        CrithWay Cr = new CrithWay();
        [TestMethod]
        public void TestMethod1()
        {
            var Test = Cr.Input(Dialog());
            Assert.AreEqual(Cr.MaxPoint(Test), 8);
        }
        [TestMethod]
        public void TestMethod2()
        {
            var Test = Cr.Input(Dialog());
            Assert.AreEqual(Cr.MinPoint(Test), 2);
        }
        [TestMethod]
        public void TestMethod3()
        {
            var Test = Cr.Input(Dialog());
            Assert.AreEqual(Cr.LengthWay(Test), 43);
        }
        [TestMethod]
        public void TestMethod4()
        {
            Assert.AreEqual(Cr.s, "");
        }
        [TestMethod]
        public void TestMethod5()
        {
            Assert.IsInstanceOfType(Cr.s, typeof(string));
        }
    }
}
