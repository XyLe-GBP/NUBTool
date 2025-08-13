namespace NUBTool
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string mutexName = "NUBTool";
            System.Threading.Mutex mutex = new(false, mutexName);

            bool hasHandle = false;
            try
            {
                try
                {
                    hasHandle = mutex.WaitOne(0, false);
                }
                catch (System.Threading.AbandonedMutexException)
                {
                    hasHandle = true;
                }
                if (hasHandle == false)
                {
                    MessageBox.Show("Multiple launch of applications is not allowed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!File.Exists(Directory.GetCurrentDirectory() + @"\res\SoX\sox.exe"))
                {
                    MessageBox.Show("The required file 'sox.exe' does not exist.\nClose the application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!File.Exists(Directory.GetCurrentDirectory() + @"\res\SoX\wget.exe"))
                {
                    MessageBox.Show("The required file 'wget.exe' does not exist.\nClose the application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!File.Exists(Directory.GetCurrentDirectory() + @"\res\updater.exe"))
                {
                    MessageBox.Show("The required file 'updater.exe' does not exist.\nClose the application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!File.Exists(Directory.GetCurrentDirectory() + @"\res\SoX\libmad-0.dll"))
                {
                    MessageBox.Show("SoX library 'libmad-0.dll' could not be found.\r\nProblems may occur during processing operations.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (!File.Exists(Directory.GetCurrentDirectory() + @"\res\SoX\libmp3lame-0.dll"))
                {
                    MessageBox.Show("SoX library 'libmp3lame-0.dll' could not be found.\r\nProblems may occur during processing operations.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (!File.Exists(Directory.GetCurrentDirectory() + @"\res\SoX\libogg-0.dll"))
                {
                    MessageBox.Show("SoX library 'libogg-0.dll' could not be found.\r\nProblems may occur during processing operations.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (!File.Exists(Directory.GetCurrentDirectory() + @"\res\SoX\libvorbis-0.dll"))
                {
                    MessageBox.Show("SoX library 'libvorbis-0.dll' could not be found.\r\nProblems may occur during processing operations.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (!File.Exists(Directory.GetCurrentDirectory() + @"\res\SoX\libflac-8.dll"))
                {
                    MessageBox.Show("SoX library 'libflac-8.dll' could not be found.\r\nProblems may occur during processing operations.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (!File.Exists(Directory.GetCurrentDirectory() + @"\res\SoX\libgcc_s_sjlj-1.dll"))
                {
                    MessageBox.Show("SoX library 'libgcc_s_sjlj-1.dll' could not be found.\r\nProblems may occur during processing operations.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (!File.Exists(Directory.GetCurrentDirectory() + @"\res\SoX\libgomp-1.dll"))
                {
                    MessageBox.Show("SoX library 'libgomp-1.dll' could not be found.\r\nProblems may occur during processing operations.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (!File.Exists(Directory.GetCurrentDirectory() + @"\res\SoX\libid3tag-0.dll"))
                {
                    MessageBox.Show("SoX library 'libid3tag-0.dll' could not be found.\r\nProblems may occur during processing operations.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (!File.Exists(Directory.GetCurrentDirectory() + @"\res\SoX\libpng16-16.dll"))
                {
                    MessageBox.Show("SoX library 'libpng16-16.dll' could not be found.\r\nProblems may occur during processing operations.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (!File.Exists(Directory.GetCurrentDirectory() + @"\res\SoX\libsox-3.dll"))
                {
                    MessageBox.Show("SoX library 'libsox-3.dll' could not be found.\r\nProblems may occur during processing operations.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (!File.Exists(Directory.GetCurrentDirectory() + @"\res\SoX\libssp-0.dll"))
                {
                    MessageBox.Show("SoX library 'libssp-0.dll' could not be found.\r\nProblems may occur during processing operations.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (!File.Exists(Directory.GetCurrentDirectory() + @"\res\SoX\libvorbisenc-2.dll"))
                {
                    MessageBox.Show("SoX library 'libvorbisenc-2.dll' could not be found.\r\nProblems may occur during processing operations.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (!File.Exists(Directory.GetCurrentDirectory() + @"\res\SoX\libvorbisfile-3.dll"))
                {
                    MessageBox.Show("SoX library 'libvorbisfile-3.dll' could not be found.\r\nProblems may occur during processing operations.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (!File.Exists(Directory.GetCurrentDirectory() + @"\res\SoX\libwavpack-1.dll"))
                {
                    MessageBox.Show("SoX library 'libwavpack-1.dll' could not be found.\r\nProblems may occur during processing operations.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (!File.Exists(Directory.GetCurrentDirectory() + @"\res\SoX\libwinpthread-1.dll"))
                {
                    MessageBox.Show("SoX library 'libwinpthread-1.dll' could not be found.\r\nProblems may occur during processing operations.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (!File.Exists(Directory.GetCurrentDirectory() + @"\res\SoX\zlib1.dll"))
                {
                    MessageBox.Show("SoX library 'zlib1.dll' could not be found.\r\nProblems may occur during processing operations.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (!File.Exists(Directory.GetCurrentDirectory() + @"\res\SoX\wget.ini"))
                {
                    MessageBox.Show("File 'wget.ini' could not be found.\r\nProblems may occur during processing operations.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }


                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ApplicationConfiguration.Initialize();
                Application.Run(new FormMain());
            }
            finally
            {
                if (hasHandle)
                {
                    mutex.ReleaseMutex();
                }
                mutex.Close();
            }
        }
    }
}