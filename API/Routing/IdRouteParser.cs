using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MTCG.API.Routing
{
    internal class IdRouteParser
    {
        public bool IsMatch(string resourcePath, string routePattern)
        {
            var pattern = "^" + routePattern.Replace("{username}", ".*").Replace("/", "\\/") + "(\\?.*)?$";
            return Regex.IsMatch(resourcePath, pattern);
        }

        public Dictionary<string, string> ParseParameters(string resourcePath, string routePattern)
        {
            // query parameters
            var parameters = ParseQueryParameters(resourcePath);
            if (!parameters.ContainsKey("format"))
            {
                parameters["format"] = null;
            }

            // username parameter
            var username = ParseUsernameParameter(resourcePath, routePattern);
            if (username != null)
            {
                parameters["username"] = username;
            }
            // trading parameter
            var tradingId = ParseTradingIdParameter(resourcePath, routePattern);
            if (tradingId != null)
            {
                parameters["tradingId"] = tradingId;
            }
            // marketDeal parameter
            var marketDealId = ParseMarketDealIdParameter(resourcePath, routePattern);
            if (marketDealId != null)
            {
                parameters["marketDealId"] = marketDealId;
            }

            return parameters;
        }

        private string ParseUsernameParameter(string resourcePath, string routePattern)
        {
            var pattern = "^" + routePattern.Replace("{username}", "(?<username>[^\\?\\/]*)").Replace("/", "\\/") + "$";
            var result = Regex.Match(resourcePath, pattern);
            return result.Groups["username"].Success ? result.Groups["username"].Value : null;
        }
        private string ParseTradingIdParameter(string resourcePath, string routePattern)
        {
            var pattern = "^" + routePattern.Replace("{tradingId}", "(?<tradingId>[^\\?\\/]*)").Replace("/", "\\/") + "$";
            var result = Regex.Match(resourcePath, pattern);
            return result.Groups["tradingId"].Success ? result.Groups["tradingId"].Value : null;
        }
        private string ParseMarketDealIdParameter(string resourcePath, string routePattern)
        {
            var pattern = "^" + routePattern.Replace("{marketDealId}", "(?<marketDealId>[^\\?\\/]*)").Replace("/", "\\/") + "$";
            var result = Regex.Match(resourcePath, pattern);
            return result.Groups["marketDealId"].Success ? result.Groups["marketDealId"].Value : null;
        }

        private Dictionary<string, string> ParseQueryParameters(string route)
        {
            var parameters = new Dictionary<string, string>();

            var query = route
                .Split('?')
                .Skip(1)
                .FirstOrDefault();

            if (query != null)
            {
                var keyValues = query
                    .Split('&')
                    .Select(p => p.Split('='))
                    .Where(p => p.Length == 2);

                foreach (var p in keyValues)
                {
                    parameters[p[0]] = p[1];
                }
            }

            return parameters;
        }
    }
}
