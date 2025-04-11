using Acme.BookStore.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Acme.BookStore.Permissions;

public class BookStorePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var bookStoreGroup = context.AddGroup(BookStorePermissions.GroupName, L("Permission:BookStore"));

        var bookPermission = bookStoreGroup.AddPermission(BookStorePermissions.Books.Default, L("Permission:Books"));
        bookPermission.AddChild(BookStorePermissions.Books.Create, L("Permission:Books.Create"));
        bookPermission.AddChild(BookStorePermissions.Books.Update, L("Permission:Books.Update"));
        bookPermission.AddChild(BookStorePermissions.Books.Delete, L("Permission:Books.Delete"));
        bookPermission.AddChild(BookStorePermissions.Books.Get, L("Permission:Books.Get"));
        bookPermission.AddChild(BookStorePermissions.Books.GetList, L("Permission:Books.GetList"));

        var assignmentPermission = bookStoreGroup.AddPermission(BookStorePermissions.Assignments.Default, L("Permission:Assignments"));
        assignmentPermission.AddChild(BookStorePermissions.Assignments.Create, L("Permission:Assignments.Create"));
        assignmentPermission.AddChild(BookStorePermissions.Assignments.Update, L("Permission:Assignments.Update"));
        assignmentPermission.AddChild(BookStorePermissions.Assignments.Delete, L("Permission:Assignments.Delete"));
        assignmentPermission.AddChild(BookStorePermissions.Assignments.Get, L("Permission:Assignments.Get"));
        assignmentPermission.AddChild(BookStorePermissions.Assignments.GetList, L("Permission:Assignments.GetList"));

        var studentAssignmentPermission = bookStoreGroup.AddPermission(BookStorePermissions.StudentAssignments.Default, L("Permission:StudentAssignments"));
        studentAssignmentPermission.AddChild(BookStorePermissions.StudentAssignments.Create, L("Permission:StudentAssignments.Create"));
        studentAssignmentPermission.AddChild(BookStorePermissions.StudentAssignments.Update, L("Permission:StudentAssignments.Update"));
        studentAssignmentPermission.AddChild(BookStorePermissions.StudentAssignments.Delete, L("Permission:StudentAssignments.Delete"));
        studentAssignmentPermission.AddChild(BookStorePermissions.StudentAssignments.Get, L("Permission:StudentAssignments.Get"));
        studentAssignmentPermission.AddChild(BookStorePermissions.StudentAssignments.GetList, L("Permission:StudentAssignments.GetList"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BookStoreResource>(name);
    }
}