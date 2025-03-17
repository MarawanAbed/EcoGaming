namespace PresentationLayer.ModelsVm.Users
{
    public class EditUsersVm
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public List<string> UserRoles { get; set; } = new List<string>();
        public List<string>? SelectedRoles { get; set; } = new List<string>(); // Roles selected in edit form

    }
}
