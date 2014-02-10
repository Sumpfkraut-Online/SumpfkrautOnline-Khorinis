using System;
using System.Collections.Generic;
using System.Text;
using GMP.Modules;
using WinApi;
using Gothic.mClasses;
using Network;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi.User.Enumeration;
using System.Threading;
using GMP.Helper;
using GMP.Injection.Synch;
using Injection;

namespace CharakterCreatorModule
{
    public class CharakterCreatorModule : StartState
    {
        //Buttons
        Button lFace;
        Button mFace;
        Button rFace;
        Button lFaceMesh;
        Button mFaceMesh;
        Button rFaceMesh;
        Button lBody;
        Button mBody;
        Button rBody;
        Button lBodyMesh;
        Button mBodyMesh;
        Button rBodyMesh;
        Button lWalkMode;
        Button mWalkMode;
        Button rWalkMode;
        Button lVoice;
        Button mVoice;
        Button rVoice;
        Button lBodyMass;
        Button mBodyMass;
        Button rBodyMass;
        Button contiueButton;

        //Anderes
        Module module;
        bool started;
        oCNpc HELPER;
        oCNpc HERO;


        public List<String> HeadMeshes = new List<string> { "HUM_HEAD_BALD", "HUM_HEAD_FATBALD", "HUM_HEAD_THIEF", "HUM_HEAD_PSIONIC", "HUM_HEAD_PONY" };
        public List<String> BodyMeshes = new List<string> { "hum_body_Naked0", "Hum_Body_Babe0" };
        public List<String> BodyMDS = new List<String> { "NON", "Humans_Mage.mds", "Humans_Babe.mds", "HumanS_Militia.mds", "HumanS_Relaxed.mds", "Humans_Tired.mds", "Humans_Arrogance.mds" };
        int bodyMeshID = 0; int headMeshID = 0; int BodyMDSID = 0;
        public ushort texHeadStart = 1; public ushort textHeadEnd = 195; ushort actHeadID = 1;
        public ushort texBodyStart = 1; public ushort texBodyEnd = 20; ushort actBodyID = 1;

        float minBodyMass = 0.1f;
        float maxBodyMass = 10;
        float activeBodyMass = 1;


        public int actVoiceID; public int voiceStart = 0; public int voiceEnd = 17;
        public String[] testVoices = new String[] { "$NOTNOW", "$WEATHER", "$HANDSOFF", "$RUNCOWARD", "$ALARM", "$GUARDS", "$KILLENEMY", "$GOODKILL", "$DIRTYTHIEF" };

        zCWaypoint wp = null;

        public override void Update(Network.Module module)
        {
            Process process = Process.ThisProcess();



            if (!started)
            {
                this.module = module;

                InputHooked.deaktivateFullControl(process);

                LoadingScreen.Show(process);

                //HelperNPC
                zString str = zString.Create(process, "PC_HERO");
                HELPER = oCObjectFactory.GetFactory(process).CreateNPC(zCParser.getParser(process).GetIndex(str));
                str.Dispose();

                HERO = oCNpc.Player(process);
                
                wp = oCGame.Game(process).World.WayNet.GetWaypointByName("START");
                HERO.TrafoObjToWorld.set(3, wp.Position.X);
                HERO.TrafoObjToWorld.set(7, wp.Position.Y);
                HERO.TrafoObjToWorld.set(11, wp.Position.Z);

                

                HELPER.Enable(wp.Position);

                //double[] dir = VectorNormalize(HERO.TrafoObjToWorld.get(2), HERO.TrafoObjToWorld.get(6),
                //    HERO.TrafoObjToWorld.get(10));

                HELPER.TrafoObjToWorld.set(3, wp.Position.X + HERO.TrafoObjToWorld.get(2)*150);
                HELPER.TrafoObjToWorld.set(7, wp.Position.Y + HERO.TrafoObjToWorld.get(6) * 150);
                HELPER.TrafoObjToWorld.set(11, wp.Position.Z + HERO.TrafoObjToWorld.get(10) * 150);

                zVec3 pos = HERO.GetPosition();
                HELPER.SetLookAt(pos);
                pos.Dispose();

                //HELPER.TrafoObjToWorld.set(6, -0.5f);
                HELPER.SetAsPlayer();
                HELPER.setShowVisual(false);
                zCAICamera.Current(process).SetByScript(zCAICamera.getFirstPersonString(process));
                started = true;

                


                //Buttons
                //Continue
                contiueButton = new Button(process, StaticVars.Languages.getValue(Program.PrimLangList, "Continue"));
                contiueButton.setPos(5725, 7123);
                contiueButton.ButtonPressed += new EventHandler<EventArgs>(continueButton);
                contiueButton.Show();

                int left = 150;
                int mid = 600;
                int right = 2160;
                int startpos = 1020; int posPerButton = 780;

                int i = 0;
                lFace = new Button(process, "");
                lFace.setTexture("button_pfeil_l.tga");
                lFace.ButtonPressed += new EventHandler<EventArgs>(buttonHead);
                lFace.setSize(600, 600);
                lFace.setPos(left, startpos + i * posPerButton);
                lFace.Show();

                rFace = new Button(process, "");
                rFace.setTexture("button_pfeil_r.tga");
                rFace.ButtonPressed += new EventHandler<EventArgs>(buttonHead);
                rFace.setSize(600, 600);
                rFace.setPos(right, startpos + i * posPerButton);
                rFace.Show();

                mFace = new Button(process, "Gesicht");
                mFace.setPos(mid, startpos + i * posPerButton);
                mFace.Show();


                i++;

                lFaceMesh = new Button(process, "");
                lFaceMesh.setTexture("button_pfeil_l.tga");
                lFaceMesh.ButtonPressed += new EventHandler<EventArgs>(buttonHeadMesh);
                lFaceMesh.setSize(600, 600);
                lFaceMesh.setPos(left, startpos + i * posPerButton);
                lFaceMesh.Show();

                rFaceMesh = new Button(process, "");
                rFaceMesh.setTexture("button_pfeil_r.tga");
                rFaceMesh.ButtonPressed += new EventHandler<EventArgs>(buttonHeadMesh);
                rFaceMesh.setSize(600, 600);
                rFaceMesh.setPos(right, startpos + i * posPerButton);
                rFaceMesh.Show();

                mFaceMesh = new Button(process, "Gesichtsmesh");
                mFaceMesh.setPos(mid, startpos + i * posPerButton);
                mFaceMesh.Show();


                i++;


                lBody = new Button(process, "");
                lBody.setTexture("button_pfeil_l.tga");
                lBody.ButtonPressed += new EventHandler<EventArgs>(buttonBody);
                lBody.setSize(600, 600);
                lBody.setPos(left, startpos + i * posPerButton);
                lBody.Show();

                rBody = new Button(process, "");
                rBody.setTexture("button_pfeil_r.tga");
                rBody.ButtonPressed += new EventHandler<EventArgs>(buttonBody);
                rBody.setSize(600, 600);
                rBody.setPos(right, startpos + i * posPerButton);
                rBody.Show();

                mBody = new Button(process, "Korper");
                mBody.ButtonPressed += new EventHandler<EventArgs>(toogleBody);
                mBody.setPos(mid, startpos + i * posPerButton);
                mBody.Show();

                i++;


                lBodyMesh = new Button(process, "");
                lBodyMesh.setTexture("button_pfeil_l.tga");
                lBodyMesh.ButtonPressed += new EventHandler<EventArgs>(buttonBodyMesh);
                lBodyMesh.setSize(600, 600);
                lBodyMesh.setPos(left, startpos + i * posPerButton);
                lBodyMesh.Show();

                rBodyMesh = new Button(process, "");
                rBodyMesh.setTexture("button_pfeil_r.tga");
                rBodyMesh.ButtonPressed += new EventHandler<EventArgs>(buttonBodyMesh);
                rBodyMesh.setSize(600, 600);
                rBodyMesh.setPos(right, startpos + i * posPerButton);
                rBodyMesh.Show();

                mBodyMesh = new Button(process, "Korpermesh");
                mBodyMesh.ButtonPressed += new EventHandler<EventArgs>(toogleBody);
                mBodyMesh.setPos(mid, startpos + i * posPerButton);
                mBodyMesh.Show();


                i++;

                lWalkMode = new Button(process, "");
                lWalkMode.setTexture("button_pfeil_l.tga");
                lWalkMode.ButtonPressed += new EventHandler<EventArgs>(WalkMode);
                lWalkMode.setSize(600, 600);
                lWalkMode.Show();
                lWalkMode.setPos(left, startpos + i * posPerButton);

                rWalkMode = new Button(process, "");
                rWalkMode.setTexture("button_pfeil_r.tga");
                rWalkMode.ButtonPressed += new EventHandler<EventArgs>(WalkMode);
                rWalkMode.setSize(600, 600);
                rWalkMode.setPos(right, startpos + i * posPerButton);
                rWalkMode.Show();

                mWalkMode = new Button(process, "Laufweise");
                mWalkMode.ButtonPressed += new EventHandler<EventArgs>(toogleWalkAnim);
                mWalkMode.setPos(mid, startpos + i * posPerButton);
                mWalkMode.Show();



                i++;

                lVoice = new Button(process, "");
                lVoice.setTexture("button_pfeil_l.tga");
                lVoice.ButtonPressed += new EventHandler<EventArgs>(changeVoice);
                lVoice.setSize(600, 600);
                lVoice.Show();
                lVoice.setPos(left, startpos + i * posPerButton);

                rVoice = new Button(process, "");
                rVoice.setTexture("button_pfeil_r.tga");
                rVoice.ButtonPressed += new EventHandler<EventArgs>(changeVoice);
                rVoice.setSize(600, 600);
                rVoice.setPos(right, startpos + i * posPerButton);
                rVoice.Show();

                mVoice = new Button(process, "Stimme");
                mVoice.ButtonPressed += new EventHandler<EventArgs>(hearVoice);
                mVoice.setPos(mid, startpos + i * posPerButton);
                mVoice.Show();

                i++;

                lBodyMass = new Button(process, "");
                lBodyMass.setTexture("button_pfeil_l.tga");
                lBodyMass.ButtonPressed += new EventHandler<EventArgs>(changeMass);
                lBodyMass.setSize(600, 600);
                lBodyMass.Show();
                lBodyMass.setPos(left, startpos + i * posPerButton);

                rBodyMass = new Button(process, "");
                rBodyMass.setTexture("button_pfeil_r.tga");
                rBodyMass.ButtonPressed += new EventHandler<EventArgs>(changeMass);
                rBodyMass.setSize(600, 600);
                rBodyMass.setPos(right, startpos + i * posPerButton);
                rBodyMass.Show();

                mBodyMass = new Button(process, "Masse");
                mBodyMass.setPos(mid, startpos + i * posPerButton);
                mBodyMass.Show();


                String path = System.IO.Path.GetDirectoryName(module.assembly.Location) + "\\conf\\" + "CharakterCreatorModule.conf";
                if (System.IO.File.Exists(path) && HERO.IsHuman() == 1)
                {
                    using (System.IO.StreamReader file = new System.IO.StreamReader(path))
                    {
                        try
                        {
                            HERO.BodyTex = Convert.ToUInt16(file.ReadLine());
                            HERO.BodyVisualString.Set(file.ReadLine());
                            HERO.HeadTex = Convert.ToUInt16(file.ReadLine());
                            HERO.HeadVisualString.Set(file.ReadLine());
                            HERO.Voice = Convert.ToInt32(file.ReadLine());

                            float val = Convert.ToSingle(file.ReadLine());
                            HERO.SetFatness(val);
                            HERO.Fatness = val;

                            HERO.InitModel();
                            HERO.SetHead();

                            String bodyMDS = file.ReadLine().Trim().ToUpper();
                            

                            for (int bID = 0; bID < BodyMDS.Count; bID++)
                            {
                                if (BodyMDS[bID].Trim().ToUpper() == bodyMDS)
                                {
                                    BodyMDSID = bID;
                                    break;
                                }
                            }

                            if (BodyMDSID != 0)
                            {
                                zString zstr = zString.Create(process, BodyMDS[BodyMDSID]);
                                HERO.ApplyOverlay(zstr);
                                zstr.Dispose();
                            }

                                

                        }
                        catch (Exception ex) { }

                    }
                }
                Cursor.ToTop(process);
                LoadingScreen.Hide(process);

                
            }

            


            if (wp != null && HERO != null)
            {
                HERO.TrafoObjToWorld.set(3, wp.Position.X);
                HERO.TrafoObjToWorld.set(7, wp.Position.Y);
                HERO.TrafoObjToWorld.set(11, wp.Position.Z);

                zVec3 pos = HERO.GetPosition();
                HELPER.SetLookAt(pos);
                pos.Dispose();
            }

            if (HERO.IsHuman() == 0)
                Next(module);
        }




        public void changeVoice(object obj, EventArgs args)
        {
            if (obj == lVoice)
            {
                actVoiceID--;
                if (actVoiceID < voiceStart)
                    actVoiceID = voiceEnd;
            }
            else if (obj == rVoice)
            {
                actVoiceID++;
                if (actVoiceID > voiceEnd)
                    actVoiceID = voiceStart;
            }

            HERO.Voice = actVoiceID;
        }

        public void changeMass(object obj, EventArgs args)
        {
            try
            {
                if (obj == lBodyMass)
                {
                    activeBodyMass -= 0.1f;
                    if (activeBodyMass < minBodyMass)
                        activeBodyMass = maxBodyMass;
                }
                else if (obj == rBodyMass)
                {
                    activeBodyMass += 0.1f;
                    if (activeBodyMass > maxBodyMass)
                        activeBodyMass = minBodyMass;
                }

                HERO.Fatness = activeBodyMass;
                HERO.SetFatness(activeBodyMass);
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'M', "Active Body Mass: " + activeBodyMass, 0, "CharakterCreatorModule.cs", 135);
                //HERO.SetFatness(activeBodyMass);
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'M', "Body Mass: " + HERO.Address + " | " + HERO.Fatness, 0, "CharakterCreatorModule.cs", 135);
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(4, 'M', "Failure: " + ex.ToString(), 0, "CharakterCreatorModule.cs", 144);
            }
            // = activeBodyMass;
        }

        public void hearVoice(object obj, EventArgs args)
        {
            Random intRand = new Random();
            int randVal = intRand.Next(0, testVoices.Length);

            Process process = Process.ThisProcess();
            External_Helper.AI_OutputSVM_Overlay(process, HERO, HERO, testVoices[randVal]);
        }



        bool armorEquiped = true;
        oCItem armor = null;
        public void toogleBody(object obj, EventArgs args)
        {
            if (armorEquiped == true)
            {
                armor = HERO.GetEquippedArmor();
                if (armor != null)
                    HERO.UnequipItem(armor);
                armorEquiped = false;
            }
            else
            {
                if (armor != null)
                {
                    HERO.Equip(armor);
                    armor = null;
                    armorEquiped = true;
                }
            }
        }

        public void WalkMode(object obj, EventArgs args)
        {
            if (obj == lWalkMode)
            {
                BodyMDSID--;
                if (BodyMDSID == -1)
                    BodyMDSID = BodyMDS.Count - 1;
            }
            else if (obj == rWalkMode)
            {
                BodyMDSID++;
                if (BodyMDSID == BodyMDS.Count)
                    BodyMDSID = 0;
            }

            RemoveAllOverlays();
            deactivateAnim();
            if (BodyMDSID != 0)
            {
                
                Process process = Process.ThisProcess();
                zString str = zString.Create(process, BodyMDS[BodyMDSID]);
                HERO.ApplyOverlay(str);
                str.Dispose();

                if (anim)
                    activateAnim();
            }
        }

        bool anim = false;
        
        public void toogleWalkAnim(object obj, EventArgs args)
        {
            if (!anim)
            {
                activateAnim();
                anim = true;
            }
            else
            {
                deactivateAnim();

                anim = false;
            }
        }



        public void RemoveAllOverlays()
        {
            Process process = Process.ThisProcess();
            foreach (string overlay in BodyMDS)
            {
                zString str = zString.Create(process, overlay);
                HERO.RemoveOverlay(str);
                str.Dispose();
            }
        }

        public void buttonHead(object obj, EventArgs args)
        {
            if (obj == lFace)
            {
                actHeadID--;
                if (actHeadID < texHeadStart)
                    actHeadID = textHeadEnd;
            }
            else if (obj == rFace)
            {
                actHeadID++;
                if (actHeadID > textHeadEnd)
                    actHeadID = texHeadStart;
            }

            HERO.HeadTex = actHeadID;
            HERO.SetHead();
        }

        public void buttonHeadMesh(object obj, EventArgs args)
        {
            if(obj == lFaceMesh)
            {
                headMeshID--;
                if (headMeshID == -1)
                    headMeshID = HeadMeshes.Count - 1;
            }
            else if (obj == rFaceMesh)
            {
                headMeshID++;
                if (headMeshID == HeadMeshes.Count)
                    headMeshID = 0;
            }

            HERO.HeadVisualString.Set(HeadMeshes[headMeshID]);
            HERO.SetHead();
        }




        public void buttonBodyMesh(object obj, EventArgs args)
        {
            if (obj == lBodyMesh)
            {
                bodyMeshID--;
                if (bodyMeshID == -1)
                    bodyMeshID = BodyMeshes.Count - 1;
            }
            else if (obj == rBodyMesh)
            {
                bodyMeshID++;
                if (bodyMeshID == BodyMeshes.Count)
                    bodyMeshID = 0;
            }
            HERO.BodyVisualString.Set(BodyMeshes[bodyMeshID]);
            HERO.InitModel();
        }


        public void buttonBody(object obj, EventArgs args)
        {
            if (obj == lBody)
            {
                actBodyID--;
                if (actBodyID < texBodyStart)
                    actBodyID = texBodyEnd;
            }
            else if (obj == rBody)
            {
                actBodyID++;
                if (actBodyID > texBodyEnd)
                    actBodyID = texBodyStart;
            }

            HERO.BodyTex = actBodyID;
            HERO.InitModel();
        }

        public void continueButton(object obj, EventArgs args)
        {
            
            Next(module);
        }


        public void activateAnim()
        {
            Process process = Process.ThisProcess();
            zString str = zString.Create(process, "S_WALKL");
            Animation.startAnimEnabled = true;
            HERO.GetModel().StartAni(str, 1);
            
            str.Dispose();
        }

        public void deactivateAnim()
        {
            Process process = Process.ThisProcess();
            zString str = zString.Create(process, "S_WALKL");
            HERO.GetModel().StopAnimation(str);
            str.Dispose();
        }

        public override void Next(Network.Module module)
        {
            Process process = Process.ThisProcess();
            
            HERO.SetAsPlayer();
            oCGame.Game(process).GetSpawnManager().DeleteNPC(HELPER);

            contiueButton.Hide();
            lFace.Hide();
            mFace.Hide();
            rFace.Hide();
            lFaceMesh.Hide();
            mFaceMesh.Hide();
            rFaceMesh.Hide();
            lBody.Hide();
            mBody.Hide();
            rBody.Hide();
            lBodyMesh.Hide();
            mBodyMesh.Hide();
            rBodyMesh.Hide();
            lWalkMode.Hide();
            mWalkMode.Hide();
            rWalkMode.Hide();

            lVoice.Hide();
            mVoice.Hide();
            rVoice.Hide();

            lBodyMass.Hide();
            rBodyMass.Hide();
            mBodyMass.Hide();

            

            if (armor != null)
            {
                HERO.Equip(armor);
            }

            deactivateAnim();
            
            
            //Abspeichern
            if (HERO.IsHuman() == 1)
            {
                String path = System.IO.Path.GetDirectoryName(module.assembly.Location) + "\\conf\\" + "CharakterCreatorModule.conf";
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
                {
                    file.WriteLine(HERO.BodyTex);
                    file.WriteLine(HERO.BodyVisualString);
                    file.WriteLine(HERO.HeadTex);
                    file.WriteLine(HERO.HeadVisualString);
                    file.WriteLine(HERO.Voice);
                    file.WriteLine(HERO.Fatness);
                    file.WriteLine(BodyMDS[BodyMDSID]);
                }
            }


            
            InputHooked.activateFullControl(process);

            base.Next(module);
        }


        double[] VectorNormalize(float a, float b, float c)
        {
            double betrag = VektorBetrag(a, b, c);
            double ergebnis = 1 / betrag;

            return new double[] { ergebnis * a, ergebnis * b, ergebnis * c };
        }

        double VektorBetrag(float a, float b, float c)
        {
            double ergebnis = a * a + b * b + c * c;
            ergebnis = Math.Sqrt(ergebnis);
            return ergebnis;
        }
    }
}
