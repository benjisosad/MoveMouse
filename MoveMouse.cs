using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MoveMouse
{
    internal sealed class MainForm : Form
    {
        private const double UnitsPerSecond = 120;
        private const int StopHotkeyId = 1;
        private const int HotkeyMessage = 0x0312;
        private const uint NoRepeat = 0x4000;
        private readonly System.Windows.Forms.Timer timer;
        private readonly Stopwatch clock = new Stopwatch();
        private readonly Label status;
        private readonly Button toggle;
        private double lastUpdate;
        private double pendingUnits;
        private bool running;
        private bool hotkeyRegistered;

        [StructLayout(LayoutKind.Sequential)]
        private struct MouseInput
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint flags;
            public uint time;
            public UIntPtr extraInfo;
        }

        // MOUSEINPUT es el miembro mas grande de la union nativa INPUT.
        // La alineacion predeterminada conserva el tamano correcto en x86/x64.
        [StructLayout(LayoutKind.Sequential)]
        private struct Input
        {
            public uint type;
            public MouseInput mouse;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint count, Input[] inputs, int size);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool RegisterHotKey(IntPtr window, int id, uint modifiers, uint key);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnregisterHotKey(IntPtr window, int id);

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public MainForm()
        {
            Text = "MoveMouse";
            ClientSize = new Size(360, 170);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10);
            AutoScaleMode = AutoScaleMode.Dpi;

            Label description = new Label();
            description.Text = "Movimiento continuo hacia la derecha.\nPulsa ESPACIO para detenerlo.";
            description.SetBounds(15, 15, 330, 45);
            description.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(description);

            status = new Label();
            status.Text = "Detenido";
            status.SetBounds(15, 68, 330, 25);
            status.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(status);

            toggle = new Button();
            toggle.Text = "Iniciar";
            toggle.SetBounds(110, 112, 140, 38);
            toggle.Click += Toggle;
            Controls.Add(toggle);

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 15;
            timer.Tick += Tick;
        }

        private void Toggle(object sender, EventArgs e)
        {
            if (running)
            {
                StopMovement();
                return;
            }

            // No iniciar sin un atajo global de emergencia disponible.
            if (!RegisterHotKey(Handle, StopHotkeyId, NoRepeat, (uint)Keys.Space))
            {
                MessageBox.Show(this,
                    "No se pudo registrar ESPACIO para detener el movimiento.\n" +
                    "Cierra otras instancias de MoveMouse e intenta de nuevo.",
                    "MoveMouse", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            hotkeyRegistered = true;
            clock.Restart();
            lastUpdate = 0;
            pendingUnits = 0;
            running = true;
            toggle.Text = "Detener";
            status.Text = "Movimiento relativo | ESPACIO: detener";
            timer.Start();
        }

        private void StopMovement()
        {
            timer.Stop();
            clock.Stop();
            running = false;
            if (hotkeyRegistered)
            {
                UnregisterHotKey(Handle, StopHotkeyId);
                hotkeyRegistered = false;
            }
            toggle.Text = "Iniciar";
            status.Text = "Detenido";
        }

        private void Tick(object sender, EventArgs e)
        {
            if (!running)
                return;

            // Usar tiempo real y conservar fracciones evita depender de la
            // precision del Timer para mantener la velocidad constante.
            double now = clock.Elapsed.TotalSeconds;
            pendingUnits += (now - lastUpdate) * UnitsPerSecond;
            lastUpdate = now;
            int units = (int)Math.Min(pendingUnits, int.MaxValue);
            pendingUnits -= units;
            if (units == 0)
                return;

            // Movimiento relativo, no reposicionamiento del cursor. Sigue
            // enviando desplazamientos aunque el cursor alcance un borde.
            Input input = new Input();
            input.type = 0; // INPUT_MOUSE
            input.mouse.dx = units;
            input.mouse.flags = 0x0001; // MOUSEEVENTF_MOVE, sin ABSOLUTE
            if (SendInput(1, new Input[] { input }, Marshal.SizeOf(typeof(Input))) != 1)
            {
                StopMovement();
                status.Text = "Windows no acepto el movimiento.";
            }
        }

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == HotkeyMessage && message.WParam.ToInt32() == StopHotkeyId)
            {
                StopMovement();
                return;
            }
            base.WndProc(ref message);
        }

        protected override bool ProcessCmdKey(ref Message message, Keys keyData)
        {
            // Espacio solo detiene: nunca debe activar el boton Iniciar.
            if ((keyData & Keys.KeyCode) == Keys.Space)
            {
                if (running)
                    StopMovement();
                return true;
            }
            return base.ProcessCmdKey(ref message, keyData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && timer != null)
            {
                StopMovement();
                timer.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
