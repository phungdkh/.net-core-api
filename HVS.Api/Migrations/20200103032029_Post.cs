using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HVS.Api.Migrations
{
    public partial class Post : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RecordOrder = table.Column<int>(nullable: false),
                    RecordDeleted = table.Column<bool>(nullable: false),
                    RecordActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    DeletedBy = table.Column<Guid>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<Guid>(nullable: true),
                    Title = table.Column<string>(maxLength: 512, nullable: false),
                    Thumbnail = table.Column<string>(maxLength: 512, nullable: false),
                    Content = table.Column<string>(maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    PostId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    RecordOrder = table.Column<int>(nullable: false),
                    RecordDeleted = table.Column<bool>(nullable: false),
                    RecordActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    DeletedBy = table.Column<Guid>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Content = table.Column<string>(maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => new { x.PostId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Comment_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comment_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_UserId",
                table: "Comment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_UserId",
                table: "Post",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Post");
        }
    }
}
