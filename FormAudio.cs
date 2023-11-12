using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Text;
using static NUBTool.Common.Generic;

namespace NUBTool
{
    public partial class FormAudio : Form
    {
        FormMain FormMain;
        //private readonly WaveIn wi = new();
        private readonly WaveOut wo = new();
        WaveFileReader reader = null!;
        private OffsetSampleProvider osp = null!;
        //private AudioFileReader reader = null!;
        long Sample, Start = 0, End = 0;
        int bytePerSec, position, length, btnpos;
        TimeSpan time;
        bool mouseDown = false, stopflag = false;

        public FormAudio(FormMain formMain)
        {
            FormMain = formMain;
            InitializeComponent();

            trackBar_Main.Scroll += TrackBar_trk_Scroll;
            trackBar_LoopStart.Scroll += TrackBar_Start_Scroll;
            trackBar_LoopEnd.Scroll += TrackBar_End_Scroll;
            trackBar_Main.MouseDown += TrackBar_trk_MouseDown;
            trackBar_Main.MouseUp += TrackBar_trk_MouseUp;
            //label_Samples.Text = Localization.SampleCaption + ":";
            label_Psamples.Text = "0";
            //label_Length.Text = Localization.LengthCaption + ":";
            label_Plength.Text = "00:00:00";
            label_LoopStart.Text = "";
            label_LoopEnd.Text = "";
            timer_Reload.Interval = 1;
        }

        private void TrackBar_End_Scroll(object? sender, EventArgs e)
        {
            numericUpDown_LoopEnd.Value = trackBar_LoopEnd.Value;
        }

        private void TrackBar_Start_Scroll(object? sender, EventArgs e)
        {
            numericUpDown_LoopStart.Value = trackBar_LoopStart.Value;
        }

        private void TrackBar_trk_Scroll(object? sender, EventArgs e)
        {
            reader.CurrentTime = TimeSpan.FromMilliseconds(trackBar_Main.Value);
        }

        private void TrackBar_trk_MouseUp(object? sender, MouseEventArgs e)
        {
            if (!mouseDown) return;
            mouseDown = false;
        }

        private void TrackBar_trk_MouseDown(object? sender, MouseEventArgs e)
        {
            if (reader == null) return;
            mouseDown = true;
        }

        private void FormAudio_Load(object sender, EventArgs e)
        {
            if (OpenFilePaths.Length == 1)
            {
                reader = new(OpenFilePaths[0]);
                var fs = File.OpenRead(OpenFilePaths[0]);
                byte[] by = new byte[fs.Length];
                fs.Read(by, 0, by.Length);
                fs.Close();
                FileInfo fi = new(OpenFilePaths[0]);
                label_File.Text = fi.Name;
                button_Prev.Enabled = false;
                button_Next.Enabled = false;
            }
            else
            {
                reader = new(OpenFilePaths[0]);
                //RSS = new RawSourceWaveStream(reader, reader.WaveFormat);
                FileInfo fi = new(OpenFilePaths[0]);
                label_File.Text = fi.Name;
                button_Prev.Enabled = false;
                button_Next.Enabled = true;
                btnpos = 1;
            }

            wo.Init(reader);
            trackBar_Main.Minimum = 0;
            trackBar_Main.Maximum = (int)reader.TotalTime.TotalMilliseconds;
            trackBar_LoopStart.Minimum = 0;
            trackBar_LoopStart.Maximum = (int)reader.TotalTime.TotalMilliseconds;
            trackBar_LoopEnd.Minimum = 0;
            trackBar_LoopEnd.Maximum = (int)reader.TotalTime.TotalMilliseconds;
            trackBar_Main.TickFrequency = 1;
            trackBar_LoopStart.TickFrequency = 1;
            trackBar_LoopEnd.TickFrequency = 1;
            numericUpDown_LoopStart.Minimum = 0;
            numericUpDown_LoopStart.Maximum = (int)reader.TotalTime.TotalMilliseconds;
            numericUpDown_LoopStart.Increment = 1;
            numericUpDown_LoopEnd.Minimum = 0;
            numericUpDown_LoopEnd.Maximum = (int)reader.TotalTime.TotalMilliseconds;
            numericUpDown_LoopEnd.Increment = 1;
            wo.Volume = volumeSlider1.Volume;

            int tb = (int)reader.TotalTime.TotalMilliseconds / 2;
            numericUpDown_LoopEnd.Value = tb;
        }

        private void Button_Prev_Click(object sender, EventArgs e)
        {
            btnpos--;
            FileInfo fi = new(Common.Generic.OpenFilePaths[btnpos - 1]);
            label_File.Text = fi.Name;

            wo.Stop();
            button_Play.Text = "Play";
            reader.Position = 0;
            reader.Close();
            button_Stop.Enabled = false;

            if (btnpos == 1)
            {
                reader = new(Common.Generic.OpenFilePaths[btnpos - 1]);
                wo.Init(reader);
                ResetAFR();
                button_Prev.Enabled = false;
                button_Next.Enabled = true;
            }
            else
            {
                reader = new(Common.Generic.OpenFilePaths[btnpos - 1]);
                wo.Init(reader);
                ResetAFR();
                button_Prev.Enabled = true;
                button_Next.Enabled = true;
            }
        }

        private void Button_Next_Click(object sender, EventArgs e)
        {
            btnpos++;
            FileInfo fi = new(Common.Generic.OpenFilePaths[btnpos - 1]);
            label_File.Text = fi.Name;

            wo.Stop();
            button_Play.Text = "Play";
            reader.Position = 0;
            reader.Close();
            button_Stop.Enabled = false;

            if (btnpos == Common.Generic.OpenFilePaths.Length)
            {
                reader = new(Common.Generic.OpenFilePaths[btnpos - 1]);
                wo.Init(reader);
                ResetAFR();
                button_Next.Enabled = false;
                button_Prev.Enabled = true;
            }
            else
            {
                reader = new(Common.Generic.OpenFilePaths[btnpos - 1]);
                wo.Init(reader);
                ResetAFR();
                button_Next.Enabled = true;
                button_Prev.Enabled = true;
            }
        }

        private void NumericUpDown_LoopStart_ValueChanged(object sender, EventArgs e)
        {
            trackBar_LoopStart.Value = (int)numericUpDown_LoopStart.Value;
        }

        private void NumericUpDown_LoopEnd_ValueChanged(object sender, EventArgs e)
        {
            trackBar_LoopEnd.Value = (int)numericUpDown_LoopEnd.Value;
        }

        private void FormAudio_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (timer_Reload.Enabled == true) timer_Reload.Enabled = false;
            reader.Position = 0;
            //wi.Dispose();
            wo.Stop();
            wo.Dispose();
            reader.Close();
            reader = null!;
        }

        private void VolumeSlider1_VolumeChanged(object sender, EventArgs e)
        {
            wo.Volume = volumeSlider1.Volume;
        }

        private void Button_LS_Current_Click(object sender, EventArgs e)
        {
            trackBar_LoopStart.Value = trackBar_Main.Value;
            numericUpDown_LoopStart.Value = trackBar_LoopStart.Value;
        }

        private void Button_LE_Current_Click(object sender, EventArgs e)
        {
            trackBar_LoopEnd.Value = trackBar_Main.Value;
            numericUpDown_LoopEnd.Value = trackBar_LoopEnd.Value;
        }

        private void Button_SetStart_Click(object sender, EventArgs e)
        {
            long pos;
            TimeSpan oldc = reader.CurrentTime;
            reader.CurrentTime = TimeSpan.FromMilliseconds(trackBar_LoopStart.Value);
            pos = reader.Position / reader.WaveFormat.BlockAlign;
            reader.CurrentTime = oldc;
            label_LoopStart.Text = "LoopStart: " + pos.ToString() + " ";
            Start = pos;
            FormMain.textBox_LoopStart.Text = pos.ToString();
        }

        private void Button_SetEnd_Click(object sender, EventArgs e)
        {
            long pos;
            TimeSpan oldc = reader.CurrentTime;
            reader.CurrentTime = TimeSpan.FromMilliseconds(trackBar_LoopEnd.Value);
            pos = reader.Position / reader.WaveFormat.BlockAlign;
            reader.CurrentTime = oldc;
            label_LoopEnd.Text = "LoopEnd: " + pos.ToString() + " ";
            End = pos;
            FormMain.textBox_LoopEnd.Text = pos.ToString();
        }

        private void Button_Play_Click(object sender, EventArgs e)
        {
            switch (wo.PlaybackState)
            {
                case PlaybackState.Stopped:
                    bytePerSec = reader.WaveFormat.BitsPerSample / 8 * reader.WaveFormat.SampleRate * reader.WaveFormat.Channels;
                    length = (int)reader.Length / bytePerSec;

                    stopflag = false;
                    timer_Reload.Enabled = true;
                    wo.Play();
                    button_Play.Text = "Paused";
                    Task.Run(() => Playback());
                    button_Stop.Enabled = true;
                    break;
                case PlaybackState.Paused:
                    wo.Play();
                    button_Play.Text = "Paused";
                    break;
                case PlaybackState.Playing:
                    wo.Pause();
                    button_Play.Text = "Play";
                    break;
            }
        }

        private void Button_Stop_Click(object sender, EventArgs e)
        {
            if (wo.PlaybackState != PlaybackState.Stopped)
            {
                stopflag = true;
                wo.Stop();
                button_Play.Text = "Play";
                reader.Position = 0;
                button_Stop.Enabled = false;
            }
        }

        private void Playback()
        {
            while (wo.PlaybackState != PlaybackState.Stopped)
            {
                position = (int)reader.Position / reader.WaveFormat.AverageBytesPerSecond;
                time = new(0, 0, position);
                Sample = reader.Position / reader.BlockAlign + wo.GetPosition() / reader.BlockAlign;
            }
        }

        private void Timer_Reload_Tick(object sender, EventArgs e)
        {
            if (!mouseDown) trackBar_Main.Value = (int)reader.CurrentTime.TotalMilliseconds;
            if (IsEnableLoop == true && reader.CurrentTime >= TimeSpan.FromMilliseconds(trackBar_LoopEnd.Value))
            {
                reader.CurrentTime = TimeSpan.FromMilliseconds(trackBar_LoopStart.Value);
            }
            if (reader.CurrentTime == reader.TotalTime)
            {
                stopflag = true;
                wo.Stop();
                button_Play.Text = "Play";
                reader.Position = 0;
                button_Stop.Enabled = false;
            }
            else if (reader.Position == 0 || trackBar_Main.Value == 0)
            {
                if (!stopflag)
                {
                    wo.Stop();
                    button_Stop.Enabled = false;
                    Sample = 0;
                    reader.Position = 0;
                    wo.Play();
                    button_Play.Text = "Pause";
                    Task.Run(Playback);
                    button_Stop.Enabled = true;
                }
            }
            //label_Length.Text = Localization.LengthCaption + ":";
            label_Plength.Text = time.ToString(@"hh\:mm\:ss");
            StringBuilder str = new(Sample.ToString());
            //label_Samples.Text = Localization.SampleCaption + ":";
            label_Psamples.Text = str.ToString();
        }

        private void ResetAFR()
        {
            trackBar_Main.Minimum = 0;
            trackBar_Main.Maximum = (int)reader.TotalTime.TotalMilliseconds;
            trackBar_LoopStart.Minimum = 0;
            trackBar_LoopStart.Maximum = (int)reader.TotalTime.TotalMilliseconds;
            trackBar_LoopEnd.Minimum = 0;
            trackBar_LoopEnd.Maximum = (int)reader.TotalTime.TotalMilliseconds;
            trackBar_Main.TickFrequency = 1;
            trackBar_LoopStart.TickFrequency = 1;
            trackBar_LoopEnd.TickFrequency = 1;
            numericUpDown_LoopStart.Minimum = 0;
            numericUpDown_LoopStart.Maximum = (int)reader.TotalTime.TotalMilliseconds;
            numericUpDown_LoopStart.Increment = 1;
            numericUpDown_LoopEnd.Minimum = 0;
            numericUpDown_LoopEnd.Maximum = (int)reader.TotalTime.TotalMilliseconds;
            numericUpDown_LoopEnd.Increment = 1;
            wo.Volume = volumeSlider1.Volume;

            int tb = (int)reader.TotalTime.TotalMilliseconds / 2;
            numericUpDown_LoopStart.Value = 0;
            numericUpDown_LoopEnd.Value = tb;
        }
    }
}
