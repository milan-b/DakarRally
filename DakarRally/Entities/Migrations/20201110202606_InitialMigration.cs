using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "race",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Year = table.Column<ushort>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_race", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_type",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    MaxSpeed = table.Column<byte>(nullable: false),
                    RepairmentTimeInHovers = table.Column<byte>(nullable: false),
                    PercentageOfLightMalfunctionsPerHour = table.Column<byte>(nullable: false),
                    PercentageOfHeavyMalfunctionsPerHour = table.Column<byte>(nullable: false),
                    SuperType = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_type", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "simulation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: true),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    RaceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_simulation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_simulation_race_RaceId",
                        column: x => x.RaceId,
                        principalTable: "race",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vehicle",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TeamName = table.Column<string>(nullable: false),
                    Model = table.Column<string>(nullable: false),
                    ManucaturingDate = table.Column<DateTime>(nullable: false),
                    VehicleTypeId = table.Column<string>(nullable: false),
                    RaceId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vehicle_race_RaceId",
                        column: x => x.RaceId,
                        principalTable: "race",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vehicle_vehicle_type_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "vehicle_type",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_statistic",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Distance = table.Column<double>(nullable: false),
                    Malfunctions = table.Column<ushort>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    VehicleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_statistic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vehicle_statistic_vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "vehicle_type",
                columns: new[] { "Name", "MaxSpeed", "PercentageOfHeavyMalfunctionsPerHour", "PercentageOfLightMalfunctionsPerHour", "RepairmentTimeInHovers", "SuperType" },
                values: new object[] { "sportsCar", (byte)140, (byte)2, (byte)12, (byte)5, "car" });

            migrationBuilder.InsertData(
                table: "vehicle_type",
                columns: new[] { "Name", "MaxSpeed", "PercentageOfHeavyMalfunctionsPerHour", "PercentageOfLightMalfunctionsPerHour", "RepairmentTimeInHovers", "SuperType" },
                values: new object[] { "terrainCar", (byte)100, (byte)1, (byte)3, (byte)5, "car" });

            migrationBuilder.InsertData(
                table: "vehicle_type",
                columns: new[] { "Name", "MaxSpeed", "PercentageOfHeavyMalfunctionsPerHour", "PercentageOfLightMalfunctionsPerHour", "RepairmentTimeInHovers", "SuperType" },
                values: new object[] { "truck", (byte)80, (byte)4, (byte)6, (byte)7, "truck" });

            migrationBuilder.InsertData(
                table: "vehicle_type",
                columns: new[] { "Name", "MaxSpeed", "PercentageOfHeavyMalfunctionsPerHour", "PercentageOfLightMalfunctionsPerHour", "RepairmentTimeInHovers", "SuperType" },
                values: new object[] { "crossMotorcycle", (byte)85, (byte)2, (byte)3, (byte)3, "motorcycle" });

            migrationBuilder.InsertData(
                table: "vehicle_type",
                columns: new[] { "Name", "MaxSpeed", "PercentageOfHeavyMalfunctionsPerHour", "PercentageOfLightMalfunctionsPerHour", "RepairmentTimeInHovers", "SuperType" },
                values: new object[] { "sportMotorcycle", (byte)130, (byte)10, (byte)18, (byte)3, "motorcycle" });

            migrationBuilder.CreateIndex(
                name: "IX_simulation_RaceId",
                table: "simulation",
                column: "RaceId");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_RaceId",
                table: "vehicle",
                column: "RaceId");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_VehicleTypeId",
                table: "vehicle",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_statistic_VehicleId",
                table: "vehicle_statistic",
                column: "VehicleId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "simulation");

            migrationBuilder.DropTable(
                name: "vehicle_statistic");

            migrationBuilder.DropTable(
                name: "vehicle");

            migrationBuilder.DropTable(
                name: "race");

            migrationBuilder.DropTable(
                name: "vehicle_type");
        }
    }
}
