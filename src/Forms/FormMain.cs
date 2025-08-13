using static NUBTool.Common.Generic;
using static NUBTool.Common;
using static NUBTool.Common.NUB;
using static NUBTool.Common.Utils;
using NUBTool.Localizable;
using NUBTool.src.Forms;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace NUBTool
{
    public partial class FormMain : Form
    {
        #region NetworkCommon
        private static readonly HttpClientHandler handler = new()
        {
            UseProxy = false,
            UseCookies = false
        };
        private static readonly HttpClient appUpdatechecker = new(handler);
        #endregion

        static FormSplash? fs;
        static object? lockobj;

        private static FormMain _formMainInstance = null!;
        public static FormMain FormMainInstance
        {
            get
            {
                return _formMainInstance;
            }
            set
            {
                _formMainInstance = value;
            }
        }

        public bool Meta
        {
            get
            {
                SetMetaLabels();
                return true;
            }
        }

        public FormMain()
        {
            InitializeComponent();
            comboBox_Format.SelectedIndex = 0;
            comboBox_Samplerate.SelectedIndex = 0;
            comboBox_Volume.SelectedIndex = 1;
            comboBox_Channels.SelectedIndex = 1;
            comboBox_version.SelectedIndex = 0;
        }

        FormAudio formAudio;

        private void FormMain_Load(object sender, EventArgs e)
        {
            FileVersionInfo ver = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
            Text = "NUBTool";

            if (Directory.Exists(Directory.GetCurrentDirectory() + @"\_temp\"))
            {
                Utils.DeleteDirectoryFiles(Directory.GetCurrentDirectory() + @"\_temp\");
            }
            if (Directory.Exists(Directory.GetCurrentDirectory() + @"\_tempAudio"))
            {
                Utils.DeleteDirectoryFiles(Directory.GetCurrentDirectory() + @"\_tempAudio");
            }

            if (!File.Exists(xmlpath))
            {
                InitConfig();
            }

            Config.Load(Common.xmlpath);

            if (File.Exists(Directory.GetCurrentDirectory() + @"\updated.dat"))
            {
                TopMost = true;
                TopMost = false;
            }

            FormMainInstance = this;

            if (!bool.Parse(Config.Entry["HideSplash"].Value)) // Splash
            {
                lockobj = new object();

                lock (lockobj)
                {
                    ThreadStart tds = new(StartThread);
                    Thread thread = new(tds)
                    {
                        Name = "Splash",
                        IsBackground = true
                    };
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();

                    Dmes d = new(ShowMessage);
                    fs?.Invoke(d, Localization.InitializationCaption);
                    Thread.Sleep(1000);
                    foreach (var files in Directory.GetFiles(Directory.GetCurrentDirectory() + @"\res", "*", SearchOption.AllDirectories))
                    {
                        FileInfo fi = new(files);
                        if (fs != null)
                        {
                            fs.Invoke(d, string.Format(Localization.SplashFormFileCaption, fi.Name));
                            Thread.Sleep(50);
                        }
                    }

                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\_temp");
                    ResetControl();

                    fs?.Invoke(d, Localization.SplashFormConfigCaption);

                    if (bool.Parse(Config.Entry["LoopWarning"].Value))
                    {
                        Generic.IsLoopWarning = true;
                    }
                    else
                    {
                        Generic.IsLoopWarning = false;
                    }

                    try
                    {
                        if (bool.Parse(Config.Entry["Check_Update"].Value))
                        {
                            fs?.Invoke(d, Localization.SplashFormUpdateCaption);
                            Thread.Sleep(500);
                            if (File.Exists(Directory.GetCurrentDirectory() + @"\updated.dat"))
                            {
                                fs?.Invoke(d, Localization.SplashFormUpdatingCaption);
                                File.Delete(Directory.GetCurrentDirectory() + @"\updated.dat");
                                string updpath = Directory.GetCurrentDirectory()[..Directory.GetCurrentDirectory().LastIndexOf('\\')];
                                DirectoryInfo di = new(updpath + @"\updater-temp");
                                Common.Utils.RemoveReadonlyAttribute(di);
                                File.Delete(updpath + @"\updater.exe");
                                File.Delete(updpath + @"\nubtool.zip");
                                Common.Utils.DeleteDirectory(updpath + @"\updater-temp");

                                fs?.Invoke(d, Localization.SplashFormUpdatedCaption);
                                MessageBox.Show(fs, Localization.UpdateCompletedCaption, Localization.MSGBoxSuccessCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                var update = Task.Run(() => CheckForUpdatesForInit());
                                update.Wait();
                            }
                        }
                        else
                        {
                            fs?.Invoke(d, "Skip Update");
                            Thread.Sleep(500);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(fs, "An error occured.\n" + ex, Localization.MSGBoxWarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    fs?.Invoke(d, "Starting...");
                    Thread.Sleep(1000);
                }

                CloseSplash();
            }
            else // No Splash
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\_temp");
                ResetControl();

                if (bool.Parse(Config.Entry["LoopWarning"].Value))
                {
                    Generic.IsLoopWarning = true;
                }
                else
                {
                    Generic.IsLoopWarning = false;
                }

                if (bool.Parse(Config.Entry["Check_Update"].Value))
                {
                    try
                    {
                        if (File.Exists(Directory.GetCurrentDirectory() + @"\updated.dat"))
                        {
                            File.Delete(Directory.GetCurrentDirectory() + @"\updated.dat");
                            string updpath = Directory.GetCurrentDirectory()[..Directory.GetCurrentDirectory().LastIndexOf('\\')];
                            DirectoryInfo di = new(updpath + @"\updater-temp");
                            Common.Utils.RemoveReadonlyAttribute(di);
                            File.Delete(updpath + @"\updater.exe");
                            File.Delete(updpath + @"\nubtool.zip");
                            Common.Utils.DeleteDirectory(updpath + @"\updater-temp");

                            MessageBox.Show(this, Localization.UpdateCompletedCaption, Localization.MSGBoxSuccessCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            var update = Task.Run(() => CheckForUpdatesForInit());
                            update.Wait();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(fs, "An error occured.\n" + ex, Localization.MSGBoxWarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

            }

            if (ver.FileVersion != null)
            {
                Text = "NUBTool ( build: " + ver.FileVersion.ToString() + "-Beta )";
            }
            else
            {
                Text = "NUBTool";
            }

            //panel_Main.BackgroundImage = Resources.SIEv2;
            Activate();

            if (Generic.GlobalException is not null)
            {
                MessageBox.Show(this, string.Format(Localization.UnExpectedCaption, Generic.GlobalException), Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (formAudio is not null && formAudio.Visible == true)
            {
                formAudio.Close();
            }
            DeleteDirectory(TempPath);
            DeleteDirectory(AudioTempPath);
        }

        private void InitializeSubControls()
        {
            ByteOrderBE = true;
            Signed = true;
            button_Run.Enabled = true;
            checkBox_LoopEnable.Enabled = true;
            comboBox_Format.Enabled = true;
            comboBox_Samplerate.Enabled = true;
            comboBox_Volume.Enabled = true;
            comboBox_Channels.Enabled = true;
            comboBox_version.Enabled = true;
            textBox_id.Enabled = true;
            textBox_id.Text = "0";
            textBox_nos.Enabled = true;
            textBox_nos.Text = "1";
            label_NoInfo.Visible = false;
            label_id.Visible = true;
            label_NStream.Visible = true;
            label_Filename.Visible = true;
            label_OrigFilepath.Visible = true;
            label_Totalstream.Visible = true;
            label_lsPosition.Visible = true;
            label_lePosition.Visible = true;
            label_Samplerate_Info.Visible = true;
            label_Channel_Info.Visible = true;
            label_Volume_Info.Visible = true;
            label_Format.Enabled = true;
            label_id_c.Enabled = true;
            label_id_hex.Enabled = true;
            label_Samplerate.Enabled = true;
            label_nos.Enabled = true;
            label_nos_hex.Enabled = true;
            label_Volume.Enabled = true;
            label_Channel.Enabled = true;
            label_version.Enabled = true;
            formAudio = new(this)
            {
                TopLevel = false
            };
            label_Openhere.Visible = false;
            panel_Main.Controls.Add(formAudio);
            formAudio.Show();
        }

        private void ResetControl()
        {
            AllowDrop = true;
            if (Generic.IsATWCancelled)
            {
                Utils.ATWCheck(Generic.IsATW, true);
                Generic.IsATWCancelled = false;
            }
            else
            {
                Utils.ATWCheck(Generic.IsATW);
                Utils.ConvertedNUBCheck(IsNTW);
            }
            Generic.IsWave = false;
            Generic.IsNUB = false;
            Generic.IsNUBLooped = false;
            Common.Generic.OpenFilePaths = null!;
            Common.Generic.OriginalPaths = null!;
            Common.Generic.SavePath = null!;
            Common.Generic.FolderSavePath = null!;
            Common.Generic.ProcessFlag = -1;
            Common.Generic.ProgressMax = -1;

            FileSizes = 0;
            IsEnableLoop = false;
            VolumeNG = false;
            LoopNG = false;
            IdNG = false;
            NoSNG = false;
            LoopStartSamples = 0;
            LoopEndSamples = 0;

            button_Run.Enabled = false;
            checkBox_LoopEnable.Enabled = false;
            checkBox_LoopEnable.Checked = false;
            comboBox_Format.Enabled = false;
            comboBox_Samplerate.Enabled = false;
            comboBox_Channels.Enabled = false;
            comboBox_Volume.Enabled = false;
            comboBox_version.Enabled = false;
            comboBox_Format.SelectedIndex = 0;
            comboBox_Samplerate.SelectedIndex = 0;
            comboBox_Volume.SelectedIndex = 1;
            comboBox_Channels.SelectedIndex = 1;
            comboBox_version.SelectedIndex = 0;
            textBox_id.Enabled = false;
            textBox_id.Text = "0";
            textBox_nos.Enabled = false;
            textBox_nos.Text = "1";
            textBox_CustomVol.Enabled = false;
            textBox_CustomVol.Text = "16512";
            textBox_LoopStart.Enabled = false;
            textBox_LoopEnd.Text = null;
            textBox_LoopEnd.Enabled = false;
            textBox_LoopStart.Text = null;
            label_NoInfo.Visible = true;
            label_id.Visible = false;
            label_NStream.Visible = false;
            label_Filename.Visible = false;
            label_OrigFilepath.Visible = false;
            label_Totalstream.Visible = false;
            label_lsPosition.Visible = false;
            label_lePosition.Visible = false;
            label_Volume_Info.Visible = false;
            label_Samplerate_Info.Visible = false;
            label_Channel_Info.Visible = false;
            label_Openhere.Visible = true;
            label_Format.Enabled = false;
            label_id_c.Enabled = false;
            label_id_hex.Enabled = false;
            label_id_hex.ForeColor = Color.Black;
            label_id_hex.Text = "0x0";
            label_Samplerate.Enabled = false;
            label_nos.Enabled = false;
            label_nos_hex.Enabled = false;
            label_nos_hex.ForeColor = Color.Black;
            label_nos_hex.Text = "0x1";
            label_Volume.Enabled = false;
            label_CustomVol.Enabled = false;
            label_CustomVolHex.Enabled = false;
            label_CustomVolHex.ForeColor = Color.Black;
            label_CustomVolHex.Text = Localization.HexValueCaption + ": 0x4080";
            label_LS.Enabled = false;
            label_LE.Enabled = false;
            label_Channel.Enabled = false;
            label_version.Enabled = false;
            closeCToolStripMenuItem.Enabled = false;

            MultipleNuSoundVersion = [];
            MultipleNumberOfStreams = [];
            MultipleStreamId = [];
            MultipleChannels = [];
            MultipleFilesLoopOKFlags = [];
            MultipleLoopStarts = [];
            MultipleLoopEnds = [];
            MultipleSampleRate = [];
            MultipleVolume = [];
        }

        public void SetMetaLabels()
        {
            if (IsNUB && OpenFilePaths.Length == 1)
            {
                int[] loopbuf = new int[2];
                if (GetNUBLooped(NUBMetadataBuffers, loopbuf))
                {
                    IsNUBLooped = true;
                    checkBox_LoopEnable.Checked = true;
                    FormAudio.FormAudioInstance.BufferLoopPosition = loopbuf;
                }
                else
                {
                    IsNUBLooped = false;
                }
                int lst = NUBMetadataBuffers[2] * 4, let = NUBMetadataBuffers[3] * 4;
                label_Filename.Text = "FILE_NAME: " + GetFileName(OriginalPaths[0]);
                label_OrigFilepath.Text = "FILE_PATH: " + OriginalPaths[0];
                label_id.Text = "STREAM_ID: " + NUBMetadataBuffers[7].ToString() + " (0x" + DecToHex(NUBMetadataBuffers[7]).ToString("X") + ")";
                label_NStream.Text = "NUMBER_OF_STREAMS: " + NUBMetadataBuffers[8].ToString() + " (0x" + DecToHex(NUBMetadataBuffers[8]).ToString("X") + ")";
                label_Totalstream.Text = "(meta) STREAM_TOTAL: " + NUBMetadataBuffers[1].ToString() + " (FILE_SIZE: " + GetFileSize(OpenFilePaths[0]).ToString() + " )";
                label_lsPosition.Text = "(meta) LOOP_START: " + NUBMetadataBuffers[2].ToString() + " (Dec Binary: " + lst.ToString() + ")";
                label_lePosition.Text = "(meta) LOOP_END: " + NUBMetadataBuffers[3].ToString() + " (Dec Binary: " + let.ToString() + ")";
                label_Volume_Info.Text = "(meta) VOLUME: " + NUBMetadataBuffers[4].ToString() + " (0x" + DecToHex(NUBMetadataBuffers[4]).ToString("X") + ")";
                label_Channel_Info.Text = "(meta) CHANNELS: " + NUBMetadataBuffers[5].ToString();
                label_Samplerate_Info.Text = "(meta) SAMPLE_LATES: " + NUBMetadataBuffers[6].ToString() + "Hz";
            }
            else if (IsNUB && IsAudioStreamingReloaded || IsNUB && OpenFilePaths.Length > 1)
            {
                uint pos = FormAudio.FormAudioInstance.ButtonPosition;
                var data = NUBMultiMetadataBuffers[(int)pos - 1];
                int[] loopbuf = new int[2];
                if (GetNUBLooped(data, loopbuf))
                {
                    IsNUBLooped = true;
                    checkBox_LoopEnable.Checked = true;
                    FormAudio.FormAudioInstance.BufferLoopPosition = loopbuf;
                }
                else
                {
                    IsNUBLooped = false;
                }
                int lst = data[2] * 4, let = data[3] * 4;
                label_Filename.Text = "FILE_NAME: " + GetFileName(OriginalPaths[pos - 1]);
                label_OrigFilepath.Text = "FILE_PATH: " + OriginalPaths[pos - 1];
                label_id.Text = "STREAM_ID: " + data[7].ToString() + " (0x" + DecToHex(data[7]).ToString("X") + ")";
                label_NStream.Text = "NUMBER_OF_STREAMS: " + data[8].ToString() + " (0x" + DecToHex(data[8]).ToString("X") + ")";
                label_Totalstream.Text = "(meta) STREAM_TOTAL: " + data[1].ToString() + " (FILE_SIZE: " + GetFileSize(OpenFilePaths[pos - 1]).ToString() + " )";
                label_lsPosition.Text = "(meta) LOOP_START: " + data[2].ToString() + " (Dec Binary: " + lst.ToString() + ")";
                label_lePosition.Text = "(meta) LOOP_END: " + data[3].ToString() + " (Dec Binary: " + let.ToString() + ")";
                label_Volume_Info.Text = "(meta) VOLUME: " + data[4].ToString() + " (0x" + DecToHex(data[4]).ToString("X") + ")";
                label_Channel_Info.Text = "(meta) CHANNELS: " + data[5].ToString();
                label_Samplerate_Info.Text = "(meta) SAMPLE_LATES: " + data[6].ToString() + "Hz";
            }
            else if (!IsNUB && IsAudioStreamingReloaded && OpenFilePaths.Length > 1)
            {
                uint pos = FormAudio.FormAudioInstance.ButtonPosition;
                label_Filename.Text = "FILE_NAME: " + GetFileName(OriginalPaths[pos - 1]);
                label_OrigFilepath.Text = "FILE_PATH: " + OriginalPaths[pos - 1];
                label_id.Text = "STREAM_ID: -";
                label_NStream.Text = "NUMBER_OF_STREAMS: -";
                label_Totalstream.Text = "STREAM_TOTAL: " + GetStreamTotals(OpenFilePaths[pos - 1]).ToString() + " (FILE_SIZE: " + GetFileSize(OpenFilePaths[pos - 1]).ToString() + " )";
                label_lsPosition.Text = "LOOP_START: -";
                label_lePosition.Text = "LOOP_END: -";
                label_Volume_Info.Text = "VOLUME: -";
                label_Channel_Info.Text = "CHANNELS: -";
                label_Samplerate_Info.Text = "SAMPLE_LATES: -";
            }
            else
            {
                label_Filename.Text = "FILE_NAME: " + GetFileName(OriginalPaths[0]);
                label_OrigFilepath.Text = "FILE_PATH: " + OriginalPaths[0];
                label_id.Text = "STREAM_ID: -";
                label_NStream.Text = "NUMBER_OF_STREAMS: -";
                label_Totalstream.Text = "STREAM_TOTAL: " + GetStreamTotals(OpenFilePaths[0]).ToString() + " (FILE_SIZE: " + GetFileSize(OpenFilePaths[0]).ToString() + " )";
                label_lsPosition.Text = "LOOP_START: -";
                label_lePosition.Text = "LOOP_END: -";
                label_Volume_Info.Text = "VOLUME: -";
                label_Channel_Info.Text = "CHANNELS: -";
                label_Samplerate_Info.Text = "SAMPLE_LATES: -";
            }
        }

        private static void DeleteCurrentFiles(string[] files)
        {
            DeleteDirectoryFiles(TempPath);
            DeleteDirectoryFiles(AudioTempPath);
        }

        private void CheckBox_LoopEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (!IsOpenMulti) // Single
            {
                if (checkBox_LoopEnable.Checked != true)
                {
                    if (IsLoopWarning && MultipleFilesLoopOKFlags[0] && button_Run.Enabled)
                    {
                        DialogResult dr = MessageBox.Show(Localization.LoopingAlreadySetWarningCaption, Localization.MSGBoxWarningCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.No)
                        {
                            checkBox_LoopEnable.Checked = true;
                            return;
                        }
                    }

                    FormAudio.FormAudioInstance.LoopStartLabel = string.Empty;
                    FormAudio.FormAudioInstance.LoopEndLabel = string.Empty;
                    MultipleFilesLoopOKFlags[0] = false;
                    MultipleLoopStarts[0] = 0;
                    MultipleLoopEnds[0] = 0;
                    formAudio.customTrackBar_LoopStart.Enabled = false;
                    formAudio.customTrackBar_LoopEnd.Enabled = false;
                    formAudio.numericUpDown_LoopStart.Enabled = false;
                    formAudio.numericUpDown_LoopEnd.Enabled = false;
                    formAudio.button_LS_Current.Enabled = false;
                    formAudio.button_LE_Current.Enabled = false;
                    formAudio.button_SetEnd.Enabled = false;
                    formAudio.button_SetStart.Enabled = false;
                    formAudio.label_StartTweak.Enabled = false;
                    formAudio.label_EndTweak.Enabled = false;
                    IsEnableLoop = false;
                    textBox_LoopStart.Enabled = false;
                    textBox_LoopEnd.Text = null;
                    textBox_LoopEnd.Enabled = false;
                    textBox_LoopStart.Text = null;
                    label_LS.Enabled = false;
                    label_LE.Enabled = false;
                    LoopNG = false;
                }
                else
                {
                    formAudio.customTrackBar_LoopStart.Enabled = true;
                    formAudio.customTrackBar_LoopEnd.Enabled = true;
                    formAudio.numericUpDown_LoopStart.Enabled = true;
                    formAudio.numericUpDown_LoopEnd.Enabled = true;
                    formAudio.button_LS_Current.Enabled = true;
                    formAudio.button_LE_Current.Enabled = true;
                    formAudio.button_SetEnd.Enabled = true;
                    formAudio.button_SetStart.Enabled = true;
                    formAudio.label_StartTweak.Enabled = true;
                    formAudio.label_EndTweak.Enabled = true;
                    IsEnableLoop = true;
                    textBox_LoopStart.Enabled = true;
                    textBox_LoopEnd.Enabled = true;
                    label_LS.Enabled = true;
                    label_LE.Enabled = true;
                    LoopNG = true;
                }
            }
            else // multi
            {
                if (checkBox_LoopEnable.Checked != true)
                {
                    if (IsLoopWarning && MultipleFilesLoopOKFlags[FormAudio.FormAudioInstance.ButtonPosition - 1] && button_Run.Enabled)
                    {
                        DialogResult dr = MessageBox.Show(Localization.LoopingAlreadySetWarningCaption, Localization.MSGBoxWarningCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.No)
                        {
                            checkBox_LoopEnable.Checked = true;
                            return;
                        }
                    }

                    FormAudio.FormAudioInstance.LoopStartLabel = string.Empty;
                    FormAudio.FormAudioInstance.LoopEndLabel = string.Empty;
                    MultipleFilesLoopOKFlags[FormAudio.FormAudioInstance.ButtonPosition - 1] = false;
                    MultipleLoopStarts[FormAudio.FormAudioInstance.ButtonPosition - 1] = 0;
                    MultipleLoopEnds[FormAudio.FormAudioInstance.ButtonPosition - 1] = 0;
                    formAudio.customTrackBar_LoopStart.Enabled = false;
                    formAudio.customTrackBar_LoopEnd.Enabled = false;
                    formAudio.numericUpDown_LoopStart.Enabled = false;
                    formAudio.numericUpDown_LoopEnd.Enabled = false;
                    formAudio.button_LS_Current.Enabled = false;
                    formAudio.button_LE_Current.Enabled = false;
                    formAudio.button_SetEnd.Enabled = false;
                    formAudio.button_SetStart.Enabled = false;
                    formAudio.label_StartTweak.Enabled = false;
                    formAudio.label_EndTweak.Enabled = false;
                    IsEnableLoop = false;
                    textBox_LoopStart.Enabled = false;
                    textBox_LoopEnd.Text = null;
                    textBox_LoopEnd.Enabled = false;
                    textBox_LoopStart.Text = null;
                    label_LS.Enabled = false;
                    label_LE.Enabled = false;
                    LoopNG = false;
                }
                else
                {
                    formAudio.customTrackBar_LoopStart.Enabled = true;
                    formAudio.customTrackBar_LoopEnd.Enabled = true;
                    formAudio.numericUpDown_LoopStart.Enabled = true;
                    formAudio.numericUpDown_LoopEnd.Enabled = true;
                    formAudio.button_LS_Current.Enabled = true;
                    formAudio.button_LE_Current.Enabled = true;
                    formAudio.button_SetEnd.Enabled = true;
                    formAudio.button_SetStart.Enabled = true;
                    formAudio.label_StartTweak.Enabled = true;
                    formAudio.label_EndTweak.Enabled = true;
                    IsEnableLoop = true;
                    textBox_LoopStart.Enabled = true;
                    textBox_LoopEnd.Enabled = true;
                    label_LS.Enabled = true;
                    label_LE.Enabled = true;
                    LoopNG = true;
                }
                /*if (IsAudioStreamingReloaded)
                {
                    if (checkBox_LoopEnable.Checked != true)
                    {
                        textBox_LoopStart.Enabled = false;
                        textBox_LoopEnd.Text = null;
                        textBox_LoopEnd.Enabled = false;
                        textBox_LoopStart.Text = null;
                        label_LS.Enabled = false;
                        label_LE.Enabled = false;
                    }
                    else
                    {
                        textBox_LoopStart.Enabled = true;
                        textBox_LoopEnd.Enabled = true;
                        label_LS.Enabled = true;
                        label_LE.Enabled = true;
                    }
                    return;
                }
                else
                {
                    if (checkBox_LoopEnable.Checked != true)
                    {
                        if (IsLoopWarning && MultipleFilesLoopOKFlags[FormAudio.FormAudioInstance.ButtonPosition - 1] && button_Run.Enabled)
                        {
                            DialogResult dr = MessageBox.Show(Localization.LoopingAlreadySetWarningCaption, Localization.MSGBoxWarningCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (dr == DialogResult.No)
                            {
                                checkBox_LoopEnable.Checked = true;
                                return;
                            }
                        }

                        FormAudio.FormAudioInstance.LoopStartLabel = string.Empty;
                        FormAudio.FormAudioInstance.LoopEndLabel = string.Empty;
                        MultipleFilesLoopOKFlags[FormAudio.FormAudioInstance.ButtonPosition - 1] = false;
                        MultipleLoopStarts[FormAudio.FormAudioInstance.ButtonPosition - 1] = 0;
                        MultipleLoopEnds[FormAudio.FormAudioInstance.ButtonPosition - 1] = 0;
                        formAudio.customTrackBar_LoopStart.Enabled = false;
                        formAudio.customTrackBar_LoopEnd.Enabled = false;
                        formAudio.numericUpDown_LoopStart.Enabled = false;
                        formAudio.numericUpDown_LoopEnd.Enabled = false;
                        formAudio.button_LS_Current.Enabled = false;
                        formAudio.button_LE_Current.Enabled = false;
                        formAudio.button_SetEnd.Enabled = false;
                        formAudio.button_SetStart.Enabled = false;
                        IsEnableLoop = false;
                        textBox_LoopStart.Enabled = false;
                        textBox_LoopEnd.Text = null;
                        textBox_LoopEnd.Enabled = false;
                        textBox_LoopStart.Text = null;
                        label_LS.Enabled = false;
                        label_LE.Enabled = false;
                        LoopNG = false;
                    }
                    else
                    {
                        formAudio.customTrackBar_LoopStart.Enabled = true;
                        formAudio.customTrackBar_LoopEnd.Enabled = true;
                        formAudio.numericUpDown_LoopStart.Enabled = true;
                        formAudio.numericUpDown_LoopEnd.Enabled = true;
                        formAudio.button_LS_Current.Enabled = true;
                        formAudio.button_LE_Current.Enabled = true;
                        formAudio.button_SetEnd.Enabled = true;
                        formAudio.button_SetStart.Enabled = true;
                        IsEnableLoop = true;
                        textBox_LoopStart.Enabled = true;
                        textBox_LoopEnd.Enabled = true;
                        label_LS.Enabled = true;
                        label_LE.Enabled = true;
                        LoopNG = true;
                    }
                }*/


            }
        }

        private void Button_Run_Click(object sender, EventArgs e)
        {
            if (OpenFilePaths.Length == 1) // Single
            {
                if (IdNG || NoSNG)
                {
                    if (IdNG && !NoSNG)
                    {
                        MessageBox.Show(Localization.StreamIDValueErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (!IdNG && NoSNG)
                    {
                        MessageBox.Show(Localization.NumberOfStreamValueErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        MessageBox.Show(Localization.StreamIDAndNOSValueErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                /*if (LoopNG || VolumeNG)
                {
                    if (LoopNG && !VolumeNG)
                    {
                        MessageBox.Show(Localization.LoopNotSetCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (!LoopNG && VolumeNG)
                    {
                        MessageBox.Show(Localization.VolumeValueErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        MessageBox.Show(Localization.LoopAndVolumeValueErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }*/
                if (checkBox_LoopEnable.Checked == true)
                {
                    if (LoopStartNG || LoopEndNG)
                    {
                        if (VolumeNG)
                        {
                            MessageBox.Show(Localization.LoopAndVolumeValueErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show(Localization.LoopNotSetCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        
                        return;
                    }
                    else if (VolumeNG)
                    {
                        MessageBox.Show(Localization.VolumeValueErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    /*else if (!LoopStartNG || !LoopEndNG && !VolumeNG)
                    {
                        MessageBox.Show(Localization.LoopAndVolumeValueErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }*/

                    if (string.IsNullOrWhiteSpace(textBox_LoopStart.Text) || string.IsNullOrWhiteSpace(textBox_LoopEnd.Text))
                    {
                        MessageBox.Show(Localization.LoopNotSetCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    SaveFileDialog sfd = new()
                    {
                        Filter = "Namco nuSound / nuSound2 (*.nub)|*.nub",
                        OverwritePrompt = true,
                    };
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        if (formAudio.Visible == true)
                        {
                            formAudio.Close();
                        }

                        Common.Generic.ProgressMax = Common.Generic.OpenFilePaths.Length;
                        Common.Generic.ProcessFlag = 1;
                        SavePath = sfd.FileName;

                        Form formProgress = new FormProgress();
                        formProgress.ShowDialog();
                        formProgress.Dispose();

                        if (Common.Generic.Result == false || Generic.cts.IsCancellationRequested)
                        {
                            Common.Generic.cts.Dispose();
                            MessageBox.Show(this, Localization.CancelledCaption, Localization.MSGBoxAbortedCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ResetControl();
                            return;
                        }

                        if (File.Exists(sfd.FileName))
                        {
                            MessageBox.Show(Localization.SingleEncodeSuccessCaption, Localization.MSGBoxSuccessCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (formAudio.Visible == true)
                            {
                                formAudio.Close();
                            }
                            IsNUB = false;

                            closeCToolStripMenuItem.Enabled = false;
                            DeleteCurrentFiles(OpenFilePaths);
                            OpenFilePaths = null!;
                            ResetControl();
                            Utils.ShowFolder(sfd.FileName, bool.Parse(Config.Entry["ShowFolder"].Value));
                            return;
                        }
                        else
                        {
                            MessageBox.Show(string.Format(Localization.UnExpectedCaption, ExceptionInformation), Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (formAudio.Visible == true)
                            {
                                formAudio.Close();
                            }
                            IsNUB = false;

                            closeCToolStripMenuItem.Enabled = false;
                            DeleteCurrentFiles(OpenFilePaths);
                            OpenFilePaths = null!;
                            ResetControl();
                            return;
                        }

                        
                    }
                    else
                    {
                        return;
                    }
                }
                else // Loop Disabled
                {
                    SaveFileDialog sfd = new()
                    {
                        Filter = "Namco nuSound / nuSound2 (*.nub)|*.nub",
                        OverwritePrompt = true,
                    };
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        if (formAudio.Visible == true)
                        {
                            formAudio.Close();
                        }

                        Common.Generic.ProgressMax = Common.Generic.OpenFilePaths.Length;
                        Common.Generic.ProcessFlag = 1;
                        SavePath = sfd.FileName;

                        Form formProgress = new FormProgress();
                        formProgress.ShowDialog();
                        formProgress.Dispose();

                        if (Common.Generic.Result == false || Generic.cts.IsCancellationRequested)
                        {
                            Common.Generic.cts.Dispose();
                            MessageBox.Show(this, Localization.CancelledCaption, Localization.MSGBoxAbortedCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ResetControl();
                            return;
                        }

                        if (File.Exists(sfd.FileName))
                        {
                            MessageBox.Show(Localization.SingleEncodeSuccessCaption, Localization.MSGBoxSuccessCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (formAudio.Visible == true)
                            {
                                formAudio.Close();
                            }
                            IsNUB = false;

                            closeCToolStripMenuItem.Enabled = false;
                            DeleteCurrentFiles(OpenFilePaths);
                            OpenFilePaths = null!;
                            ResetControl();
                            Utils.ShowFolder(sfd.FileName, bool.Parse(Config.Entry["ShowFolder"].Value));
                            return;
                        }
                        else
                        {
                            MessageBox.Show(string.Format(Localization.UnExpectedCaption, ExceptionInformation), Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (formAudio.Visible == true)
                            {
                                formAudio.Close();
                            }
                            IsNUB = false;

                            closeCToolStripMenuItem.Enabled = false;
                            DeleteCurrentFiles(OpenFilePaths);
                            OpenFilePaths = null!;
                            ResetControl();
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else // multiple
            {
                if (IdNG || NoSNG)
                {
                    if (IdNG && !NoSNG)
                    {
                        MessageBox.Show(Localization.StreamIDValueErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (!IdNG && NoSNG)
                    {
                        MessageBox.Show(Localization.NumberOfStreamValueErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        MessageBox.Show(Localization.StreamIDAndNOSValueErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                int mLoopCount = 0;
                foreach (var loops in MultipleFilesLoopOKFlags)
                {
                    FileInfo mFi = new(OriginalPaths[mLoopCount]);

                    if (loops)
                    {
                        if (string.IsNullOrWhiteSpace(MultipleLoopStarts[mLoopCount].ToString()) || string.IsNullOrWhiteSpace(MultipleLoopEnds[mLoopCount].ToString()))
                        {
                            MessageBox.Show(string.Format(Localization.MultipleFileLoopPointNotsetCaption, mFi.Name), Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else if (MultipleLoopStarts[mLoopCount] == 0 || MultipleLoopEnds[mLoopCount] == 0)
                        {
                            MessageBox.Show(string.Format(Localization.MultipleFileLoopPointNotsetCaption, mFi.Name), Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            mLoopCount++;
                            continue;
                        }
                    }
                    else
                    {
                        mLoopCount++;
                        continue;
                    }
                }

                FolderBrowserDialog fbd = new()
                {
                    Description = Localization.SaveMultiNUBDescription,
                    RootFolder = Environment.SpecialFolder.MyDocuments,
                    ShowNewFolderButton = true,
                    SelectedPath = @""
                };
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    if (Directory.GetFiles(fbd.SelectedPath, "*", SearchOption.AllDirectories).Length != 0)
                    {
                        DialogResult dr = MessageBox.Show(this, Localization.AlreadyExistsCaption, Localization.MSGBoxWarningCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.Yes)
                        {
                            Common.Utils.DeleteDirectoryFiles(fbd.SelectedPath);
                        }
                        else
                        {
                            return;
                        }
                    }

                    if (formAudio.Visible == true)
                    {
                        formAudio.Close();
                    }

                    Common.Generic.ProgressMax = Common.Generic.OpenFilePaths.Length;
                    Common.Generic.ProcessFlag = 1;
                    FolderSavePath = fbd.SelectedPath;

                    Form formProgress = new FormProgress();
                    formProgress.ShowDialog();
                    formProgress.Dispose();

                    if (Common.Generic.Result == false || Generic.cts.IsCancellationRequested)
                    {
                        Common.Generic.cts.Dispose();
                        MessageBox.Show(this, Localization.CancelledCaption, Localization.MSGBoxAbortedCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ResetControl();
                        return;
                    }

                    int count = FormProgress.FormProgressInstance.EncodeCount;
                    int error = FormProgress.FormProgressInstance.EncodeErrorCount;

                    // Verify
                    if (count == OpenFilePaths.Length)
                    {
                        MessageBox.Show(string.Format(Localization.EncodeSuccessCaption, count), Localization.MSGBoxSuccessCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (formAudio.Visible == true)
                        {
                            formAudio.Close();
                        }
                        IsNUB = false;

                        closeCToolStripMenuItem.Enabled = false;
                        DeleteCurrentFiles(OpenFilePaths);
                        OpenFilePaths = null!;
                        ResetControl();
                        Utils.ShowFolder(fbd.SelectedPath, bool.Parse(Config.Entry["ShowFolder"].Value));
                        return;
                    }
                    else if (error != 0)
                    {
                        MessageBox.Show(string.Format(Localization.EncodePartialCaption, count, error), Localization.MSGBoxWarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (formAudio.Visible == true)
                        {
                            formAudio.Close();
                        }
                        IsNUB = false;

                        closeCToolStripMenuItem.Enabled = false;
                        DeleteCurrentFiles(OpenFilePaths);
                        OpenFilePaths = null!;
                        ResetControl();
                        Utils.ShowFolder(fbd.SelectedPath, bool.Parse(Config.Entry["ShowFolder"].Value));
                        return;
                    }
                    else
                    {
                        MessageBox.Show(string.Format(Localization.UnExpectedCaption, ExceptionInformation), Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (formAudio.Visible == true)
                        {
                            formAudio.Close();
                        }
                        IsNUB = false;

                        closeCToolStripMenuItem.Enabled = false;
                        DeleteCurrentFiles(OpenFilePaths);
                        OpenFilePaths = null!;
                        ResetControl();
                        return;
                    }

                }
                else // Cancelled
                {
                    return;
                }
            }
        }

        private void Label_Openhere_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        /// <summary>
        /// ファイルをドラッグアンドドロップした際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_Openhere_DragDrop(object sender, DragEventArgs e)
        {
            ATWCheck(IsATW);
            ConvertedNUBCheck(IsNTW);

            if (e.Data is not null)
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop)!;

                foreach (var check in files)
                {
                    FileInfo file = new(check);
                    switch (file.Extension.ToUpper())
                    {
                        case ".WAV":
                            continue;
                        case ".MP3":
                            continue;
                        case ".M4A":
                            continue;
                        case ".AAC":
                            continue;
                        case ".AIFF":
                            continue;
                        case ".ALAC":
                            continue;
                        case ".FLAC":
                            continue;
                        case ".OGG":
                            continue;
                        case ".OPUS":
                            continue;
                        case ".WMA":
                            continue;
                        case ".SND":
                            continue;
                        case ".RAW":
                            continue;
                        case ".NUB":
                            continue;
                        default:
                            MessageBox.Show(this, Localization.NotAllowedExtensionCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }
                }

                List<string> lst = new();
                foreach (string fp in files)
                {
                    lst.Add(fp);
                }
                Common.Generic.OpenFilePaths = lst.ToArray();
                Common.Generic.OriginalPaths = lst.ToArray();

                if (Common.Generic.OpenFilePaths.Length == 1) // Single
                {
                    IsOpenMulti = false;

                    MultipleNuSoundVersion = new int[OpenFilePaths.Length];
                    MultipleNumberOfStreams = new ushort[OpenFilePaths.Length];
                    MultipleStreamId = new ushort[OpenFilePaths.Length];
                    MultipleChannels = new ushort[OpenFilePaths.Length];
                    MultipleLoopStarts = new int[OpenFilePaths.Length];
                    MultipleLoopEnds = new int[OpenFilePaths.Length];
                    MultipleSampleRate = new uint[OpenFilePaths.Length];
                    MultipleVolume = new int[OpenFilePaths.Length];
                    MultipleFilesLoopOKFlags = new bool[OpenFilePaths.Length];

                    FileInfo file = new(files[0]);
                    FileSizes = file.Length;

                    switch (file.Extension.ToUpper())
                    {
                        case ".WAV":
                            if (bool.Parse(Config.Entry["ForceConvertWaveOnly"].Value))
                            {
                                FormatSorter(true, true);
                            }
                            else
                            {
                                FormatSorter(true);
                            }
                            break;
                        case ".MP3":
                            FormatSorter(true, true);
                            break;
                        case ".M4A":
                            FormatSorter(true, true);
                            break;
                        case ".AAC":
                            FormatSorter(true, true);
                            break;
                        case ".FLAC":
                            FormatSorter(true, true);
                            break;
                        case ".ALAC":
                            FormatSorter(true, true);
                            break;
                        case ".AIFF":
                            FormatSorter(true, true);
                            break;
                        case ".OGG":
                            FormatSorter(true, true);
                            break;
                        case ".OPUS":
                            FormatSorter(true, true);
                            break;
                        case ".WMA":
                            FormatSorter(true, true);
                            break;
                        case ".SND":
                            FormatSorter(true, true);
                            break;
                        case ".RAW":
                            FormatSorter(true, true);
                            break;
                        case ".NUB":
                            ReadMetadatas(OpenFilePaths[0], NUBMetadataBuffers);
                            if (DeleteNUBHeaderWraw2wav(OpenFilePaths[0]))
                            {
                                FormatSorter(false);
                            }
                            else
                            {
                                return;
                            }
                            break;
                    }

                    closeCToolStripMenuItem.Enabled = true;
                    return;
                }
                else // multiple
                {
                    IsOpenMulti = true;
                    FileInfo fs = new(files[0]);
                    FileSizes = fs.Length;
                    foreach (string file in files)
                    {
                        FileInfo fi = new(file);
                        FileSizes += fi.Length;
                    }

                    string Ft = "";
                    int count = 0, wavcount = 0;
                    List<string> multiextlst = new();

                    foreach (var file in Common.Generic.OpenFilePaths)
                    {
                        FileInfo fi = new(file);

                        if (count != 0)
                        {
                            if (Ft == ".NUB")
                            {
                                if (Ft != fi.Extension.ToUpper())
                                {
                                    MessageBox.Show(this, Localization.FileMixedWithNUBCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    ResetControl();
                                    return;
                                }
                            }
                            else
                            {
                                if (fi.Extension.ToUpper() == ".NUB")
                                {
                                    MessageBox.Show(this, Localization.FileMixedWithNUBCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    ResetControl();
                                    return;
                                }
                                if (count == Generic.OpenFilePaths.Length - 1)
                                {
                                    if (wavcount == Generic.OpenFilePaths.Length - 1)
                                    {
                                        if (bool.Parse(Config.Entry["ForceConvertWaveOnly"].Value))
                                        {
                                            Ft = ".NOT";
                                        }
                                        else
                                        {
                                            Ft = ".WAV";
                                        }

                                    }
                                    else
                                    {
                                        Ft = ".NOT";
                                    }
                                }
                                else
                                {
                                    if (fi.Extension.ToUpper() == Ft)
                                    {
                                        Ft = fi.Extension.ToUpper();
                                        wavcount++;
                                    }
                                    else if (fi.Extension.ToUpper() != Ft)
                                    {
                                        Ft = fi.Extension.ToUpper();
                                    }
                                    else
                                    {
                                        Ft = fi.Extension.ToUpper();
                                    }
                                }

                            }
                        }
                        else
                        {
                            if (fi.Extension.ToUpper() == ".WAV")
                            {
                                Ft = fi.Extension.ToUpper();
                                multiextlst.Add(file);
                                wavcount++;
                                count++;
                                continue;
                            }
                            else if (fi.Extension.ToUpper() == ".NUB")
                            {
                                Ft = fi.Extension.ToUpper();
                            }
                            else
                            {
                                Ft = fi.Extension.ToUpper();
                                multiextlst.Add(file);
                            }
                        }

                        count++;
                    }

                    closeCToolStripMenuItem.Enabled = true;

                    MultipleNuSoundVersion = new int[OpenFilePaths.Length];
                    MultipleNumberOfStreams = new ushort[OpenFilePaths.Length];
                    MultipleStreamId = new ushort[OpenFilePaths.Length];
                    MultipleChannels = new ushort[OpenFilePaths.Length];
                    MultipleLoopStarts = new int[OpenFilePaths.Length];
                    MultipleLoopEnds = new int[OpenFilePaths.Length];
                    MultipleSampleRate = new uint[OpenFilePaths.Length];
                    MultipleVolume = new int[OpenFilePaths.Length];
                    MultipleFilesLoopOKFlags = new bool[OpenFilePaths.Length];

                    switch (Ft.ToUpper())
                    {
                        case ".WAV":
                            FormatSorter(true);
                            break;
                        case ".NOT":
                            FormatSorter(true, true);
                            break;
                        case ".NUB":
                            Common.Generic.ProgressMax = Common.Generic.OpenFilePaths.Length;
                            Common.Generic.ProcessFlag = 0;

                            Form formProgress = new FormProgress();
                            formProgress.ShowDialog();
                            formProgress.Dispose();

                            if (Common.Generic.Result == false || Generic.cts.IsCancellationRequested)
                            {
                                Common.Generic.cts.Dispose();
                                MessageBox.Show(this, Localization.CancelledCaption, Localization.MSGBoxAbortedCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                ResetControl();
                                return;
                            }

                            FormatSorter(false);
                            /*if (DeleteNUBHeaderWraw2wavMulti(OpenFilePaths))
                            {
                                FormatSorter(false);
                            }
                            else
                            {
                                return;
                            }*/
                            break;
                    }

                    return;
                }

            }

        }

        /// <summary>
        /// ファイルに応じて動作を変更
        /// </summary>
        /// <param name="IsEncode">変換対象かどうか</param>
        /// <param name="IsNotWave">Waveファイルではないかどうか</param>
        private void FormatSorter(bool IsEncode, bool IsNotWave = false)
        {
            if (IsEncode != false)
            {
                if (IsNotWave != true) // Wave
                {
                    IsATW = false;
                    IsWave = true;
                    IsNUB = false;
                    InitializeSubControls();
                    SetMetaLabels();
                }
                else // NotWave
                {
                    Debug.WriteLine("array[{0}]", string.Join(", ", OriginalPaths));
                    if (AudioToWaveConvert())
                    {
                        Debug.WriteLine("arrayATW[{0}]", string.Join(", ", OriginalPaths));
                        IsATW = true;
                        IsWave = false;
                        IsNUB = false;
                        InitializeSubControls();
                        SetMetaLabels();
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else // NUB
            {
                Debug.WriteLine("array[{0}]", string.Join(", ", OriginalPaths));
                IsATW = false;
                IsWave = false;
                IsNUB = true;
                InitializeSubControls();
                SetMetaLabels();
            }
        }

        /// <summary>
        /// サポートされている形式をWaveに変換
        /// </summary>
        /// <returns></returns>
        private bool AudioToWaveConvert()
        {
            if (Common.Generic.IsWave != true && Common.Generic.IsNUB != true)
            {
                string TempAudioDir;
                if (Directory.Exists(Directory.GetCurrentDirectory() + @"\_tempAudio"))
                {
                    TempAudioDir = Directory.GetCurrentDirectory() + @"\_tempAudio";
                }
                else
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\_tempAudio");
                    TempAudioDir = Directory.GetCurrentDirectory() + @"\_tempAudio";
                }

                if (Common.Generic.OpenFilePaths.Length == 1) // 単一
                {
                    if (bool.Parse(Config.Entry["FixedConvert"].Value)) // Fix
                    {
                        FileInfo file = new(Common.Generic.OpenFilePaths[0]);
                        Common.Generic.WTAmethod = int.Parse(Config.Entry["ConvertType"].Value);

                        //Common.Generic.SavePath = file.Directory + @"\" + file.Name + @".wav";
                        Common.Generic.SavePath = TempAudioDir + @"\" + file.Name.Replace(file.Extension, "") + @".wav";
                        Common.Generic.ProgressMax = 1;

                        Common.Generic.ProcessFlag = 2;

                        Form formProgress = new FormProgress();
                        formProgress.ShowDialog();
                        formProgress.Dispose();

                        if (Common.Generic.Result == false || Generic.cts.IsCancellationRequested)
                        {
                            Generic.IsATWCancelled = true;
                            Common.Generic.cts.Dispose();
                            MessageBox.Show(Localization.CancelledCaption, Localization.MSGBoxAbortedCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ResetControl();
                            return false;
                        }

                        FileInfo fi = new(Common.Generic.SavePath);
                        if (File.Exists(Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name))
                        {
                            if (File.Exists(Common.Generic.SavePath))  // �t�@�C�������ɑ��݂���ꍇ�͍폜���Ă���Move����
                            {
                                File.Delete(Common.Generic.SavePath);
                                File.Move(Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name, Common.Generic.SavePath);
                                Common.Utils.DeleteDirectoryFiles(Directory.GetCurrentDirectory() + @"\_temp");
                            }
                            else
                            {
                                File.Move(Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name, Common.Generic.SavePath);
                                Common.Utils.DeleteDirectoryFiles(Directory.GetCurrentDirectory() + @"\_temp");
                            }

                            if (File.Exists(Common.Generic.SavePath)) // �t�@�C������������Ă��邩�ǂ���
                            {
                                label_OrigFilepath.Text = Common.Generic.OpenFilePaths[0];
                                Common.Generic.OpenFilePaths[0] = Common.Generic.SavePath;
                            }
                            else // �G���[
                            {
                                ResetControl();
                                MessageBox.Show(Localization.ConvertErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                            return true;
                        }
                        else // Error
                        {
                            ResetControl();
                            Common.Utils.DeleteDirectoryFiles(Directory.GetCurrentDirectory() + @"\_temp");
                            MessageBox.Show(Localization.ConvertErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    else // normal
                    {
                        FileInfo file = new(Common.Generic.OpenFilePaths[0]);
                        using Form formAtWST = new FormAtWSelectTarget();
                        DialogResult dr = formAtWST.ShowDialog();
                        if (dr != DialogResult.Cancel && dr != DialogResult.None)
                        {
                            //Common.Generic.SavePath = file.Directory + @"\" + file.Name + @".wav";
                            Common.Generic.SavePath = TempAudioDir + @"\" + file.Name + @".wav";
                            Common.Generic.ProgressMax = 1;

                            Common.Generic.ProcessFlag = 2;

                            Form formProgress = new FormProgress();
                            formProgress.ShowDialog();
                            formProgress.Dispose();

                            if (Common.Generic.Result == false || Generic.cts.IsCancellationRequested)
                            {
                                Generic.IsATWCancelled = true;
                                Common.Generic.cts.Dispose();
                                MessageBox.Show(Localization.CancelledCaption, Localization.MSGBoxAbortedCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                ResetControl();
                                return false;
                            }

                            FileInfo fi = new(Common.Generic.SavePath);
                            if (File.Exists(Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name))
                            {
                                if (File.Exists(Common.Generic.SavePath))  // �t�@�C�������ɑ��݂���ꍇ�͍폜���Ă���Move����
                                {
                                    File.Delete(Common.Generic.SavePath);
                                    File.Move(Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name, Common.Generic.SavePath);
                                    Common.Utils.DeleteDirectoryFiles(Directory.GetCurrentDirectory() + @"\_temp");
                                }
                                else
                                {
                                    File.Move(Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name, Common.Generic.SavePath);
                                    Common.Utils.DeleteDirectoryFiles(Directory.GetCurrentDirectory() + @"\_temp");
                                }

                                if (File.Exists(Common.Generic.SavePath)) // �t�@�C������������Ă��邩�ǂ���
                                {
                                    Debug.WriteLine("arrayATWbegin[{0}]", string.Join(", ", OriginalPaths));
                                    //label_OrigFilepath.Text = Common.Generic.OpenFilePaths[0];
                                    label_OrigFilepath.Text = Common.Generic.OriginalPaths[0];
                                    Common.Generic.OpenFilePaths[0] = Common.Generic.SavePath;
                                }
                                else // �G���[
                                {
                                    ResetControl();
                                    MessageBox.Show(Localization.ConvertErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return false;
                                }

                                return true;
                            }
                            else // Error
                            {
                                ResetControl();
                                Common.Utils.DeleteDirectoryFiles(Directory.GetCurrentDirectory() + @"\_temp");
                                MessageBox.Show(Localization.ConvertErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                        else
                        {
                            Generic.IsATW = false;
                            ResetControl();
                            return false;
                        }
                    }
                }
                else // multiple
                {
                    Generic.IsOpenMulti = true;
                    if (bool.Parse(Config.Entry["FixedConvert"].Value)) // Fix
                    {
                        //FileInfo fp = new(Common.Generic.OpenFilePaths[0]);
                        Common.Generic.WTAmethod = int.Parse(Config.Entry["ConvertType"].Value);

                        Common.Generic.ProgressMax = Common.Generic.OpenFilePaths.Length;

                        Common.Generic.ProcessFlag = 2;

                        Form formProgress = new FormProgress();
                        formProgress.ShowDialog();
                        formProgress.Dispose();

                        if (Common.Generic.Result == false || Generic.cts.IsCancellationRequested)
                        {
                            Generic.IsATWCancelled = true;
                            Common.Generic.cts.Dispose();
                            MessageBox.Show(this, Localization.CancelledCaption, Localization.MSGBoxAbortedCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ResetControl();
                            return false;
                        }

                        int count = 0;
                        label_OrigFilepath.Text = Generic.OpenFilePaths[0];

                        foreach (var file in Common.Generic.OpenFilePaths)
                        {
                            FileInfo fi = new(file);
                            if (File.Exists(Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav"))
                            {
                                if (File.Exists(TempAudioDir + @"\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav"))  // �t�@�C�������ɑ��݂���ꍇ�͍폜���Ă���Move����
                                {
                                    File.Delete(TempAudioDir + @"\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav");
                                    File.Move(Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav", TempAudioDir + @"\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav");
                                }
                                else
                                {
                                    File.Move(Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav", TempAudioDir + @"\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav");
                                }

                            }
                            else // Error
                            {
                                Common.Utils.DeleteDirectoryFiles(Directory.GetCurrentDirectory() + @"\_temp");
                                MessageBox.Show(this, Localization.ConvertErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                ResetControl();
                                return false;
                            }

                            if (File.Exists(TempAudioDir + @"\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav")) // �t�@�C�����݊m�F
                            {
                                Common.Generic.OpenFilePaths[count] = TempAudioDir + @"\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav";
                                count++;
                            }
                            else // error
                            {
                                ResetControl();
                                return false;
                            }
                        }
                        Common.Utils.DeleteDirectoryFiles(Directory.GetCurrentDirectory() + @"\_temp");

                        FileInfo fisize = new(label_OrigFilepath.Text);
                        long FS = fisize.Length;
                        //label_Sizetxt.Text = string.Format(Localization.FileSizeCaption, FS / 1024);

                        return true;
                    }
                    else // normal
                    {
                        //FileInfo fp = new(Common.Generic.OpenFilePaths[0]);
                        using Form formAtWST = new FormAtWSelectTarget();
                        DialogResult dr = formAtWST.ShowDialog();
                        if (dr != DialogResult.Cancel && dr != DialogResult.None)
                        {
                            Common.Generic.ProgressMax = Common.Generic.OpenFilePaths.Length;

                            Common.Generic.ProcessFlag = 2;

                            Form formProgress = new FormProgress();
                            formProgress.ShowDialog();
                            formProgress.Dispose();

                            if (Common.Generic.Result == false || Generic.cts.IsCancellationRequested)
                            {
                                Generic.IsATWCancelled = true;
                                Common.Generic.cts.Dispose();
                                MessageBox.Show(this, Localization.CancelledCaption, Localization.MSGBoxAbortedCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                ResetControl();
                                return false;
                            }

                            int count = 0;
                            label_OrigFilepath.Text = Generic.OpenFilePaths[0];

                            foreach (var file in Common.Generic.OpenFilePaths)
                            {
                                FileInfo fi = new(file);
                                if (File.Exists(Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav"))
                                {
                                    if (File.Exists(TempAudioDir + @"\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav"))  // �t�@�C�������ɑ��݂���ꍇ�͍폜���Ă���Move����
                                    {
                                        File.Delete(TempAudioDir + @"\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav");
                                        File.Move(Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav", TempAudioDir + @"\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav");
                                    }
                                    else
                                    {
                                        File.Move(Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav", TempAudioDir + @"\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav");
                                    }

                                }
                                else // Error
                                {
                                    Common.Utils.DeleteDirectoryFiles(Directory.GetCurrentDirectory() + @"\_temp");
                                    MessageBox.Show(this, Localization.ConvertErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    ResetControl();
                                    return false;
                                }

                                if (File.Exists(TempAudioDir + @"\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav")) // �t�@�C�����݊m�F
                                {
                                    Common.Generic.OpenFilePaths[count] = TempAudioDir + @"\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav";
                                    count++;
                                }
                                else // �G���[
                                {
                                    ResetControl();
                                    return false;
                                }
                            }
                            Common.Utils.DeleteDirectoryFiles(Directory.GetCurrentDirectory() + @"\_temp");

                            FileInfo fisize = new(label_OrigFilepath.Text);
                            long FS = fisize.Length;
                            //label_Sizetxt.Text = string.Format(Localization.FileSizeCaption, FS / 1024);

                            return true;
                        }
                        else
                        {
                            Generic.IsATW = false;
                            ResetControl();
                            return false;
                        }
                    }

                }
            }
            else
            {
                return false;
            }
        }

        #region MenuItem

        private void OpenOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new()
            {
                FileName = "",
                InitialDirectory = "",
                FilterIndex = 12,
                Filter = Localization.Filters,
                Title = Localization.OpenDialogTitle,
                Multiselect = true,
                RestoreDirectory = true
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                closeCToolStripMenuItem.PerformClick();

                //ResetControl();

                Utils.ATWCheck(Generic.IsATW);
                Utils.ConvertedNUBCheck(Generic.IsNTW);

                List<string> lst = new();
                foreach (string files in ofd.FileNames)
                {
                    lst.Add(files);
                }
                Common.Generic.OpenFilePaths = lst.ToArray();
                Common.Generic.OriginalPaths = lst.ToArray();

                //OpenFilePaths = ofd.FileNames;
                if (OpenFilePaths.Length == 1) // Single
                {
                    IsOpenMulti = false;
                    IsAudioStreamingReloaded = false;

                    MultipleNuSoundVersion = new int[OpenFilePaths.Length];
                    MultipleNumberOfStreams = new ushort[OpenFilePaths.Length];
                    MultipleStreamId = new ushort[OpenFilePaths.Length];
                    MultipleChannels = new ushort[OpenFilePaths.Length];
                    MultipleLoopStarts = new int[OpenFilePaths.Length];
                    MultipleLoopEnds = new int[OpenFilePaths.Length];
                    MultipleSampleRate = new uint[OpenFilePaths.Length];
                    MultipleVolume = new int[OpenFilePaths.Length];
                    MultipleFilesLoopOKFlags = new bool[OpenFilePaths.Length];

                    FileInfo file = new(ofd.FileName);
                    FileSizes = file.Length;

                    closeCToolStripMenuItem.Enabled = true;

                    switch (file.Extension.ToUpper())
                    {
                        case ".WAV":
                            if (bool.Parse(Config.Entry["ForceConvertWaveOnly"].Value))
                            {
                                FormatSorter(true, true);
                            }
                            else
                            {
                                FormatSorter(true);
                            }
                            break;
                        case ".MP3":
                            FormatSorter(true, true);
                            break;
                        case ".M4A":
                            FormatSorter(true, true);
                            break;
                        case ".AAC":
                            FormatSorter(true, true);
                            break;
                        case ".FLAC":
                            FormatSorter(true, true);
                            break;
                        case ".ALAC":
                            FormatSorter(true, true);
                            break;
                        case ".AIFF":
                            FormatSorter(true, true);
                            break;
                        case ".OGG":
                            FormatSorter(true, true);
                            break;
                        case ".OPUS":
                            FormatSorter(true, true);
                            break;
                        case ".WMA":
                            FormatSorter(true, true);
                            break;
                        case ".SND":
                            FormatSorter(true, true);
                            break;
                        case ".RAW":
                            FormatSorter(true, true);
                            break;
                        case ".NUB":
                            ReadMetadatas(OpenFilePaths[0], NUBMetadataBuffers);
                            if (DeleteNUBHeaderWraw2wav(OpenFilePaths[0]))
                            {
                                FormatSorter(false);
                            }
                            else
                            {
                                return;
                            }
                            break;
                        default:
                            return;
                    }
                    return;
                }
                else // multiple
                {
                    IsOpenMulti = true;
                    IsAudioStreamingReloaded = true;

                    foreach (string file in Common.Generic.OpenFilePaths)
                    {
                        FileInfo fi = new(file);
                        FileSizes += fi.Length;
                    }

                    string Ft = "";
                    int count = 0, wavcount = 0;
                    List<string> multiextlst = new();

                    foreach (var file in Common.Generic.OpenFilePaths)
                    {
                        FileInfo fi = new(file);

                        if (count != 0)
                        {
                            if (Ft == ".NUB")
                            {
                                if (Ft != fi.Extension.ToUpper())
                                {
                                    MessageBox.Show(this, Localization.FileMixedWithNUBCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    ResetControl();
                                    return;
                                }
                            }
                            else
                            {
                                if (fi.Extension.ToUpper() == ".NUB")
                                {
                                    MessageBox.Show(this, Localization.FileMixedWithNUBCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    ResetControl();
                                    return;
                                }
                                if (count == Generic.OpenFilePaths.Length - 1)
                                {
                                    if (wavcount == Generic.OpenFilePaths.Length - 1)
                                    {
                                        if (bool.Parse(Config.Entry["ForceConvertWaveOnly"].Value))
                                        {
                                            Ft = ".NOT";
                                        }
                                        else
                                        {
                                            Ft = ".WAV";
                                        }

                                    }
                                    else
                                    {
                                        Ft = ".NOT";
                                    }
                                }
                                else
                                {
                                    if (fi.Extension.ToUpper() == Ft)
                                    {
                                        Ft = fi.Extension.ToUpper();
                                        wavcount++;
                                    }
                                    else if (fi.Extension.ToUpper() != Ft)
                                    {
                                        Ft = fi.Extension.ToUpper();
                                    }
                                    else
                                    {
                                        Ft = fi.Extension.ToUpper();
                                    }
                                }

                            }
                        }
                        else
                        {
                            if (fi.Extension.ToUpper() == ".WAV")
                            {
                                Ft = fi.Extension.ToUpper();
                                multiextlst.Add(file);
                                wavcount++;
                                count++;
                                continue;
                            }
                            else if (fi.Extension.ToUpper() == ".NUB")
                            {
                                Ft = fi.Extension.ToUpper();
                            }
                            else
                            {
                                Ft = fi.Extension.ToUpper();
                                multiextlst.Add(file);
                            }
                        }

                        count++;
                    }

                    closeCToolStripMenuItem.Enabled = true;

                    MultipleNuSoundVersion = new int[OpenFilePaths.Length];
                    MultipleNumberOfStreams = new ushort[OpenFilePaths.Length];
                    MultipleStreamId = new ushort[OpenFilePaths.Length];
                    MultipleChannels = new ushort[OpenFilePaths.Length];
                    MultipleLoopStarts = new int[OpenFilePaths.Length];
                    MultipleLoopEnds = new int[OpenFilePaths.Length];
                    MultipleSampleRate = new uint[OpenFilePaths.Length];
                    MultipleVolume = new int[OpenFilePaths.Length];
                    MultipleFilesLoopOKFlags = new bool[OpenFilePaths.Length];

                    switch (Ft.ToUpper())
                    {
                        case ".WAV":
                            FormatSorter(true);
                            break;
                        case ".NOT":
                            FormatSorter(true, true);
                            break;
                        case ".NUB":
                            Common.Generic.ProgressMax = Common.Generic.OpenFilePaths.Length;
                            Common.Generic.ProcessFlag = 0;

                            Form formProgress = new FormProgress();
                            formProgress.ShowDialog();
                            formProgress.Dispose();

                            if (Common.Generic.Result == false || Generic.cts.IsCancellationRequested)
                            {
                                Common.Generic.cts.Dispose();
                                MessageBox.Show(this, Localization.CancelledCaption, Localization.MSGBoxAbortedCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                ResetControl();
                                return;
                            }

                            FormatSorter(false);
                            /*if (DeleteNUBHeaderWraw2wavMulti(OpenFilePaths))
                            {
                                FormatSorter(false);
                            }
                            else
                            {
                                return;
                            }*/

                            break;
                    }

                    return;
                }
            }
            else
            {
                return;
            }

        }

        private void CloseCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formAudio.Visible == true)
            {
                formAudio.Close();
            }
            DeleteCurrentFiles(OpenFilePaths);
            ResetControl();
        }

        private void AboutAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using FormAbout Form = new();
            Form.ShowDialog();
        }

        private void PreferencesSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool oldSSValue = bool.Parse(Config.Entry["SmoothSamples"].Value);
            using FormPreferences Form = new();
            Form.ShowDialog();

            if (oldSSValue != bool.Parse(Config.Entry["SmoothSamples"].Value))
            {
                if (formAudio is not null && formAudio.Visible == true)
                {
                    formAudio.Close();

                    formAudio = new(this)
                    {
                        TopLevel = false
                    };
                    panel_Main.Controls.Add(formAudio);
                    formAudio.Show();
                }
            }
            else
            {
                return;
            }
        }

        private void ExitXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// (Task) 起動時にアップデートを確認
        /// </summary>
        /// <returns></returns>
        private async Task CheckForUpdatesForInit()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    string hv = null!;

                    using Stream hcs = await Task.Run(() => Common.Network.GetWebStreamAsync(appUpdatechecker, Common.Network.GetUri("https://raw.githubusercontent.com/XyLe-GBP/NUBTool/master/VERSIONINFO")));
                    using StreamReader hsr = new(hcs);
                    hv = await Task.Run(() => hsr.ReadToEndAsync());
                    Common.Generic.GitHubLatestVersion = hv[8..].Replace("\n", "");

                    FileVersionInfo ver = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);

                    if (ver.FileVersion != null)
                    {
                        switch (ver.FileVersion.ToString().CompareTo(hv[8..].Replace("\n", "")))
                        {
                            case -1:
                                DialogResult dr = MessageBox.Show(Localization.LatestCaption + hv[8..].Replace("\n", "") + "\n" + Localization.CurrentCaption + ver.FileVersion + "\n" + Localization.UpdateConfirmCaption, Localization.MSGBoxConfirmCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (dr == DialogResult.Yes)
                                {
                                    using FormUpdateApplicationType fuat = new();
                                    if (fuat.ShowDialog() == DialogResult.Cancel) { return; }

                                    if (File.Exists(Directory.GetCurrentDirectory() + @"\res\nubtool.zip"))
                                    {
                                        File.Delete(Directory.GetCurrentDirectory() + @"\res\nubtool.zip");
                                    }

                                    Common.Generic.ProcessFlag = 4;
                                    Common.Generic.ProgressMax = 100;
                                    using FormProgress form = new();
                                    form.ShowDialog();

                                    if (Common.Generic.Result == false)
                                    {
                                        Common.Generic.cts.Dispose();
                                        MessageBox.Show(Localization.CancelledCaption, Localization.MSGBoxAbortedCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }
                                    string updpath = Directory.GetCurrentDirectory()[..Directory.GetCurrentDirectory().LastIndexOf('\\')];

                                    if (File.Exists(updpath + @"\updater.exe"))
                                    {
                                        File.Delete(updpath + @"\updater.exe");
                                    }
                                    if (Directory.Exists(updpath + @"\updater-temp"))
                                    {
                                        Common.Utils.DeleteDirectory(updpath + @"\updater-temp");
                                    }
                                    if (File.Exists(updpath + @"\nubtool.zip"))
                                    {
                                        File.Delete(updpath + @"\nubtool.zip");
                                    }

                                    File.Move(Directory.GetCurrentDirectory() + @"\res\updater.exe", updpath + @"\updater.exe");
                                    string wtext;
                                    switch (Common.Generic.ApplicationPortable)
                                    {
                                        case false:
                                            {
                                                wtext = Directory.GetCurrentDirectory() + "\r\nrelease";
                                            }
                                            break;
                                        case true:
                                            {
                                                wtext = Directory.GetCurrentDirectory() + "\r\nportable";
                                            }
                                            break;
                                    }
                                    File.WriteAllText(updpath + @"\updater.txt", wtext);
                                    File.Move(updpath + @"\updater.txt", updpath + @"\updater.dat");
                                    if (File.Exists(Directory.GetCurrentDirectory() + @"\res\nubtool.zip"))
                                    {
                                        File.Move(Directory.GetCurrentDirectory() + @"\res\nubtool.zip", updpath + @"\nubtool.zip");
                                    }

                                    ProcessStartInfo pi = new()
                                    {
                                        FileName = updpath + @"\updater.exe",
                                        Arguments = null,
                                        UseShellExecute = true,
                                        WindowStyle = ProcessWindowStyle.Normal,
                                    };
                                    Process.Start(pi);
                                    Close();
                                    return;
                                }
                                else
                                {
                                    DialogResult dr2 = MessageBox.Show(Localization.LatestCaption + hv[8..].Replace("\n", "") + "\n" + Localization.CurrentCaption + ver.FileVersion + "\n" + Localization.SiteOpenCaption, Localization.MSGBoxConfirmCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                    if (dr2 == DialogResult.Yes)
                                    {
                                        Common.Utils.OpenURI("https://github.com/XyLe-GBP/NUBTool/releases");
                                        return;
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                            case 0:
                                break;
                            case 1:
                                throw new Exception(hv[8..].Replace("\n", "").ToString() + " < " + ver.FileVersion.ToString());
                        }
                        return;
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        #endregion

        #region ComboBox
        private void ComboBox_Format_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox_Format.SelectedIndex)
            {
                case 0:
                    Signed = true;
                    ByteOrderBE = true;
                    Format = 0;
                    break;
                case 1:
                    Signed = true;
                    ByteOrderBE = false;
                    Format = 1;
                    break;
                case 2:
                    {
                        DialogResult dr = MessageBox.Show(Localization.UnsignedConversionWarningCaption, Localization.MSGBoxWarningCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.Yes)
                        {
                            Signed = false;
                            ByteOrderBE = true;
                            Format = 2;
                            break;
                        }
                        else
                        {
                            comboBox_Format.SelectedIndex = 0;
                            Signed = true;
                            ByteOrderBE = true;
                            Format = 0;
                            break;
                        }
                    }
                case 3:
                    {
                        DialogResult dr = MessageBox.Show(Localization.UnsignedConversionWarningCaption, Localization.MSGBoxWarningCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.Yes)
                        {
                            Signed = false;
                            ByteOrderBE = false;
                            Format = 3;
                            break;
                        }
                        else
                        {
                            comboBox_Format.SelectedIndex = 0;
                            Signed = true;
                            ByteOrderBE = true;
                            Format = 0;
                            break;
                        }
                    }
                default:
                    Signed = true;
                    ByteOrderBE = true;
                    Format = 0;
                    break;
            }
        }

        private void ComboBox_Samplerate_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox_Samplerate.SelectedIndex)
            {
                case 0:
                    SampleRate = 44100;
                    break;
                case 1:
                    SampleRate = 48000;
                    break;
                default:
                    SampleRate = 44100;
                    break;
            }
        }

        private void ComboBox_Channels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsOpenMulti) // Single
            {
                switch (comboBox_Channels.SelectedIndex)
                {
                    case 0:
                        Channels = 1;
                        break;
                    case 1:
                        Channels = 2;
                        break;
                    case 2:
                        Channels = 3;
                        break;
                    case 3:
                        Channels = 4;
                        break;
                    case 4:
                        Channels = 5;
                        break;
                    case 5:
                        Channels = 6;
                        break;
                    default:
                        Channels = 2;
                        break;
                }
            }
            else // Multiple
            {
                uint pos = FormAudio.FormAudioInstance.ButtonPosition - 1;
                switch (comboBox_Channels.SelectedIndex)
                {
                    case 0:
                        Channels = 1;
                        MultipleChannels[pos] = 1;
                        break;
                    case 1:
                        Channels = 2;
                        MultipleChannels[pos] = 2;
                        break;
                    case 2:
                        Channels = 3;
                        MultipleChannels[pos] = 3;
                        break;
                    case 3:
                        Channels = 4;
                        MultipleChannels[pos] = 4;
                        break;
                    case 4:
                        Channels = 5;
                        MultipleChannels[pos] = 5;
                        break;
                    case 5:
                        Channels = 6;
                        MultipleChannels[pos] = 6;
                        break;
                    default:
                        Channels = 2;
                        MultipleChannels[pos] = 2;
                        break;
                }
            }
        }

        private void ComboBox_Volume_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsOpenMulti) // Single
            {
                switch (comboBox_Volume.SelectedIndex)
                {
                    case 0:
                        label_CustomVol.Enabled = false;
                        label_CustomVolHex.Enabled = false;
                        label_CustomVolHex.ForeColor = Color.Black;
                        label_CustomVolHex.Text = Localization.HexValueCaption + ": 0x3F00";
                        textBox_CustomVol.Enabled = false;
                        textBox_CustomVol.Text = "16128";
                        Volume = 16128;
                        VolumeNG = false;
                        break;
                    case 1:
                        label_CustomVol.Enabled = false;
                        label_CustomVolHex.Enabled = false;
                        label_CustomVolHex.ForeColor = Color.Black;
                        label_CustomVolHex.Text = Localization.HexValueCaption + ": 0x4080";
                        textBox_CustomVol.Enabled = false;
                        textBox_CustomVol.Text = "16512";
                        Volume = 16512;
                        VolumeNG = false;
                        break;
                    case 2:
                        label_CustomVol.Enabled = false;
                        label_CustomVolHex.Enabled = false;
                        label_CustomVolHex.ForeColor = Color.Black;
                        label_CustomVolHex.Text = Localization.HexValueCaption + ": 0x40C0";
                        textBox_CustomVol.Enabled = false;
                        textBox_CustomVol.Text = "16576";
                        Volume = 16576;
                        VolumeNG = false;
                        break;
                    case 3:
                        label_CustomVol.Enabled = false;
                        label_CustomVolHex.Enabled = false;
                        label_CustomVolHex.ForeColor = Color.Black;
                        label_CustomVolHex.Text = Localization.HexValueCaption + ": 0x4100";
                        textBox_CustomVol.Enabled = false;
                        textBox_CustomVol.Text = "16640";
                        Volume = 16640;
                        VolumeNG = false;
                        break;
                    case 4:
                        label_CustomVol.Enabled = true;
                        label_CustomVolHex.Enabled = true;
                        label_CustomVolHex.Text = Localization.HexValueCaption + ": 0x4080";
                        textBox_CustomVol.Enabled = true;
                        textBox_CustomVol.Text = "16512";
                        VolumeNG = false;
                        break;
                    default:
                        label_CustomVol.Enabled = false;
                        label_CustomVolHex.Enabled = false;
                        label_CustomVolHex.ForeColor = Color.Black;
                        label_CustomVolHex.Text = Localization.HexValueCaption + ": 0x0";
                        textBox_CustomVol.Enabled = false;
                        textBox_CustomVol.Text = string.Empty;
                        Volume = 16512;
                        VolumeNG = false;
                        break;
                }
            }
            else // Multiple
            {
                uint pos = FormAudio.FormAudioInstance.ButtonPosition - 1;
                switch (comboBox_Volume.SelectedIndex)
                {
                    case 0:
                        label_CustomVol.Enabled = false;
                        label_CustomVolHex.Enabled = false;
                        label_CustomVolHex.ForeColor = Color.Black;
                        label_CustomVolHex.Text = Localization.HexValueCaption + ": 0x3F00";
                        textBox_CustomVol.Enabled = false;
                        textBox_CustomVol.Text = "16128";
                        Volume = 16128;
                        MultipleVolume[pos] = Volume;
                        break;
                    case 1:
                        label_CustomVol.Enabled = false;
                        label_CustomVolHex.Enabled = false;
                        label_CustomVolHex.ForeColor = Color.Black;
                        label_CustomVolHex.Text = Localization.HexValueCaption + ": 0x4080";
                        textBox_CustomVol.Enabled = false;
                        textBox_CustomVol.Text = "16512";
                        Volume = 16512;
                        MultipleVolume[pos] = Volume;
                        break;
                    case 2:
                        label_CustomVol.Enabled = false;
                        label_CustomVolHex.Enabled = false;
                        label_CustomVolHex.ForeColor = Color.Black;
                        label_CustomVolHex.Text = Localization.HexValueCaption + ": 0x40C0";
                        textBox_CustomVol.Enabled = false;
                        textBox_CustomVol.Text = "16576";
                        Volume = 16576;
                        MultipleVolume[pos] = Volume;
                        break;
                    case 3:
                        label_CustomVol.Enabled = false;
                        label_CustomVolHex.Enabled = false;
                        label_CustomVolHex.ForeColor = Color.Black;
                        label_CustomVolHex.Text = Localization.HexValueCaption + ": 0x4100";
                        textBox_CustomVol.Enabled = false;
                        textBox_CustomVol.Text = "16640";
                        Volume = 16640;
                        MultipleVolume[pos] = Volume;
                        break;
                    case 4:
                        label_CustomVol.Enabled = true;
                        label_CustomVolHex.Enabled = true;
                        label_CustomVolHex.Text = Localization.HexValueCaption + ": 0x4080";
                        textBox_CustomVol.Enabled = true;
                        textBox_CustomVol.Text = "16512";
                        MultipleVolume[pos] = Volume;
                        break;
                    default:
                        label_CustomVol.Enabled = false;
                        label_CustomVolHex.Enabled = false;
                        label_CustomVolHex.ForeColor = Color.Black;
                        label_CustomVolHex.Text = Localization.HexValueCaption + ": 0x0";
                        textBox_CustomVol.Enabled = false;
                        textBox_CustomVol.Text = string.Empty;
                        Volume = 16512;
                        MultipleVolume[pos] = Volume;
                        break;
                }
            }
        }



        private void ComboBox_version_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsOpenMulti) // Single
            {
                switch (comboBox_version.SelectedIndex)
                {
                    case 0:
                        nuSoundVersion = 0x00010201;
                        break;
                    case 1:
                        nuSoundVersion = 0x00010200;
                        break;
                    case 2:
                        nuSoundVersion = 0x00000200;
                        break;
                    default:
                        nuSoundVersion = 0x00010201;
                        break;
                }
            }
            else // Multiple
            {
                uint pos = FormAudio.FormAudioInstance.ButtonPosition - 1;
                switch (comboBox_version.SelectedIndex)
                {
                    case 0:
                        nuSoundVersion = 0x00010201;
                        MultipleNuSoundVersion[pos] = 0x00010201;
                        break;
                    case 1:
                        nuSoundVersion = 0x00010200;
                        MultipleNuSoundVersion[pos] = 0x00010200;
                        break;
                    case 2:
                        nuSoundVersion = 0x00000200;
                        MultipleNuSoundVersion[pos] = 0x00000200;
                        break;
                    default:
                        nuSoundVersion = 0x00010201;
                        MultipleNuSoundVersion[pos] = 0x00010201;
                        break;
                }
                
            }
            
        }

        #endregion

        #region TextBox_TextChanged
        private void TextBox_CustomVol_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_CustomVol.Text))
            {
                label_CustomVolHex.ForeColor = Color.Red;
                label_CustomVolHex.Text = Localization.HexValueCaption + ": 0x0 (NG!)";
                VolumeNG = true;
            }
            else if (textBox_CustomVol.Text == "0")
            {
                MessageBox.Show(Localization.NotZeroFieldCaption, Localization.MSGBoxWarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_CustomVol.Text = string.Empty;
                VolumeNG = true;
            }
            else if (int.Parse(textBox_CustomVol.Text) >= 16128 && int.Parse(textBox_CustomVol.Text) <= 16704)
            {
                label_CustomVolHex.ForeColor = Color.Black;
                label_CustomVolHex.Text = Localization.HexValueCaption + ": 0x" + DecToHex(int.Parse(textBox_CustomVol.Text)).ToString("X");
                VolumeNG = false;
            }
            else
            {
                label_CustomVolHex.ForeColor = Color.Red;
                label_CustomVolHex.Text = Localization.HexValueCaption + ": 0x" + DecToHex(int.Parse(textBox_CustomVol.Text)).ToString("X") + " (NG!)";
                VolumeNG = true;
            }

        }

        private void TextBox_LoopStart_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_LoopStart.Text))
            {
                FormAudio.FormAudioInstance.LoopStartLabel = string.Empty;
                LoopStartNG = true;
            }
            else if (textBox_LoopStart.Text == "0")
            {
                FormAudio.FormAudioInstance.LoopStartLabel = string.Empty;
                textBox_LoopStart.Text = string.Empty;

                MessageBox.Show(Localization.NotZeroFieldCaption, Localization.MSGBoxWarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoopStartNG = true;
            }
            else
            {
                if (textBox_LoopStart.TextLength == 1)
                {
                    LoopStartNG = true;
                    return;
                }
                else
                {
                    switch (FormAudio.FormAudioInstance.SampleRate)
                    {
                        case 44100:
                            if (textBox_LoopStart.TextLength == 2 && int.Parse(textBox_LoopStart.Text) <= 43)
                            {
                                MessageBox.Show(Localization.LoopSample44kErrorCaption, Localization.MSGBoxWarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                FormAudio.FormAudioInstance.LoopStartLabel = string.Empty;
                                textBox_LoopStart.Text = string.Empty;
                                LoopStartNG = true;
                                return;
                            }
                            LoopStartNG = false;
                            break;
                        case 48000:
                            if (textBox_LoopStart.TextLength == 2 && int.Parse(textBox_LoopStart.Text) <= 47)
                            {
                                MessageBox.Show(Localization.LoopSample48kErrorCaption, Localization.MSGBoxWarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                FormAudio.FormAudioInstance.LoopStartLabel = string.Empty;
                                textBox_LoopStart.Text = string.Empty;
                                LoopStartNG = true;
                                return;
                            }
                            LoopStartNG = false;
                            break;
                        default:
                            LoopStartNG = true;
                            break;
                    }
                }
                
            }
        }

        private void TextBox_LoopEnd_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_LoopEnd.Text))
            {
                FormAudio.FormAudioInstance.LoopEndLabel = string.Empty;
                LoopEndNG = true;
            }
            else if (textBox_LoopEnd.Text == "0")
            {
                FormAudio.FormAudioInstance.LoopEndLabel = string.Empty;
                textBox_LoopEnd.Text = string.Empty;

                MessageBox.Show(Localization.NotZeroFieldCaption, Localization.MSGBoxWarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoopEndNG = true;
            }
            else
            {
                if (textBox_LoopEnd.TextLength == 1)
                {
                    LoopEndNG = true;
                    return;
                }
                else
                {
                    switch (FormAudio.FormAudioInstance.SampleRate)
                    {
                        case 44100:
                            if (textBox_LoopEnd.TextLength == 2 && int.Parse(textBox_LoopEnd.Text) <= 43)
                            {
                                MessageBox.Show(Localization.LoopSample44kErrorCaption, Localization.MSGBoxWarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                FormAudio.FormAudioInstance.LoopEndLabel = string.Empty;
                                textBox_LoopEnd.Text = string.Empty;
                                LoopEndNG = true;
                                return;
                            }
                            LoopEndNG = false;
                            break;
                        case 48000:
                            if (textBox_LoopEnd.TextLength == 2 && int.Parse(textBox_LoopEnd.Text) <= 47)
                            {
                                MessageBox.Show(Localization.LoopSample48kErrorCaption, Localization.MSGBoxWarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                FormAudio.FormAudioInstance.LoopEndLabel = string.Empty;
                                textBox_LoopEnd.Text = string.Empty;
                                LoopEndNG = true;
                                return;
                            }
                            LoopEndNG = false;
                            break;
                        default:
                            LoopEndNG = true;
                            break;
                    }
                }
                
            }
        }

        private void TextBox_id_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_id.Text))
            {
                IdNG = true;
                label_id_hex.ForeColor = Color.Red;
                label_id_hex.Text = "(NG!)";
            }
            else
            {
                if (textBox_id.Text == "00" || textBox_id.Text == "000")
                {
                    textBox_id.Text = "0";
                }
                IdNG = false;
                StreamId = ushort.Parse(textBox_id.Text);
                label_id_hex.ForeColor = Color.Black;
                label_id_hex.Text = "0x" + DecToHex(int.Parse(textBox_id.Text)).ToString("X");
            }
        }

        private void TextBox_nos_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_nos.Text))
            {
                NoSNG = true;
                label_nos_hex.ForeColor = Color.Red;
                label_nos_hex.Text = "(NG!)";
            }
            else
            {
                if (textBox_nos.Text == "0")
                {
                    MessageBox.Show(Localization.NotZeroFieldCaption, Localization.MSGBoxWarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox_nos.Text = string.Empty;
                    NoSNG = true;
                    label_nos_hex.ForeColor = Color.Red;
                    label_nos_hex.Text = "(NG!)";
                }
                else
                {
                    NoSNG = false;
                    NumberOfStreams = ushort.Parse(textBox_nos.Text);
                    label_nos_hex.ForeColor = Color.Black;
                    label_nos_hex.Text = "0x" + DecToHex(int.Parse(textBox_nos.Text)).ToString("X");
                }
            }

        }
        #endregion

        #region TextBox_KeyPress
        private void TextBox_CustomVol_KeyPress(object sender, KeyPressEventArgs e)
        {
            //バックスペースが押された時は有効（Deleteキーも有効）
            if (e.KeyChar == '\b')
            {
                return;
            }

            //数値0～9以外が押された時はイベントをキャンセルする
            if ((e.KeyChar < '0' || '9' < e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TextBox_LoopStart_KeyPress(object sender, KeyPressEventArgs e)
        {
            //バックスペースが押された時は有効（Deleteキーも有効）
            if (e.KeyChar == '\b')
            {
                return;
            }

            //数値0～9以外が押された時はイベントをキャンセルする
            if ((e.KeyChar < '0' || '9' < e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TextBox_LoopEnd_KeyPress(object sender, KeyPressEventArgs e)
        {
            //バックスペースが押された時は有効（Deleteキーも有効）
            if (e.KeyChar == '\b')
            {
                return;
            }

            //数値0～9以外が押された時はイベントをキャンセルする
            if ((e.KeyChar < '0' || '9' < e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TextBox_id_KeyPress(object sender, KeyPressEventArgs e)
        {
            //バックスペースが押された時は有効（Deleteキーも有効）
            if (e.KeyChar == '\b')
            {
                return;
            }

            //数値0～9以外が押された時はイベントをキャンセルする
            if ((e.KeyChar < '0' || '9' < e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TextBox_nos_KeyPress(object sender, KeyPressEventArgs e)
        {
            //バックスペースが押された時は有効（Deleteキーも有効）
            if (e.KeyChar == '\b')
            {
                return;
            }

            //数値0～9以外が押された時はイベントをキャンセルする
            if ((e.KeyChar < '0' || '9' < e.KeyChar))
            {
                e.Handled = true;
            }
        }
        #endregion

        #region SplashScreenCommon
        private static void StartThread()
        {
            fs = new FormSplash();
            Application.Run(fs);
        }


        private static void CloseSplash()
        {
            Dop d = new(CloseForm);
            fs?.Invoke(d);
        }

        private delegate void Dop();
        private static void CloseForm()
        {
            fs?.Close();
        }

        private delegate void Dmes(string message);
        private static void ShowMessage(string message)
        {
            fs!.label_log.Text = message;
        }
        #endregion

        private async void CheckForUpdatesUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    string hv = null!;

                    using Stream hcs = await Task.Run(() => Common.Network.GetWebStreamAsync(appUpdatechecker, Common.Network.GetUri("https://raw.githubusercontent.com/XyLe-GBP/NUBTool/master/VERSIONINFO")));
                    using StreamReader hsr = new(hcs);
                    hv = await Task.Run(hsr.ReadToEndAsync);
                    Common.Generic.GitHubLatestVersion = hv[8..].Replace("\n", "");

                    FileVersionInfo ver = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);

                    if (ver.FileVersion != null)
                    {
                        switch (ver.FileVersion.ToString().CompareTo(hv[8..].Replace("\n", "")))
                        {
                            case -1:
                                DialogResult dr = MessageBox.Show(Localization.LatestCaption + hv[8..].Replace("\n", "") + "\n" + Localization.CurrentCaption + ver.FileVersion + "\n" + Localization.UpdateConfirmCaption, Localization.MSGBoxConfirmCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (dr == DialogResult.Yes)
                                {
                                    using FormUpdateApplicationType fuat = new();
                                    if (fuat.ShowDialog() == DialogResult.Cancel) { return; }

                                    if (File.Exists(Directory.GetCurrentDirectory() + @"\res\nubtool.zip"))
                                    {
                                        File.Delete(Directory.GetCurrentDirectory() + @"\res\nubtool.zip");
                                    }

                                    Common.Generic.ProcessFlag = 4;
                                    Common.Generic.ProgressMax = 100;
                                    using FormProgress form = new();
                                    form.ShowDialog();

                                    if (Common.Generic.Result == false)
                                    {
                                        Common.Generic.cts.Dispose();
                                        MessageBox.Show(Localization.CancelledCaption, Localization.MSGBoxAbortedCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }

                                    string updpath = Directory.GetCurrentDirectory()[..Directory.GetCurrentDirectory().LastIndexOf('\\')];

                                    if (File.Exists(updpath + @"\updater.exe"))
                                    {
                                        File.Delete(updpath + @"\updater.exe");
                                    }
                                    if (Directory.Exists(updpath + @"\updater-temp"))
                                    {
                                        Common.Utils.DeleteDirectory(updpath + @"\updater-temp");
                                    }
                                    if (File.Exists(updpath + @"\nubtool.zip"))
                                    {
                                        File.Delete(updpath + @"\nubtool.zip");
                                    }

                                    File.Move(Directory.GetCurrentDirectory() + @"\res\updater.exe", updpath + @"\updater.exe");
                                    string wtext;
                                    switch (Common.Generic.ApplicationPortable)
                                    {
                                        case false:
                                            {
                                                wtext = Directory.GetCurrentDirectory() + "\r\nrelease";
                                            }
                                            break;
                                        case true:
                                            {
                                                wtext = Directory.GetCurrentDirectory() + "\r\nportable";
                                            }
                                            break;
                                    }
                                    File.WriteAllText(updpath + @"\updater.txt", wtext);
                                    File.Move(updpath + @"\updater.txt", updpath + @"\updater.dat");
                                    if (File.Exists(Directory.GetCurrentDirectory() + @"\res\nubtool.zip"))
                                    {
                                        File.Move(Directory.GetCurrentDirectory() + @"\res\nubtool.zip", updpath + @"\nubtool.zip");
                                    }

                                    ProcessStartInfo pi = new()
                                    {
                                        FileName = updpath + @"\updater.exe",
                                        Arguments = null,
                                        UseShellExecute = true,
                                        WindowStyle = ProcessWindowStyle.Normal,
                                    };
                                    Process.Start(pi);
                                    Close();
                                    return;
                                }
                                else
                                {
                                    DialogResult dr2 = MessageBox.Show(this, Localization.LatestCaption + hv[8..].Replace("\n", "") + "\n" + Localization.CurrentCaption + ver.FileVersion + "\n" + Localization.SiteOpenCaption, Localization.MSGBoxConfirmCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                    if (dr2 == DialogResult.Yes)
                                    {
                                        Common.Utils.OpenURI("https://github.com/XyLe-GBP/NUBTool/releases");
                                        return;
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                            case 0:
                                MessageBox.Show(this, Localization.LatestCaption + hv[8..].Replace("\n", "") + "\n" + Localization.CurrentCaption + ver.FileVersion + "\n" + Localization.UptodateCaption, Localization.MSGBoxSuccessCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            case 1:
                                throw new Exception(hv[8..].Replace("\n", "").ToString() + " < " + ver.FileVersion.ToString());
                        }
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, string.Format(Localization.UnExpectedCaption, ex.ToString()), Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show(this, Localization.NetworkNotConnectedCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}