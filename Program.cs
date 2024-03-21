using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MiAPITranslatorExample.MiAPI.RETURN_CODE;

namespace MiAPITranslatorExample {
    internal static unsafe class Program {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main() {

#if DEBUG
            const bool debug = true;
#else
            const bool debug = false;
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try {
                MiAPI.RETURN_CODE code = MiAPI.Start();
                
                switch (code) {

                    case RESP_OK:
                        Console.WriteLine("[MiAPI] Init successfull!");

                        Console.WriteLine("[MiAPI] Version {0}", MiAPI.GetMiAPIVersion());
                        Console.WriteLine("[MiAPI] System: {0}", MiAPI.GetProductName());

                        break;

                    default:
                        Console.WriteLine("[MiAPI] System: {0}", MiAPI.GetProductName());

                        Console.WriteLine(MiAPI.GetCodeMeaning(code));

                        MessageBoxDefaultButton defB = MessageBoxDefaultButton.Button2;

                        if (debug)
                            defB = MessageBoxDefaultButton.Button1;

                        DialogResult dR = MessageBox.Show(string.Format("{0}\n\nContinue?", MiAPI.GetCodeMeaning(code)), MiAPI.GetCodeMeaning(code), MessageBoxButtons.YesNo, MessageBoxIcon.Warning, defB);

                        switch (dR) {
                            case DialogResult.Yes:
                                break;
                            default:
                                return (int)code;
                        }
                        break;
                }
            } catch (BadImageFormatException e) {
                // https://blog.mattmags.com/2007/06/30/accessing-32-bit-dlls-from-64-bit-code/
                const string DLL = "[MiAPI] Unable to load the DLL file. Is it whitelisted on this system? Make sure this program was compiled for x86!";
                Console.WriteLine(DLL);
                MessageBox.Show(DLL, "DLL load error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(e.Message);
                return 1;
            }

         // Application.Run(new FMain());

         MiAPI.Exit();
            return 0;
        }
    }
}
