using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterDoodle.OAuth {

    public class OAuthSignatureEntity {

        private readonly string _consumerSecret;
        private string _tokenSecret;

        public OAuthSignatureEntity(string consumerSecret) {

            _consumerSecret = consumerSecret;
        }

        public OAuthSignatureEntity(string consumerSecret, string tokenSecret) {

            _consumerSecret = consumerSecret;
        }

        public string ConsumerSecret { 

            get {
                return _consumerSecret;
            }
        }

        public string TokenSecret { 

            get {
                return _tokenSecret;
            } 
            set {
                _tokenSecret = value;
            } 
        }
    }
}
