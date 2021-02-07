using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reactive.Linq;
using System.Collections.Generic;

namespace VoiceMeeterTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestRetrieveText()
        {
            using (var _ = await VoiceMeeter.Remote.Initialize(Voicemeeter.RunVoicemeeterParam.VoicemeeterPotato).ConfigureAwait(false))
            {
                var parameter = "Strip[0].Label";
                var test = VoiceMeeter.Remote.GetTextParameter(parameter);
                await Task.Delay(1000);
                Assert.IsFalse(string.IsNullOrEmpty(test));
            }
        }

        [TestMethod]
        [Ignore]
        public async Task TestSetText()
        {
            using (var _ = await VoiceMeeter.Remote.Initialize(Voicemeeter.RunVoicemeeterParam.VoicemeeterPotato).ConfigureAwait(false))
            {
                var parameter = "Strip[0].Label";
                VoiceMeeter.Remote.SetTextParameter(parameter, "Testing");
                await Task.Delay(1000);
            }
        }

        [TestMethod]
        [Ignore]
        public async Task TestVolume()
        {
            using (var _ = await VoiceMeeter.Remote.Initialize(Voicemeeter.RunVoicemeeterParam.VoicemeeterPotato).ConfigureAwait(false))
            {
                var parameter = "Strip[0].Gain";
                float setValue = 2;

                VoiceMeeter.Remote.SetParameter(parameter, setValue);
                await Task.Delay(5000);
                var result = VoiceMeeter.Remote.GetParameter(parameter);
                await Task.Delay(1000);
                VoiceMeeter.Remote.SetParameter(parameter, 0);

                Assert.AreEqual(setValue, result);
            }
        }

        [TestMethod]
        //[Ignore]
        public async Task TestMute()
        {
            using (var _ = await VoiceMeeter.Remote.Initialize(Voicemeeter.RunVoicemeeterParam.VoicemeeterPotato).ConfigureAwait(false))
            {
                var parameter = "Strip[0].Mute";

                if (VoiceMeeter.Remote.IsParametersDirty() != 0)
                {
                    // Get the current setting
                    var oldMuteSetting = VoiceMeeter.Remote.GetParameter(parameter);

                    // Mute it
                    VoiceMeeter.Remote.SetParameter(parameter, 1);
                    await Task.Delay(1000);

                    if (VoiceMeeter.Remote.IsParametersDirty() != 0)
                    {
                        // Get the test value
                        var mute = VoiceMeeter.Remote.GetParameter(parameter);

                        // Set it back
                        VoiceMeeter.Remote.SetParameter(parameter, oldMuteSetting);
                        await Task.Delay(1000);

                        Assert.AreEqual(mute, 1);
                    }
                }
            }
        }

        [TestMethod]
        [Ignore]
        public async Task TestShutdown()
        {
            using (var _ = await VoiceMeeter.Remote.Initialize(Voicemeeter.RunVoicemeeterParam.VoicemeeterPotato).ConfigureAwait(false))
            {
                VoiceMeeter.Remote.Shutdown();
                await Task.Delay(1000);
            }
        }

        [TestMethod]
        [Ignore]
        public async Task TestShow()
        {
            using (var _ = await VoiceMeeter.Remote.Initialize(Voicemeeter.RunVoicemeeterParam.VoicemeeterPotato).ConfigureAwait(false))
            {
                VoiceMeeter.Remote.Show();
                await Task.Delay(1000);
            }
        }

        [TestMethod]
        [Ignore]
        public async Task TestEject()
        {
            using (var _ = await VoiceMeeter.Remote.Initialize(Voicemeeter.RunVoicemeeterParam.VoicemeeterPotato).ConfigureAwait(false))
            {
                VoiceMeeter.Remote.Eject();
                await Task.Delay(1000);
            }
        }

        [TestMethod]
        [Ignore]
        public async Task TestRestart()
        {
            using (var _ = await VoiceMeeter.Remote.Initialize(Voicemeeter.RunVoicemeeterParam.VoicemeeterPotato).ConfigureAwait(false))
            {
                VoiceMeeter.Remote.Restart();
                await Task.Delay(1000);
            }
        }

        [TestMethod]
        [Ignore]
        public async Task TestLevel()
        {
            using (var _ = await VoiceMeeter.Remote.Initialize(Voicemeeter.RunVoicemeeterParam.VoicemeeterPotato).ConfigureAwait(false))
            {
                var level = VoiceMeeter.Remote.GetLevel(Voicemeeter.LevelType.Output, 0);
                Assert.AreNotEqual(level, 0);
            }
        }


        [TestMethod]
        [Ignore]
        public async Task TestLevelSubscribe()
        {
            using (var _ = await VoiceMeeter.Remote.Initialize(Voicemeeter.RunVoicemeeterParam.VoicemeeterPotato).ConfigureAwait(false))
            {
                var channels = new Voicemeeter.Levels.Channel[] {
                    new Voicemeeter.Levels.Channel {
                        LevelType = Voicemeeter.LevelType.Output,
                        ChannelNumber = 0
                    }
                };
                var results = new List<float>();
                var levels = new Voicemeeter.Levels(channels, 20);
                var subscription = levels.Subscribe(x => results.Add(x[0]));

                await Task.Delay(1000);
                Assert.AreNotEqual(results.Count, 0);
            }
        }
    }
}
