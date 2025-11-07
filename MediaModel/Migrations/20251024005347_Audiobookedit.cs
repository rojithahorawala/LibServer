using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaModel.Migrations
{
    /// <inheritdoc />
    public partial class Audiobookedit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey(
                name: "PK_audiobooks",
                table: "audiobooks",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_audiobooks",
                table: "audiobooks");
        }
    }
}
