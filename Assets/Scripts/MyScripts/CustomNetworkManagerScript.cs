//using UnityEngine;
//using Mirror;

//public class CustomNetworkManager : NetworkManager
//{
//    private int numberofPlayers;
//    public override void Update()
//    {
//        base.Update();
//        Debug.Log(numPlayers);
//        Debug.Log(numberofPlayers);
//    }
//    public override void Start()
//    {
//        base.Start();
//        numberofPlayers = numPlayers;
//        changenumRpc();
//        Debug.Log("Initial Player Count: " + numberofPlayers);

//        // Check if this instance should be the host
//        if (ShouldStartAsHost())
//        {
//            StartHost();
//        }
//        else
//        {
//            StartClient();
//        }
//    }
//    [ClientRpc]
//    void changenumRpc()
//    {
//        numberofPlayers = numPlayers;
//    }

//    private bool ShouldStartAsHost()
//    {
//        return numberofPlayers == 0; 
//    }

    

//    //public override void OnServerAddPlayer(NetworkConnectionToClient conn)
//    //{
//    //    // Call the base method to create the player object
//    //    base.OnServerAddPlayer(conn);

//    //    // Check if this is the first player
//    //    if (numPlayers == 1)
//    //    {
//    //        // Automatically assign this player as the host
//    //        //GameManager.Instance.InitializeGame();
//    //    }

//    //    // Spawn GameController for the player
//    //    SpawnGameController(conn);
//    //}

//    //private void SpawnGameController(NetworkConnectionToClient conn)
//    //{
//    //    GameObject controller = Instantiate(Resources.Load("GameControllerPrefab")) as GameObject;
//    //    NetworkServer.Spawn(controller, conn);
//    //}
//}
