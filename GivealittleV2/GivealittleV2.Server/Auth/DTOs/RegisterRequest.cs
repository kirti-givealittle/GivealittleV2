public record RegisterRequest(
    string Email, 
    string Password, 
    string FName,
    string Lname,
    DateTime DateOfBirth
    );