using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Shackmeets.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Username = table.Column<string>(nullable: false),
                    LocationLatitude = table.Column<decimal>(nullable: false),
                    LocationLongitude = table.Column<decimal>(nullable: false),
                    MaxNotificationDistance = table.Column<int>(nullable: false),
                    NotificationOption = table.Column<int>(nullable: false),
                    NotifyByShackmessage = table.Column<bool>(nullable: false),
                    NotifyByEmail = table.Column<bool>(nullable: false),
                    NotificationEmail = table.Column<string>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false),
                    IsBanned = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Meets",
                columns: table => new
                {
                    MeetId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    OrganizerUsername = table.Column<string>(nullable: true),
                    EventDate = table.Column<DateTime>(nullable: false),
                    TimestampCreate = table.Column<DateTime>(nullable: false),
                    TimestampChange = table.Column<DateTime>(nullable: true),
                    LocationName = table.Column<string>(nullable: true),
                    LocationAddress = table.Column<string>(nullable: true),
                    LocationState = table.Column<string>(nullable: true),
                    LocationCountry = table.Column<string>(nullable: true),
                    LocationLatitude = table.Column<decimal>(nullable: false),
                    LocationLongitude = table.Column<decimal>(nullable: false),
                    WillPostAnnouncement = table.Column<bool>(nullable: false),
                    LastAnnouncementPostDate = table.Column<DateTime>(nullable: true),
                    IsCancelled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meets", x => x.MeetId);
                    table.ForeignKey(
                        name: "FK_Meets_Users_OrganizerUsername",
                        column: x => x.OrganizerUsername,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NotificationType = table.Column<int>(nullable: false),
                    NotificationReason = table.Column<int>(nullable: false),
                    MeetId = table.Column<int>(nullable: true),
                    TargetUsername = table.Column<string>(nullable: true),
                    MessageSubject = table.Column<string>(nullable: true),
                    MessageBody = table.Column<string>(nullable: true),
                    IsSent = table.Column<bool>(nullable: false),
                    TimestampCreate = table.Column<DateTime>(nullable: false),
                    TargetUserUsername = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notification_Meets_MeetId",
                        column: x => x.MeetId,
                        principalTable: "Meets",
                        principalColumn: "MeetId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notification_Users_TargetUserUsername",
                        column: x => x.TargetUserUsername,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rsvps",
                columns: table => new
                {
                    RsvpId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MeetId = table.Column<int>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    RsvpType = table.Column<int>(nullable: false),
                    NumAttendees = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rsvps", x => x.RsvpId);
                    table.ForeignKey(
                        name: "FK_Rsvps_Meets_MeetId",
                        column: x => x.MeetId,
                        principalTable: "Meets",
                        principalColumn: "MeetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rsvps_Users_Username",
                        column: x => x.Username,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meets_OrganizerUsername",
                table: "Meets",
                column: "OrganizerUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_MeetId",
                table: "Notification",
                column: "MeetId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_TargetUserUsername",
                table: "Notification",
                column: "TargetUserUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Rsvps_MeetId",
                table: "Rsvps",
                column: "MeetId");

            migrationBuilder.CreateIndex(
                name: "IX_Rsvps_Username",
                table: "Rsvps",
                column: "Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Rsvps");

            migrationBuilder.DropTable(
                name: "Meets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
