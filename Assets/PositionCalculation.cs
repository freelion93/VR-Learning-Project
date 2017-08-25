using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class PositionCalculation : MonoBehaviour
{
    private const string DISPLAY_TEXT_FORMAT_P = "Pose: {0:0, 0:0, 0:0}";
    private const string DISPLAY_TEXT_FORMAT_R = "Rot: {0:0, 0:0, 0:0, 0:0}";
    //private const string DISPLAY_TEXT_FORMAT_P_2 = "Real pose: {0:0, 0:0, 0:0}";
    //private const string DISPLAY_TEXT_FORMAT_R_2 = "Real rot: {0:0, 0:0, 0:0, 0:0}";

    private const float UI_LABEL_START_X = 50.0f;
    private const float UI_LABEL_START_Y = 300.0f;
    private const float UI_LABEL_SIZE_X = 200.0f;
    private const float UI_LABEL_SIZE_Y = 150.0f;

    private GUIStyle guiLabelStyle;
    private Rect guiRectLeft =
      new Rect(UI_LABEL_START_X, Screen.height - UI_LABEL_START_Y, UI_LABEL_SIZE_X, UI_LABEL_SIZE_Y);


    private const float UI_LABEL_START_X1 = 50.0f;
    private const float UI_LABEL_START_Y1 = 280.0f;
    private const float UI_LABEL_SIZE_X1 = 200.0f;
    private const float UI_LABEL_SIZE_Y1 = 150.0f;

    private GUIStyle guiLabelStyle1;
    private Rect guiRectLeft1 =
      new Rect(UI_LABEL_START_X1, Screen.height - UI_LABEL_START_Y1, UI_LABEL_SIZE_X1, UI_LABEL_SIZE_Y1);

    //private const float UI_LABEL_START_X2 = 50.0f;
    //private const float UI_LABEL_START_Y2 = 260.0f;
    //private const float UI_LABEL_SIZE_X2 = 200.0f;
    //private const float UI_LABEL_SIZE_Y2 = 150.0f;

    //private GUIStyle guiLabelStyle2;
    //private Rect guiRectLeft2 =
    //  new Rect(UI_LABEL_START_X2, Screen.height - UI_LABEL_START_Y2, UI_LABEL_SIZE_X2, UI_LABEL_SIZE_Y2);


    //private const float UI_LABEL_START_X3 = 50.0f;
    //private const float UI_LABEL_START_Y3 = 240.0f;
    //private const float UI_LABEL_SIZE_X3 = 200.0f;
    //private const float UI_LABEL_SIZE_Y3 = 150.0f;

    //private GUIStyle guiLabelStyle3;
    //private Rect guiRectLeft3 =
    //  new Rect(UI_LABEL_START_X3, Screen.height - UI_LABEL_START_Y3, UI_LABEL_SIZE_X3, UI_LABEL_SIZE_Y3);

    public Color textColor = Color.white;

    private string posText;
    private string rotText;
    private string realposText;
    private string realrotText;

    void LateUpdate()
    {
        posText = string.Format(DISPLAY_TEXT_FORMAT_P, VRVUPoseClient.DataForCalcP);
        rotText = string.Format(DISPLAY_TEXT_FORMAT_R, VRVUPoseClient.DataForCalcR);
        //realposText = string.Format(DISPLAY_TEXT_FORMAT_P_2, VRVUPoseClient.DataForCalcP);
        //realrotText = string.Format(DISPLAY_TEXT_FORMAT_R_2, VRVUPoseClient.DataForCalcR);

    }

    void OnGUI()
    {
        if (guiLabelStyle == null)
        {
            guiLabelStyle = new GUIStyle(GUI.skin.label);
            guiLabelStyle.richText = false;
            guiLabelStyle.fontSize = 12;
        }

        if (guiLabelStyle1 == null)
        {
            guiLabelStyle1 = new GUIStyle(GUI.skin.label);
            guiLabelStyle1.richText = false;
            guiLabelStyle1.fontSize = 12;
        }

        // Draw Coordinates
        GUI.color = textColor;
        GUI.Label(guiRectLeft, posText, guiLabelStyle);
        GUI.Label(guiRectLeft1, rotText, guiLabelStyle1);
        //GUI.Label(guiRectLeft2, realposText, guiLabelStyle2);
        //GUI.Label(guiRectLeft3, realrotText, guiLabelStyle3);
    }
}
