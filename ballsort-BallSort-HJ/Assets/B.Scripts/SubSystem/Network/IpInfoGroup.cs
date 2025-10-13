using Models;

public class IpInfoGroup
{
    public IPInfo IPInfo_1 { get; set; }
    public IPInfo IPInfo_2 { get; set; }

    public bool HaveValidIp()
    {
        return IPInfo_1 != null || IPInfo_2 != null;
    }

    public IPInfo GetValidIp()
    {
        if (IPInfo_1 != null)
        {
            return IPInfo_1;
        }
        else if (IPInfo_2 != null)
        {
            return IPInfo_2;
        }

        return null;
    }
    
    public bool IsIPAvailable()
    {
        string[] cities = { "Cupertino", "San JosÃ©", "San Jose", "Reno", "Austin" };
        string[] countries = { "SG" };
        string[] orgs = { "apple", "facebook", "google" };

        for (var i = 0; i < cities.Length; i++)
        {
            if (IPInfo_1 != null && IPInfo_1.city == cities[i])

            {
                return false;
            }
            if (IPInfo_2 != null && IPInfo_2.city == cities[i])
            {
                return false;
            }
        }
        for (var i = 0; i < countries.Length; i++)
        {
            if (IPInfo_1 != null && IPInfo_1.country == countries[i])
            {
                return false;
            }
            if (IPInfo_2 != null && IPInfo_2.countryCode == countries[i])
            {
                return false;
            }
        }
        for (var i = 0; i < orgs.Length; i++)
        {

            if (IPInfo_1 != null && IPInfo_1.org.ToLower().Contains(orgs[i]))

            {
                return false;
            }
            if (IPInfo_2 != null && IPInfo_2.org.ToLower().Contains(orgs[i]))
            {
                return false;
            }
        }
        return true;

    }
}