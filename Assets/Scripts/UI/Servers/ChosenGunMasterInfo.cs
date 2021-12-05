using System;

public class ChosenGunMasterInfo : IEquatable<ChosenGunMasterInfo>
{
    private int id;
    
    public ChosenGunMasterInfo(int id)
    {
        this.id = id;
    }

    public bool Equals(ChosenGunMasterInfo other)
    {
        return id == other.id;
    }

    //This function will not support 100+
    public string getName()
    {
        string suffix = id switch
        {
            1 => "st",
            2 => "nd",
            3 => "rd",
            _ => "th",
        };
        return $"{id}{suffix}";
    }
}
