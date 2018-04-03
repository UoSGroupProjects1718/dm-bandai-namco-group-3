using UnityEngine;

public class Team
{
    public static Team Red = new Team(new Color(255.0f / 255.0f, 105.0f / 255.0f, 97.0f / 255.0f));
    public static Team Green = new Team(new Color(119.0f / 255.0f, 211.0f / 255.0f, 119.0f / 255.0f));

    public Color Colour;

    public Team(Color colour)
    {
        Colour = colour;
    }
}
