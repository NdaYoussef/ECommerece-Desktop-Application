using ECommerece.Application.DTOs.OrderDtos;
using ECommerece.Application.IServices.IOrderService;
using ECommerece.Domain.Entities;
using ECommerece.Presentation.Forms.DashboardForms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

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
        private Label lblStatusTitle;
        private Panel statusProgressPanel;
        private string _userName = "John Doe";
        private List<OrderDto> _orders = new List<OrderDto>();
        private OrderDto _selectedOrder;

        public CustomerOrderManagementForm(IServiceProvider serviceProvider, IOrdercustomerService orderCustomerService)
        {
            _serviceProvider = serviceProvider;
            _orderCustomerService = orderCustomerService;
            InitializeComponents();
            LoadOrdersAsync();
        }

        public void SetUserName(string name)
        {
            _userName = name;
        }

        private void InitializeComponents()
        {
            this.Text = "My Orders";
            this.Size = new Size(1280, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 9f);

            // ===== TOP BAR =====
            topBarPanel = new Panel
            {
                Size = new Size(this.ClientSize.Width, 50),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(18, 18, 24)
            };
            this.Controls.Add(topBarPanel);

            var lblPageTitle = new Label
            {
                Text = "My Orders",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 13),
                BackColor = Color.Transparent
            };
            topBarPanel.Controls.Add(lblPageTitle);

            // User avatar + name (top right)
            var lblUserName = new Label
            {
                Text = _userName,
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent
            };
            lblUserName.Location = new Point(this.ClientSize.Width - lblUserName.PreferredWidth - 60, 15);
            topBarPanel.Controls.Add(lblUserName);

            var avatarPanel = new Panel
            {
                Size = new Size(32, 32),
                Location = new Point(this.ClientSize.Width - 52, 9),
                BackColor = Color.FromArgb(59, 91, 219)
            };
            avatarPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(59, 91, 219)),
                    0, 0, avatarPanel.Width - 1, avatarPanel.Height - 1);
                var initial = _userName.Length > 0 ? _userName[0].ToString().ToUpper() : "U";
                var font = new Font("Segoe UI", 11f, FontStyle.Bold);
                var size = e.Graphics.MeasureString(initial, font);
                e.Graphics.DrawString(initial, font, Brushes.White,
                    (avatarPanel.Width - size.Width) / 2, (avatarPanel.Height - size.Height) / 2);
            };
            topBarPanel.Controls.Add(avatarPanel);

            // Back button
            var btnBack = new Button
            {
                Text = "← Back",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(180, 180, 200),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(80, 30),
                Location = new Point(this.ClientSize.Width - 200, 10),
                Cursor = Cursors.Hand
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Click += (s, e) =>
            {
                var dash = _serviceProvider.GetRequiredService<CustomerDashboardForm>();
                dash.Show();
                this.Hide();
            };
            topBarPanel.Controls.Add(btnBack);

            // ===== MAIN CONTENT =====
            mainContentPanel = new Panel
            {
                Location = new Point(0, 50),
                Size = new Size(this.ClientSize.Width, this.ClientSize.Height - 50),
                BackColor = Color.FromArgb(245, 247, 250),
                AutoScroll = true
            };
            this.Controls.Add(mainContentPanel);

            // ===== ORDERS CARD =====
            ordersCardPanel = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(this.ClientSize.Width - 40, 280),
                BackColor = Color.White
            };
            ordersCardPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(220, 225, 235), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, ordersCardPanel.Width - 1, ordersCardPanel.Height - 1);
            };
            mainContentPanel.Controls.Add(ordersCardPanel);

            // Card title
            var lblMyOrders = new Label
            {
                Text = "My Orders",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(20, 18),
                BackColor = Color.Transparent
            };
            ordersCardPanel.Controls.Add(lblMyOrders);

            var lblSubtitle = new Label
            {
                Text = "View your order history and track deliveries",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(130, 140, 160),
                AutoSize = true,
                Location = new Point(20, 42),
                BackColor = Color.Transparent
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
                ScrollBars = ScrollBars.Vertical
            };
            dgvOrders.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 252);
            dgvOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(100, 116, 139);
            dgvOrders.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            dgvOrders.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dgvOrders.ColumnHeadersHeight = 38;
            dgvOrders.RowTemplate.Height = 42;
            dgvOrders.DefaultCellStyle.SelectionBackColor = Color.FromArgb(237, 242, 255);
            dgvOrders.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);
            dgvOrders.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dgvOrders.EnableHeadersVisualStyles = false;
            dgvOrders.Cursor = Cursors.Hand;

            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { Name = "colOrderId", HeaderText = "Order ID", FillWeight = 20 });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDate", HeaderText = "Date", FillWeight = 30 });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTotal", HeaderText = "Total", FillWeight = 25 });

            // Status column with custom rendering
            var statusCol = new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "Status",
                FillWeight = 25
            };
            dgvOrders.Columns.Add(statusCol);

            dgvOrders.CellPainting += DgvOrders_CellPainting;
            dgvOrders.SelectionChanged += DgvOrders_SelectionChanged;
            ordersCardPanel.Controls.Add(dgvOrders);

            // ===== BOTTOM SECTION =====
            int bottomY = 320;
            int bottomHeight = this.ClientSize.Height - 50 - bottomY - 20;

            // Order Items card
            orderItemsCardPanel = new Panel
            {
                Location = new Point(20, bottomY),
                Size = new Size((int)((this.ClientSize.Width - 40) * 0.64), bottomHeight),
                BackColor = Color.White
            };
            orderItemsCardPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(220, 225, 235), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, orderItemsCardPanel.Width - 1, orderItemsCardPanel.Height - 1);
            };
            mainContentPanel.Controls.Add(orderItemsCardPanel);

            lblOrderItemsTitle = new Label
            {
                Text = "Order Items",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(20, 18),
                BackColor = Color.Transparent
            };
            orderItemsCardPanel.Controls.Add(lblOrderItemsTitle);

            lblOrderItemsSubtitle = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(130, 140, 160),
                AutoSize = true,
                Location = new Point(20, 42),
                BackColor = Color.Transparent
            };
            orderItemsCardPanel.Controls.Add(lblOrderItemsSubtitle);

            // Order Items DataGridView
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
                ScrollBars = ScrollBars.Vertical
            };
            dgvOrderItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 252);
            dgvOrderItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(100, 116, 139);
            dgvOrderItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgvOrderItems.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dgvOrderItems.ColumnHeadersHeight = 38;
            dgvOrderItems.RowTemplate.Height = 42;
            dgvOrderItems.DefaultCellStyle.SelectionBackColor = Color.FromArgb(237, 242, 255);
            dgvOrderItems.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);
            dgvOrderItems.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dgvOrderItems.EnableHeadersVisualStyles = false;

            dgvOrderItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colProduct", HeaderText = "Product", FillWeight = 40 });
            dgvOrderItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colQty", HeaderText = "Quantity", FillWeight = 20 });
            dgvOrderItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPrice", HeaderText = "Price", FillWeight = 20 });
            dgvOrderItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSubtotal", HeaderText = "Subtotal", FillWeight = 20 });

            orderItemsCardPanel.Controls.Add(dgvOrderItems);

            // Total row at bottom of items card
            var totalPanel = new Panel
            {
                Location = new Point(0, orderItemsCardPanel.Height - 40),
                Size = new Size(orderItemsCardPanel.Width, 40),
                BackColor = Color.White,
                Tag = "totalPanel"
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
                Name = "lblTotalLabel"
            };
            totalPanel.Controls.Add(lblTotalLabel);

            var lblTotalValue = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(totalPanel.Width - 110, 10),
                BackColor = Color.Transparent,
                Name = "lblTotalValue"
            };
            totalPanel.Controls.Add(lblTotalValue);

            // ===== ORDER STATUS CARD =====
            int statusX = 20 + orderItemsCardPanel.Width + 10;
            int statusWidth = this.ClientSize.Width - 40 - orderItemsCardPanel.Width - 10;
            orderStatusCardPanel = new Panel
            {
                Location = new Point(statusX, bottomY),
                Size = new Size(statusWidth, bottomHeight),
                BackColor = Color.White
            };
            orderStatusCardPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(220, 225, 235), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, orderStatusCardPanel.Width - 1, orderStatusCardPanel.Height - 1);
            };
            mainContentPanel.Controls.Add(orderStatusCardPanel);

            // Status card header
            var statusIconLabel = new Label
            {
                Text = "🔄",
                Font = new Font("Segoe UI", 13f),
                AutoSize = true,
                Location = new Point(15, 18),
                BackColor = Color.Transparent
            };
            orderStatusCardPanel.Controls.Add(statusIconLabel);

            lblStatusTitle = new Label
            {
                Text = "Order Status",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(40, 21),
                BackColor = Color.Transparent
            };
            orderStatusCardPanel.Controls.Add(lblStatusTitle);

            // Separator
            var statusSep = new Panel
            {
                Location = new Point(0, 55),
                Size = new Size(statusWidth, 1),
                BackColor = Color.FromArgb(230, 235, 245)
            };
            orderStatusCardPanel.Controls.Add(statusSep);

            // Status progress area
            statusProgressPanel = new Panel
            {
                Location = new Point(0, 56),
                Size = new Size(statusWidth, bottomHeight - 56),
                BackColor = Color.White,
                Name = "statusProgressPanel"
            };
            orderStatusCardPanel.Controls.Add(statusProgressPanel);

            RenderStatusPanel(null);
        }

        private void DgvOrders_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex >= 0)
            {
                e.PaintBackground(e.ClipBounds, true);
                string statusText = e.Value?.ToString() ?? "";
                Color badgeBg = GetStatusBadgeColor(statusText);
                Color badgeFg = GetStatusTextColor(statusText);

                int badgeW = 90, badgeH = 24;
                int bx = e.CellBounds.X + (e.CellBounds.Width - badgeW) / 2;
                int by = e.CellBounds.Y + (e.CellBounds.Height - badgeH) / 2;

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = RoundedRect(new Rectangle(bx, by, badgeW, badgeH), 5))
                using (var brush = new SolidBrush(badgeBg))
                    e.Graphics.FillPath(brush, path);

                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString(statusText, new Font("Segoe UI", 8.5f), new SolidBrush(badgeFg),
                    new RectangleF(bx, by, badgeW, badgeH), sf);

                e.Handled = true;
            }
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(bounds.Right - radius * 2, bounds.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(bounds.Right - radius * 2, bounds.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            return path;
        }

        private Color GetStatusBadgeColor(string status)
        {
            return status switch
            {
                "Pending" => Color.FromArgb(255, 247, 230),
                "Approved" => Color.FromArgb(220, 252, 231),
                "Processing" => Color.FromArgb(255, 237, 213),
                "Shipped" => Color.FromArgb(219, 234, 254),
                "Delivered" => Color.FromArgb(220, 252, 231),
                "Cancelled" => Color.FromArgb(254, 226, 226),
                "Rejected" => Color.FromArgb(254, 226, 226),
                _ => Color.FromArgb(243, 244, 246)
            };
        }

        private Color GetStatusTextColor(string status)
        {
            return status switch
            {
                "Pending" => Color.FromArgb(180, 120, 0),
                "Approved" => Color.FromArgb(21, 128, 61),
                "Processing" => Color.FromArgb(194, 65, 12),
                "Shipped" => Color.FromArgb(29, 78, 216),
                "Delivered" => Color.FromArgb(21, 128, 61),
                "Cancelled" => Color.FromArgb(185, 28, 28),
                "Rejected" => Color.FromArgb(185, 28, 28),
                _ => Color.FromArgb(75, 85, 99)
            };
        }

        private void DgvOrders_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0) return;

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

            // Update total label
            var totalPanel = orderItemsCardPanel.Controls["totalPanel"] as Panel;
            if (totalPanel != null)
            {
                var lblTotalValue = totalPanel.Controls["lblTotalValue"] as Label;
                if (lblTotalValue != null)
                    lblTotalValue.Text = $"${total:F2}";
            }
        }

        private void RenderStatusPanel(OrderDto order)
        {
            statusProgressPanel.Controls.Clear();

            var statuses = new[] { "Pending", "Processing", "Shipped", "Delivered" };
            string currentStatus = order?.Status.ToString() ?? "";
            int currentIndex = Array.IndexOf(statuses, currentStatus);

            // Current status label
            var lblCurrent = new Label
            {
                Text = string.IsNullOrEmpty(currentStatus) ? "No order selected" : currentStatus,
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(130, 140, 160),
                AutoSize = true,
                Location = new Point(statusProgressPanel.Width - 100, 15),
                BackColor = Color.Transparent
            };
            statusProgressPanel.Controls.Add(lblCurrent);

            // Progress bar background
            int barY = 50;
            int barX = 20;
            int barW = statusProgressPanel.Width - 40;
            int barH = 6;

            var progressBg = new Panel
            {
                Location = new Point(barX, barY),
                Size = new Size(barW, barH),
                BackColor = Color.FromArgb(226, 232, 240)
            };
            statusProgressPanel.Controls.Add(progressBg);

            // Progress fill
            if (currentIndex >= 0)
            {
                int fillW = currentIndex == 0 ? barW / 4 : (barW * Math.Min(currentIndex, statuses.Length - 1)) / (statuses.Length - 1);
                var progressFill = new Panel
                {
                    Location = new Point(barX, barY),
                    Size = new Size(fillW, barH),
                    BackColor = Color.FromArgb(59, 91, 219)
                };
                statusProgressPanel.Controls.Add(progressFill);
            }

            // Status steps
            for (int i = 0; i < statuses.Length; i++)
            {
                bool isCompleted = i <= currentIndex;
                bool isCurrent = i == currentIndex;

                int stepY = barY + 20;

                // Circle
                int circleSize = 20;
                int circleX = barX + (i * barW / (statuses.Length - 1)) - circleSize / 2;
                if (i == 0) circleX = barX;
                if (i == statuses.Length - 1) circleX = barX + barW - circleSize;

                var circlePanel = new Panel
                {
                    Location = new Point(circleX, stepY),
                    Size = new Size(circleSize, circleSize),
                    BackColor = Color.Transparent
                };

                bool captured_isCompleted = isCompleted;
                bool captured_isCurrent = isCurrent;
                string captured_status = statuses[i];

                circlePanel.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    if (captured_isCompleted)
                    {
                        e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(59, 91, 219)),
                            0, 0, circlePanel.Width - 1, circlePanel.Height - 1);
                        if (captured_isCurrent)
                        {
                            // checkmark
                            e.Graphics.DrawString("✓", new Font("Segoe UI", 9f, FontStyle.Bold),
                                Brushes.White, 2, 1);
                        }
                    }
                    else
                    {
                        e.Graphics.FillEllipse(Brushes.White, 0, 0, circlePanel.Width - 1, circlePanel.Height - 1);
                        using (Pen pen = new Pen(Color.FromArgb(200, 210, 225), 1.5f))
                            e.Graphics.DrawEllipse(pen, 0, 0, circlePanel.Width - 1, circlePanel.Height - 1);
                    }
                };
                statusProgressPanel.Controls.Add(circlePanel);

                // Label below circle
                var lblStep = new Label
                {
                    Text = statuses[i],
                    Font = new Font("Segoe UI", 9f, isCurrent ? FontStyle.Bold : FontStyle.Regular),
                    ForeColor = isCompleted ? Color.FromArgb(30, 40, 80) : Color.FromArgb(160, 170, 190),
                    AutoSize = true,
                    BackColor = Color.Transparent
                };
                lblStep.Location = new Point(circleX - (lblStep.PreferredWidth / 2) + circleSize / 2, stepY + circleSize + 6);
                statusProgressPanel.Controls.Add(lblStep);
            }
        }

        private async void LoadOrdersAsync()
        {
            try
            {
                _orders = await _orderCustomerService.getMyOrdersAsync();
                PopulateOrdersGrid();
            }
            catch
            {
                // Load sample data for design preview
                LoadSampleData();
            }
        }

        private void LoadSampleData()
        {
            _orders = new List<OrderDto>
            {
                new OrderDto { Id = 1006, OrderDate = new DateTime(2026,2,22), TotalAmount = 1949.97m, Status = OrderStatus.Pending },
                new OrderDto { Id = 1005, OrderDate = new DateTime(2026,2,20), TotalAmount = 399.99m, Status = OrderStatus.Processing },
                new OrderDto { Id = 1004, OrderDate = new DateTime(2026,2,17), TotalAmount = 699.99m, Status = OrderStatus.Delivered },
                new OrderDto { Id = 1003, OrderDate = new DateTime(2026,2,15), TotalAmount = 249.99m, Status = OrderStatus.Delivered },
                new OrderDto { Id = 1002, OrderDate = new DateTime(2026,2,10), TotalAmount = 899.99m, Status = OrderStatus.Delivered },
                new OrderDto { Id = 1001, OrderDate = new DateTime(2026,2,5),  TotalAmount = 1299.99m, Status = OrderStatus.Delivered }
            };

            _orders[0].Items = new System.Collections.Generic.List<OrderItemDto>
            {
                new OrderItemDto { ProductName = "Laptop Pro 15\"", Quantity = 1, UnitPrice = 1299.99m },
                new OrderItemDto { ProductName = "Wireless Headphones", Quantity = 1, UnitPrice = 249.99m },
                new OrderItemDto { ProductName = "Smartwatch Pro", Quantity = 1, UnitPrice = 399.99m }
            };

            PopulateOrdersGrid();
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

            // Auto-select first row
            if (dgvOrders.Rows.Count > 0)
            {
                dgvOrders.Rows[0].Selected = true;
                DgvOrders_SelectionChanged(null, null);
            }
        }
    }
}