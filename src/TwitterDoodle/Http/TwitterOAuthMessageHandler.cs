using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwitterDoodle.Data;
using TwitterDoodle.OAuth;

namespace TwitterDoodle.Http {

    internal class TwitterOAuthMessageHandler : DelegatingHandler {

        private readonly OAuthState _oAuthState;

        public TwitterOAuthMessageHandler(OAuthCredential oAuthCredential, OAuthSignatureEntity signatureEntity,
            TwitterQueryCollection parameters, HttpMessageHandler innerHandler) : base(innerHandler) {

            //TODO: Parameters needs to come here as encoded so that they can be encoded twice
            //      for the signature base. Handle that.

            //TODO: We don't even need to get parameters seperately. We can get them through 
            //      query string and by reading the body but reading the body is a overhead here.

            _oAuthState = new OAuthState() { 
                Credential = new OAuthCredentialState() { 
                    ConsumerKey = oAuthCredential.ConsumerKey,
                    //encode it here first
                    CallbackUrl = OAuthUtil.PercentEncode(oAuthCredential.CallbackUrl),
                    Token = oAuthCredential.Token
                },
                SignatureEntity = new OAuthSignatureEntityState() { 
                    ConsumerSecret = signatureEntity.ConsumerSecret,
                    TokenSecret = signatureEntity.TokenSecret
                },
                Parameters = parameters,
                Nonce = GenerateNonce(),
                SignatureMethod = GetOAuthSignatureMethod(),
                Timestamp = GenerateTimestamp(),
                Version = GetVersion()
            };
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {

            if (_oAuthState.Parameters != null) { 

                //clear the content first. If parameters are passed, 
                //that means the request is FormUrlEncoded
                var query = _oAuthState.Parameters.ToString();
                var httpContent = new StringContent(query);
                httpContent.Headers.ContentType = MediaTypeConstants.ApplicationFormUrlEncodedMediaType;

                request.Content = httpContent;
            }

            //Add the auth header
            request.Headers.Authorization = new AuthenticationHeaderValue(
                "OAuth", GenerateAuthHeader(_oAuthState, request)
            );

            return base.SendAsync(request, cancellationToken);
        }

        //OAuth helper methods

        private string GetVersion() {

            return "1.0";
        }

        private string GetOAuthSignatureMethod() {

            return Constants.HMACSHA1SignatureType;
        }

        private string GenerateTimestamp() {

            TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64(timeSpan.TotalSeconds).ToString(CultureInfo.InvariantCulture);
        }

        private string GenerateNonce() {

            return Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture)));
        }

        private string GenerateSignature(OAuthState oAuthState, HttpRequestMessage request) {

            //https://dev.twitter.com/docs/auth/creating-signature
            //http://garyshortblog.wordpress.com/2011/02/11/a-twitter-oauth-example-in-c/

            //This dictionary will hold the twice-encoded values
            SortedDictionary<string, string> signatureBaseCollection = new SortedDictionary<string, string>();

            //Required for all requests
            signatureBaseCollection.Add(Constants.OAuthConsumerKey, oAuthState.Credential.ConsumerKey);
            signatureBaseCollection.Add(Constants.OAuthNonce, oAuthState.Nonce);
            signatureBaseCollection.Add(Constants.OAuthVersion, oAuthState.Version);
            signatureBaseCollection.Add(Constants.OAuthTimestamp, oAuthState.Timestamp);
            signatureBaseCollection.Add(Constants.OAuthSignatureMethod, oAuthState.SignatureMethod);

            //Parameters
            if (oAuthState.Parameters != null) {

                //these are already encoded. At the string building phase, 
                //they will be encoded one more time.
                oAuthState.Parameters.ForEach(x => signatureBaseCollection.Add(x.Key, x.Value));
            }

            //Optionals
            if (!string.IsNullOrEmpty(oAuthState.Credential.Token))
                signatureBaseCollection.Add(Constants.OAuthToken, oAuthState.Credential.Token);

            //this needs to be encoded twice. So, we leave it as it is to be encode one more time
            if (!string.IsNullOrEmpty(oAuthState.Credential.CallbackUrl))
                signatureBaseCollection.Add(Constants.OAuthCallback, oAuthState.Credential.CallbackUrl);

            //Build the signature
            StringBuilder strBuilder = new StringBuilder();

            //these two ampersand chars needs not to be encoded
            strBuilder.AppendFormat("{0}&", request.Method.Method.ToUpper());
            strBuilder.AppendFormat("{0}&", OAuthUtil.PercentEncode(request.RequestUri.ToString()));

            //encode the values for signature base
            signatureBaseCollection.ForEach(x =>
                strBuilder.Append(
                    OAuthUtil.PercentEncode(string.Format("{0}={1}&", x.Key, x.Value))
                )
            );

            //Remove the trailing ambersand char from the signatureBase.
            //Remember, it's been urlEncoded so you have to remove the
            //last 3 chars - %26
            string baseSignatureString = strBuilder.ToString();
            baseSignatureString = baseSignatureString.Substring(0, baseSignatureString.Length - 3);

            //Build the signing key
            string signingKey = string.Format(
                "{0}&{1}", OAuthUtil.PercentEncode(oAuthState.SignatureEntity.ConsumerSecret),
                string.IsNullOrEmpty(oAuthState.SignatureEntity.TokenSecret) ? "" : OAuthUtil.PercentEncode(oAuthState.SignatureEntity.TokenSecret)
            );

            //Sign the request
            using (HMACSHA1 hashAlgorithm = new HMACSHA1(new ASCIIEncoding().GetBytes(signingKey))) {

                return Convert.ToBase64String(
                    hashAlgorithm.ComputeHash(
                        new ASCIIEncoding().GetBytes(baseSignatureString)
                    )
                );
            }
        }

        private string GenerateAuthHeader(OAuthState oAuthState, HttpRequestMessage request) {

            SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
            sortedDictionary.Add(Constants.OAuthNonce, OAuthUtil.PercentEncode(oAuthState.Nonce));
            sortedDictionary.Add(Constants.OAuthSignatureMethod, OAuthUtil.PercentEncode(oAuthState.SignatureMethod));
            sortedDictionary.Add(Constants.OAuthTimestamp, OAuthUtil.PercentEncode(oAuthState.Timestamp));
            sortedDictionary.Add(Constants.OAuthConsumerKey, OAuthUtil.PercentEncode(oAuthState.Credential.ConsumerKey));
            sortedDictionary.Add(Constants.OAuthVersion, OAuthUtil.PercentEncode(oAuthState.Version));

            if (!string.IsNullOrEmpty(_oAuthState.Credential.Token))
                sortedDictionary.Add(Constants.OAuthToken, OAuthUtil.PercentEncode(oAuthState.Credential.Token));

            //don't encode it here again.
            //we already did that and this auth header field doesn't require it to be encoded twice
            if (!string.IsNullOrEmpty(_oAuthState.Credential.CallbackUrl))
                sortedDictionary.Add(Constants.OAuthCallback, oAuthState.Credential.CallbackUrl);

            StringBuilder strBuilder = new StringBuilder();
            var valueFormat = "{0}=\"{1}\",";

            sortedDictionary.ForEach(x => {
                strBuilder.AppendFormat(valueFormat, x.Key, x.Value);
            });

            //oAuth parameters has to be sorted before sending, but signature has to be at the end of the authorization request
            //http://stackoverflow.com/questions/5591240/acquire-twitter-request-token-failed
            strBuilder.AppendFormat(valueFormat, Constants.OAuthSignature, OAuthUtil.PercentEncode(GenerateSignature(oAuthState, request)));

            return strBuilder.ToString().TrimEnd(',');
        }

        private class OAuthState {

            public OAuthCredentialState Credential { get; set; }
            public OAuthSignatureEntityState SignatureEntity { get; set; }
            public TwitterQueryCollection Parameters { get; set; }
            public string Nonce { get; set; }
            public string Timestamp { get; set; }
            public string Version { get; set; }
            public string SignatureMethod { get; set; }
        }

        private class OAuthCredentialState {

            public string ConsumerKey { get; set; }
            public string Token { get; set; }
            public string CallbackUrl { get; set; }
        }

        private class OAuthSignatureEntityState {

            public string ConsumerSecret { get; set; }
            public string TokenSecret { get; set; }
        }
    }
}