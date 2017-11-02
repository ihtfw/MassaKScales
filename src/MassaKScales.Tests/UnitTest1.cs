﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MassaKScales.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetWeightTest()
        {
            using (var scales = new MassaKScales())
            {
                scales.Connect("COM3");

                Assert.IsTrue(scales.IsConnected);

                var weight = scales.GetWeight();
                Assert.AreEqual(0, weight);
            }
        }
    }
}
