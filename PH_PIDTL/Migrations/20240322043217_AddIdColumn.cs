using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Polynic.Migrations
{
    /// <inheritdoc />
    public partial class AddIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PH_PIDTL",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
            migrationBuilder.CreateTable(
                name: "PH_PIDTL",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    REMARK2 = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: false),
                    ITEMCODE = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: false),
                    DESCRIPTION2 = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: false),
                    BATCH = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: false),
                    LOCATION = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: false),
                    QTY = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    UOM = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PH_PIDTL", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PH_PIDTL");
        }
    }
}
