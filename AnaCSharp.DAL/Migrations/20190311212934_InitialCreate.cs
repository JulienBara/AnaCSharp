using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnaCSharp.DAL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    ChatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Label = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.ChatId);
                });

            migrationBuilder.CreateTable(
                name: "DeterminingStates",
                columns: table => new
                {
                    DeterminingStateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeterminingStates", x => x.DeterminingStateId);
                });

            migrationBuilder.CreateTable(
                name: "MaxMarkovDegrees",
                columns: table => new
                {
                    MaxMarkovDegreeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaxMarkovDegrees", x => x.MaxMarkovDegreeId);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    WordId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Label = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.WordId);
                });

            migrationBuilder.CreateTable(
                name: "DeterminedWords",
                columns: table => new
                {
                    DeterminedWordId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DeterminingStateId = table.Column<int>(nullable: false),
                    WordId = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeterminedWords", x => x.DeterminedWordId);
                    table.ForeignKey(
                        name: "FK_DeterminedWords_DeterminingStates_DeterminingStateId",
                        column: x => x.DeterminingStateId,
                        principalTable: "DeterminingStates",
                        principalColumn: "DeterminingStateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeterminedWords_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "WordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeterminingWord",
                columns: table => new
                {
                    DeterminingWordId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WordId = table.Column<int>(nullable: false),
                    DeterminingStateId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeterminingWord", x => x.DeterminingWordId);
                    table.ForeignKey(
                        name: "FK_DeterminingWord_DeterminingStates_DeterminingStateId",
                        column: x => x.DeterminingStateId,
                        principalTable: "DeterminingStates",
                        principalColumn: "DeterminingStateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeterminingWord_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "WordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogWords",
                columns: table => new
                {
                    LogWordId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChatId = table.Column<int>(nullable: false),
                    WordId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogWords", x => x.LogWordId);
                    table.ForeignKey(
                        name: "FK_LogWords_Chat_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chat",
                        principalColumn: "ChatId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LogWords_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "WordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeterminedWords_DeterminingStateId",
                table: "DeterminedWords",
                column: "DeterminingStateId");

            migrationBuilder.CreateIndex(
                name: "IX_DeterminedWords_WordId",
                table: "DeterminedWords",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_DeterminingWord_DeterminingStateId",
                table: "DeterminingWord",
                column: "DeterminingStateId");

            migrationBuilder.CreateIndex(
                name: "IX_DeterminingWord_WordId",
                table: "DeterminingWord",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_LogWords_ChatId",
                table: "LogWords",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_LogWords_WordId",
                table: "LogWords",
                column: "WordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeterminedWords");

            migrationBuilder.DropTable(
                name: "DeterminingWord");

            migrationBuilder.DropTable(
                name: "LogWords");

            migrationBuilder.DropTable(
                name: "MaxMarkovDegrees");

            migrationBuilder.DropTable(
                name: "DeterminingStates");

            migrationBuilder.DropTable(
                name: "Chat");

            migrationBuilder.DropTable(
                name: "Words");
        }
    }
}
