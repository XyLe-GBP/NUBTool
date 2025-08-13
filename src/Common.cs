using SoxSharp;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using static NUBTool.Common;
using System;
using NUBTool.Localizable;

namespace NUBTool
{
    public class Common
    {
        public static readonly string xmlpath = Directory.GetCurrentDirectory() + @"\app.config";
        internal class Generic
        {
            public static bool IsWave = false;
            public static bool IsNUB = false;
            public static bool IsEnableLoop = false;
            public static bool IsNUBLooped = false;
            public static bool Signed = false;
            public static bool ByteOrderBE = false;
            public static bool IdNG = false;
            public static bool NoSNG = false;
            public static bool VolumeNG = false;
            public static bool LoopNG = false;
            public static bool LoopStartNG = false;
            public static bool LoopEndNG = false;
            public static string Bits = "16";
            public static string CurrentDir = Directory.GetCurrentDirectory();
            public static string TempPath = CurrentDir + @"\_temp\";
            public static string AudioTempPath = CurrentDir + @"\_tempAudio\";
            public static string SoXPath = CurrentDir + @"\res\SoX\sox.exe";
            public static string[] OpenFilePaths = null!;
            //public static string SaveFilePath = null!;
            public static string[] OriginalPaths = null!;
            public static string? ExceptionInformation;
            public static ushort StreamId = 0;
            public static ushort NumberOfStreams = 1;
            public static uint Format = 0;
            public static int nuSoundVersion = 0x01020100;
            public static int LoopStartSamples = 0;
            public static int LoopEndSamples = 0;
            public static long AudioTotalSamples = 0;
            public static ushort Channels = 2;
            public static uint SampleRate = 44100;
            public static int Volume = 16512;

            public static long FileSizes = 0;

            // Multiple files functions

            public static List<int[]> NUBMultiMetadataBuffers = [];
            public static int[] NUBMetadataBuffers = new int[9];

            public static ushort[] MultipleStreamId = [];
            public static ushort[] MultipleNumberOfStreams = [];
            public static int[] MultipleNuSoundVersion = [];
            public static bool[] MultipleFilesLoopOKFlags = [];
            public static int[] MultipleLoopStarts = [];
            public static int[] MultipleLoopEnds = [];
            public static ushort[] MultipleChannels = [];
            public static uint[] MultipleSampleRate = [];
            public static int[] MultipleVolume = [];

            public static bool IsAudioStreamingReloaded = false;

            public static int ProcessFlag = -1;
            public static CancellationTokenSource cts = null!;
            public static int ProgressMax = 0;
            public static bool Result = false;
            public static StreamReader Log = null!;
            public static Exception GlobalException = null!;

            public static bool IsOpenMulti = false;
            public static string SavePath = null!;
            public static string FolderSavePath = null!;

            public static int FormatFlag = -1;
            /// <summary>
            /// ファイルをWaveに変換したかどうかを判別するための変数
            /// </summary>
            public static bool IsATW = false;
            public static bool IsATWCancelled = false;

            public static bool IsNTW = false;
            public static bool IsNTWCancelled = false;
            /// <summary>
            /// 変換先の形式を判別するための変数
            /// </summary>
            public static int WTAFlag = -1;
            public static int WTAmethod = -1;
            public static string WTAFmt = null!;

            /// <summary>
            /// ダウンロード機能用変数
            /// </summary>
            public static System.Net.WebClient downloadClient = null!;
            public static bool IsDownloading = false;
            public static string DownloadedStatus = "";
            public static int DownloadProgress = 0;

            public static bool IsLoopWarning = true;

            public static bool ApplicationPortable = false;
            public static string? GitHubLatestVersion;
        }

        internal class Utils
        {
            /// <summary>
            /// Process.Start: Open URI for .NET
            /// </summary>
            /// <param name="URI">http://~ または https://~ から始まるウェブサイトのURL</param>
            public static void OpenURI(string URI)
            {
                try
                {
                    Process.Start(URI);
                }
                catch
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        //Windowsのとき  
                        URI = URI.Replace("&", "^&");
                        Process.Start(new ProcessStartInfo("cmd", $"/c start {URI}") { CreateNoWindow = true });
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        //Linuxのとき  
                        Process.Start("xdg-open", URI);
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        //Macのとき  
                        Process.Start("open", URI);
                    }
                    else
                    {
                        throw;
                    }
                }

                return;
            }

            /// <summary>
            /// 現在の時刻を取得する
            /// </summary>
            /// <returns>YYYY-MM-DD-HH-MM-SS (例：2000-01-01-00-00-00)</returns>
            public static string SFDRandomNumber()
            {
                DateTime dt = DateTime.Now;
                return dt.Year + "-" + dt.Month + "-" + dt.Day + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second;
            }

            public static string PathToExtension(string path)
            {
                FileInfo file = new(path);
                return file.Extension;
            }

            public static long GetFileSize(string path)
            {
                FileInfo file = new(path);
                return file.Length;
            }

            public static string GetFileName(string path)
            {
                FileInfo file = new(path);
                return file.Name;
            }

            public static int[] GetStreamLength(string[] paths)
            {
                List<int> lengths = new();
                foreach (var item in paths)
                {
                    FileInfo fi = new(item);
                    lengths.Add((int)(fi.Length - 2048));
                }
                return lengths.ToArray();
            }

            public static string ExceptionHandler(Exception e)
            {
                return e.ToString();
            }

            public static int DecToHex(int dec)
            {
                return Convert.ToInt32(Convert.ToString(dec, 16), 16);
            }

            public static byte[] StringToBytes(string str)
            {
                var bs = new List<byte>();
                for (int i = 0; i < str.Length / 2; i++)
                {
                    bs.Add(Convert.ToByte(str.Substring(i * 2, 2), 16));
                }
                // "01-AB-EF" こういう"-"区切りを想定する場合は以下のようにする
                // var bs = str.Split('-').Select(hex => Convert.ToByte(hex, 16));
                return bs.ToArray();
            }

            // Byte配列 => 16進数文字列
            public static string BytesToString(byte[] bs)
            {
                var str = BitConverter.ToString(bs);
                // "-"がいらないなら消しておく
                str = str.Replace("-", string.Empty);
                return str;
            }

            public static long GetStreamTotals(string file)
            {
                return GetFileSize(file) - 2048 / 4;
            }

            /// <summary>
            /// 指定したディレクトリ内のファイルも含めてディレクトリを削除する
            /// </summary>
            /// <param name="targetDirectoryPath">削除するディレクトリのパス</param>
            public static void DeleteDirectory(string targetDirectoryPath)
            {
                if (!Directory.Exists(targetDirectoryPath))
                {
                    return;
                }

                string[] filePaths = Directory.GetFiles(targetDirectoryPath);
                foreach (string filePath in filePaths)
                {
                    File.SetAttributes(filePath, FileAttributes.Normal);
                    File.Delete(filePath);
                }

                string[] directoryPaths = Directory.GetDirectories(targetDirectoryPath);
                foreach (string directoryPath in directoryPaths)
                {
                    DeleteDirectory(directoryPath);
                }

                Directory.Delete(targetDirectoryPath, false);
            }

            /// <summary>
            /// 指定したディレクトリ内のファイルのみを削除する
            /// </summary>
            /// <param name="targetDirectoryPath">削除するディレクトリのパス</param>
            public static void DeleteDirectoryFiles(string targetDirectoryPath)
            {
                if (!Directory.Exists(targetDirectoryPath))
                {
                    return;
                }

                DirectoryInfo di = new(targetDirectoryPath);
                FileInfo[] fi = di.GetFiles();
                foreach (var file in fi)
                {
                    file.Delete();
                }
                return;
            }

            /// <summary>
            /// ファイル、フォルダの属性を解除
            /// </summary>
            /// <param name="dirInfo"></param>
            public static void RemoveReadonlyAttribute(DirectoryInfo dirInfo)
            {
                //基のフォルダの属性を変更
                if ((dirInfo.Attributes & FileAttributes.ReadOnly) ==
                    FileAttributes.ReadOnly)
                    dirInfo.Attributes = FileAttributes.Normal;
                //フォルダ内のすべてのファイルの属性を変更
                foreach (FileInfo fi in dirInfo.GetFiles())
                    if ((fi.Attributes & FileAttributes.ReadOnly) ==
                        FileAttributes.ReadOnly)
                        fi.Attributes = FileAttributes.Normal;
                //サブフォルダの属性を回帰的に変更
                foreach (DirectoryInfo di in dirInfo.GetDirectories())
                    RemoveReadonlyAttribute(di);
            }

            /// <summary>
            /// 完了後に保存先のフォルダを開く
            /// </summary>
            /// <param name="Fullpath">フォルダのフルパス</param>
            /// <param name="Flag">フラグ</param>
            public static void ShowFolder(string Fullpath, bool Flag = true)
            {
                if (Flag != false)
                {
                    Process.Start("EXPLORER.EXE", @"/select,""" + Fullpath + @"""");
                    return;
                }
                else
                {
                    return;
                }
            }

            /// <summary>
            /// 該当ファイルが存在する場合は削除
            /// </summary>
            /// <param name="path">ファイルの場所</param>
            public static void CheckExistsFile(string path)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return;
                }
                else
                {
                    return;
                }
            }

            public static string Raw2Wav(string file)
            {
                using var sox = new Sox(Generic.SoXPath);
                InputFile input = new(file)
                {
                    Type = FileType.RAW,
                    Encoding = EncodingType.SignedInteger,
                    ByteOrder = ByteOrderType.BigEndian,
                    SampleRate = 44100,
                    Channels = 2,
                    CustomArgs = "-b 16"
                };
                sox.Output.Type = FileType.WAV;
                sox.Output.Encoding = EncodingType.SignedInteger;
                sox.Output.ByteOrder = ByteOrderType.LittleEndian;
                sox.Output.SampleRate = 44100;
                sox.Output.Channels = 2;
                sox.Output.SampleSize = 16;

                sox.Process(input, Generic.TempPath + GetFileName(file) + ".wav");

                if (File.Exists(Generic.TempPath + GetFileName(file) + ".wav"))
                {
                    return Generic.TempPath + GetFileName(file) + ".wav";
                }
                else
                {
                    return "";
                }
            }

            public static string SoX_Any2Wav(string file, bool IsInputBE, bool IsOutputBE = false)
            {
                using var sox = new Sox(Generic.SoXPath);
                ByteOrderType bot;
                if (IsInputBE)
                {
                    bot = ByteOrderType.BigEndian;
                }
                else
                {
                    bot = ByteOrderType.LittleEndian;
                }
                InputFile input = new(file)
                {
                    Type = GetSoXFileType(file),
                    Encoding = EncodingType.SignedInteger,
                    ByteOrder = bot,
                    SampleRate = 44100,
                    Channels = 2,
                    CustomArgs = "-b 16"
                };
                sox.Output.Type = FileType.WAV;
                sox.Output.Encoding = EncodingType.SignedInteger;
                sox.Output.ByteOrder = ByteOrderType.LittleEndian;
                sox.Output.SampleRate = 44100;
                sox.Output.Channels = 2;
                sox.Output.SampleSize = 16;

                sox.Process(input, Generic.TempPath + GetFileName(file) + ".wav");

                if (File.Exists(Generic.TempPath + GetFileName(file) + ".wav"))
                {
                    return Generic.TempPath + GetFileName(file) + ".wav";
                }
                else
                {
                    return "";
                }
            }

            public static string Mp32Wav(string file)
            {
                using var sox = new Sox(Generic.SoXPath);
                InputFile input = new(file);
                sox.Output.Type = FileType.WAV;
                sox.Output.Encoding = EncodingType.SignedInteger;
                sox.Output.ByteOrder = ByteOrderType.LittleEndian;
                sox.Output.SampleRate = 44100;
                sox.Output.Channels = 2;
                sox.Output.SampleSize = 16;

                sox.Process(input, Generic.TempPath + GetFileName(file) + ".wav");

                if (File.Exists(Generic.TempPath + GetFileName(file) + ".wav"))
                {
                    return Generic.TempPath + GetFileName(file) + ".wav";
                }
                else
                {
                    return "";
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="file">Fullpath</param>
            /// <param name="IsSigned">true = signed, false = unsigned</param>
            /// <param name="IsBE">true = BigEndian, false = LittleEndian</param>
            /// <param name="SampleRate">default: 44100 (Hz)</param>
            /// <param name="channels">default: 2 (stereo)</param>
            /// <param name="bitsargs">default: -b 16 (16 bit)</param>
            /// <returns></returns>
            public static string Wav2Raw(string file, bool IsSigned, bool IsBE, uint SampleRate = 44100, ushort channels = 2, string bitsargs = "-b 16")
            {
                using var sox = new Sox(Generic.SoXPath);
                InputFile input = new(file);
                sox.Output.Type = FileType.RAW;
                if (IsSigned)
                {
                    sox.Output.Encoding = EncodingType.SignedInteger;
                }
                else
                {
                    sox.Output.Encoding = EncodingType.UnsignedInteger;
                }
                if (IsBE)
                {
                    sox.Output.ByteOrder = ByteOrderType.BigEndian;
                }
                else
                {
                    sox.Output.ByteOrder = ByteOrderType.LittleEndian;
                }

                sox.Output.SampleRate = SampleRate;
                sox.Output.Channels = channels;
                sox.Output.CustomArgs = bitsargs;

                sox.Process(input, Generic.TempPath + GetFileName(file) + ".pcm");

                if (File.Exists(Generic.TempPath + GetFileName(file) + ".pcm"))
                {
                    return Generic.TempPath + GetFileName(file) + ".pcm";
                }
                else
                {
                    return "";
                }
            }

            public static string Mp32Raw(string file)
            {
                using var sox = new Sox(Generic.SoXPath);
                InputFile input = new(file);

                sox.Output.Type = FileType.RAW;
                sox.Output.Encoding = EncodingType.SignedInteger;
                sox.Output.ByteOrder = ByteOrderType.BigEndian;
                sox.Output.SampleRate = 44100;
                sox.Output.Channels = 2;
                sox.Output.CustomArgs = "-b 16";

                sox.Process(input, Generic.TempPath + GetFileName(file) + ".pcm");

                if (File.Exists(Generic.TempPath + GetFileName(file) + ".pcm"))
                {
                    return Generic.TempPath + GetFileName(file) + ".pcm";
                }
                else
                {
                    return "";
                }
            }

            [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
            private static extern int memcmp(byte[] b1, byte[] b2, nuint count);

            public static bool ByteArrayCompare(byte[] a, byte[] b)
            {
                if (ReferenceEquals(a, b))
                {
                    return true;
                }
                if (a == null || b == null || a.Length != b.Length)
                {
                    return false;
                }

                return memcmp(a, b, new nuint((uint)a.Length)) == 0;
            }

            /// <summary>
            /// 設定ファイルに全てを書き出す
            /// </summary>
            public static void InitConfig()
            {
                // 設定ダイアログ

                if (Config.Entry["Check_Update"].Value == null) // アップデートを確認 (bool)
                {
                    Config.Entry["Check_Update"].Value = "true";
                }
                if (Config.Entry["SmoothSamples"].Value == null) // サンプル値の更新を滑らかにする (bool)
                {
                    Config.Entry["SmoothSamples"].Value = "true";
                }
                if (Config.Entry["HideSplash"].Value == null) // スプラッシュスクリーンを無効化 (bool)
                {
                    Config.Entry["HideSplash"].Value = "false";
                }
                if (Config.Entry["SplashImage"].Value == null) // スプラッシュスクリーン画像 (bool)
                {
                    Config.Entry["SplashImage"].Value = "false";
                }
                if (Config.Entry["SplashImage_Path"].Value == null) // スプラッシュスクリーン画像パス (string)
                {
                    Config.Entry["SplashImage_Path"].Value = "";
                }

                if (Config.Entry["FixedConvert"].Value == null) // 形式固定 (bool)
                {
                    Config.Entry["FixedConvert"].Value = "false";
                }
                if (Config.Entry["ConvertType"].Value == null) // 形式固定有効化時の形式 (int)
                {
                    Config.Entry["ConvertType"].Value = "";
                }
                if (Config.Entry["ForceConvertWaveOnly"].Value == null) // waveファイルのみの読み込みでも変換を強制する (bool)
                {
                    Config.Entry["ForceConvertWaveOnly"].Value = "false";
                }
                if (Config.Entry["LoopWarning"].Value == null)
                {
                    Config.Entry["LoopWarning"].Value = "true";
                }
                if (Config.Entry["ShowFolder"].Value == null) // 変換後にフォルダを表示 (bool)
                {
                    Config.Entry["ShowFolder"].Value = "true";
                }
                Config.Save(xmlpath);
            }

            /// <summary>
            /// IsATWがtrueならWaveに変換したファイルを削除
            /// </summary>
            /// <param name="flag">Generic.IsATW</param>
            public static void ATWCheck(bool flag, bool IsCancelled = false)
            {
                switch (flag)
                {
                    case true:
                        {
                            if (IsCancelled)
                            {
                                if (Directory.Exists(Directory.GetCurrentDirectory() + @"\_tempAudio"))
                                {
                                    Directory.Delete(Directory.GetCurrentDirectory() + @"\_tempAudio");
                                }
                                Utils.DeleteDirectoryFiles(Directory.GetCurrentDirectory() + @"\_temp");
                            }
                            else
                            {
                                if (Generic.OpenFilePaths is not null)
                                {
                                    foreach (var file in Generic.OpenFilePaths)
                                    {
                                        if (File.Exists(file))
                                        {
                                            File.Delete(file);
                                        }
                                    }
                                }
                                
                                if (Directory.Exists(Directory.GetCurrentDirectory() + @"\_tempAudio"))
                                {
                                    Directory.Delete(Directory.GetCurrentDirectory() + @"\_tempAudio");
                                }
                            }
                            break;
                        }
                    case false:
                        {
                            break;
                        }
                }
                Generic.IsOpenMulti = false;
                Generic.IsATW = false;
                return;
            }

            /// <summary>
            /// IsNTWがtrueならPCMとWaveに変換したファイルを削除
            /// </summary>
            /// <param name="flag">Generic.IsNTW</param>
            public static void ConvertedNUBCheck(bool flag)
            {
                switch (flag)
                {
                    case true:
                        {
                            Utils.DeleteDirectoryFiles(Directory.GetCurrentDirectory() + @"\_temp");
                            break;
                        }
                    case false:
                        {
                            break;
                        }
                }
                Generic.IsNTW = false;
                return;
            }

            public static string ATWSuffix()
            {
                return Generic.WTAmethod switch
                {
                    0 => "_44k",
                    1 => "_48k",
                    2 => "_12k",
                    3 => "_24k",
                    _ => "",
                };
            }
        }

        public static FileType GetSoXFileType(string path)
        {
            switch (Utils.PathToExtension(path).ToUpper())
            {
                case ".WAV":
                    return FileType.WAV;
                case ".PCM":
                    return FileType.RAW;
                case ".MP3":
                    return FileType.MP3;
                case ".RAW":
                    return FileType.RAW;
                case ".SND":
                    return FileType.RAW;
                case ".FLAC":
                    return FileType.FLAC;
                case ".AIF":
                    return FileType.AIFF;
                case ".AIFF":
                    return FileType.AIFF;
                default:
                    return FileType.RAW;
            }
        }

        public static void ExceptionError(string ExCaption)
        {
            MessageBox.Show(string.Format(Localization.UnExpectedCaption, ExCaption), Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        internal class NUB
        {
            public static bool DeleteNUBHeaderWraw2wav(string file)
            {
                if (Generic.NUBMetadataBuffers[8] != 0 && Generic.NUBMetadataBuffers[8] > 1)
                {
                    MessageBox.Show("This file has multiple streams.\r\nNUBs with multiple streams cannot be read.", Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                using Stream fs = File.OpenRead(file);
                using var br = new BinaryReader(fs);
                byte[] header = br.ReadBytes(256);
                if (Utils.ByteArrayCompare(header, header))
                {
                    long len = fs.Length - 256;
                    byte[] r_data = new byte[len];
                    br.Read(r_data, 0, (int)len);
                    using var fs2 = new FileStream(Generic.TempPath + Utils.GetFileName(file) + ".pcm", FileMode.Create);
                    using var bw = new BinaryWriter(fs2);
                    bw.Seek(0, SeekOrigin.Begin);
                    bw.Write(r_data);

                    Generic.OpenFilePaths[0] = Utils.SoX_Any2Wav(Generic.TempPath + Utils.GetFileName(file) + ".pcm", true);

                    return true;
                }
                else
                {
                    MessageBox.Show(Localization.NUBHeaderNotFoundErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            public static bool DeleteNUBHeaderWraw2wavMulti(string[] file)
            {
                if (file.Length == 1)
                {
                    ReadMetadatas(file[0], Generic.NUBMetadataBuffers);
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
                        int[] metadatas = new int[9];
                        ReadMetadatas(read, metadatas);
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

            public static bool GetNUBLooped(int[] NUBBuffers, int[] LoopsBuffers)
            {
                if (NUBBuffers.Length != 9 || LoopsBuffers.Length != 2)
                {
                    return false;
                }

                if (NUBBuffers[2] != 0 && NUBBuffers[3] != 0)
                {
                    LoopsBuffers[0] = NUBBuffers[2];
                    LoopsBuffers[1] = NUBBuffers[3];
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public static bool CreateNUBHeader(string file)
            {
                try
                {
                    using var fs = new FileStream(file + ".nubh", FileMode.OpenOrCreate);
                    using var fs2 = new FileStream(file, FileMode.Open);
                    using var br = new BinaryReader(fs2);
                    byte[] header = new byte[] { 0x01, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x30, 0x00, 0x00, 0x00, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x77, 0x61, 0x76, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x70, 0x42, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x80, 0x3F, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xC8, 0xC2, 0xE8, 0x03, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x14, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x02, 0x00, 0x44, 0xAC, 0x00, 0x00, 0x10, 0xB1, 0x02, 0x00, 0x04, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00 };

                    using var bw = new BinaryWriter(fs);
                    bw.Seek(0, SeekOrigin.Begin);
                    bw.Write(header);

                    FileInfo fi = new(file);
                    byte[] bin = br.ReadBytes((int)fi.Length);

                    fs.Close();
                    using var fsheader = new FileStream(file + ".nubh", FileMode.Append);
                    using var bw2 = new BinaryWriter(fsheader);

                    bw2.Write(bin);

                    return true;
                }
                catch (Exception e)
                {
                    Generic.ExceptionInformation = Utils.ExceptionHandler(e);
                    return false;
                }
            }

            public static bool CreateMultiNUBHeader(string file, ushort NumberOfStreams, int[] StreamLengths, int[] HeaderOffsetBuffer)
            {
                try
                {
                    if (StreamLengths.Length != NumberOfStreams || StreamLengths.Length != HeaderOffsetBuffer.Length)
                    {
                        return false;
                    }
                    using var fs = new FileStream(file + ".nubh", FileMode.CreateNew);
                    using var br = new BinaryReader(fs);
                    byte[] startheader = new byte[] { 0x01, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x20, 0x1A, 0x01, 0x00, 0x20, 0x00, 0x00, 0x00, 0xA0, 0x00, 0x00, 0x00, 0xA0, 0x00, 0x00, 0x00 };
                    using var bw = new BinaryWriter(fs);
                    bw.Seek(0, SeekOrigin.Begin);
                    bw.Write(startheader);

                    int len = 0;
                    foreach (var length in StreamLengths)
                    {
                        byte[] plen = BitConverter.GetBytes(Utils.DecToHex(length));
                        bw.Seek(36 + len, SeekOrigin.Begin);
                        bw.Write(plen);
                        len += 4;
                    }
                    len += 36;

                    byte[] mainheader = new byte[] { 0x77, 0x61, 0x76, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x70, 0x42, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x80, 0x3F, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xC8, 0xC2, 0xE8, 0x03, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x14, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x02, 0x00, 0x44, 0xAC, 0x00, 0x00, 0x10, 0xB1, 0x02, 0x00, 0x04, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00 };

                    int count = 0;
                    foreach (var length in StreamLengths)
                    {
                        if (count == 0)
                        {
                            HeaderOffsetBuffer[count] = len + 4;
                            bw.Seek(len + 4, SeekOrigin.Begin);
                            bw.Write(mainheader);
                        }
                        else
                        {
                            HeaderOffsetBuffer[count] = len + 4 + mainheader.Length;
                            bw.Seek(len + 4 + mainheader.Length, SeekOrigin.Begin);
                            bw.Write(mainheader);
                        }
                        count++;
                    }

                    fs.Close();

                    FileInfo fi = new(Generic.TempPath + "multi.pcm");
                    using var fspcm = new FileStream(Generic.TempPath + "multi.pcm", FileMode.Open);
                    using var fs2 = new FileStream(file + ".nubh", FileMode.Append);
                    using var br2 = new BinaryReader(fspcm);
                    byte[] bin = br2.ReadBytes((int)fi.Length);

                    using var bw2 = new BinaryWriter(fs2);
                    bw2.Write(bin);

                    return true;
                }
                catch (Exception e)
                {
                    Generic.ExceptionInformation = Utils.ExceptionHandler(e);
                    return false;
                }
            }

            public static bool CreateMultiRaw(string[] files)
            {
                try
                {
                    int count = 0;
                    foreach (var file in files)
                    {
                        int oldlength = 0;
                        FileInfo fi = new(file);
                        using var fs = new FileStream(file, FileMode.Open);

                        using var br = new BinaryReader(fs);
                        byte[] bin = br.ReadBytes((int)fi.Length);

                        if (count == 0)
                        {
                            using var fs2 = new FileStream(Generic.TempPath + "multi.pcm", FileMode.OpenOrCreate);
                            using var bw = new BinaryWriter(fs2);

                            bw.Seek(0, SeekOrigin.Begin);
                            bw.Write(bin);
                            oldlength = bin.Length;
                        }
                        else
                        {
                            using var fs2 = new FileStream(Generic.TempPath + "multi.pcm", FileMode.Append);
                            using var bw = new BinaryWriter(fs2);

                            bw.Write(bin);
                            oldlength += bin.Length;
                        }
                        count++;
                    }
                    return true;
                }
                catch (Exception e)
                {
                    Generic.ExceptionInformation = Utils.ExceptionHandler(e);
                    return false;
                }
            }

            public static bool ReadMetadatas(string NUBfile, int[] buffers)
            {
                try
                {
                    if (buffers.Length != 9)
                    {
                        return false;
                    }
                    using var fs = new FileStream(NUBfile, FileMode.Open);
                    using var br = new BinaryReader(fs);

                    br.BaseStream.Seek(8, SeekOrigin.Begin);
                    byte[] id = br.ReadBytes(2);

                    br.BaseStream.Seek(12, SeekOrigin.Begin);
                    byte[] nostreams = br.ReadBytes(2);

                    br.BaseStream.Seek(20, SeekOrigin.Begin);
                    byte[] rp_stream = br.ReadBytes(4);

                    br.BaseStream.Seek(68, SeekOrigin.Begin);
                    byte[] rt_stream = br.ReadBytes(4);

                    br.BaseStream.Seek(80, SeekOrigin.Begin);
                    byte[] rloop_start = br.ReadBytes(4);

                    br.BaseStream.Seek(84, SeekOrigin.Begin);
                    byte[] rloop_end = br.ReadBytes(4);

                    br.BaseStream.Seek(102, SeekOrigin.Begin);
                    byte[] rvolume = br.ReadBytes(2);

                    br.BaseStream.Seek(238, SeekOrigin.Begin);
                    byte[] rchannel = br.ReadBytes(2);

                    br.BaseStream.Seek(240, SeekOrigin.Begin);
                    byte[] rsamplelate = br.ReadBytes(2);

                    buffers[0] = BitConverter.ToInt32(rp_stream, 0) / 4;
                    buffers[1] = BitConverter.ToInt32(rt_stream, 0) / 4;
                    buffers[2] = BitConverter.ToInt32(rloop_start, 0) / 4;
                    buffers[3] = BitConverter.ToInt32(rloop_end, 0) / 4 + buffers[2];
                    buffers[4] = BitConverter.ToInt16(rvolume, 0);
                    buffers[5] = BitConverter.ToInt16(rchannel, 0);
                    buffers[6] = BitConverter.ToUInt16(rsamplelate, 0);
                    buffers[7] = BitConverter.ToUInt16(id, 0);
                    buffers[8] = BitConverter.ToUInt16(nostreams, 0);
                    return true;
                }
                catch (Exception e)
                {
                    Generic.ExceptionInformation = Utils.ExceptionHandler(e);
                    return false;
                }
            }

            public static bool WriteMetadatas(string file)
            {
                try
                {
                    using var fs = new FileStream(file, FileMode.Open);

                    int id = Generic.StreamId, nostreams = Generic.NumberOfStreams, pstreams = Convert.ToInt32(fs.Length - 2048), tstreams = Convert.ToInt32(fs.Length - 2048), loops = Generic.LoopStartSamples * 4, loope = Generic.LoopEndSamples * 4 - loops, vol = Generic.Volume, cnl = Generic.Channels, spl = (int)Generic.SampleRate;

                    byte[] nuversion = BitConverter.GetBytes(Utils.DecToHex(Generic.nuSoundVersion));
                    byte[] stream_id = BitConverter.GetBytes(Utils.DecToHex(id));
                    byte[] b_nostreams = BitConverter.GetBytes(Utils.DecToHex(nostreams));
                    byte[] p_stream = BitConverter.GetBytes(Utils.DecToHex(pstreams));
                    byte[] t_stream = BitConverter.GetBytes(Utils.DecToHex(tstreams));
                    byte[] loop_start = BitConverter.GetBytes(Utils.DecToHex(loops));
                    byte[] loop_end = BitConverter.GetBytes(Utils.DecToHex(loope));
                    byte[] volume = BitConverter.GetBytes(Utils.DecToHex(vol));
                    byte[] channel = BitConverter.GetBytes(Utils.DecToHex(cnl));
                    byte[] smplrate = BitConverter.GetBytes(Utils.DecToHex(spl));
                    p_stream.Reverse(); // Little Endian To Big Endian
                    t_stream.Reverse();
                    loop_start.Reverse();
                    loop_end.Reverse();
                    volume.Reverse();
                    channel.Reverse();
                    smplrate.Reverse();

                    using var bw = new BinaryWriter(fs);
                    bw.Seek(0, SeekOrigin.Begin);
                    bw.Write(nuversion);

                    bw.Seek(8, SeekOrigin.Begin);
                    bw.Write(stream_id);

                    bw.Seek(12, SeekOrigin.Begin);
                    bw.Write(b_nostreams);

                    bw.Seek(20, SeekOrigin.Begin);
                    bw.Write(p_stream);

                    bw.Seek(52, SeekOrigin.Begin);
                    bw.Write(stream_id);

                    bw.Seek(68, SeekOrigin.Begin);
                    bw.Write(t_stream);

                    bw.Seek(80, SeekOrigin.Begin);
                    bw.Write(loop_start);

                    bw.Seek(84, SeekOrigin.Begin);
                    bw.Write(loop_end);

                    bw.Seek(102, SeekOrigin.Begin);
                    bw.Write(volume);

                    bw.Seek(238, SeekOrigin.Begin);
                    bw.Write(channel);

                    bw.Seek(240, SeekOrigin.Begin);
                    bw.Write(smplrate);

                    return true;
                }
                catch (Exception e)
                {
                    Generic.ExceptionInformation = Utils.ExceptionHandler(e);
                    return false;
                }
            }

            public static bool WriteMetadatasWithMulti(string file, int[] HeaderOffsetBuffer)
            {
                try
                {
                    using var fs = new FileStream(file, FileMode.Open);

                    int id = Generic.StreamId, nostreams = Generic.NumberOfStreams, pstreams = Convert.ToInt32(fs.Length - 2048), tstreams = Convert.ToInt32(fs.Length - 2048), loops = Generic.LoopStartSamples * 4, loope = Generic.LoopEndSamples * 4 - loops, vol = Generic.Volume, cnl = Generic.Channels, spl = (int)Generic.SampleRate;

                    byte[] stream_id = BitConverter.GetBytes(Utils.DecToHex(id));
                    byte[] b_nostreams = BitConverter.GetBytes(Utils.DecToHex(nostreams));
                    byte[] p_stream = BitConverter.GetBytes(Utils.DecToHex(pstreams));
                    byte[] t_stream = BitConverter.GetBytes(Utils.DecToHex(tstreams));
                    byte[] loop_start = BitConverter.GetBytes(Utils.DecToHex(loops));
                    byte[] loop_end = BitConverter.GetBytes(Utils.DecToHex(loope));
                    byte[] volume = BitConverter.GetBytes(Utils.DecToHex(vol));
                    byte[] channel = BitConverter.GetBytes(Utils.DecToHex(cnl));
                    byte[] smplrate = BitConverter.GetBytes(Utils.DecToHex(spl));
                    p_stream.Reverse(); // Little Endian To Big Endian
                    t_stream.Reverse();
                    loop_start.Reverse();
                    loop_end.Reverse();
                    volume.Reverse();
                    channel.Reverse();
                    smplrate.Reverse();

                    using var bw = new BinaryWriter(fs);
                    bw.Seek(8, SeekOrigin.Begin);
                    bw.Write(stream_id);

                    bw.Seek(12, SeekOrigin.Begin);
                    bw.Write(b_nostreams);

                    bw.Seek(20, SeekOrigin.Begin);
                    bw.Write(p_stream);

                    foreach (var offset in HeaderOffsetBuffer)
                    {
                        bw.Seek(offset + 20, SeekOrigin.Begin);
                        bw.Write(t_stream);

                        bw.Seek(offset + 36, SeekOrigin.Begin);
                        bw.Write(loop_start);

                        bw.Seek(offset + 40, SeekOrigin.Begin);
                        bw.Write(loop_end);

                        bw.Seek(offset + 58, SeekOrigin.Begin);
                        bw.Write(volume);

                        bw.Seek(offset + 190, SeekOrigin.Begin);
                        bw.Write(channel);

                        bw.Seek(offset + 192, SeekOrigin.Begin);
                        bw.Write(smplrate);
                    }


                    return true;
                }
                catch (Exception e)
                {
                    Generic.ExceptionInformation = Utils.ExceptionHandler(e);
                    return false;
                }
            }
        }

        /// <summary>
        /// ネットワーク系関数
        /// </summary>
        internal class Network
        {
            /// <summary>
            /// 文字列をURIに変換
            /// </summary>
            /// <param name="uri">URI文字列</param>
            /// <returns></returns>
            public static Uri GetUri(string uri)
            {
                return new Uri(uri);
            }

            public static Stream GetWebStream(HttpClient httpClient, Uri uri)
            {
                return httpClient.GetStreamAsync(uri).Result;
            }

            public static async Task<Stream> GetWebStreamAsync(HttpClient httpClient, Uri uri)
            {
                return await httpClient.GetStreamAsync(uri);
            }

            public static async Task<Image> GetWebImageAsync(HttpClient httpClient, Uri uri)
            {
                using Stream stream = await GetWebStreamAsync(httpClient, uri);
                return Image.FromStream(stream);
            }
        }

        public class Config
        {
            /// <summary>
            /// ルートエントリ
            /// </summary>
            public static ConfigEntry Entry = new() { Key = "ConfigRoot" };
            public static void Load(string filename)
            {
                if (!File.Exists(filename))
                    return;
                var xmlSerializer = new XmlSerializer(typeof(ConfigEntry));
                using var streamReader = new StreamReader(filename, Encoding.UTF8);
                using var xmlReader = XmlReader.Create(streamReader, new XmlReaderSettings() { CheckCharacters = false });
                Entry = (ConfigEntry)xmlSerializer.Deserialize(xmlReader)!; // （3）
            }
            public static void Save(string filename)
            {
                var serializer = new XmlSerializer(typeof(ConfigEntry));
                using var streamWriter = new StreamWriter(filename, false, Encoding.UTF8);
                serializer.Serialize(streamWriter, Entry);
            }
        }

        /// <summary>
        /// ConfigEntryクラス。設定の1レコード
        /// </summary>
        public class ConfigEntry
        {
            /// <summary>
            /// 設定レコードののキー
            /// </summary>
            public string Key { get; set; }
            /// <summary>
            /// 設定レコードの値
            /// </summary>
            public string Value { get; set; }
            /// <summary>
            /// 子アイテム
            /// </summary>
            public List<ConfigEntry>? Children { get; set; }
            /// <summary>
            /// キーを指定して子アイテムからConfigEntryを取得します。
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public ConfigEntry Get(string key)
            {
                var entry = Children?.FirstOrDefault(rec => rec.Key == key);
                if (entry == null)
                {
                    if (Children == null)
                        Children = new List<ConfigEntry>();
                    entry = new ConfigEntry() { Key = key };
                    Children.Add(entry);
                }
                return entry;
            }
            /// <summary>
            /// 子アイテムにConfigEntryを追加します。
            /// </summary>
            /// <param name="key">キー</param>
            /// <param name="o">設定値</param>
            public void Add(string key, string? o)
            {
                ConfigEntry? entry = Children?.FirstOrDefault(rec => rec.Key == key);
                if (entry != null)
                    entry.Value = o;
                else
                {
                    if (Children == null)
                        Children = new List<ConfigEntry>();
                    entry = new ConfigEntry() { Key = key, Value = o };
                    Children.Add(entry);
                }
            }
            /// <summary>
            /// 子アイテムからConfigEntryを取得します。存在しなければ新しいConfigEntryが作成されます。
            /// </summary>
            /// <param name="key">キー</param>
            /// <returns></returns>
            public ConfigEntry this[string key]
            {
                set => Add(key, null);
                get => Get(key);
            }
            /// <summary>
            /// 子アイテムからConfigEntryを取得します。存在しなければ新しいConfigEntryが作成されます。
            /// </summary>
            /// <param name="keys">キー、カンマで区切って階層指定します</param>
            /// <returns></returns>
            public ConfigEntry this[params string[] keys]
            {
                set
                {
                    ConfigEntry entry = this;
                    for (int i = 0; i < keys.Length; i++)
                    {
                        entry = entry[keys[i]];
                    }
                }
                get
                {
                    ConfigEntry entry = this;
                    for (int i = 0; i < keys.Length; i++)
                    {
                        entry = entry[keys[i]];
                    }
                    return entry;
                }
            }

            /// <summary>
            /// 指定したキーが子アイテムに存在するか調べます。再帰的調査はされません。
            /// </summary>
            /// <param name="key">キー</param>
            /// <returns>キーが存在すればTrue</returns>
            public bool Exists(string key) => Children?.Any(c => c.Key == key) ?? false;
            /// <summary>
            /// 指定したキーが子アイテムに存在するか調べます。階層をまたいだ指定をします。
            /// </summary>
            /// <param name="keys">キー、カンマで区切って階層指定します。</param>
            /// <returns>キーが存在すればTrue</returns>
            public bool Exists(params string[] keys)
            {
                ConfigEntry entry = this;
                for (int i = 0; i < keys.Length; i++)
                {
                    if (entry.Exists(keys[i]) == false)
                        return false;
                    entry = entry[keys[i]];
                }
                return true;
            }
        }
    }
}
