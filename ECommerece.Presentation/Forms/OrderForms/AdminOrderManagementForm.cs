using ECommerece.Application.DTOs.OrderDtos;
using ECommerece.Application.IServices.IOrderService;
using ECommerece.Domain.Entities;
using ECommerece.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECommerece.WinForms.Orders
{
    /// <summary>
    /// Admin "Order Management" screen.
    /// ASSUMPTIONS:
    ///   - Admin is logged in (no user filter needed — all orders shown).
    ///   - Services resolved via constructor (DI or manual).
    ///   - OrderStatus  enum: Pending, Approved, Rejected, Processing, Shipped, Delivered, Cancelled
    ///   - PaymentStatus enum: Pending=1, Completed=2, Failed=3, Refunded=4, Cancelled=5
    /// </summary>
    public class AdminOrderManagementForm : Form
    {
        // ── Services ─────────────────────────────────────────────────────────────
        private readonly IOrderAdminService _adminService;
        private readonly IServiceProvider _serviceProvider;

        // ── Sidebar ───────────────────────────────────────────────────────────────
        private Panel pnlSidebar;
        private Button btnDashboard, btnCategories, btnProducts, btnOrders, btnLogout;

        // ── Content ───────────────────────────────────────────────────────────────
        private Panel pnlContent;
        private Label lblPageTitle, lblAdminUser;

        // ── Orders card ───────────────────────────────────────────────────────────
        private Panel pnlOrdersCard;
        private DataGridView dgvOrders;

        // ── Detail card ───────────────────────────────────────────────────────────
        private Panel pnlDetailCard;
        private Label lblDetailTitle, lblDetailCustomer;
        private DataGridView dgvItems;
        private Label lblTotal, lblTotalValue;

        // ── Update Status card ────────────────────────────────────────────────────
        private Panel pnlUpdateCard;
        private Label lblUpdateTitle, lblStatusLabel, lblPaymentStatusLabel;
        private ComboBox cboStatus, cboPaymentStatus;
        private Button btnUpdateStatus;
        private Label lblPaymentBadge;   // shows current payment status in detail card

        // ── State ─────────────────────────────────────────────────────────────────
        private List<OrderDto> _orders = new();
        private OrderDto? _selectedOrder;

        // ════════════════════════════════════════════════════════════════════════
        public AdminOrderManagementForm(IOrderAdminService adminService, IServiceProvider serviceProvider)
        {
            _adminService = adminService;
            _serviceProvider = serviceProvider;
            InitializeComponent();
        }

        // ════════════════════════════════════════════════════════════════════════
        //  Build UI
        // ════════════════════════════════════════════════════════════════════════
        private void InitializeComponent()
        {
            this.Text = "E-Commerce Management";
            this.Size = new Size(1366, 768);
            this.MinimumSize = new Size(1100, 650);
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9f);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += OrderManagementForm_Load;

            BuildSidebar();
            BuildContent();
        }

        // ── Sidebar ───────────────────────────────────────────────────────────────
        private void BuildSidebar()
        {
            pnlSidebar = new Panel { Dock = DockStyle.Left, Width = 200, BackColor = Color.FromArgb(15, 23, 42) };

            var pnlLogo = new Panel { Height = 64, Dock = DockStyle.Top, BackColor = Color.FromArgb(15, 23, 42) };
            var icon = new Panel { Size = new Size(28, 28), Location = new Point(16, 18), BackColor = Color.FromArgb(59, 130, 246) };
            var lblT = new Label { Text = "E-Commerce", ForeColor = Color.White, Font = new Font("Segoe UI", 11f, FontStyle.Bold), Location = new Point(50, 12), AutoSize = true };
            var lblS = new Label { Text = "Management", ForeColor = Color.FromArgb(148, 163, 184), Font = new Font("Segoe UI", 8f), Location = new Point(50, 32), AutoSize = true };
            pnlLogo.Controls.AddRange(new Control[] { icon, lblT, lblS });

            btnDashboard = MakeSidebarBtn("  Dashboard", false);
            btnCategories = MakeSidebarBtn("  Categories", false);
            btnProducts = MakeSidebarBtn("  Products", false);
            btnOrders = MakeSidebarBtn("  Orders", true);    // active

            var navFlow = new FlowLayoutPanel { Dock = DockStyle.Top, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoSize = true };
            navFlow.Controls.AddRange(new Control[] { btnDashboard, btnCategories, btnProducts, btnOrders });

            var navPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(15, 23, 42), Padding = new Padding(0, 8, 0, 0) };
            navPanel.Controls.Add(navFlow);

            btnLogout = new Button
            {
                Text = "  Logout",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(148, 163, 184),
                TextAlign = ContentAlignment.MiddleLeft,
                Width = 200,
                Height = 40,
                Dock = DockStyle.Bottom,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += (s, e) => this.Close();

            // ── Sidebar Navigation Events ──────────────────────────────────────────
            btnDashboard.Click += (s, e) =>
            {
                var dashboard = _serviceProvider.GetRequiredService<ECommerece.Presentation.Forms.DashboardForms.DashboardForm>();
                dashboard.Show();
                this.Hide();
            };

            

            pnlSidebar.Controls.Add(navPanel);
            pnlSidebar.Controls.Add(pnlLogo);
            pnlSidebar.Controls.Add(btnLogout);
            this.Controls.Add(pnlSidebar);
        }

        private Button MakeSidebarBtn(string text, bool active)
        {
            var btn = new Button
            {
                Text = text,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = active ? Color.FromArgb(30, 58, 138) : Color.Transparent,
                ForeColor = active ? Color.White : Color.FromArgb(148, 163, 184),
                TextAlign = ContentAlignment.MiddleLeft,
                Width = 200,
                Height = 40,
                Font = new Font("Segoe UI", 9.5f)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        // ── Content ───────────────────────────────────────────────────────────────
        private void BuildContent()
        {
            pnlContent = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(248, 250, 252), Padding = new Padding(24, 16, 24, 16) };

            lblPageTitle = new Label { Text = "Order Management", Font = new Font("Segoe UI", 16f, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42), AutoSize = true, Location = new Point(24, 20) };
            lblAdminUser = new Label { Text = "Admin User  👤", AutoSize = true, Font = new Font("Segoe UI", 10f), ForeColor = Color.FromArgb(71, 85, 105) };

            // Orders card
            pnlOrdersCard = new Panel { BackColor = Color.White, Location = new Point(24, 65) };
            pnlOrdersCard.Paint += Card_Paint;

            var lblCardTitle = new Label { Text = "Order Management", Font = new Font("Segoe UI", 12f, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42), Location = new Point(16, 14), AutoSize = true };
            var lblCardSub = new Label { Text = "View and manage customer orders", Font = new Font("Segoe UI", 8.5f), ForeColor = Color.FromArgb(100, 116, 139), Location = new Point(16, 36), AutoSize = true };

            dgvOrders = BuildGrid(new[] { ("Order ID", 80), ("Customer", 0), ("Date", 110), ("Total", 90), ("Order Status", 110), ("Payment", 110) });
            dgvOrders.Location = new Point(0, 58);
            dgvOrders.SelectionChanged += DgvOrders_SelectionChanged;

            pnlOrdersCard.Controls.AddRange(new Control[] { lblCardTitle, lblCardSub, dgvOrders });

            // Detail card
            pnlDetailCard = BuildDetailCard();
            // Update-status card
            pnlUpdateCard = BuildUpdateCard();

            pnlContent.Controls.Add(lblPageTitle);
            pnlContent.Controls.Add(lblAdminUser);
            pnlContent.Controls.Add(pnlOrdersCard);
            pnlContent.Controls.Add(pnlDetailCard);
            pnlContent.Controls.Add(pnlUpdateCard);

            this.Controls.Add(pnlContent);
            this.Resize += (s, e) => AdjustLayout();
        }

        private Panel BuildDetailCard()
        {
            var card = new Panel { BackColor = Color.White };
            card.Paint += Card_Paint;

            lblDetailTitle = new Label { Text = "Order Items", Font = new Font("Segoe UI", 11f, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42), Location = new Point(16, 12), AutoSize = true };
            lblDetailCustomer = new Label { Text = "", Font = new Font("Segoe UI", 8.5f), ForeColor = Color.FromArgb(100, 116, 139), Location = new Point(16, 32), AutoSize = true };

            // Payment status badge — sits top-right of the detail card
            lblPaymentBadge = new Label
            {
                Text = "",
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Padding = new Padding(6, 3, 6, 3),
                Location = new Point(200, 14)
            };

            dgvItems = BuildGrid(new[] { ("Product", 0), ("Quantity", 70), ("Price", 90), ("Subtotal", 90) });
            dgvItems.Location = new Point(0, 55);

            lblTotal = new Label { Text = "Total:", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42), AutoSize = true };
            lblTotalValue = new Label { Text = "", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42), AutoSize = true };
            var pnlTot = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true, Dock = DockStyle.Bottom, Padding = new Padding(8, 4, 8, 8) };
            pnlTot.Controls.Add(lblTotal);
            pnlTot.Controls.Add(lblTotalValue);

            card.Controls.AddRange(new Control[] { lblDetailTitle, lblDetailCustomer, lblPaymentBadge, dgvItems, pnlTot });
            return card;
        }

        private Panel BuildUpdateCard()
        {
            var card = new Panel { BackColor = Color.White };
            card.Paint += Card_Paint;

            lblUpdateTitle = new Label
            {
                Text = "⚙  Update Status",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(16, 12),
                AutoSize = true
            };

            // ── Order Status ────────────────────────────────────────────────────
            lblStatusLabel = new Label
            {
                Text = "Order Status",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(16, 50),
                AutoSize = true
            };

            cboStatus = new ComboBox
            {
                Location = new Point(16, 68),
                Height = 32,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            cboStatus.Items.AddRange(new object[]
            {
                OrderStatus.Pending,
                OrderStatus.Approved,
                OrderStatus.Rejected,
                OrderStatus.Processing,
                OrderStatus.Shipped,
                OrderStatus.Delivered,
                OrderStatus.Cancelled
            });
            cboStatus.SelectedIndex = 0;

            // ── Payment Status ──────────────────────────────────────────────────
            lblPaymentStatusLabel = new Label
            {
                Text = "Payment Status",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(16, 108),
                AutoSize = true
            };

            cboPaymentStatus = new ComboBox
            {
                Location = new Point(16, 126),
                Height = 32,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            cboPaymentStatus.Items.AddRange(new object[]
            {
                PaymentStatus.Pending,
                PaymentStatus.Completed,
                PaymentStatus.Failed,
                PaymentStatus.Refunded,
                PaymentStatus.Cancelled
            });
            cboPaymentStatus.SelectedIndex = 0;

            // ── Update button ───────────────────────────────────────────────────
            btnUpdateStatus = new Button
            {
                Text = "Update Status",
                Location = new Point(16, 174),
                Height = 36,
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };
            btnUpdateStatus.FlatAppearance.BorderSize = 0;
            btnUpdateStatus.Click += BtnUpdateStatus_Click;

            card.Controls.AddRange(new Control[]
            {
                lblUpdateTitle,
                lblStatusLabel,      cboStatus,
                lblPaymentStatusLabel, cboPaymentStatus,
                btnUpdateStatus
            });
            return card;
        }

        // ── Generic grid builder ──────────────────────────────────────────────────
        private static DataGridView BuildGrid((string header, int width)[] cols)
        {
            var dgv = new DataGridView
            {
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(226, 232, 240),
                RowHeadersVisible = false,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(100, 116, 139),
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                    SelectionBackColor = Color.White,
                    SelectionForeColor = Color.FromArgb(100, 116, 139),
                    Padding = new Padding(8, 0, 0, 0)
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(30, 41, 59),
                    SelectionBackColor = Color.FromArgb(239, 246, 255),
                    SelectionForeColor = Color.FromArgb(30, 41, 59),
                    Padding = new Padding(8, 4, 0, 4)
                },
                Height = 220
            };

            foreach (var (header, width) in cols)
            {
                var col = new DataGridViewTextBoxColumn { HeaderText = header };
                if (width > 0) { col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; col.Width = width; }
                dgv.Columns.Add(col);
            }
            return dgv;
        }

        // ════════════════════════════════════════════════════════════════════════
        //  Data loading
        // ════════════════════════════════════════════════════════════════════════
        private async void OrderManagementForm_Load(object sender, EventArgs e)
        {
            AdjustLayout();
            await LoadOrdersAsync();
        }

        private async Task LoadOrdersAsync()
        {
            try
            {
                _orders = await _adminService.GetAllOrdersAsync();
                PopulateGrid();
                if (_orders.Any()) SelectOrder(_orders.First());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load orders:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateGrid()
        {
            dgvOrders.Rows.Clear();
            foreach (var o in _orders)
            {
                string customerName = o.user?.Name ?? o.user?.Email ?? "—";
                int ri = dgvOrders.Rows.Add(
                    $"#{o.Id}",
                    customerName,
                    o.OrderDate.ToString("yyyy-MM-dd"),
                    $"${o.TotalAmount:F2}",
                    o.Status.ToString(),
                    o.PaymentStatus.ToString()
                );
                ColorStatusCell(dgvOrders.Rows[ri].Cells[4], o.Status);
                ColorPaymentCell(dgvOrders.Rows[ri].Cells[5], o.PaymentStatus);
            }
        }

        private void SelectOrder(OrderDto order)
        {
            _selectedOrder = order;

            lblDetailTitle.Text = $"Order Items - #{order.Id}";
            string customerName = order.user?.Name ?? order.user?.Email ?? "—";
            lblDetailCustomer.Text = $"Customer: {customerName}";

            // Payment status badge in detail card header
            ApplyPaymentBadge(lblPaymentBadge, order.PaymentStatus);

            dgvItems.Rows.Clear();
            if (order.Items != null)
            {
                foreach (var item in order.Items)
                {
                    dgvItems.Rows.Add(
                        item.ProductName,
                        item.Quantity,
                        $"${item.UnitPrice:F2}",
                        $"${item.Quantity * item.UnitPrice:F2}"
                    );
                }
            }
            lblTotalValue.Text = $"${order.TotalAmount:F2}";

            // Pre-select current statuses in dropdowns
            int oIdx = cboStatus.Items.IndexOf(order.Status);
            cboStatus.SelectedIndex = oIdx >= 0 ? oIdx : 0;

            int pIdx = cboPaymentStatus.Items.IndexOf(order.PaymentStatus);
            cboPaymentStatus.SelectedIndex = pIdx >= 0 ? pIdx : 0;
        }

        // ════════════════════════════════════════════════════════════════════════
        //  Event handlers
        // ════════════════════════════════════════════════════════════════════════
        private void DgvOrders_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0) return;
            var idStr = dgvOrders.SelectedRows[0].Cells[0].Value?.ToString()?.TrimStart('#');
            if (int.TryParse(idStr, out int id))
            {
                var order = _orders.FirstOrDefault(o => o.Id == id);
                if (order != null) SelectOrder(order);
            }
        }

        private async void BtnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (_selectedOrder == null)
            {
                MessageBox.Show("Please select an order first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cboStatus.SelectedItem is not OrderStatus newOrderStatus)
            {
                MessageBox.Show("Please choose a valid order status.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cboPaymentStatus.SelectedItem is not PaymentStatus newPaymentStatus)
            {
                MessageBox.Show("Please choose a valid payment status.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool orderChanged = newOrderStatus != _selectedOrder.Status;
            bool paymentChanged = newPaymentStatus != _selectedOrder.PaymentStatus;

            if (!orderChanged && !paymentChanged)
            {
                MessageBox.Show("No changes detected.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Update order #{_selectedOrder.Id}?\n\n" +
                (orderChanged ? $"  Order Status:   {_selectedOrder.Status} → {newOrderStatus}\n" : "") +
                (paymentChanged ? $"  Payment Status: {_selectedOrder.PaymentStatus} → {newPaymentStatus}" : ""),
                "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                btnUpdateStatus.Enabled = false;
                btnUpdateStatus.Text = "Updating…";

                // ── Order Status ────────────────────────────────────────────────
                if (orderChanged)
                {
                    switch (newOrderStatus)
                    {
                        case OrderStatus.Approved:
                            await _adminService.ApproveOrder(_selectedOrder);
                            break;

                        case OrderStatus.Rejected:
                            await _adminService.RejectOrder(_selectedOrder);
                            break;

                        // For Processing, Shipped, Delivered, Cancelled add this to IOrderAdminService:
                        //   Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
                        // Then uncomment:
                        // case OrderStatus.Processing:
                        // case OrderStatus.Shipped:
                        // case OrderStatus.Delivered:
                        // case OrderStatus.Cancelled:
                        //     await _adminService.UpdateOrderStatusAsync(_selectedOrder.Id, newOrderStatus);
                        //     break;

                        default:
                            MessageBox.Show(
                                $"'{newOrderStatus}' requires UpdateOrderStatusAsync(int, OrderStatus) on IOrderAdminService.",
                                "Not Yet Wired", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                    }
                    _selectedOrder.Status = newOrderStatus;
                }

                // ── Payment Status ──────────────────────────────────────────────
                if (paymentChanged)
                {
                    // Add this to IOrderAdminService:
                    //   Task UpdatePaymentStatusAsync(int orderId, PaymentStatus status);
                    // Then replace the MessageBox below with:
                    //   await _adminService.UpdatePaymentStatusAsync(_selectedOrder.Id, newPaymentStatus);

                    MessageBox.Show(
                        $"Payment status update to '{newPaymentStatus}' requires\nUpdatePaymentStatusAsync(int, PaymentStatus) on IOrderAdminService.",
                        "Not Yet Wired", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Optimistically update local state so the UI reflects it while you wire the service
                    _selectedOrder.PaymentStatus = newPaymentStatus;
                }

                await LoadOrdersAsync();

                MessageBox.Show($"Order #{_selectedOrder.Id} updated successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update failed:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnUpdateStatus.Enabled = true;
                btnUpdateStatus.Text = "Update Status";
            }
        }

        private void AdjustLayout()
        {
            int w = pnlContent.ClientSize.Width - 48;
            int h = pnlContent.ClientSize.Height;

            pnlOrdersCard.Size = new Size(w, 290);
            dgvOrders.Width = w;

            int bottomY = pnlOrdersCard.Bottom + 12;
            int bottomH = h - bottomY - 10;
            int detailW = (int)(w * 0.63);
            int updateW = w - detailW - 12;

            pnlDetailCard.Location = new Point(24, bottomY);
            pnlDetailCard.Size = new Size(detailW, bottomH);
            dgvItems.Size = new Size(detailW, bottomH - 90);

            // Reposition payment badge to right side of detail card header
            lblPaymentBadge.Location = new Point(detailW - lblPaymentBadge.Width - 16, 14);

            pnlUpdateCard.Location = new Point(24 + detailW + 12, bottomY);
            pnlUpdateCard.Size = new Size(updateW, bottomH);
            cboStatus.Width = updateW - 32;
            cboPaymentStatus.Width = updateW - 32;
            btnUpdateStatus.Width = updateW - 32;

            lblAdminUser.Location = new Point(pnlContent.ClientSize.Width - lblAdminUser.Width - 20, 24);
        }

        private static void ColorStatusCell(DataGridViewCell cell, OrderStatus status)
        {
            (cell.Style.ForeColor, cell.Style.BackColor) = status switch
            {
                OrderStatus.Pending => (Color.FromArgb(161, 98, 7), Color.FromArgb(254, 249, 195)),  // amber
                OrderStatus.Approved => (Color.FromArgb(21, 128, 61), Color.FromArgb(220, 252, 231)),  // green
                OrderStatus.Rejected => (Color.FromArgb(185, 28, 28), Color.FromArgb(254, 226, 226)),  // red
                OrderStatus.Processing => (Color.FromArgb(234, 88, 12), Color.FromArgb(255, 237, 213)),  // orange
                OrderStatus.Shipped => (Color.FromArgb(37, 99, 235), Color.FromArgb(219, 234, 254)),  // blue
                OrderStatus.Delivered => (Color.FromArgb(21, 128, 61), Color.FromArgb(220, 252, 231)),  // green
                OrderStatus.Cancelled => (Color.FromArgb(100, 116, 139), Color.FromArgb(241, 245, 249)),  // slate
                _ => (Color.Black, Color.White)
            };
        }

        private static void ColorPaymentCell(DataGridViewCell cell, PaymentStatus status)
        {
            (cell.Style.ForeColor, cell.Style.BackColor) = status switch
            {
                PaymentStatus.Pending => (Color.FromArgb(161, 98, 7), Color.FromArgb(254, 249, 195)),  // amber
                PaymentStatus.Completed => (Color.FromArgb(21, 128, 61), Color.FromArgb(220, 252, 231)),  // green
                PaymentStatus.Failed => (Color.FromArgb(185, 28, 28), Color.FromArgb(254, 226, 226)),  // red
                PaymentStatus.Refunded => (Color.FromArgb(37, 99, 235), Color.FromArgb(219, 234, 254)),  // blue
                PaymentStatus.Cancelled => (Color.FromArgb(100, 116, 139), Color.FromArgb(241, 245, 249)),  // slate
                _ => (Color.Black, Color.White)
            };
        }

        /// <summary>Styles a Label to look like a payment-status badge.</summary>
        private static void ApplyPaymentBadge(Label lbl, PaymentStatus status)
        {
            lbl.Text = $"💳 {status}";
            (lbl.ForeColor, lbl.BackColor) = status switch
            {
                PaymentStatus.Pending => (Color.FromArgb(161, 98, 7), Color.FromArgb(254, 249, 195)),
                PaymentStatus.Completed => (Color.FromArgb(21, 128, 61), Color.FromArgb(220, 252, 231)),
                PaymentStatus.Failed => (Color.FromArgb(185, 28, 28), Color.FromArgb(254, 226, 226)),
                PaymentStatus.Refunded => (Color.FromArgb(37, 99, 235), Color.FromArgb(219, 234, 254)),
                PaymentStatus.Cancelled => (Color.FromArgb(100, 116, 139), Color.FromArgb(241, 245, 249)),
                _ => (Color.Black, Color.White)
            };
        }

        private void Card_Paint(object sender, PaintEventArgs e)
        {
            var p = (Panel)sender;
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(226, 232, 240), 1), 0, 0, p.Width - 1, p.Height - 1);
        }
    }
}
