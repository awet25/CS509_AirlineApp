

namespace AppBackend.util
{

 public class ConfirmationNumberGenerator
 {
    public static string Generate(){
        return $"FL-{Guid.NewGuid().ToString().Substring(0,8).ToUpper()}";
    }
 }

}

