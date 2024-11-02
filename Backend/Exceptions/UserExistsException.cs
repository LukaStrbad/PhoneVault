namespace PhoneVault.Exceptions;

public class UserExistsException(string message) : Exception(message);