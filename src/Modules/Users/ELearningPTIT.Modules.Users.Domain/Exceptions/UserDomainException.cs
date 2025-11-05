namespace ELearningPTIT.Modules.Users.Domain.Exceptions;

public class UserDomainException : Exception
{
    public UserDomainException(string message) : base(message) { }

    public UserDomainException(string message, Exception innerException)
        : base(message, innerException) { }
}

public class UserNotFoundException : UserDomainException
{
    public UserNotFoundException(string userId)
        : base($"User with ID '{userId}' was not found.") { }

    public UserNotFoundException(string field, string value)
        : base($"User with {field} '{value}' was not found.") { }
}

public class UserAlreadyExistsException : UserDomainException
{
    public UserAlreadyExistsException(string email)
        : base($"User with email '{email}' already exists.") { }
}

public class InvalidCredentialsException : UserDomainException
{
    public InvalidCredentialsException()
        : base("Invalid email or password.") { }
}

public class EmailNotVerifiedException : UserDomainException
{
    public EmailNotVerifiedException()
        : base("Email address has not been verified.") { }
}

public class UserInactiveException : UserDomainException
{
    public UserInactiveException()
        : base("User account is inactive. Please contact support.") { }
}

public class InvalidRefreshTokenException : UserDomainException
{
    public InvalidRefreshTokenException()
        : base("Invalid or expired refresh token.") { }
}

public class UnauthorizedAccessException : UserDomainException
{
    public UnauthorizedAccessException(string message = "You do not have permission to perform this action.")
        : base(message) { }
}
