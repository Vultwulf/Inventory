using Inventory.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inventory.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestInvalidAlphaCreditCardNumber()
        {
            string creditCardNumber = "AAA";

            Assert.IsFalse(PaymentController.IsValidCardNumber(creditCardNumber));
        }

        [TestMethod]
        public void TestInvalidNonAlphaCreditCardNumber()
        {
            string creditCardNumber = "#$%";

            Assert.IsFalse(PaymentController.IsValidCardNumber(creditCardNumber));
        }

        [TestMethod]
        public void TestInvalidEmptyCreditCardNumber()
        {
            string creditCardNumber = "";

            Assert.IsFalse(PaymentController.IsValidCardNumber(creditCardNumber));
        }

        [TestMethod]
        public void TestInvalidNullCreditCardNumber()
        {
            string creditCardNumber = null;

            Assert.IsFalse(PaymentController.IsValidCardNumber(creditCardNumber));
        }

        [TestMethod]
        public void TestInvalidNumberStringCreditCardNumber()
        {
            string creditCardNumber = "555";

            Assert.IsFalse(PaymentController.IsValidCardNumber(creditCardNumber));
        }

        [TestMethod]
        public void TestValidCreditCardNumber()
        {
            // Use a sample valid visa credit card number
            string creditCardNumber = "5105105105105100";

            Assert.IsTrue(PaymentController.IsValidCardNumber(creditCardNumber));
        }
    }
}
