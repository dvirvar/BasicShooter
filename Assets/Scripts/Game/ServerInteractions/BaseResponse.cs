using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Represent the response from the server
/// </summary>
/// <typeparam name="T">To be parsed from json into T</typeparam>
[SerializeField]
public class BaseResponse<T> where T: BaseDTO
{
   public long statusCode;
   public bool isSuccess => statusCode == 200;
   public string rawResponse;
   public T parsedResponse => JsonUtility.FromJson<T>(rawResponse);
}
[SerializeField]
public class LoginResponse: BaseResponse<LoginDTO>{}
public class RegisterResponse : BaseResponse<BaseDTO>{}