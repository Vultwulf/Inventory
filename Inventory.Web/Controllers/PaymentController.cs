using System.Linq;
using System.Web.Mvc;

namespace Inventory.Web.Controllers
{
    public class PaymentController : Controller
    {
        /// <summary>
        /// Representation of the external payment gateway interface
        /// </summary>
        /// <param name="cardNumber">The credit card number</param>
        /// <param name="amount">The amount to be charged to the credit card</param>
        /// <returns>Boolean creditCardNumber if the credit card is valid</returns>
        public static bool ChargePayment(string creditCardNumber, decimal amount)
        {
            return true;
        }

        /// <summary>
        /// Determines if a credit card is wellformed
        /// </summary>
        /// <param name="creditCardNumber">The credit card number</param>
        /// <returns>Boolean value if the credit card is valid</returns>
        public static bool IsValidCardNumber(string creditCardNumber)
        {
            if (string.IsNullOrEmpty(creditCardNumber))
            {
                // Credit card can't be null or empty
                return false;
            }

            creditCardNumber = creditCardNumber.Replace(" ", "");

            if (!creditCardNumber.All(char.IsDigit))
            {
                // This contains letters
                return false;
            }

            //FIRST STEP: Double each digit starting from the right
            int[] doubledDigits = new int[creditCardNumber.Length / 2];
            int k = 0;
            for (int i = creditCardNumber.Length - 2; i >= 0; i -= 2)
            {
                int digit = int.Parse(creditCardNumber[i].ToString());
                doubledDigits[k] = digit * 2;
                k++;
            }

            //SECOND STEP: Add up separate digits
            int total = 0;
            foreach (int i in doubledDigits)
            {
                string number = i.ToString();
                for (int j = 0; j < number.Length; j++)
                {
                    total += int.Parse(number[j].ToString());
                }
            }

            //THIRD STEP: Add up other digits
            int total2 = 0;
            for (int i = creditCardNumber.Length - 1; i >= 0; i -= 2)
            {
                int digit = int.Parse(creditCardNumber[i].ToString());
                total2 += digit;
            }

            //FOURTH STEP: Total
            int final = total + total2;

            return final % 10 == 0; //Well formed will divide evenly by 10
        }
    }
}
