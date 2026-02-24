using ECommerece.Application.DTOs.UserDto;
using ECommerece.Application.IServices.IUserService;
using ECommerece.Presentation.Forms.DashboardForms;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ECommerece.Presentation.Forms.UserForms
{
    public class LoginForm : Form
    {
        private Panel cardPanel;
        private PictureBox iconBox;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblUsername;
        private Label lblPassword;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnGoRegister;
        private Label lblDemo;
        private readonly IAccountService _accountService;

        public LoginForm(IAccountService accountService)
        {
            _accountService = accountService;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // ===== FORM =====
            this.Text = "E-Commerce Management System";
            this.Size = new Size(960, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(241, 245, 249);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 9f);

            // ===== CARD PANEL =====
            cardPanel = new Panel
            {
                Size = new Size(560, 460),
                BackColor = Color.White,
                Location = new Point((this.ClientSize.Width - 560) / 2, (this.ClientSize.Height - 460) / 2)
            };
            cardPanel.Paint += CardPanel_Paint;
            this.Controls.Add(cardPanel);

            // ===== ICON BOX =====
            iconBox = new PictureBox
            {
                Size = new Size(52, 52),
                Location = new Point(170, 40),
                BackColor = Color.Transparent
            };
            iconBox.Paint += IconBox_Paint;
            cardPanel.Controls.Add(iconBox);

            // ===== TITLE =====
            lblTitle = new Label
            {
                Text = "E-Commerce",
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(232, 42),
                BackColor = Color.Transparent
            };
            cardPanel.Controls.Add(lblTitle);

            // ===== SUBTITLE =====
            lblSubtitle = new Label
            {
                Text = "Management System",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(236, 72),
                BackColor = Color.Transparent
            };
            cardPanel.Controls.Add(lblSubtitle);

            // ===== EMAIL LABEL =====
            lblUsername = new Label
            {
                Text = "Email",
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(48, 130),
                BackColor = Color.Transparent
            };
            cardPanel.Controls.Add(lblUsername);

            // ===== EMAIL TEXTBOX =====
            txtUsername = new TextBox
            {
                PlaceholderText = "Enter your email",
                Font = new Font("Segoe UI", 11f),
                ForeColor = Color.FromArgb(71, 85, 105),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            cardPanel.Controls.Add(CreateInputPanel(txtUsername, 48, 158));

            // ===== PASSWORD LABEL =====
            lblPassword = new Label
            {
                Text = "Password",
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(48, 228),
                BackColor = Color.Transparent
            };
            cardPanel.Controls.Add(lblPassword);

            // ===== PASSWORD TEXTBOX =====
            txtPassword = new TextBox
            {
                PlaceholderText = "Enter your password",
                Font = new Font("Segoe UI", 11f),
                ForeColor = Color.FromArgb(71, 85, 105),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                UseSystemPasswordChar = true
            };
            cardPanel.Controls.Add(CreateInputPanel(txtPassword, 48, 255));

            // ===== LOGIN BUTTON =====
            btnLogin = new Button
            {
                Text = "Login",
                Font = new Font("Segoe UI", 12f, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(59, 91, 219),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(464, 52),
                Location = new Point(48, 328),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 78, 196);
            btnLogin.FlatAppearance.MouseDownBackColor = Color.FromArgb(39, 65, 175);
            btnLogin.Click += BtnLogin_Click;
            btnLogin.Paint += BtnLogin_Paint;
            cardPanel.Controls.Add(btnLogin);

            // ===== DEMO LABEL =====
            lblDemo = new Label
            {
                Text = "Demo: Use admin@admin.com / admin",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(148, 163, 184),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            lblDemo.Location = new Point((cardPanel.Width - lblDemo.PreferredWidth) / 2, 393);
            cardPanel.Controls.Add(lblDemo);

            // ===== GO TO REGISTER BUTTON =====
            btnGoRegister = new Button
            {
                Text = "Don't have an account? Register",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(59, 91, 219),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                AutoSize = true,
                Cursor = Cursors.Hand
            };
            btnGoRegister.FlatAppearance.BorderSize = 0;
            btnGoRegister.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnGoRegister.Click += BtnGoRegister_Click;
            cardPanel.Controls.Add(btnGoRegister);

            // ===== CENTER LABELS AFTER LOAD =====
            this.Load += (s, e) =>
            {
                lblDemo.Location = new Point((cardPanel.Width - lblDemo.Width) / 2, 393);
                btnGoRegister.Location = new Point((cardPanel.Width - btnGoRegister.Width) / 2, 422);
            };
        }

        private Panel CreateInputPanel(TextBox txt, int x, int y)
        {
            Panel panel = new Panel
            {
                Size = new Size(464, 50),
                Location = new Point(x, y),
                BackColor = Color.White,
                Padding = new Padding(10, 10, 10, 10)
            };
            panel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(203, 213, 225), 1.5f))
                    e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
            };
            txt.Location = new Point(12, 11);
            txt.Size = new Size(440, 28);
            txt.Enter += (s, e) => panel.Invalidate();
            txt.Leave += (s, e) => panel.Invalidate();
            panel.Controls.Add(txt);
            return panel;
        }

        private void CardPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                e.Graphics.DrawRectangle(pen, 0, 0, cardPanel.Width - 1, cardPanel.Height - 1);
        }

        private void IconBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, 51, 51);
            using (GraphicsPath path = GetRoundedRect(rect, 12))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(59, 91, 219)))
                e.Graphics.FillPath(brush, path);

            using (Pen pen = new Pen(Color.White, 2.5f))
            {
                e.Graphics.DrawRectangle(pen, new Rectangle(10, 20, 31, 24));
                e.Graphics.DrawArc(pen, new Rectangle(17, 12, 17, 16), 180, 180);
            }
        }

        private void BtnLogin_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = GetRoundedRect(new Rectangle(0, 0, btn.Width, btn.Height), 8))
            using (SolidBrush brush = new SolidBrush(btn.BackColor))
                e.Graphics.FillPath(brush, path);

            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            using (SolidBrush textBrush = new SolidBrush(Color.White))
                e.Graphics.DrawString(btn.Text, btn.Font, textBrush,
                    new RectangleF(0, 0, btn.Width, btn.Height), sf);
        }

        private GraphicsPath GetRoundedRect(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please enter email and password!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "Loading...";

            var loginDto = new LoginDto
            {
                Email = txtUsername.Text.Trim(),
                Password = txtPassword.Text.Trim()
            };

            var result = await _accountService.LoginAsync(loginDto);

            if (result.IsAuthenticated)
            {
                var dashboard = new DashboardForm();
                dashboard.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show(result.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnLogin.Enabled = true;
                btnLogin.Text = "Login";
            }
        }

        private void BtnGoRegister_Click(object sender, EventArgs e)
        {
            var registerForm = new RegisterForm(_accountService);
            registerForm.Show();
            this.Hide();
        }
    }
}