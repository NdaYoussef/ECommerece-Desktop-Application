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
    
    public class CustomerOrderManagementForm : Form
    {
        // ── Services ────────────────────────────────────────────────────────────
        private readonly IOrdercustomerService _orderService;
        private readonly IServiceProvider _serviceProvider;

        // ── Layout panels ───────────────────────────────────────────────────────
        private Panel pnlSidebar;
        private Panel pnlContent;

        // ── Sidebar buttons ─────────────────────────────────────────────────────
        private Button btnDashboard;
        private Button btnBrowseProducts;
        private Button btnCart;
        private Button btnMyOrders;
        private Button btnLogout;
        private Label lblAppTitle;
        private Label lblAppSubtitle;

        // ── Orders list section ─────────────────────────────────────────────────
        private Label lblPageTitle;
        private Label lblUserName;
        private Panel pnlOrdersCard;
        private Label lblOrdersCardTitle;
        private Label lblOrdersCardSubtitle;
        private DataGridView dgvOrders;

        // ── Order detail section ─────────────────────────────────────────────────
        private Panel pnlDetailCard;
        private Label lblDetailTitle;
        private Label lblDetailDate;
        private DataGridView dgvItems;
        private Label lblTotal;
        private Label lblTotalValue;

        private Panel pnlStatusCard;
        private Label lblStatusCardTitle;
        private ProgressBar pbStatus;
        private Panel pnlStatusSteps;

        // ── State ────────────────────────────────────────────────────────────────
        private List<OrderDto> _orders = new();
        private OrderDto? _selectedOrder;

        // ────────────────────────────────────────────────────────────────────────
        public CustomerOrderManagementForm(IOrdercustomerService orderService, IServiceProvider serviceProvider)
        {
            _orderService = orderService;
            _serviceProvider = serviceProvider;
            InitializeComponent();
        }

        // ════════════════════════════════════════════════════════════════════════
        //  Designer-equivalent build
        // ════════════════════════════════════════════════════════════════════════
        private void InitializeComponent()
        {
            this.Text = "E-Commerce Management";
            this.Size = new Size(1366, 768);
            this.MinimumSize = new Size(1100, 650);
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9f);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += MyOrdersForm_Load;

            BuildSidebar();
            BuildContent();
        }

        // ── Sidebar ──────────────────────────────────────────────────────────────
        private void BuildSidebar()
        {
            pnlSidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                BackColor = Color.FromArgb(15, 23, 42)   // dark navy
            };

            // Logo area
            var pnlLogo = new Panel { Height = 64, Dock = DockStyle.Top, BackColor = Color.FromArgb(15, 23, 42) };
            lblAppTitle = new Label
            {
                Text = "E-Commerce",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Location = new Point(50, 12),
                AutoSize = true
            };
            lblAppSubtitle = new Label
            {
                Text = "Management",
                ForeColor = Color.FromArgb(148, 163, 184),
                Font = new Font("Segoe UI", 8f),
                Location = new Point(50, 32),
                AutoSize = true
            };
            // Simple icon placeholder
            var pnlIcon = new Panel
            {
                Size = new Size(28, 28),
                Location = new Point(16, 18),
                BackColor = Color.FromArgb(59, 130, 246)
            };
            pnlLogo.Controls.AddRange(new Control[] { pnlIcon, lblAppTitle, lblAppSubtitle });

            var pnlNav = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(15, 23, 42), Padding = new Padding(0, 8, 0, 0) };

            btnDashboard = CreateSidebarButton("  Dashboard", false);
            btnBrowseProducts = CreateSidebarButton("  Browse Products", false);
            btnCart = CreateSidebarButton("  Cart", false);
            btnMyOrders = CreateSidebarButton("  My Orders", true);   // active

            var pnlNavItems = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true
            };
            pnlNavItems.Controls.AddRange(new Control[] { btnDashboard, btnBrowseProducts, btnCart, btnMyOrders });
            pnlNav.Controls.Add(pnlNavItems);

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
            btnLogout.Click += (s, e) => { this.Close(); };

            // ── Sidebar Navigation Events ──────────────────────────────────────────
            btnDashboard.Click += (s, e) =>
            {
                var dashboard = _serviceProvider.GetRequiredService<ECommerece.Presentation.Forms.DashboardForms.DashboardForm>();
                dashboard.Show();
                this.Hide();
            };

            pnlSidebar.Controls.Add(pnlNav);
            pnlSidebar.Controls.Add(pnlLogo);
            pnlSidebar.Controls.Add(btnLogout);
            this.Controls.Add(pnlSidebar);
        }

        private Button CreateSidebarButton(string text, bool active)
        {
            var btn = new Button
            {
                Text = text,
                FlatStyle = FlatStyle.Flat,
                BackColor = active ? Color.FromArgb(30, 58, 138) : Color.Transparent,
                ForeColor = active ? Color.White : Color.FromArgb(148, 163, 184),
                TextAlign = ContentAlignment.MiddleLeft,
                Width = 200,
                Height = 40,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9.5f)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        // ── Content ──────────────────────────────────────────────────────────────
        private void BuildContent()
        {
            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(24, 16, 24, 16)
            };

            // Page header
            lblPageTitle = new Label
            {
                Text = "My Orders",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(24, 20)
            };

            lblUserName = new Label
            {
                Text = "John Doe  👤",
                AutoSize = true,
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(71, 85, 105),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            // ── Orders card ──────────────────────────────────────────────────────
            pnlOrdersCard = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Location = new Point(24, 65),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            pnlOrdersCard.Paint += RoundedCard_Paint;

            lblOrdersCardTitle = new Label
            {
                Text = "My Orders",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(16, 14),
                AutoSize = true
            };
            lblOrdersCardSubtitle = new Label
            {
                Text = "View your order history and track deliveries",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(16, 36),
                AutoSize = true
            };

            dgvOrders = BuildOrdersGrid();
            dgvOrders.Location = new Point(0, 58);
            dgvOrders.SelectionChanged += DgvOrders_SelectionChanged;

            pnlOrdersCard.Controls.AddRange(new Control[] { lblOrdersCardTitle, lblOrdersCardSubtitle, dgvOrders });

            // ── Detail + Status cards (bottom half) ──────────────────────────────
            pnlDetailCard = BuildDetailCard();
            pnlStatusCard = BuildStatusCard();

            pnlContent.Controls.Add(lblPageTitle);
            pnlContent.Controls.Add(lblUserName);
            pnlContent.Controls.Add(pnlOrdersCard);
            pnlContent.Controls.Add(pnlDetailCard);
            pnlContent.Controls.Add(pnlStatusCard);

            this.Controls.Add(pnlContent);
            this.Resize += MyOrdersForm_Resize;
        }

        private DataGridView BuildOrdersGrid()
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

            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "Order ID", Width = 80 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDate", HeaderText = "Date" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTotal", HeaderText = "Total" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colStatus", HeaderText = "Order Status" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPaymentStatus", HeaderText = "Payment" });

            return dgv;
        }

        private Panel BuildDetailCard()
        {
            var card = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            card.Paint += RoundedCard_Paint;

            lblDetailTitle = new Label
            {
                Text = "Order Items",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(16, 12),
                AutoSize = true
            };
            lblDetailDate = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(16, 32),
                AutoSize = true
            };

            // Payment status badge — top-right of detail card
            var lblPaymentBadge = new Label
            {
                Name = "lblPaymentBadge",
                Text = "",
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Padding = new Padding(6, 3, 6, 3),
                Location = new Point(200, 14)
            };

            dgvItems = new DataGridView
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
                    Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                    SelectionBackColor = Color.White
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(30, 41, 59),
                    SelectionBackColor = Color.FromArgb(239, 246, 255),
                    SelectionForeColor = Color.FromArgb(30, 41, 59)
                },
                Location = new Point(0, 55)
            };
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colProduct", HeaderText = "Product" });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colQty", HeaderText = "Quantity", Width = 70 });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPrice", HeaderText = "Price", Width = 90 });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSubtotal", HeaderText = "Subtotal", Width = 90 });

            lblTotal = new Label
            {
                Text = "Total:",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true
            };
            lblTotalValue = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true
            };
            var pnlTotalRow = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                Dock = DockStyle.Bottom,
                Padding = new Padding(8, 4, 8, 8)
            };
            pnlTotalRow.Controls.Add(lblTotal);
            pnlTotalRow.Controls.Add(lblTotalValue);

            card.Controls.AddRange(new Control[] { lblDetailTitle, lblDetailDate, lblPaymentBadge, dgvItems, pnlTotalRow });
            return card;
        }

        private Panel BuildStatusCard()
        {
            var card = new Panel { BackColor = Color.White };
            card.Paint += RoundedCard_Paint;

            var lblTitle = new Label
            {
                Text = "⚙  Order Status",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(16, 12),
                AutoSize = true
            };

            pbStatus = new ProgressBar
            {
                Location = new Point(16, 44),
                Height = 6,
                Minimum = 0,
                Maximum = 3,
                Value = 0,
                Style = ProgressBarStyle.Continuous
            };

            pnlStatusSteps = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                Location = new Point(16, 60),
                Width = 220
            };

            card.Controls.AddRange(new Control[] { lblTitle, pbStatus, pnlStatusSteps });
            return card;
        }

        // ════════════════════════════════════════════════════════════════════════
        //  Data loading
        // ════════════════════════════════════════════════════════════════════════
        private async void MyOrdersForm_Load(object sender, EventArgs e)
        {
            lblUserName.Location = new Point(pnlContent.Width - lblUserName.Width - 30, 24);
            AdjustLayout();
            await LoadOrdersAsync();
        }

        private async Task LoadOrdersAsync()
        {
            try
            {
                // ASSUMPTION: AppSession.CurrentUserId is set after login.
                // Replace with your actual session mechanism.
                _orders = await _orderService.getMyOrdersAsync();
                // Filter to current user if needed:
                // _orders = _orders.Where(o => o.user?.Id == AppSession.CurrentUserId.ToString()).ToList();

                PopulateOrdersGrid(_orders);

                if (_orders.Any())
                    SelectOrder(_orders.First());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load orders: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateOrdersGrid(List<OrderDto> orders)
        {
            dgvOrders.Rows.Clear();
            foreach (var o in orders)
            {
                int rowIdx = dgvOrders.Rows.Add(
                    $"#{o.Id}",
                    o.OrderDate.ToString("yyyy-MM-dd"),
                    $"${o.TotalAmount:F2}",
                    o.Status.ToString(),
                    o.PaymentStatus.ToString()
                );
                ColorStatusCell(dgvOrders.Rows[rowIdx].Cells["colStatus"], o.Status);
                ColorPaymentCell(dgvOrders.Rows[rowIdx].Cells["colPaymentStatus"], o.PaymentStatus);
            }
        }

        private void SelectOrder(OrderDto order)
        {
            _selectedOrder = order;

            // Highlight row
            foreach (DataGridViewRow row in dgvOrders.Rows)
            {
                if (row.Cells["colId"].Value?.ToString() == $"#{order.Id}")
                {
                    row.Selected = true;
                    break;
                }
            }

            // Populate detail header
            lblDetailTitle.Text = $"Order Items - #{order.Id}";
            lblDetailDate.Text = $"Ordered on {order.OrderDate:yyyy-MM-dd}";

            // Update payment badge
            if (pnlDetailCard.Controls["lblPaymentBadge"] is Label payBadge)
                ApplyPaymentBadge(payBadge, order.PaymentStatus);

            // Populate items
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

            // Status tracker
            UpdateStatusTracker(order.Status);
        }

        private void UpdateStatusTracker(OrderStatus status)
        {
            // Rejected / Cancelled are terminal bad states — shown in red, no progress
            bool isTerminalBad = status == OrderStatus.Rejected || status == OrderStatus.Cancelled;

            // The normal forward-progress steps
            var steps = new[] { "Pending", "Approved", "Processing", "Shipped", "Delivered" };

            int activeIndex = status switch
            {
                OrderStatus.Pending => 0,
                OrderStatus.Approved => 1,
                OrderStatus.Processing => 2,
                OrderStatus.Shipped => 3,
                OrderStatus.Delivered => 4,
                OrderStatus.Rejected => 0,   // stays at Pending visually
                OrderStatus.Cancelled => 0,
                _ => 0
            };

            pbStatus.Maximum = steps.Length - 1;
            pbStatus.Value = isTerminalBad ? 0 : Math.Min(activeIndex, pbStatus.Maximum);

            pnlStatusSteps.Controls.Clear();

            // If rejected or cancelled, show a single red banner instead of steps
            if (isTerminalBad)
            {
                var lblBanner = new Label
                {
                    Text = status == OrderStatus.Rejected ? "✗  Order Rejected" : "✗  Order Cancelled",
                    AutoSize = true,
                    ForeColor = Color.FromArgb(185, 28, 28),
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    Margin = new Padding(0, 8, 0, 0)
                };
                pnlStatusSteps.Controls.Add(lblBanner);
                return;
            }

            // Normal step indicators
            for (int i = 0; i < steps.Length; i++)
            {
                bool done = i < activeIndex;
                bool current = i == activeIndex;
                bool future = i > activeIndex;

                var pnlStep = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.LeftToRight,
                    AutoSize = true,
                    Margin = new Padding(0, 5, 0, 0)
                };

                Color circleColor = done || current
                    ? Color.FromArgb(37, 99, 235)
                    : Color.FromArgb(203, 213, 225);

                var circle = new Panel
                {
                    Size = new Size(18, 18),
                    BackColor = circleColor,
                    Margin = new Padding(0, 2, 8, 0)
                };
                // Draw as ellipse (circle)
                circle.Paint += (s, ev) =>
                {
                    var g = ev.Graphics;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    var pn = (Panel)s!;
                    g.FillEllipse(new SolidBrush(pn.BackColor), 0, 0, pn.Width - 1, pn.Height - 1);
                    if (done)
                    {
                        // Draw a checkmark tick
                        using var pen = new Pen(Color.White, 2f);
                        g.DrawLine(pen, 4, 9, 7, 13);
                        g.DrawLine(pen, 7, 13, 13, 5);
                    }
                };

                var lbl = new Label
                {
                    Text = steps[i],
                    AutoSize = true,
                    ForeColor = future ? Color.FromArgb(148, 163, 184) : Color.FromArgb(15, 23, 42),
                    Font = new Font("Segoe UI", 9f, current ? FontStyle.Bold : FontStyle.Regular)
                };

                pnlStep.Controls.Add(circle);
                pnlStep.Controls.Add(lbl);
                pnlStatusSteps.Controls.Add(pnlStep);
            }
        }

        // ════════════════════════════════════════════════════════════════════════
        //  Event handlers
        // ════════════════════════════════════════════════════════════════════════
        private void DgvOrders_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0) return;
            var row = dgvOrders.SelectedRows[0];
            var idStr = row.Cells["colId"].Value?.ToString()?.TrimStart('#');
            if (int.TryParse(idStr, out int id))
            {
                var order = _orders.FirstOrDefault(o => o.Id == id);
                if (order != null) SelectOrder(order);
            }
        }

        // ════════════════════════════════════════════════════════════════════════
        //  Helpers
        // ════════════════════════════════════════════════════════════════════════
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

        private void RoundedCard_Paint(object sender, PaintEventArgs e)
        {
            var p = (Panel)sender;
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(226, 232, 240), 1);
            g.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
        }

        private void AdjustLayout()
        {
            int w = pnlContent.ClientSize.Width - 48;
            int h = pnlContent.ClientSize.Height;

            // Orders card takes top ~40% of content area
            pnlOrdersCard.Size = new Size(w, 290);
            dgvOrders.Width = w;

            // Bottom area: detail card ~65%, status card ~33%
            int bottomY = pnlOrdersCard.Bottom + 12;
            int bottomH = h - bottomY - 10;
            int detailW = (int)(w * 0.63);
            int statusW = w - detailW - 12;

            pnlDetailCard.Location = new Point(24, bottomY);
            pnlDetailCard.Size = new Size(detailW, bottomH);
            dgvItems.Size = new Size(detailW, bottomH - 90);

            pnlStatusCard.Location = new Point(24 + detailW + 12, bottomY);
            pnlStatusCard.Size = new Size(statusW, bottomH);
            pbStatus.Width = statusW - 32;

            lblUserName.Location = new Point(pnlContent.ClientSize.Width - lblUserName.Width - 20, 24);
        }

        private void MyOrdersForm_Resize(object sender, EventArgs e) => AdjustLayout();
    }
}
