using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterDoodle {

    internal static class OAuthUtil {

        //http://stackoverflow.com/questions/10017274/c-sharp-percent-encode-a-to-c3a5-based-on-rfc-5849-oauth-1-0
        public static string PercentEncode(string value) {

            if (string.IsNullOrEmpty(value)) {
                return value;
            }

            var UnreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
            var input = new StringBuilder();

            value.ForEach(symbol => {

                if (UnreservedChars.IndexOf(symbol) != -1) {
                    input.Append(symbol);
                }
                else {
                    byte[] bytes = Encoding.UTF8.GetBytes(symbol.ToString());
                    foreach (byte b in bytes) {
                        input.AppendFormat("%{0:X2}", b);
                    }
                }

            });

            return input.ToString();
        }
    }
}