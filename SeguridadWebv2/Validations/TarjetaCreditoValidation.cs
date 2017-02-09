using SeguridadWebv2.Models.Aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SeguridadWebv2.Validations
{
    public class TarjetaCreditoValidation
    {
        // MAESTRO/SOLO: (?<Maestro>(?:5018|5020|5038|6304|6759|6761|6763|6334|6767|4903|4905|4911|4936|564182|633110|6333|6759)\\d{8,15})
        // MAESTRO: (?<Maestro>5018)|(?<Maestro>5020)|(?<Maestro>5038)|(?<Maestro>6304)|(?<Maestro>6759)|(?<Maestro>6767)
        // MAESTRO INTERNATIONAL:
        // SOLO: (?<Maestro>6334)|(?<Maestro>6767)|(?<Maestro>4903)|(?<Maestro>4905)|(?<Maestro>4911)|(?<Maestro>4936)|(?<Maestro>564182)|(?<Maestro>633110)|(?<Maestro>6333)|(?<Maestro>6759)
        // VISA DEBIT: (?<VisaDebit>(?:400626|40854749|40940002|41228586|41373337|41378788|418760|41917679|419772|420672|42159294|422793|423769|431072|444001|44400508|44620011|44621354|44625772|44627483|446286|446294|450875|45397|454313|45443235|454742|45672545|46583079|46590150|47511059|47571059|47622069|47634089|48440910|484427|49096079|49218182))
        // VISA CREDIT: (?<Visa>4\\d{3})
        // MASTERCARD CREDIT: (?<MasterCard>5[1-5]\\d{2})
        // MASTERCARD DEBIT: "(?(?<MasterCardDebit>(?:516730|516979|517000|517049|535110|535309|535420|535819|537210|537609|557347|557496|557498|557547))|" +
        // AMEX: (?<Amex>3[47]\\d{2}))([ -]?)(?(DinersClub)(?:\\d{6}\\1\\d{4})|(?(Amex)(?:\\d{6}\\1\\d{5})|(?:\\d{4}\\1\\d{4}\\1\\d{4}))

        /// <summary>
        /// Credit Card Start Code REGEX Check
        /// </summary>
        private const string CardRegex = "^(?:(?<Maestro>(?:5018|5020|5038|6304|6759|6761|6763|6334|6767|4903|4905|4911|4936|564182|633110|6333|6759))|" +
            "(?<VisaDebit>(?:400626|40854749|40940002|41228586|41373337|41378788|418760|41917679|419772|420672|42159294|422793|423769|431072|444001|44400508|44620011|44621354|44625772|44627483|446286|446294|450875|45397879|454313|45443235|454742|45672545|46583079|46590150|47511059|47571059|47622069|47634089|48440910|484427|49096079|49218182|400115|40083739|41292123|417935|419740|419741|41977376|424519|4249623|444000|48440608|48441126|48442855|49173059|49173000|491880))|" +
            "(?<Visa>4\\d{3})|" +
            "(?<MasterCardDebit>(?:516730|516979|517000|517049|535110|535309|535420|535819|537210|537609|557347|557496|557498|557547))|" +
            "(?<MasterCard>5[1-5]\\d{2})|" +
            "(?<Amex>3[47]\\d{2}))([ -]?)(?(DinersClub)(?:\\d{6}\\1\\d{4})|(?(Amex)(?:\\d{6}\\1\\d{5})|(?:(?:\\d{8,15})|(?:\\d{4}\\1\\d{4}\\1\\d{4}))))$";

        /// <summary>
        /// IsValidNumber handles the raw card code without a type
        /// </summary>
        /// <param name="cardNum"></param>
        /// <returns></returns>
        ///
        public static bool IsValidNumber(string cardNum)
        {
            //Determine the card type based on the number
            TipoTarjeta? cardType = GetCardTypeFromNumber(cardNum);

            //Call the base version of IsValidNumber and pass the 
            //number and card type
            return IsValidNumber(cardNum, cardType) ? true : false;
        }
        
        /// <summary>
        /// IsValidNumber handles the raw card code with type
        /// </summary>
        /// <param name="cardNum"></param>
        /// <param name="cardType"></param>
        /// <returns></returns>
        public static bool IsValidNumber(string cardNum, TipoTarjeta? cardType)
        {
            //Create new instance of Regex comparer with our 
            //credit card regex pattern
            var cardTest = new Regex(CardRegex);

            //Make sure the supplied number matches the supplied
            //card type
            if (cardTest.Match(cardNum).Groups[cardType.ToString()].Success)
            {
                //If the card type matches the number, then run it
                //through Luhn's test to make sure the number appears correct
                return PassesLuhnTest(cardNum) ? true : false;
            }
            else
                //The card number does not match the card type
                return false;
        }

        public static bool IsValidCvvCode(string cvvCode, TipoTarjeta? cardType)
        {
            int digits = 0;
            switch (cardType)
            {
                case TipoTarjeta.MasterCard:
                    digits = 3;
                    break;
                case TipoTarjeta.MasterCardDebit:
                    digits = 3;
                    break;
                case TipoTarjeta.VisaDebit:
                    digits = 3;
                    break;
                case TipoTarjeta.Visa:
                    digits = 3;
                    break;
                //case "DISCOVER":
                //case "EUROCARD":
                case TipoTarjeta.Maestro:
                case TipoTarjeta.Amex:
                    digits = 4;
                    break;
                default:
                    return false;
            }
            var regEx = new Regex("[0-9]{" + digits + "}");
            return (cvvCode.Length == digits && regEx.Match(cvvCode).Success);
        }

        public static string[] SplitNameOnCard(string nameOnCard)
        {
            //Nasty
            var returnedSplitNameOnCard = new string[3];
            string[] splitNameOnCard = nameOnCard.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            switch (splitNameOnCard.Length)
            {
                case 1:
                    returnedSplitNameOnCard[0] = splitNameOnCard[0];
                    returnedSplitNameOnCard[1] = splitNameOnCard[0];
                    returnedSplitNameOnCard[2] = splitNameOnCard[0];
                    break;
                case 2:
                    returnedSplitNameOnCard[0] = splitNameOnCard[0];
                    returnedSplitNameOnCard[1] = splitNameOnCard[1];
                    returnedSplitNameOnCard[2] = splitNameOnCard[0];
                    break;
                default:
                    returnedSplitNameOnCard[0] = splitNameOnCard[0];
                    returnedSplitNameOnCard[1] = splitNameOnCard[1];
                    returnedSplitNameOnCard[2] = splitNameOnCard[2];
                    break;
            }
            return returnedSplitNameOnCard;
        }

        public static string PadMMYYString(string mmyyString)
        {
            string[] mmyySplit = mmyyString.Split(new[] { '/' });
            for (int i = 0; i < mmyySplit.Length; i++)
            {
                mmyySplit[i] = string.Format("{0,2}", mmyySplit[i]).Replace(" ", "0");
            }
            return String.Join(null, mmyySplit);
        }

        public static TipoTarjeta? GetCardTypeFromNumber(string cardNum)
        {
            //Create new instance of Regex comparer with our
            //credit card regex pattern
            var cardTest = new Regex(CardRegex);

            //Compare the supplied card number with the regex
            //pattern and get reference regex named groups
            GroupCollection gc = cardTest.Match(cardNum).Groups;

            //Compare each card type to the named groups to 
            //determine which card type the number matches
            if (gc[TipoTarjeta.Amex.ToString()].Success)
            {
                return TipoTarjeta.Amex;
            }
            else if (gc[TipoTarjeta.MasterCardDebit.ToString()].Success)
            {
                return TipoTarjeta.MasterCardDebit;
            }
            else if (gc[TipoTarjeta.MasterCard.ToString()].Success)
            {
                return TipoTarjeta.MasterCard;
            }
            else if (gc[TipoTarjeta.VisaDebit.ToString()].Success)
            {
                return TipoTarjeta.VisaDebit;
            }
            else if (gc[TipoTarjeta.Visa.ToString()].Success)
            {
                return TipoTarjeta.Visa;
            }
            else if (gc[TipoTarjeta.Maestro.ToString()].Success)
            {
                return TipoTarjeta.Maestro;
            }
            else
            {
                //Card type is not supported by our system, return null
                //(You can modify this code to support more (or less)
                // card types as it pertains to your application)
                return null;
            }
        }

        public static TipoTarjeta? GetCardTypeFromCardTypeCode(string cardTypeCode)
        {
            switch (cardTypeCode)
            {
                case "SW":
                case "SD":
                    return TipoTarjeta.Maestro;
                case "MasterCard":
                case "MC":
                    return TipoTarjeta.MasterCard;
                case "MD":
                    return TipoTarjeta.MasterCardDebit;
                case "VISA":
                case "VI":
                    return TipoTarjeta.Visa;
                case "VD":
                    return TipoTarjeta.VisaDebit;
                case "AM":
                    return TipoTarjeta.Amex;
                //case "BC":
                //    return CreditCardTypeType.BarclaysConnect;
                default:
                    //Card type is not supported by our system, return null
                    //(You can modify this code to support more (or less)
                    // card types as it pertains to your application)
                    return null;
            }
        }
        /// <summary>
        /// Debugging Tool
        /// </summary>
        /// <param name="cardType"></param>
        /// <returns></returns>
        public static string GetCardTestNumber(TipoTarjeta cardType)
        {
            //Test Numbers from PayPal
            //for testing card transactions are:
            //Credit Card Type              Credit Card Number
            //*American Express             378282246310005
            //*American Express             371449635398431
            //American Express Corporate    378734493671000
            //Diners Club                   30569309025904
            //Diners Club                   38520000023237
            //Discover                      6011111111111117
            //Discover                      6011000990139424
            //*MasterCard                   5555555555554444
            //*MasterCard                   5105105105105100
            //*Visa Credit                  4111111111111111
            //*Visa Debit                   4012888888881881
            //*Maestro (switch/solo)        6331101999990016
            //Src: https://www.paypal.com/en_GB/vhelp/paypalmanager_help/credit_card_numbers.htm
            //Return bogus CC number that passes Luhn and format tests
            switch (cardType)
            {
                case TipoTarjeta.Amex:
                    return "3782 822463 10005";
                case TipoTarjeta.MasterCard:
                    return "5105 1051 0510 5100";
                case TipoTarjeta.MasterCardDebit:
                    return "5105 1051 0510 5100";
                case TipoTarjeta.Visa:
                    return "4111 1111 1111 1111";
                case TipoTarjeta.VisaDebit:
                    return "4917 4800 0000 0008";
                case TipoTarjeta.Maestro:
                    return "6331 1019 9999 0016";
                default:
                    return null;
            }
        }

        /// <summary>
        /// LUHN Algorithm Card Number Test
        /// If Mod 10 equals 0, the number is good and this will return true
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        private static bool PassesLuhnTest(string cardNumber)
        {
            var deltas = new[] { 0, 1, 2, 3, 4, -4, -3, -2, -1, 0 };
            int checksum = 0;
            char[] chars = new Regex(@"[^\d]").Replace(cardNumber, "").ToCharArray(); //Extract digits
            for (int i = chars.Length - 1; i > -1; i--)
            {
                int j = (chars[i]) - 48;
                checksum += j;
                if (((i - chars.Length) % 2) == 0)
                    checksum += deltas[j];
            }
            return ((checksum % 10) == 0);
        }

        public static bool IsValidCardType(string cardNumber)
        {
            CardType _cardTypes = CardType.All;
            // AMEX -- 34 or 37 -- 15 length

            if ((Regex.IsMatch(cardNumber, "^(34|37)"))
                 && ((_cardTypes & CardType.Amex) != 0))
                return (15 == cardNumber.Length);

            // MasterCard -- 51 through 55 -- 16 length

            else if ((Regex.IsMatch(cardNumber, "^(51|52|53|54|55)")) &&
                      ((_cardTypes & CardType.MasterCard) != 0))
                return (16 == cardNumber.Length);

            // VISA -- 4 -- 13 and 16 length

            else if ((Regex.IsMatch(cardNumber, "^(4)")) &&
                      ((_cardTypes & CardType.VISA) != 0))
                return (13 == cardNumber.Length || 16 == cardNumber.Length);

            // Diners Club -- 300-305, 36 or 38 -- 14 length

            else if ((Regex.IsMatch(cardNumber, "^(300|301|302|303|304|305|36|38)")) &&
                      ((_cardTypes & CardType.DinersClub) != 0))
                return (14 == cardNumber.Length);

            // enRoute -- 2014,2149 -- 15 length

            else if ((Regex.IsMatch(cardNumber, "^(2014|2149)")) &&
                      ((_cardTypes & CardType.DinersClub) != 0))
                return (15 == cardNumber.Length);

            // Discover -- 6011 -- 16 length

            else if ((Regex.IsMatch(cardNumber, "^(6011)")) &&
                     ((_cardTypes & CardType.Discover) != 0))
                return (16 == cardNumber.Length);

            // JCB -- 3 -- 16 length

            else if ((Regex.IsMatch(cardNumber, "^(3)")) &&
                     ((_cardTypes & CardType.JCB) != 0))
                return (16 == cardNumber.Length);

            // JCB -- 2131, 1800 -- 15 length

            else if ((Regex.IsMatch(cardNumber, "^(2131|1800)")) &&
                       ((_cardTypes & CardType.JCB) != 0))
                return (15 == cardNumber.Length);
            else
            {
                // Card type wasn't recognised, provided Unknown is in the 

                // CardTypes property, then return true, otherwise return false.

                if ((_cardTypes & CardType.Unknown) != 0)
                    return true;
                else
                    return false;
            }
        }

        public static string CleanCreditCardNumber(string CreditCardNumber)
        {
            return (Regex.Replace(CreditCardNumber, "\\D", string.Empty));

        }

        public static string ValidateCardNumber(string CardNumber)
        {

            try
            {
                CardNumber = CleanCreditCardNumber(CardNumber);

                string pattern = @"^3(?:0[0-5]|[68][0-9])[0-9]{11}$";
                Regex match = new Regex(pattern);
                bool b = match.IsMatch("CardNumber");

                if (!string.IsNullOrEmpty(CardNumber))
                {

                    //
                    //  Verify the length of the number and its prefix is valid.
                    //

                    string result = CreditCardType.Unknown;

                    // VISA -- 4 -- 13 and 16 length
                    if ((13 == CardNumber.Length || 16 == CardNumber.Length) && (Regex.IsMatch(CardNumber, "^(4)")))
                        result = CreditCardType.Visa;
                    // AMEX -- 34 or 37 -- 15 length
                    else if ((15 == CardNumber.Length) && (Regex.IsMatch(CardNumber, "^(34|37)")))
                        result = CreditCardType.AmericanExpress;

                    // Discover -- 6011,62,64,65 -- 16 length
                    else if ((16 == CardNumber.Length) && (Regex.IsMatch(CardNumber, "^(6011|62|64|65)")))
                        result = CreditCardType.Discovery;

                    // MasterCard -- 51 through 55 -- 16 length
                    else if ((16 == CardNumber.Length) && (Regex.IsMatch(CardNumber, "^(51|52|53|54|55)")))
                        result = CreditCardType.MasterCard;

                    // JCB -- 3 -- 16 length
                    // JCB -- 2131, 1800 -- 15 length
                    else if (((16 == CardNumber.Length) && (Regex.IsMatch(CardNumber, "^(35)"))) ||
                            ((15 == CardNumber.Length) && (Regex.IsMatch(CardNumber, "^(2131|1800)"))))
                        result = CreditCardType.JCB;


                    //
                    //  Run the LUHN (Mod 10) algorithm on the number.
                    //
                    if (result != CreditCardType.Unknown && result != CreditCardType.BillMeLater)
                    {
                        if (!LuhnAlgorithm(CardNumber))
                            result = CreditCardType.Unknown;
                    }

                    return result;
                }
            }
            catch
            {
                throw;
            }

            return CreditCardType.Unknown;
        }

        internal static string GetCyberSourceCardType(string CardNumber)
        {
            try
            {
                switch (ValidateCardNumber(CardNumber))
                {
                    case CreditCardType.Visa:
                        return "001";
                    case CreditCardType.MasterCard:
                        return "002";
                    case CreditCardType.AmericanExpress:
                        return "003";
                    case CreditCardType.Discovery:
                        return "004";
                    case CreditCardType.JCB:
                        return "007";
                    default:
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }

        private static bool LuhnAlgorithm(string CardNumber)
        {
            //  All supported cards are between 13 and 16 digits in length.
            //
            if (CardNumber.Length >= 13 && CardNumber.Length <= 16)
            {
                //  Initialize check sum with check digit value.
                //
                int CheckSum = 0;
                //
                //  Double every other digit starting from the
                //  right of the check digit (the right-most digit)
                //

                for (int i = CardNumber.Length - 2; i >= 0; i -= 2)
                {
                    //  The Luhn algorithm states that we are to double
                    //  the digits and, if this product results in a two digit
                    //  number, add the two digits together before adding this
                    //  result to the running total.
                    //

                    int Product = 2 * (int)Char.GetNumericValue(CardNumber[i]);

                    if (Product < 10)
                    {
                        //  The product is less than 10, which means we can simply
                        //  add it to the running total. This is simply an optimization.
                        //  We could skip this case and just run the "else" clause
                        //  unconditionally and the algorithm would work fine.
                        CheckSum += Product;
                    }
                    else
                    {
                        //  The product is between 10 and 18. In this case we must 
                        //  add the digits together and them add this sum to the
                        //  running total.
                        //
                        CheckSum += (Product % 10) + (Product / 10);
                    }
                }
                //  Add in all the other digits. Note the check digit
                //  is the right-most digit in the number and is the
                //  first digit added in the loop below.
                //
                for (int j = CardNumber.Length - 1; j >= 0; j -= 2)
                {
                    CheckSum += (int)Char.GetNumericValue(CardNumber[j]);
                }
                //  If the check sum has no remainder when divided by 10 then it's considered valid.
                //
                return ((CheckSum % 10) == 0) ? true : false;
            }
            return false;
        }
    }

    [Flags, Serializable]
    public enum CardType
    {
        MasterCard = 0x0001,
        VISA = 0x0002,
        Amex = 0x0004,
        DinersClub = 0x0008,
        enRoute = 0x0010,
        Discover = 0x0020,
        JCB = 0x0040,
        Unknown = 0x0080,
        All = CardType.Amex | CardType.DinersClub |
                         CardType.Discover | CardType.Discover |
                         CardType.enRoute | CardType.JCB |
                         CardType.MasterCard | CardType.VISA
    }
}