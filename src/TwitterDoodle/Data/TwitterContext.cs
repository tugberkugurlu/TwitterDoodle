using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterDoodle.Data {

    public class TwitterContext {

        public TwitterContext() {

            Tweet = new TweetSet();
        }

        public TweetSet Tweet { get; set; }
    }
}