using UnityEngine;

public class CustomColor {
    public static Color HSVToRGB(Vector3 hsv) {
        // ######################################################################
        // From C code by T. Nathan Mundhenk
        // mundhenk@usc.edu
        // Converted to C# by Chris Hulbert
        // Tidied up and converted fot Unity3D by Dino Dini 04/05/2014
        // Based upon http://ilab.usc.edu/wiki/index.php/HSV_And_H2SV_Color_Space#HSV_Transformation_C_.2F_C.2B.2B_Code_2

        float H = hsv[0];
        float S = hsv[1];
        float V = hsv[2];

        float R, G, B;

        H = H % 360;

        if (V <= 0) {
            R = G = B = 0;
        }
        else if (S <= 0) {
            R = G = B = V;
        }
        else {
            float hf = H / 60.0f;
            int i = (int)Mathf.Floor(hf);
            float f = hf - i;
            float pv = V * (1 - S);
            float qv = V * (1 - S * f);
            float tv = V * (1 - S * (1 - f));
            switch (i) {
                // Red is the dominant color
                case 0:
                    R = V;
                    G = tv;
                    B = pv;
                    break;
                // Green is the dominant color
                case 1:
                    R = qv;
                    G = V;
                    B = pv;
                    break;
                case 2:
                    R = pv;
                    G = V;
                    B = tv;
                    break;
                // Blue is the dominant color
                case 3:
                    R = pv;
                    G = qv;
                    B = V;
                    break;
                case 4:
                    R = tv;
                    G = pv;
                    B = V;
                    break;
                // Red is the dominant color
                case 5:
                    R = V;
                    G = pv;
                    B = qv;
                    break;
                // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.
                case 6:
                    R = V;
                    G = tv;
                    B = pv;
                    break;
                case -1:
                    R = V;
                    G = pv;
                    B = qv;
                    break;
                // The color is not defined, we should throw an error.
                default:
                    //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                    R = G = B = V; // Just pretend its black/white
                    break;
            }
        }
        return new Color(
            Mathf.Clamp01(R),
            Mathf.Clamp01(G),
            Mathf.Clamp01(B));
    }
}