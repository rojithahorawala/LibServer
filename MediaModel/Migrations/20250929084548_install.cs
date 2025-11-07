using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaModel.Migrations
{
    /// <inheritdoc />
    public partial class install : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: false),
                    author = table.Column<string>(type: "nchar(25)", fixedLength: true, maxLength: 25, nullable: false),
                    coAuthor = table.Column<string>(type: "nchar(25)", fixedLength: true, maxLength: 25, nullable: false),
                    language = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    publisher = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: false),
                    publicationYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_books", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "audiobooks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bookid = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nchar(30)", fixedLength: true, maxLength: 30, nullable: false),
                    author = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    language = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    narrator = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: false),
                    publisher = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: false),
                    publicationYear = table.Column<string>(type: "nchar(4)", fixedLength: true, maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_audiobooks_books",
                        column: x => x.bookid,
                        principalTable: "books",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_audiobooks_bookid",
                table: "audiobooks",
                column: "bookid");


            migrationBuilder.AlterColumn<string>(
                name: "coAuthor",
                table: "books",
                type: "nchar(25)",
                nullable: false,
                defaultValue: "",    // default empty string
                oldClrType: typeof(string),
                oldType: "nchar(25)");




        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audiobooks");

            migrationBuilder.DropTable(
                name: "books");
        }
    }
}
