using SoxSharp;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NUBTool
{
    internal class Common
    {
        internal class Generic
        {
            public static bool IsNUB = false;
            public static bool IsEnableLoop = false;
            public static bool Signed = false;
            public static bool ByteOrderBE = false;
            public static bool IdNG = false;
            public static bool NoSNG = false;
            public static bool VolumeNG = false;
            public static bool LoopNG = false;
            public static string Bits = "16";
            public static string CurrentDir = Directory.GetCurrentDirectory();
            public static string TempPath = CurrentDir + @"\temp_dir\";
            public static string SoXPath = CurrentDir + @"\res\SoX\sox.exe";
            public static string[]? OpenFilePaths;
            public static string? SaveFilePath;
            public static string? OriginalPath;
            public static string? ExceptionInformation;
            public static ushort StreamId = 0;
            public static ushort NumberOfStreams = 1;
            public static uint Format = 0;
            public static int nuSoundVersion = 0x01020100;
            public static int LoopStartSamples = 0;
            public static int LoopEndSamples = 0;
            public static int[] NUBMetadataBuffers = new int[9];
            public static ushort Channels = 2;
            public static uint SampleRate = 44100;
            public static int Volume = 16512;
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

            public static string Any2Wav(string file)
            {
                using var sox = new Sox(Generic.SoXPath);
                InputFile input = new(file)
                {
                    Type = GetSoXFileType(file),
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

            [DllImport("msvcrt.dll",
    CallingConvention = CallingConvention.Cdecl)]
            private static extern int memcmp(byte[] b1, byte[] b2, UIntPtr count);

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

                return memcmp(a, b, new UIntPtr((uint)a.Length)) == 0;
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

        internal class NUB
        {
            public static bool DeleteNUBHeaderWraw2wav(string file)
            {
                if (Generic.NUBMetadataBuffers[8] != 0 && Generic.NUBMetadataBuffers[8] > 1)
                {
                    MessageBox.Show("This file has multiple streams.\r\nNUBs with multiple streams cannot be read.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    Generic.OpenFilePaths[0] = Utils.Any2Wav(Generic.TempPath + Utils.GetFileName(file) + ".pcm");

                    return true;
                }
                else
                {
                    MessageBox.Show("This file does not contain NUB header information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    }
}
