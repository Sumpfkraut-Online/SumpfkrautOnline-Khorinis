using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Animations
{
    /* Save your animation overlays here like this:
     * 
     * enum Overlays_"BaseMDSName" : byte
     * {
     *    OverlayName1,
     *    OverlayName2,
     *    ...
     * }
     * 
     */


    enum Overlays_HumanS : byte
    {
        HumanS_1hST1,
        HumanS_1hST2,
        HumanS_2hST1,
        HumanS_2hST2,
        HumanS_Acrobatic,
        HumanS_Arrogance,
        HumanS_Babe,
        HumanS_BowT1,
        HumanS_BowT2,
        HumanS_CBowT1,
        HumanS_CBowT2,
        HumanS_Flee,
        HumanS_Mage,
        HumanS_Militia,
        HumanS_Relaxed,
        HumanS_Skeleton,
        HumanS_Skeleton_Fly,
        HumanS_Sprint,
        HumanS_Swim,
        HumanS_Tired,
        HumanS_Torch
    }

    enum Overlays_Orc : byte
    {
        Orc_Torch
    }

    enum Overlays_Scavenger : byte
    {
        Orcbiter
    }

    enum Overlays_Waran : byte
    {
        Firewaran
    }

    enum Overlays_Golem : byte
    {
        Golem_Firegolem,
        Golem_Icegolem
    }
}
