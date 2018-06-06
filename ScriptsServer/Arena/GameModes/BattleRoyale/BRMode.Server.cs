using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Arena.GameModes.BattleRoyale
{
    partial class BRMode
    {
        public void Join(ArenaClient client)
        {
            if (Phase != GamePhase.WarmUp)
                return;

            players.Add(client);

            client.GMClass = BRScenario.StartClass;
            SpawnCharacter(client, Randomizer.Get(Scenario.Spawnpoints));
        }

        protected override void Start(GameScenario scenario)
        {
            base.Start(scenario);

            BRWorldLoader.Load(World);
        }
    }
}
