using UnityEngine;
using System.Collections;

public enum Cardinal {
    None = 0,
    North,
    East,
    South,
    West
}

public static class Extensions {

    static public string ToHex(this Color32 color) {
        return ("#" + color.r.ToString("x2") + color.g.ToString("x2") + color.b.ToString("x2")).ToLower();
    }
    static public string Colorize(this string str) {
        return "<color=red>" + str + "</color>";
    }

    static public string Colorize(this string str, Color color) {
        return "<color=" + color.ToHex() + ">" + str + "</color>";
    }

    static public string[] ToTokens(this string str) {
        return str.ToLower().Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
    }

    static public Cardinal ToCardinal(this string str) {
        Debug.Log(str.Colorize(Color.red));
        switch (str.Split()[0].ToLower()) {
            case "n":
            case "north":
                return Cardinal.North;
            case "e":
            case "east":
                return Cardinal.East;
            case "s":
            case "south":
                return Cardinal.South;
            case "w":
            case "west":
                return Cardinal.West;
            default:
                return Cardinal.None;
        }
    }



    public static Cardinal Inverse(this Cardinal direction) {
        switch (direction) {
            case Cardinal.North:
                return Cardinal.South;
            case Cardinal.East:
                return Cardinal.West;
            case Cardinal.South:
                return Cardinal.North;
            case Cardinal.West:
                return Cardinal.East;
            case Cardinal.None:
            default:
                return Cardinal.None;
        }
    }

    public static string ToColoredString(this Cardinal direction) {
        return direction.ToString().Colorize(Color.yellow);
    }
}