using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using GUC.Log;
using Gothic.Sound;
using GUC.WorldObjects;
using GUC.Types;
using Gothic.Objects;

namespace GUC
{
    public class SoundDefinition
    {
        public zCSndFX_MSS zSFX { get; private set; }

        string name;
        public string Name { get { return name; } }
        public SoundDefinition(string sfxName)
        {
            this.name = sfxName;
            this.zSFX = zCSndSys_MSS.LoadSoundFX(sfxName);
        }
    }

    public class SoundInstance
    {
        int id;
        internal int ID { get { return this.id; } }

        SoundDefinition def;
        public SoundDefinition Definition { get { return this.def; } }

        internal SoundInstance(SoundDefinition def, int id)
        {
            this.id = id;
            this.def = def;
        }
    }

    public static class SoundHandler
    {
        public enum MusicType
        {
            Normal,
            Threat,
            Fight
        }

        static MusicType currentMusicType = MusicType.Normal;

        public static MusicType CurrentMusicType
        {
            get { return SoundHandler.currentMusicType; }
            set
            {
                if (SoundHandler.currentMusicType == value)
                    return;

                SoundHandler.currentMusicType = value;
                Process.Write(0x6C2D10, 0xB8, (byte)value, 0x00, 0x00, 0x00, 0xC3);

                Logger.Log("SoundHandler: Play music type " + value);
            }
        }

        public static float MasterVolume
        {
            get { return zCSndSys_MSS.GetMasterVolume(); }
            set { zCSndSys_MSS.SetMasterVolume(value); }
        }

        public static SoundInstance PlaySound(SoundDefinition sound, float volume = 1.0f)
        {
            if (sound == null)
                throw new ArgumentNullException("Sound is null!");
            return new SoundInstance(sound, zCSndSys_MSS.PlaySound(sound.zSFX, false, 0, volume, 0));
        }

        struct ActiveSound
        {
            public long startTime;
            public int idPtr;
            public zTSound3DParams sndParams;
            public SoundDefinition sfx;
            public zCVob sndVob;

            public ActiveSound(int id, zTSound3DParams sndParams, SoundDefinition sfx, zCVob sndVob = null)
            {
                this.startTime = GameTime.Ticks;
                this.idPtr = id;
                this.sndParams = sndParams;
                this.sfx = sfx;
                this.sndVob = sndVob;
            }
        }

        static bool CanPlay(SoundDefinition sound)
        {
            /*for (int i = 0; i < vobSounds.Count; i++)
            {
                if (vobSounds[i].sfx == sound || vobSounds[i].sfx.zSFX.Address == sound.zSFX.Address)
                {
                    if (GameTime.Ticks - vobSounds[i].startTime < 50 * TimeSpan.TicksPerMillisecond)
                        return false;
                }
            }

            for (int i = 0; i < locSounds.Count; i++)
            {
                if (locSounds[i].sfx == sound || locSounds[i].sfx.zSFX.Address == sound.zSFX.Address)
                {
                    if (GameTime.Ticks - locSounds[i].startTime < 50 * TimeSpan.TicksPerMillisecond)
                        return false;
                }
            }*/

            return true;
        }

        public static void StopSound(SoundInstance snd)
        {
            zCSndSys_MSS.StopSound(snd.ID);
        }

        static readonly List<ActiveSound> vobSounds = new List<ActiveSound>();
        static readonly List<ActiveSound> locSounds = new List<ActiveSound>();
        static readonly List<zCVob> sndVobs = new List<zCVob>();

        internal static int VobSoundCount { get { return vobSounds.Count; } }
        internal static int PosSoundCount { get { return locSounds.Count; } }

        public static void PlaySound3D(SoundDefinition sound, BaseVob vob, float range = 3000, float volume = 1.0f)
        {
            if (sound == null)
                throw new ArgumentNullException("Sound is null!");
            if (vob == null)
                throw new ArgumentNullException("Vob is null!");

            if (!CanPlay(sound)) // don't play too many sounds at once
                return;

            Logger.Log("PlayVobSound: " + sound.Name);
            var param = zTSound3DParams.Create();
            param.Volume = volume;
            param.Radius = range;
            param.Reverb = 0.15f;
            param.IsAmbient = true;
            int idPtr = Process.Alloc(4).ToInt32();
            Process.Write(idPtr, zCSndSys_MSS.PlaySound3D(sound.zSFX, vob.gVob, 0, param));

            vobSounds.Add(new ActiveSound(idPtr, param, sound));
        }

        public static SoundInstance PlaySound3D(SoundDefinition sound, Vec3f location, float range = 2500, float volume = 1.0f, bool loop = false)
        {
            if (sound == null)
                throw new ArgumentNullException("Sound is null!");

            if (!CanPlay(sound)) // don't play too many sounds at once
                return null;

            Logger.Log("PlayLocSound: " + sound.Name);
            var param = zTSound3DParams.Create();
            param.Volume = volume;
            param.Radius = range;
            param.Reverb = 0.15f;
            param.LoopType = loop ? 1 : 0;
            param.IsAmbient = true;

            zCVob vob;
            if (sndVobs.Count == 0)
            {
                vob = zCVob.Create();
            }
            else
            {
                int index = sndVobs.Count - 1;
                vob = sndVobs[index];
                sndVobs.RemoveAt(index);
            }

            GothicGlobals.Game.GetWorld().AddVob(vob);
            vob.TrafoObjToWorld.Position = location.ToArray();
            vob.SetPositionWorld(location.X, location.Y, location.Z);

            int idPtr = Process.Alloc(4).ToInt32();
            int id = zCSndSys_MSS.PlaySound3D(sound.zSFX, vob, 0, param);
            Process.Write(idPtr, id);

            locSounds.Add(new ActiveSound(idPtr, param, sound, vob));
            return new SoundInstance(sound, id);
        }

        static int frame = 0;
        internal static void Update3DSounds()
        {
            try
            {
                if (frame++ % 3 == 0) // only every third frame
                    return;
                
                for (int i = vobSounds.Count - 1; i >= 0; i--)
                {
                    var tup = vobSounds[i];

                    if (!zCSndSys_MSS.UpdateSound3D(tup.idPtr, tup.sndParams) && !zCSndSys_MSS.IsSoundActive(tup.idPtr))
                    {
                        Process.Free(new IntPtr(tup.idPtr), 4);
                        tup.sndParams.Free();
                        vobSounds.RemoveAt(i);
                    }
                }

                for (int i = locSounds.Count - 1; i >= 0; i--)
                {
                    var tup = locSounds[i];
                    
                    if (!zCSndSys_MSS.UpdateSound3D(tup.idPtr, tup.sndParams) && !zCSndSys_MSS.IsSoundActive(tup.idPtr))
                    {
                        Process.Free(new IntPtr(tup.idPtr), 4);

                        tup.sndVob.RemoveVobFromWorld();
                        sndVobs.Add(tup.sndVob);
                        //tup.Item2.refCtr--;
                        //tup.Item2.Dispose();

                        tup.sndParams.Free();
                        locSounds.RemoveAt(i);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogWarning(e);
            }
        }
    }
}
