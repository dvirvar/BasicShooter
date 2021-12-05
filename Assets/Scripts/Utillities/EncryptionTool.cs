using System.Security.Cryptography;
public class EncryptionTool {
    public static string encryptString(string str)
    {
        byte[] data = System.Text.Encoding.Unicode.GetBytes(str);
        data = new SHA256Managed().ComputeHash(data);
        return System.Text.Encoding.Unicode.GetString(data);
    }
}
