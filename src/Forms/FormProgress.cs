using MediaToolkit.Model;
using MediaToolkit.Options;
using MediaToolkit;
using System.ComponentModel;
using NUBTool.Localizable;
using static NUBTool.Common;
using System.Threading.Channels;

namespace NUBTool.src.Forms
{
    public partial class FormProgress : Form
    {
        int EncCount = 0, EncErrorCount = 0;
        private static FormProgress _formProgressInstance = null!;
        public static FormProgress FormProgressInstance
        {
            get
            {
                return _formProgressInstance;
            }
            set
            {
                _formProgressInstance = value;
            }
        }

        public int EncodeCount
        {
            get
            {
                return EncCount;
            }
        }

        public int EncodeErrorCount
        {
            get
            {
                return EncErrorCount;
            }
        }

        public FormProgress()
        {
            InitializeComponent();
        }

        private int DownloadProgress = 0;
        private string DownloadStatus = "";

        private void FormProgress_Load(object sender, EventArgs e)
        {
            _formProgressInstance = this;

            Text = Localization.ProcessingCaption;
            timer_interval.Interval = 2000;
            progressBar_MainProgress.Value = 0;
            progressBar_MainProgress.Minimum = 0;
            progressBar_MainProgress.Maximum = Generic.ProgressMax;
            RunTask();
        }

        private async void RunTask()
        {
            switch (Generic.ProcessFlag)
            {
                case 0: // Decode
                    {
                        Generic.cts = new CancellationTokenSource();
                        var cToken = Generic.cts.Token;
                        var p = new Progress<int>(UpdateProgress);
                        label_Caption.Text = Localization.NUBDecodingCaption;
                        Generic.Result = await Task.Run(() => Decode_DoWork(p, cToken));
                        break;
                    }
                case 1: // Encode
                    {
                        Generic.cts = new CancellationTokenSource();
                        var cToken = Generic.cts.Token;
                        var p = new Progress<int>(UpdateProgress);
                        label_Caption.Text = Localization.NUBEncodingCaption;
                        Generic.Result = await Task.Run(() => Encode_DoWork(p, cToken));
                        break;
                    }
                case 2: // Audio To Wave
                    {
                        Generic.cts = new CancellationTokenSource();
                        var cToken = Generic.cts.Token;
                        var p = new Progress<int>(UpdateProgress);
                        label_Caption.Text = Localization.ProcessingCaption;

                        Generic.Result = await Task.Run(() => AudioConverter_ATW_DoWork(p, cToken));
                        break;
                    }
                case 3: // Wave To Audio
                    {
                        Generic.cts = new CancellationTokenSource();
                        var cToken = Generic.cts.Token;
                        var p = new Progress<int>(UpdateProgress);
                        label_Caption.Text = Localization.ProcessingCaption;

                        Generic.Result = await Task.Run(() => AudioConverter_WTA_DoWork(p, cToken));
                        break;
                    }
                case 4: // Update Program
                    {
                        Text = Localization.ProcessingCaption;
                        label_Caption.Text = Localization.DownloadStatusCaption;
                        label_Status.Text = Localization.InitializationCaption;
                        Generic.cts = new CancellationTokenSource();
                        var cToken = Generic.cts.Token;
                        var p = new Progress<int>(UpdateProgress);

                        Generic.Result = await Task.Run(() => Download_DoWork(p, cToken));
                        break;
                    }
                default:
                    Close();
                    break;
            }
            timer_interval.Enabled = true;
        }

        private static bool Decode_DoWork(IProgress<int> p, CancellationToken cToken)
        {
            string[] file = Generic.OpenFilePaths;
            if (file.Length == 1)
            {
                NUB.ReadMetadatas(file[0], Generic.NUBMetadataBuffers);
                if (Generic.NUBMetadataBuffers[8] != 0 && Generic.NUBMetadataBuffers[8] > 1)
                {
                    MessageBox.Show("This file has multiple streams.\r\nNUBs with multiple streams cannot be read.", Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                using Stream fs = File.OpenRead(file[0]);
                using var br = new BinaryReader(fs);
                byte[] header = br.ReadBytes(256);
                if (Utils.ByteArrayCompare(header, header))
                {
                    long len = fs.Length - 256;
                    byte[] r_data = new byte[len];
                    br.Read(r_data, 0, (int)len);
                    using var fs2 = new FileStream(Generic.TempPath + Utils.GetFileName(file[0]) + ".pcm", FileMode.Create);
                    using var bw = new BinaryWriter(fs2);
                    bw.Seek(0, SeekOrigin.Begin);
                    bw.Write(r_data);

                    Generic.OpenFilePaths[0] = Utils.SoX_Any2Wav(Generic.TempPath + Utils.GetFileName(file[0]) + ".pcm", true);

                    p.Report(1);
                    Generic.IsNTW = true;
                    return true;
                }
                else
                {
                    MessageBox.Show(Localization.NUBHeaderNotFoundErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                Generic.NUBMultiMetadataBuffers.Clear();
                int count = 0;
                foreach (var read in file)
                {
                    if (cToken.IsCancellationRequested == true)
                    {
                        return false;
                    }
                    int[] metadatas = new int[9];
                    NUB.ReadMetadatas(read, metadatas);
                    if (metadatas[8] != 0 && metadatas[8] > 1)
                    {
                        MessageBox.Show("This file has multiple streams.\r\nNUBs with multiple streams cannot be read.", Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    using Stream fs = File.OpenRead(read);
                    using var br = new BinaryReader(fs);
                    byte[] header = br.ReadBytes(256);
                    if (Utils.ByteArrayCompare(header, header))
                    {
                        long len = fs.Length - 256;
                        byte[] r_data = new byte[len];
                        br.Read(r_data, 0, (int)len);
                        using var fs2 = new FileStream(Generic.TempPath + Utils.GetFileName(read) + ".pcm", FileMode.Create);
                        using var bw = new BinaryWriter(fs2);
                        bw.Seek(0, SeekOrigin.Begin);
                        bw.Write(r_data);

                        Generic.OpenFilePaths[count] = Utils.SoX_Any2Wav(Generic.TempPath + Utils.GetFileName(read) + ".pcm", true);

                        Generic.NUBMultiMetadataBuffers.Add(metadatas);
                        count++;
                        p.Report(count);
                        continue;
                    }
                    else
                    {
                        MessageBox.Show(Localization.NUBHeaderNotFoundErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (file.Length == Generic.OpenFilePaths.Length)
                {
                    Generic.IsNTW = true;
                    return true;
                }
                return false;
            }
        }

        private bool Encode_DoWork(IProgress<int> p, CancellationToken cToken)
        {
            if (Generic.OpenFilePaths.Length == 1) // Single
            {
                if (FormMain.FormMainInstance.checkBox_LoopEnable.Checked) // Looped
                {
                    string np = Utils.Wav2Raw(Generic.OpenFilePaths[0], Generic.Signed, Generic.ByteOrderBE, Generic.SampleRate, Generic.Channels, "-b " + Generic.Bits);
                    if (NUB.CreateNUBHeader(np))
                    {
                        np = np + ".nubh";

                        Generic.LoopStartSamples = int.Parse(FormMain.FormMainInstance.textBox_LoopStart.Text);
                        Generic.LoopEndSamples = int.Parse(FormMain.FormMainInstance.textBox_LoopEnd.Text);

                        if (NUB.WriteMetadatas(np))
                        {
                            if (File.Exists(Generic.SavePath))
                            {
                                File.Delete(Generic.SavePath);
                            }
                            File.Move(np, Generic.SavePath);

                            if (File.Exists(Generic.SavePath))
                            {
                                p.Report(1);
                                return true;
                            }
                            else
                            {
                                p.Report(1);
                                return false;
                            }

                        }
                        else
                        {
                            p.Report(1);
                            ExceptionError("Failed to write NUB metadata information.");
                            return false;
                        }
                    }
                    else
                    {
                        p.Report(1);
                        ExceptionError("Failed to write NUB header information.");
                        return false;
                    }
                }
                else // No Looped
                {
                    if (Generic.NumberOfStreams != 1) // Multi Stream NUB (alpha)
                    {
                        int[] offsetbuf = new int[Generic.NumberOfStreams];
                        List<string> raws = new();
                        foreach (var item in Generic.OpenFilePaths)
                        {
                            string c = Utils.Wav2Raw(item, Generic.Signed, Generic.ByteOrderBE, Generic.SampleRate, Generic.Channels, "-b " + Generic.Bits);
                            raws.Add(c);
                        }

                        if (NUB.CreateMultiRaw(raws.ToArray()))
                        {
                            if (NUB.CreateMultiNUBHeader(Generic.OpenFilePaths[0], Generic.NumberOfStreams, Utils.GetStreamLength(Generic.OpenFilePaths), offsetbuf))
                            {
                                if (NUB.WriteMetadatasWithMulti(Generic.OpenFilePaths[0] + ".nubh", offsetbuf))
                                {
                                    if (File.Exists(Generic.SavePath))
                                    {
                                        File.Delete(Generic.SavePath);
                                    }
                                    File.Move(Generic.OpenFilePaths[0] + ".nubh", Generic.SavePath);

                                    p.Report(1);
                                    return true;
                                }
                                else
                                {
                                    p.Report(1);
                                    ExceptionError("Failed to write Multi NUB metadata information.");
                                    return false;
                                }
                            }
                            else
                            {
                                p.Report(1);
                                ExceptionError("Failed to write Multi NUB header information.");
                                return false;
                            }
                        }
                        else
                        {
                            p.Report(1);
                            return false;
                        }
                    }
                    else
                    {
                        string np = Utils.Wav2Raw(Generic.OpenFilePaths[0], Generic.Signed, Generic.ByteOrderBE, Generic.SampleRate, Generic.Channels, "-b " + Generic.Bits);
                        if (NUB.CreateNUBHeader(np))
                        {
                            np = np + ".nubh";

                            if (NUB.WriteMetadatas(np))
                            {
                                if (File.Exists(Generic.SavePath))
                                {
                                    File.Delete(Generic.SavePath);
                                }
                                File.Move(np, Generic.SavePath);
                                if (File.Exists(Generic.SavePath))
                                {
                                    p.Report(1);
                                    return true;
                                }
                                else
                                {
                                    p.Report(1);
                                    return false;
                                }

                            }
                            else
                            {
                                p.Report(1);
                                ExceptionError("Failed to write NUB metadata information.");
                                return false;
                            }
                        }
                        else
                        {
                            p.Report(1);
                            ExceptionError("Failed to write NUB header information.");
                            return false;
                        }
                    }
                }
                
            }
            else // Multiple
            {
                int count = 0, error = 0;
                foreach (var Loopflag in Generic.MultipleFilesLoopOKFlags)
                {
                    if (Loopflag) // Looped
                    {
                        string np = Utils.Wav2Raw(Generic.OpenFilePaths[count], Generic.Signed, Generic.ByteOrderBE, Generic.SampleRate, Generic.Channels, "-b " + Generic.Bits);
                        if (NUB.CreateNUBHeader(np))
                        {
                            np = np + ".nubh";

                            Generic.LoopStartSamples = Generic.MultipleLoopStarts[count];
                            Generic.LoopEndSamples = Generic.MultipleLoopEnds[count];

                            FileInfo fi = new(Generic.OriginalPaths[count]);

                            if (NUB.WriteMetadatas(np))
                            {
                                File.Move(np, Generic.FolderSavePath + @"\" + fi.Name + ".nub");
                                count++;
                                p.Report(count);
                                continue;
                            }
                            else
                            {
                                ExceptionError("Failed to write NUB metadata information.");
                                count++;
                                error++;
                                p.Report(count);
                                continue;
                            }
                        }
                        else
                        {
                            ExceptionError("Failed to write NUB header information.");
                            count++;
                            error++;
                            p.Report(count);
                            continue;
                        }
                    }
                    else // no loops
                    {
                        if (Generic.NumberOfStreams != 1) // Multi Stream NUB (not supported)
                        {
                            return false;
                        }
                        else // Single NUB
                        {
                            string np = Utils.Wav2Raw(Generic.OpenFilePaths[count], Generic.Signed, Generic.ByteOrderBE, Generic.SampleRate, Generic.Channels, "-b " + Generic.Bits);
                            if (NUB.CreateNUBHeader(np))
                            {
                                np = np + ".nubh";

                                Generic.LoopStartSamples = Generic.MultipleLoopStarts[count];
                                Generic.LoopEndSamples = Generic.MultipleLoopEnds[count];

                                FileInfo fi = new(Generic.OriginalPaths[count]);

                                if (NUB.WriteMetadatas(np))
                                {
                                    File.Move(np, Generic.FolderSavePath + @"\" + fi.Name + ".nub");
                                    count++;
                                    p.Report(count);
                                    continue;
                                }
                                else
                                {
                                    ExceptionError("Failed to write NUB metadata information.");
                                    count++;
                                    error++;
                                    p.Report(count);
                                    continue;
                                }
                            }
                            else
                            {
                                ExceptionError("Failed to write NUB header information.");
                                count++;
                                error++;
                                p.Report(count);
                                continue;
                            }
                        }
                    }
                }

                if (count == Generic.OpenFilePaths.Length)
                {
                    EncCount = count;
                    EncErrorCount = error;
                    return true;
                }
                else if (error != 0)
                {
                    EncCount = count;
                    EncErrorCount = error;
                    return true;
                }
                else
                {
                    EncCount = count;
                    EncErrorCount = error;
                    return false;
                }
            }

        }

        private static bool AudioConverter_ATW_DoWork(IProgress<int> p, CancellationToken cToken)
        {
            int length = Generic.OpenFilePaths.Length;
            switch (Generic.WTAmethod)
            {
                case 0: // 44100Hz
                    {
                        if (length == 1)
                        {
                            p.Report(Directory.GetFiles(Directory.GetCurrentDirectory() + @"\_temp", "*").Length);
                            FileInfo fi = new(Generic.SavePath);
                            var source = new MediaFile { Filename = Generic.OpenFilePaths[0] };
                            var output = new MediaFile { Filename = Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name };

                            var co = new ConversionOptions
                            {
                                AudioSampleRate = AudioSampleRate.Hz44100,
                            };

                            using var engine = new Engine();
                            var task = MTK_ConvertAsync(engine, source, output, co);

                            while (task.IsCompleted != true)
                            {
                                int files = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\_temp", "*").Length;

                                if (cToken.IsCancellationRequested == true)
                                {
                                    if (task.IsCompleted == true)
                                        return false;
                                }
                                else
                                {
                                    p.Report(files);
                                    continue;
                                }
                            }
                            return true;
                        }
                        else
                        {
                            p.Report(Directory.GetFiles(Directory.GetCurrentDirectory() + @"\_temp", "*").Length);
                            int count = 0;
                            foreach (var file in Generic.OpenFilePaths)
                            {
                                FileInfo fi = new(file);
                                var source = new MediaFile { Filename = file };
                                var output = new MediaFile { Filename = Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav" };

                                var co = new ConversionOptions
                                {
                                    AudioSampleRate = AudioSampleRate.Hz44100,
                                };

                                using var engine = new Engine();
                                var task = MTK_ConvertAsync(engine, source, output, co);
                                count++;

                                while (task.IsCompleted != true)
                                {
                                    int files = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\_temp", "*").Length;

                                    if (cToken.IsCancellationRequested == true)
                                    {
                                        if (task.IsCompleted == true)
                                            return false;
                                    }
                                    else
                                    {
                                        p.Report(files);
                                        continue;
                                    }
                                }
                            }
                            return true;
                        }
                    }
                case 1: // 48000Hz
                    {
                        if (length == 1)
                        {
                            p.Report(Directory.GetFiles(Directory.GetCurrentDirectory() + @"\_temp", "*").Length);
                            FileInfo fi = new(Generic.SavePath);
                            var source = new MediaFile { Filename = Generic.OpenFilePaths[0] };
                            var output = new MediaFile { Filename = Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name };

                            var co = new ConversionOptions
                            {
                                AudioSampleRate = AudioSampleRate.Hz48000,
                            };

                            using var engine = new Engine();
                            var task = MTK_ConvertAsync(engine, source, output, co);

                            while (task.IsCompleted != true)
                            {
                                int files = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\_temp", "*").Length;

                                if (cToken.IsCancellationRequested == true)
                                {
                                    if (task.IsCompleted == true)
                                        return false;
                                }
                                else
                                {
                                    p.Report(files);
                                    continue;
                                }
                            }

                            return true;
                        }
                        else
                        {
                            p.Report(Directory.GetFiles(Directory.GetCurrentDirectory() + @"\_temp", "*").Length);
                            foreach (var file in Generic.OpenFilePaths)
                            {
                                FileInfo fi = new(file);
                                var source = new MediaFile { Filename = file };
                                var output = new MediaFile { Filename = Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav" };

                                var co = new ConversionOptions
                                {
                                    AudioSampleRate = AudioSampleRate.Hz48000,
                                };

                                using var engine = new Engine();
                                var task = MTK_ConvertAsync(engine, source, output, co);

                                while (task.IsCompleted != true)
                                {
                                    int files = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\_temp", "*").Length;

                                    if (cToken.IsCancellationRequested == true)
                                    {
                                        if (task.IsCompleted == true)
                                            return false;
                                    }
                                    else
                                    {
                                        p.Report(files);
                                        continue;
                                    }
                                }
                            }
                            return true;
                        }
                    }
                case 2: // 12000Hz
                    {
                        if (length == 1)
                        {
                            p.Report(Directory.GetFiles(Directory.GetCurrentDirectory() + @"\_temp", "*").Length);
                            FileInfo fi = new(Generic.SavePath);
                            var source = new MediaFile { Filename = Generic.OpenFilePaths[0] };
                            var output = new MediaFile { Filename = Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name };

                            var co = new ConversionOptions
                            {
                                AudioSampleRate = (AudioSampleRate)12000,
                            };

                            using var engine = new Engine();
                            var task = MTK_ConvertAsync(engine, source, output, co);

                            while (task.IsCompleted != true)
                            {
                                int files = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\_temp", "*").Length;

                                if (cToken.IsCancellationRequested == true)
                                {
                                    if (task.IsCompleted == true)
                                        return false;
                                }
                                else
                                {
                                    p.Report(files);
                                    continue;
                                }
                            }

                            return true;
                        }
                        else
                        {
                            p.Report(Directory.GetFiles(Directory.GetCurrentDirectory() + @"\_temp", "*").Length);
                            foreach (var file in Generic.OpenFilePaths)
                            {
                                FileInfo fi = new(file);
                                var source = new MediaFile { Filename = file };
                                var output = new MediaFile { Filename = Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav" };

                                var co = new ConversionOptions
                                {
                                    AudioSampleRate = (AudioSampleRate)12000,
                                };

                                using var engine = new Engine();
                                var task = MTK_ConvertAsync(engine, source, output, co);

                                while (task.IsCompleted != true)
                                {
                                    int files = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\_temp", "*").Length;

                                    if (cToken.IsCancellationRequested == true)
                                    {
                                        if (task.IsCompleted == true)
                                            return false;
                                    }
                                    else
                                    {
                                        p.Report(files);
                                        continue;
                                    }
                                }
                            }
                            return true;
                        }
                    }
                case 3: // 24000Hz
                    {
                        if (length == 1)
                        {
                            p.Report(Directory.GetFiles(Directory.GetCurrentDirectory() + @"\_temp", "*").Length);
                            FileInfo fi = new(Generic.SavePath);
                            var source = new MediaFile { Filename = Generic.OpenFilePaths[0] };
                            var output = new MediaFile { Filename = Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name };

                            var co = new ConversionOptions
                            {
                                AudioSampleRate = (AudioSampleRate)24000,
                            };

                            using var engine = new Engine();
                            var task = MTK_ConvertAsync(engine, source, output, co);

                            while (task.IsCompleted != true)
                            {
                                int files = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\_temp", "*").Length;

                                if (cToken.IsCancellationRequested == true)
                                {
                                    if (task.IsCompleted == true)
                                        return false;
                                }
                                else
                                {
                                    p.Report(files);
                                    continue;
                                }
                            }

                            return true;
                        }
                        else
                        {
                            p.Report(Directory.GetFiles(Directory.GetCurrentDirectory() + @"\_temp", "*").Length);
                            foreach (var file in Generic.OpenFilePaths)
                            {
                                FileInfo fi = new(file);
                                var source = new MediaFile { Filename = file };
                                var output = new MediaFile { Filename = Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name.Replace(fi.Extension, "") + Utils.ATWSuffix() + ".wav" };

                                var co = new ConversionOptions
                                {
                                    AudioSampleRate = (AudioSampleRate)24000,
                                };

                                using var engine = new Engine();
                                var task = MTK_ConvertAsync(engine, source, output, co);

                                while (task.IsCompleted != true)
                                {
                                    int files = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\_temp", "*").Length;

                                    if (cToken.IsCancellationRequested == true)
                                    {
                                        if (task.IsCompleted == true)
                                            return false;
                                    }
                                    else
                                    {
                                        p.Report(files);
                                        continue;
                                    }
                                }
                            }
                            return true;
                        }
                    }
                default:
                    return false;
            }
        }

        private static bool AudioConverter_WTA_DoWork(IProgress<int> p, CancellationToken cToken)
        {
            int length = Generic.OpenFilePaths.Length;

            if (length == 1)
            {
                p.Report(Directory.GetFiles(Directory.GetCurrentDirectory() + @"\_temp", "*").Length);
                FileInfo fi = new(Generic.SavePath);
                var source = new MediaFile { Filename = Generic.OpenFilePaths[0] };
                var output = new MediaFile { Filename = Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name };
                var co = new ConversionOptions
                {
                    AudioSampleRate = AudioSampleRate.Hz44100,
                };

                using var engine = new Engine();
                var task = MTK_ConvertAsync(engine, source, output, co);

                while (task.IsCompleted != true)
                {
                    int files = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\_temp", "*").Length;

                    if (cToken.IsCancellationRequested == true)
                    {
                        if (task.IsCompleted == true)
                            return false;
                    }
                    else
                    {
                        p.Report(files);
                        continue;
                    }
                }

                return true;
            }
            else
            {
                p.Report(Directory.GetFiles(Directory.GetCurrentDirectory() + @"\_temp", "*").Length);
                foreach (var file in Generic.OpenFilePaths)
                {
                    FileInfo fi = new(file);
                    var source = new MediaFile { Filename = file };
                    var output = new MediaFile { Filename = Directory.GetCurrentDirectory() + @"\_temp\" + fi.Name.Replace(fi.Extension, "") + Generic.WTAFmt };
                    var co = new ConversionOptions
                    {
                        AudioSampleRate = AudioSampleRate.Hz44100,
                    };

                    using var engine = new Engine();
                    var task = MTK_ConvertAsync(engine, source, output, co);

                    while (task.IsCompleted != true)
                    {
                        int files = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\_temp", "*").Length;

                        if (cToken.IsCancellationRequested == true)
                        {
                            if (task.IsCompleted == true)
                                return false;
                        }
                        else
                        {
                            p.Report(files);
                            continue;
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Download task function
        /// </summary>
        /// <param name="p"></param>
        /// <param name="cToken"></param>
        /// <returns></returns>
        private bool Download_DoWork(IProgress<int> p, CancellationToken cToken)
        {
            if (Generic.downloadClient == null)
            {
#pragma warning disable SYSLIB0014 // 型またはメンバーが旧型式です
                Generic.downloadClient = new System.Net.WebClient();
#pragma warning restore SYSLIB0014 // 型またはメンバーが旧型式です
                Generic.downloadClient.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(DownloadClient_DownloadProgressChanged);
                Generic.downloadClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadClient_DownloadFileCompleted);
            }

            Invoke(new Action(() => Text = Localization.FormDownloadingCaption));
            var task = Download(cToken);

            while (task.IsCompleted != true)
            {
                if (task.Result == false)
                {
                    return false;
                }
            }
            if (cToken.IsCancellationRequested == true)
            {
                return false;
            }
            else
            {
                Invoke(new Action(() => Text = Localization.ProcessingCaption));
                Invoke(new Action(() => label_Status.Text = Localization.ProcessingCaption));
                Invoke(new Action(() => button_Abort.Enabled = false));
            }

            return true;
        }

        private async Task<bool> Download(CancellationToken cToken)
        {
            Uri uri;

            switch (Generic.ApplicationPortable)
            {
                case false:
                    {
                        uri = new("https://github.com/XyLe-GBP/NUBTool/releases/download/v" + Generic.GitHubLatestVersion + "/nubtool-release.zip");
                    }
                    break;
                case true:
                    {
                        uri = new("https://github.com/XyLe-GBP/NUBTool/releases/download/v" + Generic.GitHubLatestVersion + "/nubtool-portable.zip");
                    }
                    break;
            }

            switch (Generic.ApplicationPortable)
            {
                case false: // release
                    {
                        Generic.downloadClient.DownloadFileAsync(uri, Directory.GetCurrentDirectory() + @"\res\nubtool.zip");
                    }
                    break;
                case true: // portable
                    {
                        Generic.downloadClient.DownloadFileAsync(uri, Directory.GetCurrentDirectory() + @"\res\nubtool.zip");
                    }
                    break;
            }
            Generic.IsDownloading = true;

            while (Generic.downloadClient.IsBusy)
            {
                if (cToken.IsCancellationRequested == true)
                {
                    return await Task.FromResult(false);
                }
                Invoke(new Action(() => progressBar_MainProgress.Value = DownloadProgress));
                Invoke(new Action(() => label_Status.Text = DownloadStatus));
            }
            return await Task.FromResult(true);
        }

        private void DownloadClient_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            if (Generic.IsDownloading == true)
            {
                progressBar_MainProgress.Maximum = 100;
                Generic.IsDownloading = false;
            }
            DownloadProgress = e.ProgressPercentage;
            DownloadStatus = string.Format(Localization.DownloadingCaption, e.ProgressPercentage, e.TotalBytesToReceive / 1024, e.BytesReceived / 1024);
        }

        private void DownloadClient_DownloadFileCompleted(object? sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled) // Cancelled
            {
                Generic.downloadClient!.Dispose();
            }
            else if (e.Error != null) // Error
            {
                Generic.downloadClient!.Dispose();
            }
            else
            {
                Generic.downloadClient!.Dispose();
            }
        }

        private void UpdateProgress(int p)
        {
            switch (Generic.ProcessFlag)
            {
                default:
                    progressBar_MainProgress.Value = p;
                    label_Status.Text = string.Format(Localization.StatusCaption, p, Generic.OpenFilePaths.Length);
                    break;
            }
        }

        private void Button_abort_Click(object sender, EventArgs e)
        {
            if (Generic.cts != null)
            {
                if (Generic.downloadClient is not null && Generic.downloadClient.IsBusy)
                {
                    DialogResult dr = MessageBox.Show(Localization.DownloadAbortConfirmCaption, Localization.MSGBoxConfirmCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        Generic.cts.Cancel();
                        Generic.downloadClient.CancelAsync();
                        Close();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    Generic.cts.Cancel();
                    Close();
                }
            }
        }

        private static async Task MTK_ConvertAsync(Engine engine, MediaFile source, MediaFile dest, ConversionOptions co)
        {
            try
            {
                await Task.Run(() => engine.Convert(source, dest, co));
            }
            catch (NullReferenceException)
            {
                return;
            }
        }

        private void Timer_interval_Tick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
