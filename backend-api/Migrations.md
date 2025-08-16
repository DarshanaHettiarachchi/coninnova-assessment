### 🛠 Running EF Core Migrations from Infrastructure Project

This guide shows how to add and apply Entity Framework Core
migrations when your `DbContext` is located
inside the `CivCost.Infrastructure` project,
and your `Program.cs` (startup) is in `CivCost.API`.

---

###  Run following commands

```bash
dotnet tool install --global dotnet-ef

cd src/CivCost.Infrastructure


dotnet ef migrations add [MigratinName] `
  --project . `
  --startup-project ..\CivCost.Api `
  --output-dir Migrations

dotnet ef database update `
  --project . `
  --startup-project ..\CivCost.Api


   public partial class AddPurchaseOrderSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create the SQL Server sequence
            migrationBuilder.Sql(@"
                CREATE SEQUENCE PurchaseOrderSeq
                START WITH 1
                INCREMENT BY 1;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the sequence if rolling back
            migrationBuilder.Sql("DROP SEQUENCE PurchaseOrderSeq;");
        }
    }
