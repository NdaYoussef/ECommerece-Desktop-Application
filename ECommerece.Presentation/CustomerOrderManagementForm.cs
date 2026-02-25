using System;
using System.Drawing;
using System.Windows.Forms;

namespace OrderManagement
{
    public class OrderManagementForm : Form
    {
        private Panel headerPanel;
        private Label titleLabel;
        private Label adminLabel;
        private Panel adminAvatar;
        private Label avatarIcon;
        private Panel mainCard;
        private Label cardTitle;
        private Label cardSubtitle;
        private DataGridView ordersGrid;

        public OrderManagementForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Order Management";
            this.Size = new Size(1100, 620);
            this.BackColor = Color.FromArgb(243, 244, 246);
            this.Font = new Font("Segoe UI", 9f);
            this.StartPosition = FormStartPosition.CenterScreen;

            // ===== Header =====
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(20, 0, 20, 0)
            };

            titleLabel = new Label
            {
                Text = "Order Management",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 30),
                AutoSize = true,
                Location = new Point(20, 17)
            };

            adminLabel = new Label
            {
                Text = "Admin User",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(60, 60, 60),
                AutoSize = true,
                Location = new Point(920, 20)
            };

            adminAvatar = new Panel
            {
                Size = new Size(38, 38),
                Location = new Point(1020, 11),
                BackColor = Color.FromArgb(37, 99, 235)
            };
            adminAvatar.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.Clear(Color.FromArgb(37, 99, 235));
                using var brush = new SolidBrush(Color.White);
                using var font = new Font("Segoe UI", 13f, FontStyle.Bold);
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString("A", font, brush, new RectangleF(0, 0, 38, 38), sf);
            };
            adminAvatar.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, 38, 38, 19, 19));

            headerPanel.Controls.AddRange(new Control[] { titleLabel, adminLabel, adminAvatar });

            // Separator line under header
            var separator = new Panel
            {
                Dock = DockStyle.Top,
                Height = 1,
                BackColor = Color.FromArgb(229, 231, 235)
            };

            // ===== Main Card =====
            mainCard = new Panel
            {
                BackColor = Color.White,
                Margin = new Padding(20),
                Padding = new Padding(24)
            };
            mainCard.Paint += (s, e) =>
            {
                var rect = new Rectangle(0, 0, mainCard.Width - 1, mainCard.Height - 1);
                using var pen = new Pen(Color.FromArgb(229, 231, 235));
                e.Graphics.DrawRectangle(pen, rect);
            };

            cardTitle = new Label
            {
                Text = "Order Management",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 30),
                AutoSize = true,
                Location = new Point(24, 20)
            };

            cardSubtitle = new Label
            {
                Text = "View and manage customer orders",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Location = new Point(24, 46)
            };

            // ===== DataGridView =====
            ordersGrid = new DataGridView
            {
                Location = new Point(24, 80),
                BorderStyle = BorderStyle.None,
                BackgroundColor = Color.White,
                GridColor = Color.FromArgb(229, 231, 235),
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single,
                Font = new Font("Segoe UI", 9.5f),
                RowTemplate = { Height = 52 }
            };

            ordersGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(239, 246, 255);
            ordersGrid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 30, 30);
            ordersGrid.DefaultCellStyle.BackColor = Color.White;
            ordersGrid.DefaultCellStyle.ForeColor = Color.FromArgb(60, 60, 60);
            ordersGrid.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            ordersGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            ordersGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);

            ordersGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            ordersGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(100, 110, 125);
            ordersGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            ordersGrid.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            ordersGrid.ColumnHeadersHeight = 44;
            ordersGrid.EnableHeadersVisualStyles = false;

            // Columns
            ordersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "OrderID", HeaderText = "Order ID", FillWeight = 15 });
            ordersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Customer", HeaderText = "Customer", FillWeight = 25 });
            ordersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Date", HeaderText = "Date", FillWeight = 20 });
            ordersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Total", HeaderText = "Total", FillWeight = 15 });
            ordersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", FillWeight = 25 });

            ordersGrid.CellPainting += OrdersGrid_CellPainting;

            mainCard.Controls.AddRange(new Control[] { cardTitle, cardSubtitle, ordersGrid });

            // Layout container for card with padding
            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };
            contentPanel.Controls.Add(mainCard);
            contentPanel.Resize += (s, e) =>
            {
                mainCard.Location = new Point(20, 20);
                mainCard.Size = new Size(contentPanel.Width - 40, contentPanel.Height - 40);
                ordersGrid.Size = new Size(mainCard.Width - 48, mainCard.Height - 100);
            };

            this.Controls.Add(contentPanel);
            this.Controls.Add(separator);
            this.Controls.Add(headerPanel);
        }

        private void OrdersGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == 4 && e.RowIndex >= 0)
            {
                e.PaintBackground(e.ClipBounds, true);

                string status = e.Value?.ToString() ?? "";
                Color bgColor, fgColor;

                switch (status)
                {
                    case "Pending":
                        bgColor = Color.FromArgb(243, 244, 246);
                        fgColor = Color.FromArgb(75, 85, 99);
                        break;
                    case "Processing":
                        bgColor = Color.FromArgb(254, 243, 199);
                        fgColor = Color.FromArgb(146, 64, 14);
                        break;
                    case "Shipped":
                        bgColor = Color.FromArgb(219, 234, 254);
                        fgColor = Color.FromArgb(29, 78, 216);
                        break;
                    case "Delivered":
                        bgColor = Color.FromArgb(209, 250, 229);
                        fgColor = Color.FromArgb(6, 95, 70);
                        break;
                    default:
                        bgColor = Color.FromArgb(243, 244, 246);
                        fgColor = Color.FromArgb(75, 85, 99);
                        break;
                }

                // Draw badge
                var badgeRect = new RectangleF(e.CellBounds.X + 10, e.CellBounds.Y + 13, 90, 26);
                using (var path = GetRoundedRect(badgeRect, 13))
                using (var brush = new SolidBrush(bgColor))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(brush, path);
                }

                using var textBrush = new SolidBrush(fgColor);
                using var font = new Font("Segoe UI", 9f, FontStyle.Regular);
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString(status, font, textBrush, badgeRect, sf);

                e.Handled = true;
            }
        }

        private System.Drawing.Drawing2D.GraphicsPath GetRoundedRect(RectangleF rect, float radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            return path;
        }

        [System.Runtime.InteropServices.DllImport("Gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        private void LoadData()
        {
            var orders = new[]
            {
                new { ID = "#1001", Customer = "John Doe",     Date = "2026-02-20", Total = "$1549.98", Status = "Pending" },
                new { ID = "#1002", Customer = "Jane Smith",   Date = "2026-02-19", Total = "$899.99",  Status = "Processing" },
                new { ID = "#1003", Customer = "Bob Johnson",  Date = "2026-02-18", Total = "$249.99",  Status = "Shipped" },
                new { ID = "#1004", Customer = "Alice Williams",Date = "2026-02-17", Total = "$699.99", Status = "Delivered" },
                new { ID = "#1005", Customer = "Charlie Brown", Date = "2026-02-16", Total = "$399.99", Status = "Processing" },
            };

            foreach (var o in orders)
                ordersGrid.Rows.Add(o.ID, o.Customer, o.Date, o.Total, o.Status);
        }

        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new OrderManagementForm());
        //}
    }
}