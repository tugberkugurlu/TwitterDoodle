using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterDoodle.Data {

    public interface ITwitterContext {

        ITwitterSet<Tweet> Tweet { get; set; }
        ITwitterSet<TwitterUser> TwitterUser { get; set; }
    }
}