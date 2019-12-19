using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NyimboProject.Attributes;
using System.ComponentModel.DataAnnotations;
using NyimboProject.Models.Authentication;

namespace UnitTestNyimboProject
{
    [TestClass]
    public class MyUnitTest
    {
        [TestMethod]
        public void Password_Equal_ConfirmPassword_ValidCompareAttribute()
        {
            // Init
            Registration registration = new Registration()
            {
                NickName = "TestName",
                Email = "TestName@gmail.com",
                Password = "0000",
                ConfirmPassword = "0000"
            };

            ValidCompareAttribute attribute = new ValidCompareAttribute("Password", "ConfirmPassword");

            //Act
            bool result = attribute.IsValid(registration);

            //Expectation
            Assert.AreEqual(true, result);

        }

        [TestMethod]
        public void ExtensionsAttribute()
        {
            // Init
            string fileName = "file.mp3";
            string ex = "mp3";

            ExtensionsAttribute attribute = new ExtensionsAttribute(ex);

            //Act
            bool result = attribute.IsValid(fileName);

            //Expectation
            Assert.AreEqual(true, result);
        }
    }
}
