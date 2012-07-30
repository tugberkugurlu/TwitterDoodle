using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterDoodle {

    internal static class ICloneableExtensions {

        internal static T Clone<T>(this T value) where T : ICloneable {

            return (T)value.Clone();
        }
    }
}