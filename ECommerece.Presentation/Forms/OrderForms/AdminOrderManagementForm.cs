using ECommerece.Application.DTOs.OrderDtos;
using ECommerece.Application.IServices.IOrderService;
using ECommerece.Domain.Entities;
using ECommerece.Domain.Enums;
using ECommerece.Presentation.Forms.CategoryForms;
using ECommerece.Presentation.Forms.DashboardForms;
using ECommerece.Presentation.Forms.ProductForms;
using ECommerece.Presentation.Forms.UserForms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ECommerece.Presentation.Forms.OrderForms
{
    public class AdminOrderManagementForm : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IOrderAdminService _orderAdminService;

        // Sidebar
        private Panel sidebarPanel;
        private Panel mainPanel;

        // Orders section
        private DataGridView dgvOrders;

        // Order Items section
        private Panel orderItemsCard;
        private DataGridView dgvOrderItems;
        private Label lblOrderItemsTitle;
        private Label lblOrderItemsCustomer;
        private Label lblTotalValue;

        // Update Status section
        private Panel updateStatusCard;
        private ComboBox cmbStatus;

        private List<OrderDto> _orders = new List<OrderDto>();
        private OrderDto _selectedOrder;

        public AdminOrderManagementForm(IServiceProvider serviceProvider, IOrderAdminService orderAdminService)
        {
            _serviceProvider = serviceProvider;
            _orderAdminService = orderAdminService;
            InitializeComponents();
            LoadOrdersAsync();
        }

        private void InitializeComponents()
        {
            this.Text = "E-Commerce Management System";
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
                BackColor = Color.FromArgb(15, 23, 42)
            };
            this.Controls.Add(sidebarPanel);

            var lblLogo = new Label
            {
                Text = "🛒  E-Commerce",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 30),
                BackColor = Color.Transparent
            };
            sidebarPanel.Controls.Add(lblLogo);

            var separator = new Panel
            {
                Size = new Size(180, 1),
                Location = new Point(20, 70),
                BackColor = Color.FromArgb(30, 41, 59)
            };
            sidebarPanel.Controls.Add(separator);

            var btnDashboard = CreateSidebarButton("📊  Dashboard", 90, false);
            var btnCategories = CreateSidebarButton("🗂️  Categories", 145, false);
            var btnProducts = CreateSidebarButton("📦  Products", 200, false);
            var btnOrders = CreateSidebarButton("🧾  Orders", 255, true);
            var btnCustomers = CreateSidebarButton("👥  Customers", 310, false);

            sidebarPanel.Controls.Add(btnDashboard);
            sidebarPanel.Controls.Add(btnCategories);
            sidebarPanel.Controls.Add(btnProducts);
            sidebarPanel.Controls.Add(btnOrders);
            sidebarPanel.Controls.Add(btnCustomers);

            btnDashboard.Click += (s, e) =>
            {
                _serviceProvider.GetRequiredService<DashboardForm>().Show();
                this.Hide();
            };
            btnCategories.Click += (s, e) =>
            {
                _serviceProvider.GetRequiredService<CategoryForm>().Show();
                this.Hide();
            };
            btnProducts.Click += (s, e) =>
            {
                _serviceProvider.GetRequiredService<AdminProductsForm>().Show();
                this.Hide();
            };

            var btnLogout = new Button
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
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 41, 59);
            btnLogout.Click += (s, e) =>
            {
                if (MessageBox.Show("Are you sure you want to logout?", "Logout",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _serviceProvider.GetRequiredService<LoginForm>().Show();
                    this.Close();
                }
            };
            sidebarPanel.Controls.Add(btnLogout);

            // ===== MAIN PANEL =====
            mainPanel = new Panel
            {
                Size = new Size(this.ClientSize.Width - 220, this.ClientSize.Height),
                Location = new Point(220, 0),
                BackColor = Color.FromArgb(241, 245, 249)
            };
            this.Controls.Add(mainPanel);

            // ===== TOP BAR =====
            var topBar = new Panel
            {
                Size = new Size(mainPanel.Width, 50),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(18, 18, 24)
            };
            mainPanel.Controls.Add(topBar);

            var lblPageTitle = new Label
            {
                Text = "Order Management",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 13),
                BackColor = Color.Transparent
            };
            topBar.Controls.Add(lblPageTitle);

            var lblAdminName = new Label
            {
                Text = "Admin User",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent
            };
            lblAdminName.Location = new Point(mainPanel.Width - lblAdminName.PreferredWidth - 55, 15);
            topBar.Controls.Add(lblAdminName);

            // Avatar circle
            var avatarPanel = new Panel
            {
                Size = new Size(30, 30),
                Location = new Point(mainPanel.Width - 44, 10),
                BackColor = Color.Transparent
            };
            avatarPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(59, 91, 219)), 0, 0, 29, 29);
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString("A", new Font("Segoe UI", 11f, FontStyle.Bold), Brushes.White,
                    new RectangleF(0, 0, 29, 29), sf);
            };
            topBar.Controls.Add(avatarPanel);

            // ===== ORDERS CARD =====
            int contentY = 60;
            int cardW = mainPanel.Width - 30;

            var ordersCard = new Panel
            {
                Location = new Point(15, contentY),
                Size = new Size(cardW, 235),
                BackColor = Color.White
            };
            ordersCard.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(218, 226, 240), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, ordersCard.Width - 1, ordersCard.Height - 1);
            };
            mainPanel.Controls.Add(ordersCard);

            var lblCardTitle = new Label
            {
                Text = "Order Management",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(18, 16),
                BackColor = Color.Transparent
            };
            ordersCard.Controls.Add(lblCardTitle);

            var lblCardSub = new Label
            {
                Text = "View and manage customer orders",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(130, 145, 165),
                AutoSize = true,
                Location = new Point(18, 38),
                BackColor = Color.Transparent
            };
            ordersCard.Controls.Add(lblCardSub);

            // Orders DataGridView
            dgvOrders = new DataGridView
            {
                Location = new Point(0, 62),
                Size = new Size(ordersCard.Width, ordersCard.Height - 62),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single,
                GridColor = Color.FromArgb(232, 238, 248),
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9.5f),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                MultiSelect = false,
                ScrollBars = ScrollBars.Vertical,
                Cursor = Cursors.Hand
            };
            dgvOrders.EnableHeadersVisualStyles = false;
            dgvOrders.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 252);
            dgvOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(100, 116, 139);
            dgvOrders.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgvOrders.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgvOrders.ColumnHeadersHeight = 38;
            dgvOrders.RowTemplate.Height = 40;
            dgvOrders.DefaultCellStyle.SelectionBackColor = Color.FromArgb(237, 242, 255);
            dgvOrders.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);
            dgvOrders.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "Order ID", FillWeight = 12 });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCustomer", HeaderText = "Customer", FillWeight = 28 });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDate", HeaderText = "Date", FillWeight = 25 });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTotal", HeaderText = "Total", FillWeight = 20 });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { Name = "colStatus", HeaderText = "Status", FillWeight = 15 });

            dgvOrders.CellPainting += DgvOrders_CellPainting;
            dgvOrders.SelectionChanged += DgvOrders_SelectionChanged;
            ordersCard.Controls.Add(dgvOrders);

            // ===== BOTTOM SECTION =====
            int bottomY = contentY + ordersCard.Height + 10;
            int bottomH = this.ClientSize.Height - bottomY - 10;
            int itemsW = (int)(cardW * 0.655);
            int statusW = cardW - itemsW - 8;

            // ── Order Items Card ──────────────────────────────────────────────────
            orderItemsCard = new Panel
            {
                Location = new Point(15, bottomY),
                Size = new Size(itemsW, bottomH),
                BackColor = Color.White
            };
            orderItemsCard.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(218, 226, 240), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, orderItemsCard.Width - 1, orderItemsCard.Height - 1);
            };
            mainPanel.Controls.Add(orderItemsCard);

            lblOrderItemsTitle = new Label
            {
                Text = "Order Items",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(18, 16),
                BackColor = Color.Transparent
            };
            orderItemsCard.Controls.Add(lblOrderItemsTitle);

            lblOrderItemsCustomer = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(130, 145, 165),
                AutoSize = true,
                Location = new Point(18, 38),
                BackColor = Color.Transparent
            };
            orderItemsCard.Controls.Add(lblOrderItemsCustomer);

            dgvOrderItems = new DataGridView
            {
                Location = new Point(0, 62),
                Size = new Size(orderItemsCard.Width, orderItemsCard.Height - 104),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                GridColor = Color.FromArgb(232, 238, 248),
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
            dgvOrderItems.EnableHeadersVisualStyles = false;
            dgvOrderItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 252);
            dgvOrderItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(100, 116, 139);
            dgvOrderItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgvOrderItems.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgvOrderItems.ColumnHeadersHeight = 38;
            dgvOrderItems.RowTemplate.Height = 40;
            dgvOrderItems.DefaultCellStyle.SelectionBackColor = Color.FromArgb(237, 242, 255);
            dgvOrderItems.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);
            dgvOrderItems.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

            dgvOrderItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colProduct", HeaderText = "Product", FillWeight = 40 });
            dgvOrderItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colQty", HeaderText = "Quantity", FillWeight = 20 });
            dgvOrderItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPrice", HeaderText = "Price", FillWeight = 20 });
            dgvOrderItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSubtotal", HeaderText = "Subtotal", FillWeight = 20 });

            orderItemsCard.Controls.Add(dgvOrderItems);

            // Total row at bottom of items card
            var totalRow = new Panel
            {
                Location = new Point(0, orderItemsCard.Height - 42),
                Size = new Size(orderItemsCard.Width, 42),
                BackColor = Color.White
            };
            totalRow.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(225, 232, 242), 1))
                    e.Graphics.DrawLine(pen, 0, 0, totalRow.Width, 0);
            };
            orderItemsCard.Controls.Add(totalRow);

            var lblTotalLabel = new Label
            {
                Text = "Total:",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(totalRow.Width - 160, 12),
                BackColor = Color.Transparent
            };
            totalRow.Controls.Add(lblTotalLabel);

            lblTotalValue = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(totalRow.Width - 95, 12),
                BackColor = Color.Transparent
            };
            totalRow.Controls.Add(lblTotalValue);

            // ── Update Status Card ────────────────────────────────────────────────
            updateStatusCard = new Panel
            {
                Location = new Point(15 + itemsW + 8, bottomY),
                Size = new Size(statusW, bottomH),
                BackColor = Color.White
            };
            updateStatusCard.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(218, 226, 240), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, updateStatusCard.Width - 1, updateStatusCard.Height - 1);
            };
            mainPanel.Controls.Add(updateStatusCard);

            // Header: icon + title
            var iconLbl = new Label
            {
                Text = "🔄",
                Font = new Font("Segoe UI", 12f),
                AutoSize = true,
                Location = new Point(15, 17),
                BackColor = Color.Transparent
            };
            updateStatusCard.Controls.Add(iconLbl);

            var lblUpdateTitle = new Label
            {
                Text = "Update Status",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(38, 20),
                BackColor = Color.Transparent
            };
            updateStatusCard.Controls.Add(lblUpdateTitle);

            // Separator
            var hdrSep = new Panel
            {
                Location = new Point(0, 54),
                Size = new Size(statusW, 1),
                BackColor = Color.FromArgb(228, 235, 245)
            };
            updateStatusCard.Controls.Add(hdrSep);

            // "Order Status" label
            var lblStatusLabel = new Label
            {
                Text = "Order Status",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(80, 95, 115),
                AutoSize = true,
                Location = new Point(15, 72),
                BackColor = Color.Transparent
            };
            updateStatusCard.Controls.Add(lblStatusLabel);

            // ComboBox
            cmbStatus = new ComboBox
            {
                Location = new Point(15, 92),
                Size = new Size(statusW - 30, 32),
                Font = new Font("Segoe UI", 9.5f),
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(30, 42, 60)
            };
            foreach (var st in Enum.GetNames(typeof(OrderStatus)))
                cmbStatus.Items.Add(st);
            cmbStatus.SelectedIndex = 0;
            updateStatusCard.Controls.Add(cmbStatus);

            // Update Status button
            var btnUpdate = new Button
            {
                Text = "Update Status",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(59, 91, 219),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(15, 140),
                Size = new Size(statusW - 30, 36),
                Cursor = Cursors.Hand
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.FlatAppearance.MouseOverBackColor = Color.FromArgb(48, 78, 200);
            btnUpdate.Click += BtnUpdateStatus_Click;
            updateStatusCard.Controls.Add(btnUpdate);
        }

        // ═══ Helpers ══════════════════════════════════════════════════════════════

        private Button CreateSidebarButton(string text, int y, bool isActive)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 10f),
                ForeColor = isActive ? Color.White : Color.FromArgb(148, 163, 184),
                BackColor = isActive ? Color.FromArgb(59, 91, 219) : Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(220, 45),
                Location = new Point(0, y),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = isActive
                ? Color.FromArgb(59, 91, 219)
                : Color.FromArgb(30, 41, 59);
            return btn;
        }

        // ═══ Status badge painting ════════════════════════════════════════════════

        private void DgvOrders_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex != 4 || e.RowIndex < 0) return;

            e.PaintBackground(e.ClipBounds, true);
            string status = e.Value?.ToString() ?? "";

            int bw = 88, bh = 24;
            int bx = e.CellBounds.X + (e.CellBounds.Width - bw) / 2;
            int by = e.CellBounds.Y + (e.CellBounds.Height - bh) / 2;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = RoundedRect(new Rectangle(bx, by, bw, bh), 5))
            using (var brush = new SolidBrush(GetBadgeBg(status)))
                e.Graphics.FillPath(brush, path);

            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            e.Graphics.DrawString(status, new Font("Segoe UI", 8.5f),
                new SolidBrush(GetBadgeFg(status)),
                new RectangleF(bx, by, bw, bh), sf);

            e.Handled = true;
        }

        private static GraphicsPath RoundedRect(Rectangle b, int r)
        {
            var p = new GraphicsPath();
            p.AddArc(b.X, b.Y, r * 2, r * 2, 180, 90);
            p.AddArc(b.Right - r * 2, b.Y, r * 2, r * 2, 270, 90);
            p.AddArc(b.Right - r * 2, b.Bottom - r * 2, r * 2, r * 2, 0, 90);
            p.AddArc(b.X, b.Bottom - r * 2, r * 2, r * 2, 90, 90);
            p.CloseFigure();
            return p;
        }

        private Color GetBadgeBg(string s) => s switch
        {
            "Pending" => Color.FromArgb(243, 244, 246),
            "Approved" => Color.FromArgb(220, 252, 231),
            "Processing" => Color.FromArgb(255, 237, 213),
            "Shipped" => Color.FromArgb(219, 234, 254),
            "Delivered" => Color.FromArgb(220, 252, 231),
            "Cancelled" => Color.FromArgb(254, 226, 226),
            "Rejected" => Color.FromArgb(254, 226, 226),
            _ => Color.FromArgb(243, 244, 246)
        };

        private Color GetBadgeFg(string s) => s switch
        {
            "Pending" => Color.FromArgb(75, 85, 99),
            "Approved" => Color.FromArgb(21, 128, 61),
            "Processing" => Color.FromArgb(194, 65, 12),
            "Shipped" => Color.FromArgb(29, 78, 216),
            "Delivered" => Color.FromArgb(21, 128, 61),
            "Cancelled" => Color.FromArgb(185, 28, 28),
            "Rejected" => Color.FromArgb(185, 28, 28),
            _ => Color.FromArgb(75, 85, 99)
        };

        // ═══ Grid selection ═══════════════════════════════════════════════════════

        private void DgvOrders_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0) return;

            var row = dgvOrders.SelectedRows[0];
            string idStr = row.Cells["colId"].Value?.ToString()?.Replace("#", "") ?? "0";

            if (int.TryParse(idStr, out int orderId))
            {
                _selectedOrder = _orders.Find(o => o.Id == orderId);
                if (_selectedOrder != null)
                {
                    LoadOrderItems(_selectedOrder);
                    cmbStatus.SelectedItem = _selectedOrder.Status.ToString();
                }
            }
        }

        private void LoadOrderItems(OrderDto order)
        {
            lblOrderItemsTitle.Text = $"Order Items - #{order.Id}";
            string name = order.user?.Name ?? order.user?.Email ?? "Customer";
            lblOrderItemsCustomer.Text = $"Customer: {name}";

            dgvOrderItems.Rows.Clear();
            decimal total = 0;
            if (order.Items != null)
            {
                foreach (var item in order.Items)
                {
                    decimal sub = item.Quantity * item.UnitPrice;
                    total += sub;
                    dgvOrderItems.Rows.Add(item.ProductName, item.Quantity,
                        $"${item.UnitPrice:F2}", $"${sub:F2}");
                }
            }
            lblTotalValue.Text = $"${total:F2}";
        }

        // ═══ Update Status ════════════════════════════════════════════════════════

        private async void BtnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (_selectedOrder == null)
            {
                MessageBox.Show("Please select an order first.", "No Order Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbStatus.SelectedItem == null) return;

            if (!Enum.TryParse<OrderStatus>(cmbStatus.SelectedItem.ToString(), out var newStatus))
                return;

            try
            {
                _selectedOrder.Status = newStatus;

                if (newStatus == OrderStatus.Approved)
                    await _orderAdminService.ApproveOrder(_selectedOrder);
                else if (newStatus == OrderStatus.Rejected)
                    await _orderAdminService.RejectOrder(_selectedOrder);
                // Other statuses: extend IOrderAdminService as needed

                // Update badge in grid
                if (dgvOrders.SelectedRows.Count > 0)
                    dgvOrders.SelectedRows[0].Cells["colStatus"].Value = newStatus.ToString();

                dgvOrders.Refresh();

                MessageBox.Show($"Order #{_selectedOrder.Id} updated to \"{newStatus}\" successfully.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ═══ Load / Sample data ═══════════════════════════════════════════════════

        private async void LoadOrdersAsync()
        {
            try
            {
                _orders = await _orderAdminService.GetAllOrdersAsync();
                PopulateGrid();
            }
            catch
            {
                LoadSampleData();
            }
        }

        private void LoadSampleData()
        {
            _orders = new List<OrderDto>
            {
                new OrderDto
                {
                    Id = 1001, OrderDate = new DateTime(2026, 2, 20), TotalAmount = 1549.98m,
                    Status = OrderStatus.Pending,
                    user = new User { Name = "John Doe" },
                    Items = new List<OrderItemDto>
                    {
                        new OrderItemDto { ProductName = "Laptop Pro 15\"",     Quantity = 1, UnitPrice = 1299.99m },
                        new OrderItemDto { ProductName = "Wireless Headphones", Quantity = 1, UnitPrice = 249.99m  }
                    }
                },
                new OrderDto { Id = 1002, OrderDate = new DateTime(2026, 2, 19), TotalAmount = 899.99m,
                    Status = OrderStatus.Processing, user = new User { Name = "Jane Smith" } },
                new OrderDto { Id = 1003, OrderDate = new DateTime(2026, 2, 18), TotalAmount = 249.99m,
                    Status = OrderStatus.Shipped,    user = new User { Name = "Bob Johnson" } },
                new OrderDto { Id = 1004, OrderDate = new DateTime(2026, 2, 17), TotalAmount = 699.99m,
                    Status = OrderStatus.Delivered,  user = new User { Name = "Alice Williams" } },
                new OrderDto { Id = 1005, OrderDate = new DateTime(2026, 2, 16), TotalAmount = 399.99m,
                    Status = OrderStatus.Processing, user = new User {Name = "Charlie Brown" } }
            };

            PopulateGrid();
        }

        private void PopulateGrid()
        {
            if (dgvOrders.InvokeRequired) { dgvOrders.Invoke(new Action(PopulateGrid)); return; }

            dgvOrders.Rows.Clear();
            foreach (var o in _orders)
            {
                string name = o.user?.Name ?? o.user?.Email ?? "—";
                dgvOrders.Rows.Add(
                    $"#{o.Id}",
                    name,
                    o.OrderDate.ToString("yyyy-MM-dd"),
                    $"${o.TotalAmount:F2}",
                    o.Status.ToString()
                );
            }

            if (dgvOrders.Rows.Count > 0)
            {
                dgvOrders.Rows[0].Selected = true;
                DgvOrders_SelectionChanged(null, null);
            }
        }
    }
}