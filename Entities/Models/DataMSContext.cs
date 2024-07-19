using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

public partial class DataMSContext : DbContext
{
    public DataMSContext()
    {
    }

    public DataMSContext(DbContextOptions<DataMSContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Action> Action { get; set; }
    public virtual DbSet<AddressClient> AddressClient { get; set; }
    public virtual DbSet<AllCode> AllCode { get; set; }
    public virtual DbSet<Article> Article { get; set; }
    public virtual DbSet<ArticleCategory> ArticleCategory { get; set; }
    public virtual DbSet<ArticleRelated> ArticleRelated { get; set; }
    public virtual DbSet<ArticleTag> ArticleTag { get; set; }
    public virtual DbSet<AttachFile> AttachFile { get; set; }
    public virtual DbSet<CampaignAds> CampaignAds { get; set; }
    public virtual DbSet<Cashback> Cashback { get; set; }
    public virtual DbSet<Client> Client { get; set; }
    public virtual DbSet<District> District { get; set; }
    public virtual DbSet<GroupProduct> GroupProduct { get; set; }
    public virtual DbSet<ImageProduct> ImageProduct { get; set; }
    public virtual DbSet<IndustrySpecialLuxury> IndustrySpecialLuxury { get; set; }
    public virtual DbSet<Label> Label { get; set; }
    public virtual DbSet<Menu> Menu { get; set; }
    public virtual DbSet<Note> Note { get; set; }
    public virtual DbSet<Order> Order { get; set; }
    public virtual DbSet<OrderItem> OrderItem { get; set; }
    public virtual DbSet<Payment> Payment { get; set; }
    public virtual DbSet<Permission> Permission { get; set; }
    public virtual DbSet<Position> Position { get; set; }
    public virtual DbSet<PriceProductLevel> PriceProductLevel { get; set; }
    public virtual DbSet<Product> Product { get; set; }
    public virtual DbSet<ProductClassification> ProductClassification { get; set; }
    public virtual DbSet<ProductHistory> ProductHistory { get; set; }
    public virtual DbSet<ProductOtherLink> ProductOtherLink { get; set; }
    public virtual DbSet<PropertiesProduct> PropertiesProduct { get; set; }
    public virtual DbSet<Province> Province { get; set; }
    public virtual DbSet<Role> Role { get; set; }
    public virtual DbSet<RolePermission> RolePermission { get; set; }
    public virtual DbSet<Tag> Tag { get; set; }
    public virtual DbSet<User> User { get; set; }
    public virtual DbSet<UserRole> UserRole { get; set; }
    public virtual DbSet<Voucher> Voucher { get; set; }
    public virtual DbSet<VoucherCampaign> VoucherCampaign { get; set; }
    public virtual DbSet<VoucherLogActivity> VoucherLogActivity { get; set; }
    public virtual DbSet<Ward> Ward { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=103.163.216.41;Initial Catalog=Hulotoy;Persist Security Info=True;User ID=us;Password=us@585668;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Action>(entity =>
        {
            entity.ToTable("Action");

            entity.Property(e => e.ActionName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ControllerName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Menu).WithMany(p => p.Actions)
                .HasForeignKey(d => d.MenuId)
                .HasConstraintName("FK_Action_Menu");

            entity.HasOne(d => d.Permission).WithMany(p => p.Actions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK_Action_Permission");
        });

        modelBuilder.Entity<AddressClient>(entity =>
        {
            entity.ToTable("AddressClient");

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DistrictId).HasMaxLength(5);
            entity.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Đây là số điện thoại nhận hàng");
            entity.Property(e => e.ProvinceId).HasMaxLength(5);
            entity.Property(e => e.ReceiverName).HasMaxLength(255);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.WardId).HasMaxLength(5);

            entity.HasOne(d => d.Client).WithMany(p => p.AddressClients)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AddressClient_Client");
        });

        modelBuilder.Entity<AffiliateGroupProduct>(entity =>
        {
            entity.ToTable("AffiliateGroupProduct");
        });

        modelBuilder.Entity<AllCode>(entity =>
        {
            entity.ToTable("AllCode");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.ToTable("Article");

            entity.Property(e => e.Body)
                .IsRequired()
                .HasColumnType("ntext");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DownTime).HasColumnType("datetime");
            entity.Property(e => e.Image11).HasMaxLength(350);
            entity.Property(e => e.Image169)
                .IsRequired()
                .HasMaxLength(350);
            entity.Property(e => e.Image43).HasMaxLength(350);
            entity.Property(e => e.Lead)
                .IsRequired()
                .HasMaxLength(400);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.PublishDate).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.UpTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<ArticleCategory>(entity =>
        {
            entity.ToTable("ArticleCategory");
        });

        modelBuilder.Entity<ArticleRelated>(entity =>
        {
            entity.ToTable("ArticleRelated");

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleRelateds)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK_ArticleRelated_Article");
        });

        modelBuilder.Entity<ArticleTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ArticleTags");

            entity.ToTable("ArticleTag");

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleTags)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK_ArticleTags_Article");

            entity.HasOne(d => d.Tag).WithMany(p => p.ArticleTags)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("FK_ArticleTags_Tags");
        });

        modelBuilder.Entity<AttachFile>(entity =>
        {
            entity.ToTable("AttachFile");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Ext)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Path).HasMaxLength(400);
        });

        modelBuilder.Entity<AutomaticPurchaseAmz>(entity =>
        {
            entity.ToTable("AutomaticPurchaseAmz");

            entity.Property(e => e.AutoBuyMappingId).HasColumnName("AutoBuyMappingID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.OrderCode)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.OrderDetailUrl).HasColumnName("OrderDetailURL");
            entity.Property(e => e.OrderEstimatedDeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.OrderMappingId).HasMaxLength(150);
            entity.Property(e => e.OrderedSuccessUrl).HasColumnName("OrderedSuccessURL");
            entity.Property(e => e.ProductCode).HasMaxLength(100);
            entity.Property(e => e.PurchaseUrl)
                .IsRequired()
                .HasColumnName("PurchaseURL");
            entity.Property(e => e.PurchasedOrderId)
                .HasMaxLength(150)
                .HasColumnName("PurchasedOrderID");
            entity.Property(e => e.PurchasedSellerName).HasMaxLength(250);
            entity.Property(e => e.PurchasedSellerStoreUrl)
                .HasMaxLength(250)
                .HasColumnName("PurchasedSellerStoreURL");
            entity.Property(e => e.UpdateLast).HasColumnType("datetime");
        });

        modelBuilder.Entity<AutomaticPurchaseHistory>(entity =>
        {
            entity.ToTable("AutomaticPurchaseHistory");

            entity.Property(e => e.AutomaticPurchaseId).HasColumnName("AutomaticPurchaseID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<CampaignAd>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CampaignName).HasMaxLength(300);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(400);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<CampaignGroupProduct>(entity =>
        {
            entity.ToTable("CampaignGroupProduct");
        });

        modelBuilder.Entity<Cashback>(entity =>
        {
            entity.ToTable("Cashback");

            entity.Property(e => e.CashbackDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(500);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tblAccount");

            entity.ToTable("Client");

            entity.Property(e => e.ActiveToken).HasMaxLength(100);
            entity.Property(e => e.Avartar)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Birthday).HasColumnType("datetime");
            entity.Property(e => e.ClientName).HasMaxLength(256);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ForgotPasswordToken).HasMaxLength(100);
            entity.Property(e => e.IsReceiverInfoEmail).HasColumnName("isReceiverInfoEmail");
            entity.Property(e => e.JoinDate).HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(400);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordBackup)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReferralId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SourceLoginId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.TokenCreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<ClientLinkAff>(entity =>
        {
            entity.ToTable("ClientLinkAff");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ClientId).ValueGeneratedOnAdd();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.LinkAff)
                .IsRequired()
                .IsUnicode(false);
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__District__85FDA4C6D88CB461");

            entity.ToTable("District");

            entity.Property(e => e.DistrictId)
                .IsRequired()
                .HasMaxLength(5);
            entity.Property(e => e.Location).HasMaxLength(30);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.ProvinceId)
                .IsRequired()
                .HasMaxLength(5);
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(30);
        });

        modelBuilder.Entity<GroupProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_groupProduct");

            entity.ToTable("GroupProduct");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ImagePath)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(400);
            entity.Property(e => e.Path)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<GroupProductStore>(entity =>
        {
            entity.ToTable("GroupProductStore");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.LinkStoreMenu)
                .IsRequired()
                .HasMaxLength(500);
        });

        modelBuilder.Entity<ImageProduct>(entity =>
        {
            entity.ToTable("ImageProduct");

            entity.Property(e => e.Image)
                .HasMaxLength(400)
                .IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany(p => p.ImageProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ImageProduct_Product");
        });

        modelBuilder.Entity<ImageSize>(entity =>
        {
            entity.ToTable("ImageSize");

            entity.Property(e => e.PositionName)
                .IsRequired()
                .HasMaxLength(250);
        });

        modelBuilder.Entity<IndustrySpecialLuxury>(entity =>
        {
            entity.ToTable("IndustrySpecialLuxury");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ExpireDate).HasColumnType("datetime");
            entity.Property(e => e.GroupLabelId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("Nhóm sản phẩm áp dụng so sánh với nhóm sản phẩm nổi trội");
            entity.Property(e => e.UpdateLast).HasColumnType("datetime");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.ToTable("Job");

            entity.Property(e => e.Type).HasComment("1: sync client ; 2 : sync order");
        });

        modelBuilder.Entity<Label>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Store");

            entity.ToTable("Label");

            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.DescExpire).HasMaxLength(300);
            entity.Property(e => e.Domain)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Icon).HasMaxLength(500);
            entity.Property(e => e.PrefixOrderCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.StoreName).HasMaxLength(50);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<LocationProduct>(entity =>
        {
            entity.ToTable("LocationProduct");

            entity.Property(e => e.CreateOn).HasColumnType("datetime");
            entity.Property(e => e.ProductCode)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateLast).HasColumnType("datetime");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.ToTable("Menu");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Icon).HasMaxLength(50);
            entity.Property(e => e.Link).HasMaxLength(200);
            entity.Property(e => e.MenuCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);
        });

        modelBuilder.Entity<Mfauser>(entity =>
        {
            entity.ToTable("MFAUser");

            entity.Property(e => e.BackupCode)
                .IsRequired()
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SecretKey)
                .IsRequired()
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedYear)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.ToTable("Note");

            entity.Property(e => e.Comment).HasMaxLength(400);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.Address).HasMaxLength(400);
            entity.Property(e => e.ClientName).HasMaxLength(100);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OrderNo)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TrackingId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateLast).HasColumnType("datetime");
            entity.Property(e => e.UtmCampaign)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UtmFirstTime)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UtmMedium)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.UtmSource)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VoucherName)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Client).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_Order_Client");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItem", tb => tb.HasComment("Pound"));

            entity.Property(e => e.CreateOn).HasColumnType("datetime");
            entity.Property(e => e.ImageThumb)
                .HasMaxLength(4000)
                .IsUnicode(false);
            entity.Property(e => e.UpdateLast).HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItem_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItem_Product");
        });

        modelBuilder.Entity<OrderProgress>(entity =>
        {
            entity.ToTable("OrderProgress");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.OrderNo)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payment");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("Permission");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.ToTable("Position");

            entity.Property(e => e.PositionName)
                .IsRequired()
                .HasMaxLength(250);
        });

        modelBuilder.Entity<PriceProductLevel>(entity =>
        {
            entity.HasKey(e => e.PriceId);

            entity.ToTable("PriceProductLevel");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.LabelId)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Note).HasMaxLength(2000);
            entity.Property(e => e.ToDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.CreateOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Information).HasColumnType("ntext");
            entity.Property(e => e.LinkSource)
                .HasMaxLength(4000)
                .IsUnicode(false);
            entity.Property(e => e.Manufacturer).HasMaxLength(400);
            entity.Property(e => e.Path)
                .HasMaxLength(4000)
                .IsUnicode(false);
            entity.Property(e => e.ProductCode)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Rating)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SellerId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SellerName)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(300);
            entity.Property(e => e.UnitWeight)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdateLast).HasColumnType("datetime");
            entity.Property(e => e.Variations).HasMaxLength(400);
        });

        modelBuilder.Entity<ProductClassification>(entity =>
        {
            entity.ToTable("ProductClassification");

            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.Link).IsRequired();
            entity.Property(e => e.LinkProductTarget)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Note).HasMaxLength(400);
            entity.Property(e => e.ProductName).HasMaxLength(400);
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.UtmPath)
                .HasMaxLength(400)
                .IsUnicode(false);

            entity.HasOne(d => d.Capaign).WithMany(p => p.ProductClassifications)
                .HasForeignKey(d => d.CapaignId)
                .HasConstraintName("FK_ProductClassification_CampaignAds");

            entity.HasOne(d => d.GroupIdChoiceNavigation).WithMany(p => p.ProductClassifications)
                .HasForeignKey(d => d.GroupIdChoice)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductClassification_groupProduct");

            entity.HasOne(d => d.Label).WithMany(p => p.ProductClassifications)
                .HasForeignKey(d => d.LabelId)
                .HasConstraintName("FK_ProductClassification_label");

            entity.HasOne(d => d.User).WithMany(p => p.ProductClassifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_ProductClassification_User");
        });

        modelBuilder.Entity<ProductHistory>(entity =>
        {
            entity.ToTable("ProductHistory");

            entity.Property(e => e.CreateOn).HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductHistories)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductHistory_Product");
        });

        modelBuilder.Entity<ProductOtherLink>(entity =>
        {
            entity.ToTable("ProductOtherLink");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.LinkOrder)
                .HasMaxLength(4000)
                .IsUnicode(false);
            entity.Property(e => e.Note).HasMaxLength(4000);
            entity.Property(e => e.PriceCheckout).HasComment("Là giá checkout khi khách đã chuyển tiền và Sale bắt đầu tạo đơn");
            entity.Property(e => e.PriceDyamic).HasComment("Khi sửa giá do biến động giá khi đầu Mỹ tiến hành mua sau khi đã tạo đơn. Thí sẽ lưu giá sửa đó vào trường này");
            entity.Property(e => e.ProductOrderId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<PropertiesProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PropertiesProduct_1");

            entity.ToTable("PropertiesProduct");

            entity.Property(e => e.CreateOn).HasColumnType("datetime");
            entity.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(300);

            entity.HasOne(d => d.Product).WithMany(p => p.PropertiesProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PropertiesProduct_Product");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Province__FD0A6F838767F971");

            entity.ToTable("Province");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.ProvinceId)
                .IsRequired()
                .HasMaxLength(5);
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(30);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(250);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.ToTable("RolePermission");

            entity.HasOne(d => d.Menu).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermission_Menu");

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermission_Permission");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermission_Role");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Tags");

            entity.ToTable("Tag");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.TagName).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User_1");

            entity.ToTable("User");

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Avata).HasMaxLength(500);
            entity.Property(e => e.BirthDay).HasColumnType("datetime");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(500);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(2500);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ResetPassword)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRole");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_Role");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_User");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.ToTable("Voucher");

            entity.Property(e => e.CampaignId).HasColumnName("campaign_id");
            entity.Property(e => e.Cdate)
                .HasColumnType("datetime")
                .HasColumnName("cdate");
            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EDate)
                .HasColumnType("datetime")
                .HasColumnName("eDate");
            entity.Property(e => e.GroupUserPriority)
                .IsUnicode(false)
                .HasComment("Trường này để lưu nhóm những user được áp dụng trên voucher này")
                .HasColumnName("group_user_priority");
            entity.Property(e => e.IsFreeLuxury).HasColumnName("is_free_luxury");
            entity.Property(e => e.IsLimitVoucher).HasColumnName("is_limit_voucher");
            entity.Property(e => e.IsMaxFee).HasColumnName("is_max_fee");
            entity.Property(e => e.IsMaxPriceProduct).HasColumnName("is_max_price_product");
            entity.Property(e => e.IsPriceFirstPoundFee)
                .HasDefaultValue(false)
                .HasColumnName("is_price_first_pound_fee");
            entity.Property(e => e.IsPublic)
                .HasComment("Nêu set true thì hiểu voucher này được public cho các user thanh toán đơn hàng")
                .HasColumnName("is_public");
            entity.Property(e => e.LimitTotalDiscount).HasColumnName("limit_total_discount");
            entity.Property(e => e.LimitUse).HasColumnName("limitUse");
            entity.Property(e => e.MinTotalAmount).HasColumnName("min_total_amount");
            entity.Property(e => e.PriceSales)
                .HasColumnType("money")
                .HasColumnName("price_sales");
            entity.Property(e => e.RuleType)
                .HasComment("Trường này dùng để phân biệt voucher triển khai này chạy theo rule nào. Ví dụ: rule giảm giá với 1 số tiền vnđ trên toàn bộ đơn hàng. Giảm giá 20% phí first pound đầu tiên của nhãn hàng amazon. 1: triển khai rule giảm giá cho toàn bộ đơn hàng. 2 là rule áp dụng cho 20% phí first pound đầu tiên.")
                .HasColumnName("rule_type");
            entity.Property(e => e.StoreApply)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("store_apply");
            entity.Property(e => e.Udate)
                .HasColumnType("datetime")
                .HasColumnName("udate");
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("unit");
        });

        modelBuilder.Entity<VoucherCampaign>(entity =>
        {
            entity.ToTable("VoucherCampaign");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CampaignVoucher)
                .HasMaxLength(400)
                .HasColumnName("campaign_voucher");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
        });

        modelBuilder.Entity<VoucherLogActivity>(entity =>
        {
            entity.ToTable("voucherLogActivity");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PriceSaleVnd).HasColumnName("price_sale_vnd");
            entity.Property(e => e.Status)
                .HasComment("Trang thai giao dịch voucher. 1: khoa. 0: dang ap dung")
                .HasColumnName("status");
            entity.Property(e => e.StoreId).HasColumnName("store_id");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("update_time");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.VoucherId).HasColumnName("voucher_id");

            entity.HasOne(d => d.Voucher).WithMany(p => p.VoucherLogActivities)
                .HasForeignKey(d => d.VoucherId)
                .HasConstraintName("FK_voucherLogActivity_Voucher");
        });

        modelBuilder.Entity<Ward>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ward__C6BD9BCA1EF01D69");

            entity.ToTable("Ward");

            entity.Property(e => e.DistrictId)
                .IsRequired()
                .HasMaxLength(5);
            entity.Property(e => e.Location).HasMaxLength(30);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(30);
            entity.Property(e => e.WardId)
                .IsRequired()
                .HasMaxLength(5);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
