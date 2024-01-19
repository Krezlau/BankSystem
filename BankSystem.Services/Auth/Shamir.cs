using System.Numerics;

namespace BankSystem.Services.Auth;

public static class Shamir
{
    public static (BigInteger secret, List<(BigInteger x, BigInteger y)> shares) MakeRandomShares(int threshold, int sharesCount)
    {
        var secret = new BigInteger(0);
        var shares = new List<(BigInteger x, BigInteger y)>();

        // TODO
        
        return (secret, shares);
    }
}