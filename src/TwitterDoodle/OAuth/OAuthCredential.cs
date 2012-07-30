using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterDoodle.OAuth {

    public class OAuthCredential {

        private readonly string _consumerKey;
        private string _token;
        private string _callbackUrl;

        public OAuthCredential(string consumerKey) {

            if (string.IsNullOrEmpty(consumerKey)) {
                throw new ArgumentNullException("consumerKey");
            }

            _consumerKey = consumerKey;
        }

        public string ConsumerKey {

            get {
                return _consumerKey;
            }
        }

        public string Token {

            get {
                return _token;
            }

            set {
                _token = value;
            }
        }

        public string CallbackUrl {

            get {
                return _callbackUrl;
            }

            set {
                _callbackUrl = value;
            }
        }
    }
}