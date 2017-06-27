using System.Collections.Generic;

public class NetworkConstants
{
    //TYPE OF UPDATABLES
    public const int TYPE_PLAYER = 0;
    public const int TYPE_MINION = 1;

    //IN GAME ACTIONS POSITION/ROTATION
    public const int ACTION_UPDATE_POSITION = 0;
    public const int ACTION_ATTACK = 1;
    public const int ACTION_STATUS = 2;
    public const int ACTION_DESTROY = 3;


    // INPUT ACTIONS
    public const string INPUT_POSITION = "10 "; // Requires 4 arguments: uid x y z

    //NETWORK SERVER FUNCTIONS
    public const string ACTION_SERVER_LOGIN = "100";
    public const string ACTION_BROADCAST = "/b";
}