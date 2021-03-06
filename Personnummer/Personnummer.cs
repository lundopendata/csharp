﻿using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Personnummer
{
    /// <summary>
    /// Class used to verify Swedish social security numbers.
    /// </summary>
    public static class Personnummer
    {
        private static readonly Regex regex;
        private static readonly CultureInfo cultureInfo;

        static Personnummer()
        {
            cultureInfo = new CultureInfo("sv-SE");
            regex       = new Regex(@"(\d{2}){0,1}(\d{2})(\d{2})(\d{2})([-|+]{0,1})?(\d{3})(\d{0,1})");
        }

        /// <summary>
        /// Calculates the checksum value of a given digit-sequence as string by using the luhn/mod10 algorithm.
        /// </summary>
        /// <param name="value">Sequense of digits as a string.</param>
        /// <returns>Resulting checksum value.</returns>
       

        /// <summary>
        /// Function to make sure that the passed year, month and day is parseable to a date.
        /// </summary>
        /// <param name="year">Years as string.</param>
        /// <param name="month">Month as int.</param>
        /// <param name="day">Day as int.</param>
        /// <returns>Result.</returns>
        private static bool TestDate(string year, int month, int day)
        {
            try
            {
                DateTime dt = new DateTime(cultureInfo.Calendar.ToFourDigitYear(int.Parse(year)), month, day);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validate Swedish social security number.
        /// </summary>
        /// <param name="value">Value as string.</param>
        /// <returns>Result.</returns>
        public static bool Valid(string value)
        {
            MatchCollection matches = regex.Matches(value);

            if (matches.Count < 1 || matches[0].Groups.Count < 7)
            {
                return false;
            }

            GroupCollection groups = matches[0].Groups;
            int month, day, check;
            string yStr;
            try
            {
                yStr  = (groups[2].Value.Length == 4) ? groups[2].Value.Substring(2) : groups[2].Value;
                month = int.Parse(groups[3].Value);
                day   = int.Parse(groups[4].Value);
                check = int.Parse(groups[7].Value);
            }
            catch
            {
                // Could not parse. So invalid.
                return false;
            }

            bool valid = Luhn.LuhnCheck(yStr + groups[3].Value + groups[4].Value + groups[6].Value + groups[7].Value) ;
            return valid && (TestDate(yStr, month, day) || TestDate(yStr, month, day - 60));
        }

        /// <summary>
        /// Validate Swedish social security number.
        /// </summary>
        /// <param name="value">Value as long.</param>
        /// <returns>Result.</returns>
        public static bool Valid(long value)
        {
            return Valid(value.ToString());
        }

    }
}
