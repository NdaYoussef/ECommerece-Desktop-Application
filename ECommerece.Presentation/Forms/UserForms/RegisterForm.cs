using ECommerece.Application.DTOs.UserDto;
using ECommerece.Application.IServices.IUserService;
using ECommerece.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ECommerece.Presentation.Forms.UserForms
{
    public class RegisterForm : Form
    {
        private Panel cardPanel;
        private PictureBox iconBox;
        private Label lblTitle;
        private Label lblSubtitle;

        private Label lblName;
        private Label lblEmail;
        private Label lblPassword;

        private TextBox txtName;
        private TextBox txtEmail;
        private TextBox txtPassword;

        private Button btnRegister;
        private Button btnBackToLogin;
        private readonly IAccountService _accountService;
        private readonly IServiceProvider _serviceProvider; // ✅ ضيف

        public RegisterForm(IAccountService accountService, IServiceProvider serviceProvider) // ✅ ضيف
        {
            _accountService = accountService;
            _serviceProvider = serviceProvider;
            InitializeComponents();
        }
        private void InitializeComponents()
        {
            this.Text = "E-Commerce - Register";
            this.Size = new Size(960, 620);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(241, 245, 249);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 9f);

            cardPanel = new Panel
            {
                Size = new Size(560, 480),
                BackColor = Color.White,
                Location = new Point((this.ClientSize.Width - 560) / 2, (this.ClientSize.Height - 480) / 2)
            };
            cardPanel.Paint += CardPanel_Paint;
            this.Controls.Add(cardPanel);

            iconBox = new PictureBox
            {
                Size = new Size(52, 52),
                Location = new Point(170, 30),
                BackColor = Color.Transparent
            };
            iconBox.Paint += IconBox_Paint;
            cardPanel.Controls.Add(iconBox);

            lblTitle = new Label
            {
                Text = "E-Commerce",
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(232, 32),
                BackColor = Color.Transparent
            };
            cardPanel.Controls.Add(lblTitle);

            lblSubtitle = new Label
            {
                Text = "Create a new account",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(236, 62),
                BackColor = Color.Transparent
            };
            cardPanel.Controls.Add(lblSubtitle);

            // NAME
            lblName = new Label
            {
                Text = "Full Name",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(48, 110),
                BackColor = Color.Transparent
            };
            cardPanel.Controls.Add(lblName);

            txtName = new TextBox
            {
                PlaceholderText = "Enter your full name",
                Font = new Font("Segoe UI", 11f),
                ForeColor = Color.FromArgb(71, 85, 105),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            cardPanel.Controls.Add(CreateInputPanel(txtName, 48, 133));

            // EMAIL
            lblEmail = new Label
            {
                Text = "Email",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(48, 203),
                BackColor = Color.Transparent
            };
            cardPanel.Controls.Add(lblEmail);

            txtEmail = new TextBox
            {
                PlaceholderText = "Enter your email",
                Font = new Font("Segoe UI", 11f),
                ForeColor = Color.FromArgb(71, 85, 105),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            cardPanel.Controls.Add(CreateInputPanel(txtEmail, 48, 226));

            // PASSWORD
            lblPassword = new Label
            {
                Text = "Password",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(48, 296),
                BackColor = Color.Transparent
            };
            cardPanel.Controls.Add(lblPassword);

            txtPassword = new TextBox
            {
                PlaceholderText = "Enter your password",
                Font = new Font("Segoe UI", 11f),
                ForeColor = Color.FromArgb(71, 85, 105),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                UseSystemPasswordChar = true
            };
            cardPanel.Controls.Add(CreateInputPanel(txtPassword, 48, 319));

            // REGISTER BUTTON
            btnRegister = new Button
            {
                Text = "Create Account",
                Font = new Font("Segoe UI", 12f),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(59, 91, 219),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(464, 52),
                Location = new Point(48, 390),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 78, 196);
            btnRegister.FlatAppearance.MouseDownBackColor = Color.FromArgb(39, 65, 175);
            btnRegister.Click += BtnRegister_Click;
            btnRegister.Paint += BtnPaint;
            cardPanel.Controls.Add(btnRegister);

            // BACK TO LOGIN
            btnBackToLogin = new Button
            {
                Text = "Already have an account? Login",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(59, 91, 219),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                AutoSize = true,
                Cursor = Cursors.Hand
            };
            btnBackToLogin.FlatAppearance.BorderSize = 0;
            btnBackToLogin.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnBackToLogin.Click += BtnBackToLogin_Click;
            btnBackToLogin.Location = new Point((cardPanel.Width - btnBackToLogin.PreferredSize.Width) / 2, 455);
            cardPanel.Controls.Add(btnBackToLogin);
        }

        private Panel CreateInputPanel(TextBox txt, int x, int y)
        {
            Panel panel = new Panel
            {
                Size = new Size(464, 50),
                Location = new Point(x, y),
                BackColor = Color.White
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

        private void BtnPaint(object sender, PaintEventArgs e)
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

        private async void BtnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) ||
                string.IsNullOrEmpty(txtEmail.Text) ||
                string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please fill all fields!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnRegister.Enabled = false;
            btnRegister.Text = "Creating...";

            var registerDto = new RegisterDto
            {
                Name = txtName.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                Role = UserRoles.Customer
            };

            var result = await _accountService.RegisterAsync(registerDto);

            if (result.IsAuthenticated)
            {
                MessageBox.Show("Account created successfully! Please login.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // ✅ روح للـ LoginForm
                var loginForm = _serviceProvider.GetRequiredService<LoginForm>();
                loginForm.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(result.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnRegister.Enabled = true;
                btnRegister.Text = "Create Account";
            }
        }

        private void BtnBackToLogin_Click(object sender, EventArgs e)
        {
            // ✅ بس Close والـ LoginForm هترجع تلقائياً
            this.Close();
        }
    }
}