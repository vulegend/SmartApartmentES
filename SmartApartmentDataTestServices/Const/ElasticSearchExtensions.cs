using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestServices.Const
{
    public static class ElasticSearchExtensions
    {
        #region String Extensions

        /// <summary>
        /// Generates multiple query string for markets
        /// </summary>
        /// <param name="marketString"></param>
        /// <returns></returns>
        public static string GenerateMultiQueryString(this string marketString)
        {
            var formatMarketString = marketString.Split(',');
            var formattedMarketString = "";

            if (formatMarketString.Length > 0)
            {
                for (int i = 0; i < formatMarketString.Length; i++)
                {
                    formatMarketString[i] = formatMarketString[i].Insert(0, "(").Insert(formatMarketString[i].Length + 1, ")");
                }

                formattedMarketString = string.Join(" OR ", formatMarketString);
            }
            else
            {
                formattedMarketString = marketString;
            }

            return formattedMarketString;
        }

        #endregion
    }
}
