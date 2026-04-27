using System.Collections.Generic;

namespace COServer.Game.MsgTournaments
{
    public class KillerSystem
    {
        private Dictionary<uint, int> _kills = new Dictionary<uint, int>();

        public void Update(Client.GameClient killer)
        {
            if (killer == null) return;
            uint uid = killer.Player.UID;
            if (_kills.ContainsKey(uid))
                _kills[uid]++;
            else
                _kills[uid] = 1;
        }

        public void CheckDead(uint uid)
        {
            if (_kills.ContainsKey(uid))
                _kills.Remove(uid);
        }

        public int GetKills(uint uid)
        {
            int count;
            return _kills.TryGetValue(uid, out count) ? count : 0;
        }

        public void Reset()
        {
            _kills.Clear();
        }
    }
}
