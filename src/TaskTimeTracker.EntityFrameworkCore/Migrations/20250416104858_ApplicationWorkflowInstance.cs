using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTimeTracker.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationWorkflowInstance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OperationWorkflowConfiguration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationWorkflowConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperationWorkflowInstance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationWorkflowInstance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workflow",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workflow", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowStage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowStage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowSubStage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowSubStage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationWorkflowInstances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    OperationWorkflowInstanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationWorkflowConfigurationId = table.Column<Guid>(type: "uuid", nullable: false),
                    InstanceId = table.Column<Guid>(type: "uuid", nullable: true),
                    InitialData = table.Column<string>(type: "text", nullable: false),
                    IntermediateData = table.Column<string>(type: "text", nullable: false),
                    WorkflowStageId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowSubStageId = table.Column<Guid>(type: "uuid", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: false),
                    WorkflowStageDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RequestedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationWorkflowInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationWorkflowInstances_OperationWorkflowConfiguration~",
                        column: x => x.OperationWorkflowConfigurationId,
                        principalTable: "OperationWorkflowConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationWorkflowInstances_OperationWorkflowInstance_Oper~",
                        column: x => x.OperationWorkflowInstanceId,
                        principalTable: "OperationWorkflowInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationWorkflowInstances_WorkflowStage_WorkflowStageId",
                        column: x => x.WorkflowStageId,
                        principalTable: "WorkflowStage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationWorkflowInstances_WorkflowSubStage_WorkflowSubSt~",
                        column: x => x.WorkflowSubStageId,
                        principalTable: "WorkflowSubStage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationWorkflowInstances_Workflow_InstanceId",
                        column: x => x.InstanceId,
                        principalTable: "Workflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationWorkflowInstances_InstanceId",
                table: "ApplicationWorkflowInstances",
                column: "InstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationWorkflowInstances_OperationWorkflowConfiguration~",
                table: "ApplicationWorkflowInstances",
                column: "OperationWorkflowConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationWorkflowInstances_OperationWorkflowInstanceId",
                table: "ApplicationWorkflowInstances",
                column: "OperationWorkflowInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationWorkflowInstances_WorkflowStageId",
                table: "ApplicationWorkflowInstances",
                column: "WorkflowStageId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationWorkflowInstances_WorkflowSubStageId",
                table: "ApplicationWorkflowInstances",
                column: "WorkflowSubStageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationWorkflowInstances");

            migrationBuilder.DropTable(
                name: "OperationWorkflowConfiguration");

            migrationBuilder.DropTable(
                name: "OperationWorkflowInstance");

            migrationBuilder.DropTable(
                name: "WorkflowStage");

            migrationBuilder.DropTable(
                name: "WorkflowSubStage");

            migrationBuilder.DropTable(
                name: "Workflow");
        }
    }
}
