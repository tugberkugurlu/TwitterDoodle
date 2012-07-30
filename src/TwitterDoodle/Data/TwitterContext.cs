using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterDoodle.Data {

    public class TwitterContext : ITwitterContext {

        public ITwitterSet<Tweet> Tweet { get; set; }
        public ITwitterSet<TwitterUser> TwitterUser { get; set; }
    }
}