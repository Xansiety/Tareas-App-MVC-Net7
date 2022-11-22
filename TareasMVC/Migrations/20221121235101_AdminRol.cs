using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TareasMVC.Migrations
{
    /// <inheritdoc />
    public partial class AdminRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT id FROM MVCTareasDB.dbo.AspNetRoles WHERE id = 'eb4419ac-9ebd-4f90-b34f-914bcad3db12')
                                    BEGIN 
	                                    insert into MVCTareasDB.dbo.AspNetRoles (id, Name, NormalizedName)
	                                    values('eb4419ac-9ebd-4f90-b34f-914bcad3db12', 'admin', 'ADMIN')
                                    END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM MVCTareasDB.dbo.AspNetRoles WHERE id = 'eb4419ac-9ebd-4f90-b34f-914bcad3db12'");
        }
    }
}
