using FunnyMoneyTransfer.Data.GeneratedModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FunnyMoneyTransfer.Data
{
    public partial class FunnyMoneyTransferContext
    {
        private void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity => entity.HasKey(e => e.Id));
            modelBuilder.Entity<StartingBalance>(entity => entity.HasKey(e => e.Id));
            modelBuilder.Entity<Transfer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable(t => t.HasCheckConstraint("CK_transfers_not_same_account", "(account_id_from<>account_id_to)"));
                entity.ToTable(t => t.HasCheckConstraint("CK_transfers_amount_gt_0", "(amount > 0)"));
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired();
                entity.Ignore(e => e.ShowSendAndRequestButtons);
            });
            modelBuilder.Entity<TransferType>(entity => entity.HasKey(e => e.Id));
            modelBuilder.Entity<TransferStatus>(entity => entity.HasKey(e => e.Id));
        }

        private static string GetConnectionStringFromConfiguration()  //TODO: Get this into Startup
        {
            string currentDirectory = Environment.CurrentDirectory;
            string configFileName = "appsettings.json";
            string fullPathToConfigFile = Path.Combine(currentDirectory, @"..\..\..\..\FunnyMoneyTransfer.Data", configFileName);

            IConfigurationRoot builder = new ConfigurationBuilder()
                .AddJsonFile(fullPathToConfigFile, optional: false)
                .Build();

            return builder.GetConnectionString("Project") ?? string.Empty;
        }
    }
}
