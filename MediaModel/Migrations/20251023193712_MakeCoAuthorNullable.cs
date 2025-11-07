using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaModel.Migrations
{
    /// <inheritdoc />
    public partial class MakeCoAuthorNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "coAuthor",
                table: "books",
                type: "nchar(25)",
                fixedLength: true,
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(25)",
                oldFixedLength: true,
                oldMaxLength: 25);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "coAuthor",
                table: "books",
                type: "nchar(25)",
                fixedLength: true,
                maxLength: 25,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nchar(25)",
                oldFixedLength: true,
                oldMaxLength: 25,
                oldNullable: true);
        }
    }
}
