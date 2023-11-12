using static NUBTool.Common.Generic;
using static NUBTool.Common.Utils;
using static NUBTool.Common.NUB;
using static NUBTool.Common;

namespace NUBTool
{
    public partial class FormMain : Form
    {
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
            if (!Directory.Exists(TempPath))
            {
                Directory.CreateDirectory(TempPath);
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (formAudio is not null && formAudio.Visible == true)
            {
                formAudio.Close();
            }
            if (Directory.Exists(TempPath))
            {
                Directory.Delete(TempPath, true);
            }
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
            label_CustomVolHex.Text = "Hex: 0x4080";
            label_LS.Enabled = false;
            label_LE.Enabled = false;
            label_Channel.Enabled = false;
            label_version.Enabled = false;
        }

        private void SetMetaLabels()
        {
            if (IsNUB)
            {
                int lst = NUBMetadataBuffers[2] * 4, let = NUBMetadataBuffers[3] * 4;
                label_Filename.Text = "FILE_NAME: " + GetFileName(OriginalPath);
                label_OrigFilepath.Text = "FILE_PATH: " + OriginalPath;
                label_id.Text = "STREAM_ID: " + NUBMetadataBuffers[7].ToString() + " (0x" + DecToHex(NUBMetadataBuffers[7]).ToString("X") + ")";
                label_NStream.Text = "NUMBER_OF_STREAMS: " + NUBMetadataBuffers[8].ToString() + " (0x" + DecToHex(NUBMetadataBuffers[8]).ToString("X") + ")";
                label_Totalstream.Text = "(meta) STREAM_TOTAL: " + NUBMetadataBuffers[1].ToString() + " (FILE_SIZE: " + GetFileSize(OpenFilePaths[0]).ToString() + " )";
                label_lsPosition.Text = "(meta) LOOP_START: " + NUBMetadataBuffers[2].ToString() + " (Dec Binary: " + lst.ToString() + ")";
                label_lePosition.Text = "(meta) LOOP_END: " + NUBMetadataBuffers[3].ToString() + " (Dec Binary: " + let.ToString() + ")";
                label_Volume_Info.Text = "(meta) VOLUME: " + NUBMetadataBuffers[4].ToString() + " (0x" + DecToHex(NUBMetadataBuffers[4]).ToString("X") + ")";
                label_Channel_Info.Text = "(meta) CHANNELS: " + NUBMetadataBuffers[5].ToString();
                label_Samplerate_Info.Text = "(meta) SAMPLE_LATES: " + NUBMetadataBuffers[6].ToString() + "Hz";
            }
            else
            {
                label_Filename.Text = "FILE_NAME: " + GetFileName(OriginalPath);
                label_OrigFilepath.Text = "FILE_PATH: " + OriginalPath;
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
            foreach (string file in files)
            {
                if (File.Exists(Generic.TempPath + GetFileName(file) + ".pcm"))
                {
                    File.Delete(Generic.TempPath + GetFileName(file) + ".pcm");
                }
                if (File.Exists(Generic.TempPath + GetFileName(file) + ".wav"))
                {
                    File.Delete(Generic.TempPath + GetFileName(file) + ".wav");
                }
            }
        }

        private void CheckBox_LoopEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_LoopEnable.Checked != true)
            {
                formAudio.trackBar_LoopStart.Enabled = false;
                formAudio.trackBar_LoopEnd.Enabled = false;
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
                formAudio.trackBar_LoopStart.Enabled = true;
                formAudio.trackBar_LoopEnd.Enabled = true;
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
        }

        private void Button_Run_Click(object sender, EventArgs e)
        {
            if (IdNG || NoSNG)
            {
                if (IdNG && !NoSNG)
                {
                    MessageBox.Show("The value at the stream id is incorrect.\nPlease set all items to correct values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (!IdNG && NoSNG)
                {
                    MessageBox.Show("Incorrect number of streams value. Please set the correct value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    MessageBox.Show("The value at the stream id or the number of stream value is incorrect.\nPlease set all items to correct values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (LoopNG || VolumeNG)
            {
                if (LoopNG && !VolumeNG)
                {
                    MessageBox.Show("The value at the loop start point or the value at the loop end point is incorrect.\nPlease set all items to correct values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (!LoopNG && VolumeNG)
                {
                    MessageBox.Show("Incorrect volume value. Please set the correct value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    MessageBox.Show("The value at the loop start point or the value at the loop end point or the volume value is incorrect.\nPlease set all items to correct values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (checkBox_LoopEnable.Checked == true)
            {
                if (string.IsNullOrEmpty(textBox_LoopStart.Text) || string.IsNullOrEmpty(textBox_LoopEnd.Text))
                {
                    MessageBox.Show("Loop sample value is not set correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    string np = Wav2Raw(OpenFilePaths[0], Signed, ByteOrderBE, SampleRate, Channels, "-b " + Bits);
                    if (CreateNUBHeader(np))
                    {
                        np = np + ".nubh";

                        LoopStartSamples = int.Parse(textBox_LoopStart.Text);
                        LoopEndSamples = int.Parse(textBox_LoopEnd.Text);

                        if (WriteMetadatas(np))
                        {
                            if (File.Exists(sfd.FileName))
                            {
                                File.Delete(sfd.FileName);
                            }
                            File.Move(np, sfd.FileName);
                            MessageBox.Show("NUB file generation is completed.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (formAudio.Visible == true)
                            {
                                formAudio.Close();
                            }
                            IsNUB = false;

                            closeCToolStripMenuItem.Enabled = false;
                            DeleteCurrentFiles(OpenFilePaths);
                            OpenFilePaths = null;
                            ResetControl();
                            return;
                        }
                        else
                        {
                            MessageBox.Show(string.Format("An error has occurred.\n{0}", ExceptionInformation), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show(string.Format("An error has occurred.\n{0}", ExceptionInformation), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    if (NumberOfStreams != 1) // Multi NUB (alpha)
                    {
                        int[] offsetbuf = new int[NumberOfStreams];
                        List<string> raws = new();
                        foreach (var item in OpenFilePaths)
                        {
                            string c = Wav2Raw(item, Signed, ByteOrderBE, SampleRate, Channels, "-b " + Bits);
                            raws.Add(c);
                        }

                        if (CreateMultiRaw(raws.ToArray()))
                        {
                            if (CreateMultiNUBHeader(OpenFilePaths[0], NumberOfStreams, GetStreamLength(OpenFilePaths), offsetbuf))
                            {
                                if (WriteMetadatasWithMulti(OpenFilePaths[0] + ".nubh", offsetbuf))
                                {
                                    if (File.Exists(sfd.FileName))
                                    {
                                        File.Delete(sfd.FileName);
                                    }
                                    File.Move(OpenFilePaths[0] + ".nubh", sfd.FileName);
                                    MessageBox.Show("NUB file generation is completed.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    IsNUB = false;
                                    closeCToolStripMenuItem.Enabled = false;
                                    DeleteCurrentFiles(OpenFilePaths);
                                    OpenFilePaths = null;
                                    ResetControl();
                                    return;
                                }
                                else
                                {
                                    MessageBox.Show(string.Format("An error has occurred.\n{0}", ExceptionInformation), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show(string.Format("An error has occurred.\n{0}", ExceptionInformation), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show(string.Format("An error has occurred.\n{0}", ExceptionInformation), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }


                    }
                    else // Single NUB
                    {
                        string np = Wav2Raw(OpenFilePaths[0], Signed, ByteOrderBE, SampleRate, Channels, "-b " + Bits);
                        if (CreateNUBHeader(np))
                        {
                            FileInfo fi = new(OpenFilePaths[0]);

                            if (WriteMetadatas(np))
                            {
                                if (File.Exists(sfd.FileName))
                                {
                                    File.Delete(sfd.FileName);
                                }
                                File.Copy(np, sfd.FileName);
                                MessageBox.Show("NUB file generation is completed.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                IsNUB = false;
                                closeCToolStripMenuItem.Enabled = false;
                                DeleteCurrentFiles(OpenFilePaths);
                                OpenFilePaths = null;
                                ResetControl();
                                return;
                            }
                            else
                            {
                                MessageBox.Show(string.Format("An error has occurred.\n{0}", ExceptionInformation), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show(string.Format("An error has occurred.\n{0}", ExceptionInformation), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private void Label_Openhere_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void Label_Openhere_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data is not null)
            {
                OpenFilePaths = (string[]?)e.Data.GetData(DataFormats.FileDrop, false);
                if (OpenFilePaths is not null && OpenFilePaths.Length == 1)
                {
                    if (PathToExtension(OpenFilePaths[0]).ToUpper() == ".NUB")
                    {
                        Generic.OriginalPath = OpenFilePaths[0];
                        ReadMetadatas(OpenFilePaths[0], NUBMetadataBuffers);
                        if (DeleteNUBHeaderWraw2wav(OpenFilePaths[0]))
                        {
                            IsNUB = true;
                            InitializeSubControls();
                            SetMetaLabels();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (PathToExtension(OpenFilePaths[0]).ToUpper() == ".RAW" || PathToExtension(OpenFilePaths[0]).ToUpper() == ".SND" || PathToExtension(OpenFilePaths[0]).ToUpper() == ".WAV" || PathToExtension(OpenFilePaths[0]).ToUpper() == ".MP3" || PathToExtension(OpenFilePaths[0]).ToUpper() == ".FLAC" || PathToExtension(OpenFilePaths[0]).ToUpper() == ".AIF" || PathToExtension(OpenFilePaths[0]).ToUpper() == ".AIFF")
                    {
                        Generic.OriginalPath = OpenFilePaths[0];
                        OpenFilePaths[0] = Any2Wav(OpenFilePaths[0]);
                        IsNUB = false;
                        InitializeSubControls();
                        SetMetaLabels();
                    }
                    else
                    {
                        return;
                    }
                    closeCToolStripMenuItem.Enabled = true;
                    return;
                }
                else
                {
                    MessageBox.Show("Loading multiple files is not supported.\r\nPlease wait for future updates.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

        }

        #region MenuItem

        private void OpenOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new()
            {
                FilterIndex = 7,
                Filter = "Namco nuSound / nuSound2 (*.nub)|*.nub|Read After Write Sound (*.raw,*.snd)|*.raw;*.snd|RIFF waveform Audio Format (*.wav)|*.wav|MPEG-1 audio layer 3 (*.mp3)|*.mp3|Free Lossless Audio Codec (*.flac)|*.flac|Audio Interchange File Format (*.aif,*.aiff)|*.aif;*.aiff|All Supported Files|*.nub;*.raw;*.snd;*.wav;*.mp3;*.flac;*.aif;*.aiff",
                Multiselect = true,
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ResetControl();

                OpenFilePaths = ofd.FileNames;
                if (OpenFilePaths.Length == 1)
                {
                    if (PathToExtension(OpenFilePaths[0]).ToUpper() == ".NUB")
                    {
                        Generic.OriginalPath = OpenFilePaths[0];
                        ReadMetadatas(OpenFilePaths[0], NUBMetadataBuffers);
                        if (DeleteNUBHeaderWraw2wav(OpenFilePaths[0]))
                        {
                            IsNUB = true;
                            InitializeSubControls();
                            SetMetaLabels();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (PathToExtension(OpenFilePaths[0]).ToUpper() == ".RAW" || PathToExtension(OpenFilePaths[0]).ToUpper() == ".SND" || PathToExtension(OpenFilePaths[0]).ToUpper() == ".WAV" || PathToExtension(OpenFilePaths[0]).ToUpper() == ".MP3" || PathToExtension(OpenFilePaths[0]).ToUpper() == ".FLAC" || PathToExtension(OpenFilePaths[0]).ToUpper() == ".AIF" || PathToExtension(OpenFilePaths[0]).ToUpper() == ".AIFF")
                    {
                        Generic.OriginalPath = OpenFilePaths[0];
                        OpenFilePaths[0] = Any2Wav(OpenFilePaths[0]);
                        IsNUB = false;
                        InitializeSubControls();
                        SetMetaLabels();
                    }
                    else
                    {
                        return;
                    }
                    closeCToolStripMenuItem.Enabled = true;
                    return;
                }
                else // multiple
                {
                    MessageBox.Show("Loading multiple files is not supported.\r\nPlease wait for future updates.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                return;
            }

        }

        private void closeCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formAudio.Visible == true)
            {
                formAudio.Close();
            }
            DeleteCurrentFiles(OpenFilePaths);
            IsNUB = false;
            OpenFilePaths = null;
            closeCToolStripMenuItem.Enabled = false;
            ResetControl();
        }

        private void AboutAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using FormAbout Form = new();
            Form.ShowDialog();
        }

        private void PreferencesSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using FormPreferences Form = new();
            Form.ShowDialog();
        }

        private void ExitXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
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
                    Signed = false;
                    ByteOrderBE = true;
                    Format = 2;
                    break;
                case 3:
                    Signed = false;
                    ByteOrderBE = false;
                    Format = 3;
                    break;
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

        private void ComboBox_Volume_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox_Volume.SelectedIndex)
            {
                case 0:
                    label_CustomVol.Enabled = false;
                    label_CustomVolHex.Enabled = false;
                    label_CustomVolHex.ForeColor = Color.Black;
                    label_CustomVolHex.Text = "Hex: 0x3F00";
                    textBox_CustomVol.Enabled = false;
                    textBox_CustomVol.Text = "16128";
                    Volume = 16128;
                    break;
                case 1:
                    label_CustomVol.Enabled = false;
                    label_CustomVolHex.Enabled = false;
                    label_CustomVolHex.ForeColor = Color.Black;
                    label_CustomVolHex.Text = "Hex: 0x4080";
                    textBox_CustomVol.Enabled = false;
                    textBox_CustomVol.Text = "16512";
                    Volume = 16512;
                    break;
                case 2:
                    label_CustomVol.Enabled = false;
                    label_CustomVolHex.Enabled = false;
                    label_CustomVolHex.ForeColor = Color.Black;
                    label_CustomVolHex.Text = "Hex: 0x40C0";
                    textBox_CustomVol.Enabled = false;
                    textBox_CustomVol.Text = "16576";
                    Volume = 16576;
                    break;
                case 3:
                    label_CustomVol.Enabled = false;
                    label_CustomVolHex.Enabled = false;
                    label_CustomVolHex.ForeColor = Color.Black;
                    label_CustomVolHex.Text = "Hex: 0x4100";
                    textBox_CustomVol.Enabled = false;
                    textBox_CustomVol.Text = "16640";
                    Volume = 16640;
                    break;
                case 4:
                    label_CustomVol.Enabled = true;
                    label_CustomVolHex.Enabled = true;
                    label_CustomVolHex.Text = "Hex: 0x4080";
                    textBox_CustomVol.Enabled = true;
                    textBox_CustomVol.Text = "16512";
                    break;
                default:
                    label_CustomVol.Enabled = false;
                    label_CustomVolHex.Enabled = false;
                    label_CustomVolHex.ForeColor = Color.Black;
                    label_CustomVolHex.Text = "Hex: 0x0";
                    textBox_CustomVol.Enabled = false;
                    textBox_CustomVol.Text = string.Empty;
                    Volume = 16512;
                    break;
            }
        }



        private void ComboBox_version_SelectedIndexChanged(object sender, EventArgs e)
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

#endregion

        #region TextBox_TextChanged
        private void TextBox_CustomVol_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_CustomVol.Text))
            {
                label_CustomVolHex.ForeColor = Color.Red;
                label_CustomVolHex.Text = "Hex: 0x0 (NG!)";
                VolumeNG = true;
            }
            else if (textBox_CustomVol.Text == "0")
            {
                MessageBox.Show("The value of this field cannot be 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_CustomVol.Text = string.Empty;
                VolumeNG = true;
            }
            else if (int.Parse(textBox_CustomVol.Text) >= 16128 && int.Parse(textBox_CustomVol.Text) <= 16704)
            {
                label_CustomVolHex.ForeColor = Color.Black;
                label_CustomVolHex.Text = "Hex: 0x" + DecToHex(int.Parse(textBox_CustomVol.Text)).ToString("X");
                VolumeNG = false;
            }
            else
            {
                label_CustomVolHex.ForeColor = Color.Red;
                label_CustomVolHex.Text = "Hex: 0x" + DecToHex(int.Parse(textBox_CustomVol.Text)).ToString("X") + " (NG!)";
                VolumeNG = true;
            }

        }

        private void TextBox_LoopStart_TextChanged(object sender, EventArgs e)
        {
            if (textBox_LoopStart.Text == "0")
            {
                MessageBox.Show("The value of this field cannot be 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_LoopStart.Text = string.Empty;
                LoopNG = true;
            }
            else
            {
                LoopNG = false;
            }
        }

        private void TextBox_LoopEnd_TextChanged(object sender, EventArgs e)
        {
            if (textBox_LoopEnd.Text == "0")
            {
                MessageBox.Show("The value of this field cannot be 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_LoopEnd.Text = string.Empty;
                LoopNG = true;
            }
            else
            {
                LoopNG = false;
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
                    MessageBox.Show("The value of this field cannot be 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        
    }
}