using Microsoft.EntityFrameworkCore;
using WebTiemVangKimCuc.SER.Domain.Entities;
using WebTiemVangKimCuc.SER.Domain.Entities.DanhMuc;

namespace WebTiemVangKimCuc.SER.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Create Table
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<DmTrangSuc> PhanLoaiTrangSucs { get; set; }
        public DbSet<DmChatLieu> PhanLoaiChatLieus { get; set; }
        public DbSet<User> Users { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .LogTo(Console.WriteLine)
                    .UseSqlServer(Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING"), x => x.UseNetTopologySuite());     
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.ToTable("SAN_PHAM");

                entity.HasKey(e => e.Id);

                entity.HasQueryFilter(x => !x.IsDeleted);

                entity
                    .HasOne(s => s.ChatLieu)
                    .WithMany(c => c.SanPhams)
                    .HasForeignKey(s => s.ChatLieuId);

                entity
                    .HasOne(s => s.LoaiTrangSuc)
                    .WithMany(l => l.SanPhams)
                    .HasForeignKey(s => s.LoaiTrangSucId);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.IsDeleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.LoaiTrangSucId).HasColumnName("LoaiTrangSuc_ID");
                entity.Property(e => e.ChatLieuId).HasColumnName("ChatLieu_ID");
            });

            modelBuilder.Entity<DmTrangSuc>(entity =>
            {
                entity.ToTable("DM_TRANG_SUC");

                entity.HasKey(e => e.Id);

                entity
                    .HasMany(l => l.SanPhams)
                    .WithOne(s => s.LoaiTrangSuc)
                    .HasForeignKey(s => s.LoaiTrangSucId);
            });

            modelBuilder.Entity<DmChatLieu>(entity =>
            {
                entity.ToTable("DM_CHAT_LIEU");

                entity.HasKey(e => e.Id);

                entity
                    .HasMany(c => c.SanPhams)
                    .WithOne(s => s.ChatLieu)
                    .HasForeignKey(s => s.ChatLieuId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("KIMCUC_USER");

                entity.HasKey(e => e.UserName);

                entity.Property(e => e.FullName).HasColumnName("HoTen");
                entity.Property(e => e.UserName).HasColumnName("TaiKhoan");
                entity.Property(e => e.PassWord).HasColumnName("MatKhau");
            });

            // tao migration
            //dotnet ef migrations add addTableDuAn -c ApplicationDbContext -o "Migrations"

            //xem ds migra
            //dotnet ef migrations list

            // Update SQL
            //dotnet ef database update
        }
    }    
}
