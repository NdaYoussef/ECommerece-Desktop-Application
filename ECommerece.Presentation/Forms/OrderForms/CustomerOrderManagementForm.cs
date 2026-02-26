using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ECommerece.Application.DTOs.OrderDtos;
using ECommerece.Application.IServices.IOrderService;
using ECommerece.Domain.Enums;
using ECommerece.Presentation.Forms.DashboardForms;
using ECommerece.Presentation.Forms.ProductForms;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerece.Presentation.Forms.UserForms
{
    public class CustomerOrderManagementForm : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IOrdercustomerService _orderCustomerService;

        // UI Controls
        private Panel topBarPanel;
        private Panel mainContentPanel;
        private Panel ordersCardPanel;
        private Panel orderItemsCardPanel;
        private Panel orderStatusCardPanel;
        private DataGridView dgvOrders;
        private DataGridView dgvOrderItems;
        private Label lblOrderItemsTitle;
        private Label lblOrderItemsSubtitle;
        private Panel statusProgressPanel;
        private Label lblTotalValue;

        private Panel sidebarPanel;
        private Panel mainPanel;
        private Panel headerPanel;
        private Button btnDashboard;
        private Button btnProducts;
        private Button btnCart;
        private Button btnOrders;
        private Button btnLogout;
        private Label lblPageTitle;
        private Label lblWelcome;

        // ── Colors ────────────────────────────────────────────────
        static readonly Color Navy = Color.FromArgb(15, 23, 42);
        static readonly Color Accent = Color.FromArgb(59, 91, 219);
        static readonly Color LightBg = Color.FromArgb(241, 245, 249);
        static readonly Color White = Color.White;

        private string _userName = "Customer";
        private List<OrderDto> _orders = new List<OrderDto>();
        private OrderDto _selectedOrder;

        public CustomerOrderManagementForm(
            IServiceProvider serviceProvider,
            IOrdercustomerService orderCustomerService
        )
        {
            _serviceProvider = serviceProvider;
            _orderCustomerService = orderCustomerService;
            InitializeComponents();
            LoadOrdersAsync();
        }

        public void SetUserName(string name)
        {
            _userName = name;
            if (lblWelcome != null)
                lblWelcome.Text = $"Welcome back, {name}! 👋";
        }

        private void InitializeComponents()
        {
            this.Text = "E-Commerce";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(241, 245, 249);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 9f);

            // ===== SIDEBAR =====
            sidebarPanel = new Panel
            {
                Size = new Size(220, this.ClientSize.Height),
                Location = new Point(0, 0),
                BackColor = Navy,
            };
            this.Controls.Add(sidebarPanel);

            var lblLogo = new Label
            {
                Text = "🛒  E-Commerce",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 30),
                BackColor = Color.Transparent,
            };
            sidebarPanel.Controls.Add(lblLogo);

            var lblSub = new Label
            {
                Text = "Management",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(47, 55),
                BackColor = Color.Transparent,
            };
            sidebarPanel.Controls.Add(lblSub);

            var separator = new Panel
            {
                Size = new Size(180, 1),
                Location = new Point(20, 78),
                BackColor = Color.FromArgb(30, 41, 59),
            };
            sidebarPanel.Controls.Add(separator);

            btnDashboard = CreateSidebarButton("📊  Dashboard", 100, false);
            btnProducts = CreateSidebarButton("🛍️  Browse Products", 155, false);
            btnCart = CreateSidebarButton("🛒  Cart", 210, false);
            btnOrders = CreateSidebarButton("📦  My Orders", 265, true);

            sidebarPanel.Controls.Add(btnDashboard);
            sidebarPanel.Controls.Add(btnProducts);
            sidebarPanel.Controls.Add(btnCart);
            sidebarPanel.Controls.Add(btnOrders);

            btnDashboard.Click += (s, e) =>
            {
                var dash = _serviceProvider.GetRequiredService<CustomerDashboardForm>();
                dash.Show();
                this.Hide();
            };
            btnOrders.Click += (s, e) =>
            {
                var dash = _serviceProvider.GetRequiredService<CustomerOrderManagementForm>();
                dash.Show();
                this.Hide();
            };
            btnCart.Click += (s, e) =>
            {
                var dash = _serviceProvider.GetRequiredService<CartForm>();
                dash.Show();
                this.Hide();
            };
            btnProducts.Click += (s, e) =>
            {
                var dash = _serviceProvider.GetRequiredService<CustomerProductsForm>();
                dash.Show();
                this.Hide();
            };

            // Logout Button
            btnLogout = new Button
            {
                Text = "🚪  Logout",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(148, 163, 184),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(220, 45),
                Location = new Point(0, this.ClientSize.Height - 100),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand,
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 41, 59);
            btnLogout.Click += BtnLogout_Click;
            sidebarPanel.Controls.Add(btnLogout);

            // ===== MAIN PANEL =====
            mainPanel = new Panel
            {
                Size = new Size(this.ClientSize.Width - 220, this.ClientSize.Height),
                Location = new Point(220, 0),
                BackColor = Color.FromArgb(241, 245, 249)
            };
            this.Controls.Add(mainPanel);

            // ===== HEADER =====
            headerPanel = new Panel
            {
                Size = new Size(mainPanel.Width, 70),
                Location = new Point(0, 0),
                BackColor = Color.White
            };
            headerPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawLine(pen, 0, headerPanel.Height - 1, headerPanel.Width, headerPanel.Height - 1);
            };
            mainPanel.Controls.Add(headerPanel);

            lblPageTitle = new Label
            {
                Text = "My Orders",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(30, 20),
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(lblPageTitle);

            lblWelcome = new Label
            {
                Text = $"Welcome back, {_userName}! 👋",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            lblWelcome.Location = new Point(mainPanel.Width - lblWelcome.PreferredWidth - 30, 25);
            headerPanel.Controls.Add(lblWelcome);

            // ===== MAIN CONTENT =====
            mainContentPanel = new Panel
            {
                Location = new Point(0, 70),
                Size = new Size(mainPanel.Width, this.ClientSize.Height - 70),
                BackColor = Color.FromArgb(241, 245, 249),
                AutoScroll = true,
            };
            mainPanel.Controls.Add(mainContentPanel);

            // ===== ORDERS CARD =====
            ordersCardPanel = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(mainContentPanel.Width - 40, 280),
                BackColor = Color.White,
            };
            ordersCardPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(220, 225, 235), 1))
                    e.Graphics.DrawRectangle(
                        pen,
                        0,
                        0,
                        ordersCardPanel.Width - 1,
                        ordersCardPanel.Height - 1
                    );
            };
            mainContentPanel.Controls.Add(ordersCardPanel);

            var lblMyOrders = new Label
            {
                Text = "My Orders",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(20, 18),
                BackColor = Color.Transparent,
            };
            ordersCardPanel.Controls.Add(lblMyOrders);

            var lblSubtitle = new Label
            {
                Text = "View your order history and track deliveries",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(130, 140, 160),
                AutoSize = true,
                Location = new Point(20, 42),
                BackColor = Color.Transparent,
            };
            ordersCardPanel.Controls.Add(lblSubtitle);

            // Orders DataGridView
            dgvOrders = new DataGridView
            {
                Location = new Point(0, 68),
                Size = new Size(ordersCardPanel.Width, ordersCardPanel.Height - 68),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single,
                GridColor = Color.FromArgb(235, 238, 245),
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9.5f),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                MultiSelect = false,
                ScrollBars = ScrollBars.Vertical,
                Cursor = Cursors.Hand,
            };
            dgvOrders.EnableHeadersVisualStyles = false;
            dgvOrders.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 252);
            dgvOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(100, 116, 139);
            dgvOrders.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgvOrders.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dgvOrders.ColumnHeadersHeight = 38;
            dgvOrders.RowTemplate.Height = 42;
            dgvOrders.DefaultCellStyle.SelectionBackColor = Color.FromArgb(237, 242, 255);
            dgvOrders.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);
            dgvOrders.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);

            dgvOrders.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colOrderId",
                    HeaderText = "Order ID",
                    FillWeight = 20,
                }
            );
            dgvOrders.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colDate",
                    HeaderText = "Date",
                    FillWeight = 30,
                }
            );
            dgvOrders.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colTotal",
                    HeaderText = "Total",
                    FillWeight = 25,
                }
            );
            dgvOrders.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colStatus",
                    HeaderText = "Status",
                    FillWeight = 25,
                }
            );

            dgvOrders.CellPainting += DgvOrders_CellPainting;
            dgvOrders.SelectionChanged += DgvOrders_SelectionChanged;
            ordersCardPanel.Controls.Add(dgvOrders);

            // ===== BOTTOM SECTION =====
            int bottomY = 320;
            int bottomHeight = this.ClientSize.Height - 70 - bottomY - 20;
            int itemsW = (int)((mainContentPanel.Width - 40) * 0.64);
            int statusX = 20 + itemsW + 10;
            int statusWidth = mainContentPanel.Width - 40 - itemsW - 10;

            // ── Order Items card ──────────────────────────────────────────────────
            orderItemsCardPanel = new Panel
            {
                Location = new Point(20, bottomY),
                Size = new Size(itemsW, bottomHeight),
                BackColor = Color.White,
            };
            orderItemsCardPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(220, 225, 235), 1))
                    e.Graphics.DrawRectangle(
                        pen,
                        0,
                        0,
                        orderItemsCardPanel.Width - 1,
                        orderItemsCardPanel.Height - 1
                    );
            };
            mainContentPanel.Controls.Add(orderItemsCardPanel);

            lblOrderItemsTitle = new Label
            {
                Text = "Order Items",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(20, 18),
                BackColor = Color.Transparent,
            };
            orderItemsCardPanel.Controls.Add(lblOrderItemsTitle);

            lblOrderItemsSubtitle = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(130, 140, 160),
                AutoSize = true,
                Location = new Point(20, 42),
                BackColor = Color.Transparent,
            };
            orderItemsCardPanel.Controls.Add(lblOrderItemsSubtitle);

            dgvOrderItems = new DataGridView
            {
                Location = new Point(0, 68),
                Size = new Size(orderItemsCardPanel.Width, orderItemsCardPanel.Height - 108),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                GridColor = Color.FromArgb(235, 238, 245),
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9.5f),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                MultiSelect = false,
                ScrollBars = ScrollBars.Vertical,
            };
            dgvOrderItems.EnableHeadersVisualStyles = false;
            dgvOrderItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 252);
            dgvOrderItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(100, 116, 139);
            dgvOrderItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgvOrderItems.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dgvOrderItems.ColumnHeadersHeight = 38;
            dgvOrderItems.RowTemplate.Height = 42;
            dgvOrderItems.DefaultCellStyle.SelectionBackColor = Color.FromArgb(237, 242, 255);
            dgvOrderItems.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);
            dgvOrderItems.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);

            dgvOrderItems.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colProduct",
                    HeaderText = "Product",
                    FillWeight = 40,
                }
            );
            dgvOrderItems.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colQty",
                    HeaderText = "Quantity",
                    FillWeight = 20,
                }
            );
            dgvOrderItems.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colPrice",
                    HeaderText = "Price",
                    FillWeight = 20,
                }
            );
            dgvOrderItems.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colSubtotal",
                    HeaderText = "Subtotal",
                    FillWeight = 20,
                }
            );
            orderItemsCardPanel.Controls.Add(dgvOrderItems);

            // Total row
            var totalPanel = new Panel
            {
                Location = new Point(0, orderItemsCardPanel.Height - 40),
                Size = new Size(orderItemsCardPanel.Width, 40),
                BackColor = Color.White,
                Name = "totalPanel",
            };
            totalPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(225, 230, 240), 1))
                    e.Graphics.DrawLine(pen, 0, 0, totalPanel.Width, 0);
            };
            orderItemsCardPanel.Controls.Add(totalPanel);

            var lblTotalLabel = new Label
            {
                Text = "Total:",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(totalPanel.Width - 180, 10),
                BackColor = Color.Transparent,
            };
            totalPanel.Controls.Add(lblTotalLabel);

            lblTotalValue = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(totalPanel.Width - 110, 10),
                BackColor = Color.Transparent,
                Name = "lblTotalValue",
            };
            totalPanel.Controls.Add(lblTotalValue);

            // ── Order Status card ─────────────────────────────────────────────────
            orderStatusCardPanel = new Panel
            {
                Location = new Point(statusX, bottomY),
                Size = new Size(statusWidth, bottomHeight),
                BackColor = Color.White,
            };
            orderStatusCardPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(220, 225, 235), 1))
                    e.Graphics.DrawRectangle(
                        pen,
                        0,
                        0,
                        orderStatusCardPanel.Width - 1,
                        orderStatusCardPanel.Height - 1
                    );
            };
            mainContentPanel.Controls.Add(orderStatusCardPanel);

            var statusIconLabel = new Label
            {
                Text = "🔄",
                Font = new Font("Segoe UI", 13f),
                AutoSize = true,
                Location = new Point(15, 18),
                BackColor = Color.Transparent,
            };
            orderStatusCardPanel.Controls.Add(statusIconLabel);

            var lblStatusTitle = new Label
            {
                Text = "Order Status",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(60, 21),
                BackColor = Color.Transparent,
            };
            orderStatusCardPanel.Controls.Add(lblStatusTitle);

            var statusSep = new Panel
            {
                Location = new Point(0, 55),
                Size = new Size(statusWidth, 1),
                BackColor = Color.FromArgb(230, 235, 245),
            };
            orderStatusCardPanel.Controls.Add(statusSep);

            statusProgressPanel = new Panel
            {
                Location = new Point(0, 56),
                Size = new Size(statusWidth, bottomHeight - 56),
                BackColor = Color.White,
                Name = "statusProgressPanel",
            };
            orderStatusCardPanel.Controls.Add(statusProgressPanel);

            RenderStatusPanel(null);
        }

        // ═══ Status badge painting ════════════════════════════════════════════════

        private void DgvOrders_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex != 3 || e.RowIndex < 0)
                return;

            e.PaintBackground(e.ClipBounds, true);
            string statusText = e.Value?.ToString() ?? "";

            int badgeW = 90,
                badgeH = 24;
            int bx = e.CellBounds.X + (e.CellBounds.Width - badgeW) / 2;
            int by = e.CellBounds.Y + (e.CellBounds.Height - badgeH) / 2;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = RoundedRect(new Rectangle(bx, by, badgeW, badgeH), 5))
            using (var brush = new SolidBrush(GetStatusBadgeColor(statusText)))
                e.Graphics.FillPath(brush, path);

            var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
            };
            e.Graphics.DrawString(
                statusText,
                new Font("Segoe UI", 8.5f),
                new SolidBrush(GetStatusTextColor(statusText)),
                new RectangleF(bx, by, badgeW, badgeH),
                sf
            );

            e.Handled = true;
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(bounds.Right - radius * 2, bounds.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(
                bounds.Right - radius * 2,
                bounds.Bottom - radius * 2,
                radius * 2,
                radius * 2,
                0,
                90
            );
            path.AddArc(bounds.X, bounds.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            return path;
        }

        private Color GetStatusBadgeColor(string status) =>
            status switch
            {
                "Pending" => Color.FromArgb(255, 247, 230),
                "Approved" => Color.FromArgb(220, 252, 231),
                "Processing" => Color.FromArgb(255, 237, 213),
                "Shipped" => Color.FromArgb(219, 234, 254),
                "Delivered" => Color.FromArgb(220, 252, 231),
                "Cancelled" => Color.FromArgb(254, 226, 226),
                "Rejected" => Color.FromArgb(254, 226, 226),
                _ => Color.FromArgb(243, 244, 246),
            };

        private Color GetStatusTextColor(string status) =>
            status switch
            {
                "Pending" => Color.FromArgb(180, 120, 0),
                "Approved" => Color.FromArgb(21, 128, 61),
                "Processing" => Color.FromArgb(194, 65, 12),
                "Shipped" => Color.FromArgb(29, 78, 216),
                "Delivered" => Color.FromArgb(21, 128, 61),
                "Cancelled" => Color.FromArgb(185, 28, 28),
                "Rejected" => Color.FromArgb(185, 28, 28),
                _ => Color.FromArgb(75, 85, 99),
            };

        // ═══ Grid selection ═══════════════════════════════════════════════════════

        private void DgvOrders_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
                return;
            var row = dgvOrders.SelectedRows[0];
            string idStr = row.Cells["colOrderId"].Value?.ToString()?.Replace("#", "") ?? "0";
            if (int.TryParse(idStr, out int orderId))
            {
                _selectedOrder = _orders.Find(o => o.Id == orderId);
                if (_selectedOrder != null)
                {
                    LoadOrderItems(_selectedOrder);
                    RenderStatusPanel(_selectedOrder);
                }
            }
        }

        private void LoadOrderItems(OrderDto order)
        {
            lblOrderItemsTitle.Text = $"Order Items - #{order.Id}";
            lblOrderItemsSubtitle.Text = $"Ordered on {order.OrderDate:yyyy-MM-dd}";

            dgvOrderItems.Rows.Clear();
            decimal total = 0;
            if (order.Items != null)
            {
                foreach (var item in order.Items)
                {
                    decimal subtotal = item.Quantity * item.UnitPrice;
                    total += subtotal;
                    dgvOrderItems.Rows.Add(
                        item.ProductName,
                        item.Quantity,
                        $"${item.UnitPrice:F2}",
                        $"${subtotal:F2}"
                    );
                }
            }

            lblTotalValue.Text = $"${total:F2}";
        }

        // ═══ Status progress renderer ═════════════════════════════════════════════

        private void RenderStatusPanel(OrderDto order)
        {
            statusProgressPanel.Controls.Clear();

            var statuses = new[] { "Pending", "Processing", "Shipped", "Delivered" };
            string current = order?.Status.ToString() ?? "";
            int currentIndex = Array.IndexOf(statuses, current);

            // Current status badge (top-right)
            var lblCurrent = new Label
            {
                Text = string.IsNullOrEmpty(current) ? "No order selected" : current,
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(130, 140, 160),
                AutoSize = true,
                BackColor = Color.Transparent,
            };
            lblCurrent.Location = new Point(
                statusProgressPanel.Width - lblCurrent.PreferredWidth - 15,
                15
            );
            statusProgressPanel.Controls.Add(lblCurrent);

            // Progress bar
            int barY = 50,
                barX = 20;
            int barW = statusProgressPanel.Width - 40,
                barH = 6;

            var progressBg = new Panel
            {
                Location = new Point(barX, barY),
                Size = new Size(barW, barH),
                BackColor = Color.FromArgb(226, 232, 240),
            };
            statusProgressPanel.Controls.Add(progressBg);

            if (currentIndex >= 0)
            {
                int fillW =
                    currentIndex == 0
                        ? barW / 4
                        : (barW * Math.Min(currentIndex, statuses.Length - 1))
                            / (statuses.Length - 1);

                var progressFill = new Panel
                {
                    Location = new Point(barX, barY),
                    Size = new Size(fillW, barH),
                    BackColor = Color.FromArgb(59, 91, 219),
                };
                statusProgressPanel.Controls.Add(progressFill);
            }

            // Step circles + labels
            for (int i = 0; i < statuses.Length; i++)
            {
                bool isCompleted = i <= currentIndex;
                bool isCurrent = i == currentIndex;
                int stepY = barY + 20;
                int circleSize = 20;

                int circleX = barX + (i * barW / (statuses.Length - 1)) - circleSize / 2;
                if (i == 0)
                    circleX = barX;
                if (i == statuses.Length - 1)
                    circleX = barX + barW - circleSize;

                var circlePanel = new Panel
                {
                    Location = new Point(circleX, stepY),
                    Size = new Size(circleSize, circleSize),
                    BackColor = Color.Transparent,
                };

                bool capCompleted = isCompleted;
                bool capCurrent = isCurrent;

                circlePanel.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    if (capCompleted)
                    {
                        e.Graphics.FillEllipse(
                            new SolidBrush(Color.FromArgb(59, 91, 219)),
                            0,
                            0,
                            circlePanel.Width - 1,
                            circlePanel.Height - 1
                        );
                        if (capCurrent)
                            e.Graphics.DrawString(
                                "✓",
                                new Font("Segoe UI", 9f, FontStyle.Bold),
                                Brushes.White,
                                2,
                                1
                            );
                    }
                    else
                    {
                        e.Graphics.FillEllipse(
                            Brushes.White,
                            0,
                            0,
                            circlePanel.Width - 1,
                            circlePanel.Height - 1
                        );
                        using (Pen pen = new Pen(Color.FromArgb(200, 210, 225), 1.5f))
                            e.Graphics.DrawEllipse(
                                pen,
                                0,
                                0,
                                circlePanel.Width - 1,
                                circlePanel.Height - 1
                            );
                    }
                };
                statusProgressPanel.Controls.Add(circlePanel);

                var lblStep = new Label
                {
                    Text = statuses[i],
                    Font = new Font("Segoe UI", 9f, isCurrent ? FontStyle.Bold : FontStyle.Regular),
                    ForeColor = isCompleted
                        ? Color.FromArgb(30, 40, 80)
                        : Color.FromArgb(160, 170, 190),
                    AutoSize = true,
                    BackColor = Color.Transparent,
                };
                lblStep.Location = new Point(
                    circleX - (lblStep.PreferredWidth / 2) + circleSize / 2,
                    stepY + circleSize + 6
                );
                statusProgressPanel.Controls.Add(lblStep);
            }
        }

        // ═══ Load Data ════════════════════════════════════════════════════════════

        private async void LoadOrdersAsync()
        {
            try
            {
                _orders = await _orderCustomerService.getMyOrdersAsync();
                PopulateOrdersGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to load orders:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void PopulateOrdersGrid()
        {
            if (dgvOrders.InvokeRequired)
            {
                dgvOrders.Invoke(new Action(PopulateOrdersGrid));
                return;
            }

            dgvOrders.Rows.Clear();
            foreach (var order in _orders)
            {
                dgvOrders.Rows.Add(
                    $"#{order.Id}",
                    order.OrderDate.ToString("yyyy-MM-dd"),
                    $"${order.TotalAmount:F2}",
                    order.Status.ToString()
                );
            }

            if (dgvOrders.Rows.Count > 0)
            {
                dgvOrders.ClearSelection();
                dgvOrders.Rows[0].Selected = true;
                SelectOrderByRow(dgvOrders.Rows[0]);
            }
        }

        private void SelectOrderByRow(DataGridViewRow row)
        {
            string idStr = row.Cells["colOrderId"].Value?.ToString()?.Replace("#", "") ?? "0";
            if (int.TryParse(idStr, out int orderId))
            {
                _selectedOrder = _orders.Find(o => o.Id == orderId);
                if (_selectedOrder != null)
                {
                    LoadOrderItems(_selectedOrder);
                    RenderStatusPanel(_selectedOrder);
                }
            }
        }

        private Button CreateSidebarButton(string text, int y, bool isActive)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 10f),
                ForeColor = isActive ? Color.White : Color.FromArgb(148, 163, 184),
                BackColor = isActive ? Accent : Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(220, 45),
                Location = new Point(0, y),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand,
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = isActive ? Accent : Color.FromArgb(30, 41, 59);
            return btn;
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to logout?", "Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                _serviceProvider.GetRequiredService<LoginForm>().Show();
                this.Close();
            }
        }
    }
}




















// using System;
// using System.Collections.Generic;
// using System.Drawing;
// using System.Drawing.Drawing2D;
// using System.Windows.Forms;
// using ECommerece.Application.DTOs.OrderDtos;
// using ECommerece.Application.IServices.IOrderService;
// using ECommerece.Domain.Enums;
// using ECommerece.Presentation.Forms.DashboardForms;
// using Microsoft.Extensions.DependencyInjection;

// namespace ECommerece.Presentation.Forms.UserForms
// {
//     public class CustomerOrderManagementForm : Form
//     {
//         private readonly IServiceProvider _serviceProvider;
//         private readonly IOrdercustomerService _orderCustomerService;

//         // UI Controls
//         private Panel topBarPanel;
//         private Panel mainContentPanel;
//         private Panel ordersCardPanel;
//         private Panel orderItemsCardPanel;
//         private Panel orderStatusCardPanel;
//         private DataGridView dgvOrders;
//         private DataGridView dgvOrderItems;
//         private Label lblOrderItemsTitle;
//         private Label lblOrderItemsSubtitle;
//         private Panel statusProgressPanel;
//         private Label lblTotalValue;

//         private Panel sidebarPanel;
//         private Panel mainPanel;
//         private Panel headerPanel;
//         private Button btnDashboard;
//         private Button btnProducts;
//         private Button btnCart;
//         private Button btnOrders;
//         private Button btnLogout;
//         private Label lblPageTitle;
//         private Label lblWelcome;

//         // ── Colors ────────────────────────────────────────────────
//         static readonly Color Navy = Color.FromArgb(15, 23, 42);
//         static readonly Color Accent = Color.FromArgb(59, 91, 219);
//         static readonly Color LightBg = Color.FromArgb(241, 245, 249);
//         static readonly Color White = Color.White;

//         private string _userName = "Customer";
//         private List<OrderDto> _orders = new List<OrderDto>();
//         private OrderDto _selectedOrder;

//         public CustomerOrderManagementForm(
//             IServiceProvider serviceProvider,
//             IOrdercustomerService orderCustomerService
//         )
//         {
//             _serviceProvider = serviceProvider;
//             _orderCustomerService = orderCustomerService;
//             InitializeComponents();
//             LoadOrdersAsync();
//         }

//         public void SetUserName(string name)
//         {
//             _userName = name;
//         }

//         private void InitializeComponents()
//         {
//             this.Text = "My Orders";
//             this.Size = new Size(1280, 720);
//             this.StartPosition = FormStartPosition.CenterScreen;
//             this.BackColor = Color.FromArgb(245, 247, 250);
//             this.FormBorderStyle = FormBorderStyle.FixedSingle;
//             this.MaximizeBox = false;
//             this.Font = new Font("Segoe UI", 9f);

//             // ===== TOP BAR =====
//             topBarPanel = new Panel
//             {
//                 Size = new Size(this.ClientSize.Width - 220, 50),
//                 Location = new Point(220, 0),
//                 BackColor = Color.FromArgb(18, 18, 24),
//             };
//             this.Controls.Add(topBarPanel);

//             var lblPageTitle = new Label
//             {
//                 Text = "My Orders",
//                 Font = new Font("Segoe UI", 12f, FontStyle.Bold),
//                 ForeColor = Color.White,
//                 AutoSize = true,
//                 Location = new Point(20, 13),
//                 BackColor = Color.Transparent,
//             };
//             topBarPanel.Controls.Add(lblPageTitle);

//             var lblUserName = new Label
//             {
//                 Text = _userName,
//                 Font = new Font("Segoe UI", 10f),
//                 ForeColor = Color.White,
//                 AutoSize = true,
//                 BackColor = Color.Transparent,
//             };
//             lblUserName.Location = new Point(
//                 this.ClientSize.Width - lblUserName.PreferredWidth - 60,
//                 15
//             );
//             topBarPanel.Controls.Add(lblUserName);

//             var avatarPanel = new Panel
//             {
//                 Size = new Size(32, 32),
//                 Location = new Point(this.ClientSize.Width - 48, 9),
//                 BackColor = Color.Transparent,
//             };
//             avatarPanel.Paint += (s, e) =>
//             {
//                 e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
//                 e.Graphics.FillEllipse(
//                     new SolidBrush(Color.FromArgb(59, 91, 219)),
//                     0,
//                     0,
//                     avatarPanel.Width - 1,
//                     avatarPanel.Height - 1
//                 );
//                 var initial = _userName.Length > 0 ? _userName[0].ToString().ToUpper() : "U";
//                 var font = new Font("Segoe UI", 11f, FontStyle.Bold);
//                 var sz = e.Graphics.MeasureString(initial, font);
//                 e.Graphics.DrawString(
//                     initial,
//                     font,
//                     Brushes.White,
//                     (avatarPanel.Width - sz.Width) / 2,
//                     (avatarPanel.Height - sz.Height) / 2
//                 );
//             };
//             topBarPanel.Controls.Add(avatarPanel);

//             // Back button
//             var btnBack = new Button
//             {
//                 Text = "← Back",
//                 Font = new Font("Segoe UI", 9f),
//                 ForeColor = Color.FromArgb(180, 180, 200),
//                 BackColor = Color.Transparent,
//                 FlatStyle = FlatStyle.Flat,
//                 Size = new Size(80, 30),
//                 Location = new Point(this.ClientSize.Width - 200, 10),
//                 Cursor = Cursors.Hand,
//             };
//             btnBack.FlatAppearance.BorderSize = 0;
//             btnBack.Click += (s, e) =>
//             {
//                 var dash = _serviceProvider.GetRequiredService<CustomerDashboardForm>();
//                 dash.Show();
//                 this.Hide();
//             };
//             topBarPanel.Controls.Add(btnBack);

//             // ===== SIDEBAR =====
//             sidebarPanel = new Panel
//             {
//                 Size = new Size(220, this.ClientSize.Height),
//                 Location = new Point(0, 0),
//                 BackColor = Navy,
//             };
//             this.Controls.Add(sidebarPanel);

//             var lblLogo = new Label
//             {
//                 Text = "🛒  E-Commerce",
//                 Font = new Font("Segoe UI", 13f, FontStyle.Bold),
//                 ForeColor = Color.White,
//                 AutoSize = true,
//                 Location = new Point(20, 30),
//                 BackColor = Color.Transparent,
//             };
//             sidebarPanel.Controls.Add(lblLogo);

//             var lblSub = new Label
//             {
//                 Text = "Management",
//                 Font = new Font("Segoe UI", 9f),
//                 ForeColor = Color.FromArgb(100, 116, 139),
//                 AutoSize = true,
//                 Location = new Point(47, 55),
//                 BackColor = Color.Transparent,
//             };
//             sidebarPanel.Controls.Add(lblSub);

//             var separator = new Panel
//             {
//                 Size = new Size(180, 1),
//                 Location = new Point(20, 78),
//                 BackColor = Color.FromArgb(30, 41, 59),
//             };
//             sidebarPanel.Controls.Add(separator);

//             btnDashboard = CreateSidebarButton("📊  Dashboard", 100, false);
//             btnProducts = CreateSidebarButton("🛍️  Browse Products", 155, false);
//             btnCart = CreateSidebarButton("🛒  Cart", 210, false);
//             btnOrders = CreateSidebarButton("📦  My Orders", 265, true);

//             sidebarPanel.Controls.Add(btnDashboard);
//             sidebarPanel.Controls.Add(btnProducts);
//             sidebarPanel.Controls.Add(btnCart);
//             sidebarPanel.Controls.Add(btnOrders);

//             btnDashboard.Click += (s, e) =>
//             {
//                 var dash = _serviceProvider.GetRequiredService<CustomerDashboardForm>();
//                 dash.Show();
//                 this.Hide();
//             };
//             btnOrders.Click += (s, e) =>
//             {
//                 var dash = _serviceProvider.GetRequiredService<CustomerOrderManagementForm>();
//                 dash.Show();
//                 this.Hide();
//             };
//             btnCart.Click += (s, e) =>
//             {
//                 var dash = _serviceProvider.GetRequiredService<CartForm>();
//                 dash.Show();
//                 this.Hide();
//             };

//             // Logout Button
//             btnLogout = new Button
//             {
//                 Text = "🚪  Logout",
//                 Font = new Font("Segoe UI", 10f),
//                 ForeColor = Color.FromArgb(148, 163, 184),
//                 BackColor = Color.Transparent,
//                 FlatStyle = FlatStyle.Flat,
//                 Size = new Size(220, 45),
//                 Location = new Point(0, this.ClientSize.Height - 60),
//                 TextAlign = ContentAlignment.MiddleLeft,
//                 Padding = new Padding(20, 0, 0, 0),
//                 Cursor = Cursors.Hand,
//             };
//             btnLogout.FlatAppearance.BorderSize = 0;
//             btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 41, 59);
//             btnLogout.Click += BtnLogout_Click;
//             sidebarPanel.Controls.Add(btnLogout);

//             // ===== MAIN CONTENT =====
//             mainContentPanel = new Panel
//             {
//                 Location = new Point(220, 50),
//                 Size = new Size(this.ClientSize.Width - 220, this.ClientSize.Height - 50),
//                 BackColor = Color.FromArgb(245, 247, 250),
//                 AutoScroll = true,
//             };
//             this.Controls.Add(mainContentPanel);

//             // ===== ORDERS CARD =====
//             ordersCardPanel = new Panel
//             {
//                 Location = new Point(20, 20),
//                 Size = new Size(this.ClientSize.Width - 40, 280),
//                 BackColor = Color.White,
//             };
//             ordersCardPanel.Paint += (s, e) =>
//             {
//                 using (Pen pen = new Pen(Color.FromArgb(220, 225, 235), 1))
//                     e.Graphics.DrawRectangle(
//                         pen,
//                         0,
//                         0,
//                         ordersCardPanel.Width - 1,
//                         ordersCardPanel.Height - 1
//                     );
//             };
//             mainContentPanel.Controls.Add(ordersCardPanel);

//             var lblMyOrders = new Label
//             {
//                 Text = "My Orders",
//                 Font = new Font("Segoe UI", 13f, FontStyle.Bold),
//                 ForeColor = Color.FromArgb(15, 23, 42),
//                 AutoSize = true,
//                 Location = new Point(20, 18),
//                 BackColor = Color.Transparent,
//             };
//             ordersCardPanel.Controls.Add(lblMyOrders);

//             var lblSubtitle = new Label
//             {
//                 Text = "View your order history and track deliveries",
//                 Font = new Font("Segoe UI", 9f),
//                 ForeColor = Color.FromArgb(130, 140, 160),
//                 AutoSize = true,
//                 Location = new Point(20, 42),
//                 BackColor = Color.Transparent,
//             };
//             ordersCardPanel.Controls.Add(lblSubtitle);

//             // Orders DataGridView
//             dgvOrders = new DataGridView
//             {
//                 Location = new Point(0, 68),
//                 Size = new Size(ordersCardPanel.Width, ordersCardPanel.Height - 68),
//                 BackgroundColor = Color.White,
//                 BorderStyle = BorderStyle.None,
//                 CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
//                 ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single,
//                 GridColor = Color.FromArgb(235, 238, 245),
//                 RowHeadersVisible = false,
//                 AllowUserToAddRows = false,
//                 AllowUserToDeleteRows = false,
//                 ReadOnly = true,
//                 SelectionMode = DataGridViewSelectionMode.FullRowSelect,
//                 Font = new Font("Segoe UI", 9.5f),
//                 AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
//                 MultiSelect = false,
//                 ScrollBars = ScrollBars.Vertical,
//                 Cursor = Cursors.Hand,
//             };
//             dgvOrders.EnableHeadersVisualStyles = false;
//             dgvOrders.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 252);
//             dgvOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(100, 116, 139);
//             dgvOrders.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f);
//             dgvOrders.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
//             dgvOrders.ColumnHeadersHeight = 38;
//             dgvOrders.RowTemplate.Height = 42;
//             dgvOrders.DefaultCellStyle.SelectionBackColor = Color.FromArgb(237, 242, 255);
//             dgvOrders.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);
//             dgvOrders.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);

//             dgvOrders.Columns.Add(
//                 new DataGridViewTextBoxColumn
//                 {
//                     Name = "colOrderId",
//                     HeaderText = "Order ID",
//                     FillWeight = 20,
//                 }
//             );
//             dgvOrders.Columns.Add(
//                 new DataGridViewTextBoxColumn
//                 {
//                     Name = "colDate",
//                     HeaderText = "Date",
//                     FillWeight = 30,
//                 }
//             );
//             dgvOrders.Columns.Add(
//                 new DataGridViewTextBoxColumn
//                 {
//                     Name = "colTotal",
//                     HeaderText = "Total",
//                     FillWeight = 25,
//                 }
//             );
//             dgvOrders.Columns.Add(
//                 new DataGridViewTextBoxColumn
//                 {
//                     Name = "colStatus",
//                     HeaderText = "Status",
//                     FillWeight = 25,
//                 }
//             );

//             dgvOrders.CellPainting += DgvOrders_CellPainting;
//             dgvOrders.SelectionChanged += DgvOrders_SelectionChanged;
//             ordersCardPanel.Controls.Add(dgvOrders);

//             // ===== BOTTOM SECTION =====
//             int bottomY = 320;
//             int bottomHeight = this.ClientSize.Height - 50 - bottomY - 20;
//             int itemsW = (int)((this.ClientSize.Width - 40) * 0.64);
//             int statusX = 20 + itemsW + 10;
//             int statusWidth = this.ClientSize.Width - 40 - itemsW - 10;

//             // ── Order Items card ──────────────────────────────────────────────────
//             orderItemsCardPanel = new Panel
//             {
//                 Location = new Point(20, bottomY),
//                 Size = new Size(itemsW, bottomHeight),
//                 BackColor = Color.White,
//             };
//             orderItemsCardPanel.Paint += (s, e) =>
//             {
//                 using (Pen pen = new Pen(Color.FromArgb(220, 225, 235), 1))
//                     e.Graphics.DrawRectangle(
//                         pen,
//                         0,
//                         0,
//                         orderItemsCardPanel.Width - 1,
//                         orderItemsCardPanel.Height - 1
//                     );
//             };
//             mainContentPanel.Controls.Add(orderItemsCardPanel);

//             lblOrderItemsTitle = new Label
//             {
//                 Text = "Order Items",
//                 Font = new Font("Segoe UI", 12f, FontStyle.Bold),
//                 ForeColor = Color.FromArgb(15, 23, 42),
//                 AutoSize = true,
//                 Location = new Point(20, 18),
//                 BackColor = Color.Transparent,
//             };
//             orderItemsCardPanel.Controls.Add(lblOrderItemsTitle);

//             lblOrderItemsSubtitle = new Label
//             {
//                 Text = "",
//                 Font = new Font("Segoe UI", 9f),
//                 ForeColor = Color.FromArgb(130, 140, 160),
//                 AutoSize = true,
//                 Location = new Point(20, 42),
//                 BackColor = Color.Transparent,
//             };
//             orderItemsCardPanel.Controls.Add(lblOrderItemsSubtitle);

//             dgvOrderItems = new DataGridView
//             {
//                 Location = new Point(0, 68),
//                 Size = new Size(orderItemsCardPanel.Width, orderItemsCardPanel.Height - 108),
//                 BackgroundColor = Color.White,
//                 BorderStyle = BorderStyle.None,
//                 CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
//                 ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
//                 GridColor = Color.FromArgb(235, 238, 245),
//                 RowHeadersVisible = false,
//                 AllowUserToAddRows = false,
//                 AllowUserToDeleteRows = false,
//                 ReadOnly = true,
//                 SelectionMode = DataGridViewSelectionMode.FullRowSelect,
//                 Font = new Font("Segoe UI", 9.5f),
//                 AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
//                 MultiSelect = false,
//                 ScrollBars = ScrollBars.Vertical,
//             };
//             dgvOrderItems.EnableHeadersVisualStyles = false;
//             dgvOrderItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 252);
//             dgvOrderItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(100, 116, 139);
//             dgvOrderItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f);
//             dgvOrderItems.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
//             dgvOrderItems.ColumnHeadersHeight = 38;
//             dgvOrderItems.RowTemplate.Height = 42;
//             dgvOrderItems.DefaultCellStyle.SelectionBackColor = Color.FromArgb(237, 242, 255);
//             dgvOrderItems.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);
//             dgvOrderItems.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);

//             dgvOrderItems.Columns.Add(
//                 new DataGridViewTextBoxColumn
//                 {
//                     Name = "colProduct",
//                     HeaderText = "Product",
//                     FillWeight = 40,
//                 }
//             );
//             dgvOrderItems.Columns.Add(
//                 new DataGridViewTextBoxColumn
//                 {
//                     Name = "colQty",
//                     HeaderText = "Quantity",
//                     FillWeight = 20,
//                 }
//             );
//             dgvOrderItems.Columns.Add(
//                 new DataGridViewTextBoxColumn
//                 {
//                     Name = "colPrice",
//                     HeaderText = "Price",
//                     FillWeight = 20,
//                 }
//             );
//             dgvOrderItems.Columns.Add(
//                 new DataGridViewTextBoxColumn
//                 {
//                     Name = "colSubtotal",
//                     HeaderText = "Subtotal",
//                     FillWeight = 20,
//                 }
//             );
//             orderItemsCardPanel.Controls.Add(dgvOrderItems);

//             // Total row
//             var totalPanel = new Panel
//             {
//                 Location = new Point(0, orderItemsCardPanel.Height - 40),
//                 Size = new Size(orderItemsCardPanel.Width, 40),
//                 BackColor = Color.White,
//                 Name = "totalPanel",
//             };
//             totalPanel.Paint += (s, e) =>
//             {
//                 using (Pen pen = new Pen(Color.FromArgb(225, 230, 240), 1))
//                     e.Graphics.DrawLine(pen, 0, 0, totalPanel.Width, 0);
//             };
//             orderItemsCardPanel.Controls.Add(totalPanel);

//             var lblTotalLabel = new Label
//             {
//                 Text = "Total:",
//                 Font = new Font("Segoe UI", 10f),
//                 ForeColor = Color.FromArgb(100, 116, 139),
//                 AutoSize = true,
//                 Location = new Point(totalPanel.Width - 180, 10),
//                 BackColor = Color.Transparent,
//             };
//             totalPanel.Controls.Add(lblTotalLabel);

//             lblTotalValue = new Label
//             {
//                 Text = "",
//                 Font = new Font("Segoe UI", 10f, FontStyle.Bold),
//                 ForeColor = Color.FromArgb(15, 23, 42),
//                 AutoSize = true,
//                 Location = new Point(totalPanel.Width - 110, 10),
//                 BackColor = Color.Transparent,
//                 Name = "lblTotalValue",
//             };
//             totalPanel.Controls.Add(lblTotalValue);

//             // ── Order Status card ─────────────────────────────────────────────────
//             orderStatusCardPanel = new Panel
//             {
//                 Location = new Point(statusX, bottomY),
//                 Size = new Size(statusWidth, bottomHeight),
//                 BackColor = Color.White,
//             };
//             orderStatusCardPanel.Paint += (s, e) =>
//             {
//                 using (Pen pen = new Pen(Color.FromArgb(220, 225, 235), 1))
//                     e.Graphics.DrawRectangle(
//                         pen,
//                         0,
//                         0,
//                         orderStatusCardPanel.Width - 1,
//                         orderStatusCardPanel.Height - 1
//                     );
//             };
//             mainContentPanel.Controls.Add(orderStatusCardPanel);

//             var statusIconLabel = new Label
//             {
//                 Text = "🔄",
//                 Font = new Font("Segoe UI", 13f),
//                 AutoSize = true,
//                 Location = new Point(15, 18),
//                 BackColor = Color.Transparent,
//             };
//             orderStatusCardPanel.Controls.Add(statusIconLabel);

//             var lblStatusTitle = new Label
//             {
//                 Text = "Order Status",
//                 Font = new Font("Segoe UI", 12f, FontStyle.Bold),
//                 ForeColor = Color.FromArgb(15, 23, 42),
//                 AutoSize = true,
//                 Location = new Point(40, 21),
//                 BackColor = Color.Transparent,
//             };
//             orderStatusCardPanel.Controls.Add(lblStatusTitle);

//             var statusSep = new Panel
//             {
//                 Location = new Point(0, 55),
//                 Size = new Size(statusWidth, 1),
//                 BackColor = Color.FromArgb(230, 235, 245),
//             };
//             orderStatusCardPanel.Controls.Add(statusSep);

//             statusProgressPanel = new Panel
//             {
//                 Location = new Point(0, 56),
//                 Size = new Size(statusWidth, bottomHeight - 56),
//                 BackColor = Color.White,
//                 Name = "statusProgressPanel",
//             };
//             orderStatusCardPanel.Controls.Add(statusProgressPanel);

//             RenderStatusPanel(null);
//         }

//         // ═══ Status badge painting ════════════════════════════════════════════════

//         private void DgvOrders_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
//         {
//             if (e.ColumnIndex != 3 || e.RowIndex < 0)
//                 return;

//             e.PaintBackground(e.ClipBounds, true);
//             string statusText = e.Value?.ToString() ?? "";

//             int badgeW = 90,
//                 badgeH = 24;
//             int bx = e.CellBounds.X + (e.CellBounds.Width - badgeW) / 2;
//             int by = e.CellBounds.Y + (e.CellBounds.Height - badgeH) / 2;

//             e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
//             using (var path = RoundedRect(new Rectangle(bx, by, badgeW, badgeH), 5))
//             using (var brush = new SolidBrush(GetStatusBadgeColor(statusText)))
//                 e.Graphics.FillPath(brush, path);

//             var sf = new StringFormat
//             {
//                 Alignment = StringAlignment.Center,
//                 LineAlignment = StringAlignment.Center,
//             };
//             e.Graphics.DrawString(
//                 statusText,
//                 new Font("Segoe UI", 8.5f),
//                 new SolidBrush(GetStatusTextColor(statusText)),
//                 new RectangleF(bx, by, badgeW, badgeH),
//                 sf
//             );

//             e.Handled = true;
//         }

//         private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
//         {
//             var path = new GraphicsPath();
//             path.AddArc(bounds.X, bounds.Y, radius * 2, radius * 2, 180, 90);
//             path.AddArc(bounds.Right - radius * 2, bounds.Y, radius * 2, radius * 2, 270, 90);
//             path.AddArc(
//                 bounds.Right - radius * 2,
//                 bounds.Bottom - radius * 2,
//                 radius * 2,
//                 radius * 2,
//                 0,
//                 90
//             );
//             path.AddArc(bounds.X, bounds.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
//             path.CloseFigure();
//             return path;
//         }

//         private Color GetStatusBadgeColor(string status) =>
//             status switch
//             {
//                 "Pending" => Color.FromArgb(255, 247, 230),
//                 "Approved" => Color.FromArgb(220, 252, 231),
//                 "Processing" => Color.FromArgb(255, 237, 213),
//                 "Shipped" => Color.FromArgb(219, 234, 254),
//                 "Delivered" => Color.FromArgb(220, 252, 231),
//                 "Cancelled" => Color.FromArgb(254, 226, 226),
//                 "Rejected" => Color.FromArgb(254, 226, 226),
//                 _ => Color.FromArgb(243, 244, 246),
//             };

//         private Color GetStatusTextColor(string status) =>
//             status switch
//             {
//                 "Pending" => Color.FromArgb(180, 120, 0),
//                 "Approved" => Color.FromArgb(21, 128, 61),
//                 "Processing" => Color.FromArgb(194, 65, 12),
//                 "Shipped" => Color.FromArgb(29, 78, 216),
//                 "Delivered" => Color.FromArgb(21, 128, 61),
//                 "Cancelled" => Color.FromArgb(185, 28, 28),
//                 "Rejected" => Color.FromArgb(185, 28, 28),
//                 _ => Color.FromArgb(75, 85, 99),
//             };

//         // ═══ Grid selection ═══════════════════════════════════════════════════════

//         private void DgvOrders_SelectionChanged(object sender, EventArgs e)
//         {
//             if (dgvOrders.SelectedRows.Count == 0)
//                 return;
//             var row = dgvOrders.SelectedRows[0];
//             string idStr = row.Cells["colOrderId"].Value?.ToString()?.Replace("#", "") ?? "0";
//             if (int.TryParse(idStr, out int orderId))
//             {
//                 _selectedOrder = _orders.Find(o => o.Id == orderId);
//                 if (_selectedOrder != null)
//                 {
//                     LoadOrderItems(_selectedOrder);
//                     RenderStatusPanel(_selectedOrder);
//                 }
//             }
//         }

//         private void LoadOrderItems(OrderDto order)
//         {
//             lblOrderItemsTitle.Text = $"Order Items - #{order.Id}";
//             lblOrderItemsSubtitle.Text = $"Ordered on {order.OrderDate:yyyy-MM-dd}";

//             dgvOrderItems.Rows.Clear();
//             decimal total = 0;
//             if (order.Items != null)
//             {
//                 foreach (var item in order.Items)
//                 {
//                     decimal subtotal = item.Quantity * item.UnitPrice;
//                     total += subtotal;
//                     dgvOrderItems.Rows.Add(
//                         item.ProductName,
//                         item.Quantity,
//                         $"${item.UnitPrice:F2}",
//                         $"${subtotal:F2}"
//                     );
//                 }
//             }

//             lblTotalValue.Text = $"${total:F2}";
//         }

//         // ═══ Status progress renderer ═════════════════════════════════════════════

//         private void RenderStatusPanel(OrderDto order)
//         {
//             statusProgressPanel.Controls.Clear();

//             var statuses = new[] { "Pending", "Processing", "Shipped", "Delivered" };
//             string current = order?.Status.ToString() ?? "";
//             int currentIndex = Array.IndexOf(statuses, current);

//             // Current status badge (top-right)
//             var lblCurrent = new Label
//             {
//                 Text = string.IsNullOrEmpty(current) ? "No order selected" : current,
//                 Font = new Font("Segoe UI", 9f),
//                 ForeColor = Color.FromArgb(130, 140, 160),
//                 AutoSize = true,
//                 BackColor = Color.Transparent,
//             };
//             lblCurrent.Location = new Point(
//                 statusProgressPanel.Width - lblCurrent.PreferredWidth - 15,
//                 15
//             );
//             statusProgressPanel.Controls.Add(lblCurrent);

//             // Progress bar
//             int barY = 50,
//                 barX = 20;
//             int barW = statusProgressPanel.Width - 40,
//                 barH = 6;

//             var progressBg = new Panel
//             {
//                 Location = new Point(barX, barY),
//                 Size = new Size(barW, barH),
//                 BackColor = Color.FromArgb(226, 232, 240),
//             };
//             statusProgressPanel.Controls.Add(progressBg);

//             if (currentIndex >= 0)
//             {
//                 int fillW =
//                     currentIndex == 0
//                         ? barW / 4
//                         : (barW * Math.Min(currentIndex, statuses.Length - 1))
//                             / (statuses.Length - 1);

//                 var progressFill = new Panel
//                 {
//                     Location = new Point(barX, barY),
//                     Size = new Size(fillW, barH),
//                     BackColor = Color.FromArgb(59, 91, 219),
//                 };
//                 statusProgressPanel.Controls.Add(progressFill);
//             }

//             // Step circles + labels
//             for (int i = 0; i < statuses.Length; i++)
//             {
//                 bool isCompleted = i <= currentIndex;
//                 bool isCurrent = i == currentIndex;
//                 int stepY = barY + 20;
//                 int circleSize = 20;

//                 int circleX = barX + (i * barW / (statuses.Length - 1)) - circleSize / 2;
//                 if (i == 0)
//                     circleX = barX;
//                 if (i == statuses.Length - 1)
//                     circleX = barX + barW - circleSize;

//                 var circlePanel = new Panel
//                 {
//                     Location = new Point(circleX, stepY),
//                     Size = new Size(circleSize, circleSize),
//                     BackColor = Color.Transparent,
//                 };

//                 bool capCompleted = isCompleted;
//                 bool capCurrent = isCurrent;

//                 circlePanel.Paint += (s, e) =>
//                 {
//                     e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
//                     if (capCompleted)
//                     {
//                         e.Graphics.FillEllipse(
//                             new SolidBrush(Color.FromArgb(59, 91, 219)),
//                             0,
//                             0,
//                             circlePanel.Width - 1,
//                             circlePanel.Height - 1
//                         );
//                         if (capCurrent)
//                             e.Graphics.DrawString(
//                                 "✓",
//                                 new Font("Segoe UI", 9f, FontStyle.Bold),
//                                 Brushes.White,
//                                 2,
//                                 1
//                             );
//                     }
//                     else
//                     {
//                         e.Graphics.FillEllipse(
//                             Brushes.White,
//                             0,
//                             0,
//                             circlePanel.Width - 1,
//                             circlePanel.Height - 1
//                         );
//                         using (Pen pen = new Pen(Color.FromArgb(200, 210, 225), 1.5f))
//                             e.Graphics.DrawEllipse(
//                                 pen,
//                                 0,
//                                 0,
//                                 circlePanel.Width - 1,
//                                 circlePanel.Height - 1
//                             );
//                     }
//                 };
//                 statusProgressPanel.Controls.Add(circlePanel);

//                 var lblStep = new Label
//                 {
//                     Text = statuses[i],
//                     Font = new Font("Segoe UI", 9f, isCurrent ? FontStyle.Bold : FontStyle.Regular),
//                     ForeColor = isCompleted
//                         ? Color.FromArgb(30, 40, 80)
//                         : Color.FromArgb(160, 170, 190),
//                     AutoSize = true,
//                     BackColor = Color.Transparent,
//                 };
//                 lblStep.Location = new Point(
//                     circleX - (lblStep.PreferredWidth / 2) + circleSize / 2,
//                     stepY + circleSize + 6
//                 );
//                 statusProgressPanel.Controls.Add(lblStep);
//             }
//         }

//         // ═══ Load Data ════════════════════════════════════════════════════════════

//         private async void LoadOrdersAsync()
//         {
//             try
//             {
//                 _orders = await _orderCustomerService.getMyOrdersAsync();
//                 PopulateOrdersGrid();
//             }
//             catch (Exception ex)
//             {
//                 MessageBox.Show(
//                     $"Failed to load orders:\n{ex.Message}",
//                     "Error",
//                     MessageBoxButtons.OK,
//                     MessageBoxIcon.Error
//                 );
//             }
//         }

//         private void PopulateOrdersGrid()
//         {
//             if (dgvOrders.InvokeRequired)
//             {
//                 dgvOrders.Invoke(new Action(PopulateOrdersGrid));
//                 return;
//             }

//             dgvOrders.Rows.Clear();
//             foreach (var order in _orders)
//             {
//                 dgvOrders.Rows.Add(
//                     $"#{order.Id}",
//                     order.OrderDate.ToString("yyyy-MM-dd"),
//                     $"${order.TotalAmount:F2}",
//                     order.Status.ToString()
//                 );
//             }

//             if (dgvOrders.Rows.Count > 0)
//             {
//                 dgvOrders.ClearSelection();
//                 dgvOrders.Rows[0].Selected = true;
//                 SelectOrderByRow(dgvOrders.Rows[0]);
//             }
//         }

//         private void SelectOrderByRow(DataGridViewRow row)
//         {
//             string idStr = row.Cells["colOrderId"].Value?.ToString()?.Replace("#", "") ?? "0";
//             if (int.TryParse(idStr, out int orderId))
//             {
//                 _selectedOrder = _orders.Find(o => o.Id == orderId);
//                 if (_selectedOrder != null)
//                 {
//                     LoadOrderItems(_selectedOrder);
//                     RenderStatusPanel(_selectedOrder);
//                 }
//             }
//         }

//         private Button CreateSidebarButton(string text, int y, bool isActive)
//         {
//             var btn = new Button
//             {
//                 Text = text,
//                 Font = new Font("Segoe UI", 10f),
//                 ForeColor = isActive ? Color.White : Color.FromArgb(148, 163, 184),
//                 BackColor = isActive ? Accent : Color.Transparent,
//                 FlatStyle = FlatStyle.Flat,
//                 Size = new Size(220, 45),
//                 Location = new Point(0, y),
//                 TextAlign = ContentAlignment.MiddleLeft,
//                 Padding = new Padding(20, 0, 0, 0),
//                 Cursor = Cursors.Hand,
//             };
//             btn.FlatAppearance.BorderSize = 0;
//             btn.FlatAppearance.MouseOverBackColor = isActive ? Accent : Color.FromArgb(30, 41, 59);
//             return btn;
//         }

//         private void BtnLogout_Click(object sender, EventArgs e)
//         {
//             var confirm = MessageBox.Show(
//                 "Are you sure you want to logout?", "Logout",
//                 MessageBoxButtons.YesNo, MessageBoxIcon.Question);

//             if (confirm == DialogResult.Yes)
//             {
//                 _serviceProvider.GetRequiredService<LoginForm>().Show();
//                 this.Close();
//             }
//         }
//     }
// }

