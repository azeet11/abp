using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskTimeTracker.Entities;

public class ApplicationWorkflowInstance
{
    public Guid? TenantId { get; set; }

    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid OperationWorkflowInstanceId { get; set; }

    [Required]
    public Guid OperationWorkflowConfigurationId { get; set; }

    public Guid? InstanceId { get; set; } // Changed to Guid?

    [Required]
    public string InitialData { get; set; }

    public string IntermediateData { get; set; }

    [Required]
    public Guid WorkflowStageId { get; set; }

    public Guid? WorkflowSubStageId { get; set; }

    public string Remarks { get; set; }

    public DateTime? WorkflowStageDate { get; set; }

    [StringLength(200)]
    public string RequestedBy { get; set; }

    // Navigation properties
    [ForeignKey("OperationWorkflowInstanceId")]
    public virtual OperationWorkflowInstance OperationWorkflowInstance { get; set; }

    [ForeignKey("OperationWorkflowConfigurationId")]
    public virtual OperationWorkflowConfiguration OperationWorkflowConfiguration { get; set; }

    [ForeignKey("InstanceId")]
    public virtual Workflow Workflow { get; set; }

    [ForeignKey("WorkflowStageId")]
    public virtual WorkflowStage WorkflowStage { get; set; }

    [ForeignKey("WorkflowSubStageId")]
    public virtual WorkflowSubStage WorkflowSubStage { get; set; }
}
