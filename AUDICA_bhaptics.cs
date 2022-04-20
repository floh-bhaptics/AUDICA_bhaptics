using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using HarmonyLib;
using UnityEngine;
using MyBhapticsTactsuit;

namespace AUDICA_bhaptics
{
    public class AUDICA_bhaptics : MelonMod
    {
        public static TactsuitVR tactsuitVr;

        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            tactsuitVr = new TactsuitVR();
            tactsuitVr.PlaybackHaptics("HeartBeat");
        }

        [HarmonyPatch(typeof(Gun), "Fire", new Type[] { typeof(Target), typeof(Vector3), typeof(int), typeof(bool), typeof(bool), typeof(ShootableToy) })]
        public class bhaptics_GunFire
        {
            [HarmonyPostfix]
            public static void Postfix(Gun __instance)
            {
                bool isRight = (__instance.hand == Target.TargetHandType.Right);
                tactsuitVr.Recoil("Pistol", isRight);
            }
        }
    }
}
