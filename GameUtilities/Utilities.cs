using System.Collections;
using System.Collections.Immutable;

namespace GameUtilities;


public static class Extensions{
    public static T Shuffle<T>(this T collections) where T : IList{
        // TODO: do real shuffle here
        return collections;
    }
}