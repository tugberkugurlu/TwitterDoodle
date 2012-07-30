using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterDoodle.Data {

    public interface ITwitterSet<TEntity> where TEntity : ITwitterEntity {
    }
}