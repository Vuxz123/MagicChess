using System.Collections.Generic;
using com.ethnicthv.Inner.Object.Player.Faction;

namespace com.ethnicthv.Inner.Object.Player.Impl
{
    public class FactionImpl: FactionScheme
    {
        public FactionImpl(): base(new List<PlayerAction>())
        {
            // step 1:
            actions.Add(new PlayerActionImpl());
            // step 2:
            actions.Add(new PlayerActionImpl());
            // step 3:
            actions.Add(new PlayerActionImpl());
        }
    }
}