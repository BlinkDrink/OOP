using System.IO;
using System.Net.Sockets;
using TicTacToeServer;

public class Player
{
    internal Socket connection; // Socket for accepting a connection
    private NetworkStream socketStream; // network data stream
    private MainWindow server; // reference to server
    private BinaryWriter writer; // facilitates writing to the stream
    private BinaryReader reader; // facilitates reading from the stream
    private int number; // player number
    private char mark; // player’s mark on the board
    internal bool threadSuspended = true; // if waiting for other player

    // constructor requiring Socket, TicTacToeServerForm and int
    // objects as arguments
    public Player(Socket socket, MainWindow serverValue,
    int newNumber)
    {
        mark = (newNumber == 0 ? 'X' : 'O');
        connection = socket;
        server = serverValue;
        number = newNumber;

        // create NetworkStream object for Socket
        socketStream = new NetworkStream(connection);

        // create Streams for reading/writing bytes
        writer = new BinaryWriter(socketStream);
        reader = new BinaryReader(socketStream);
    } // end constructor 
      // signal other player of move
    public void OtherPlayerMoved(int location)
    {
        // signal that opponent moved
        writer.Write("Opponent moved.");
        writer.Write(location); // send location of move
    } // end method OtherPlayerMoved

    // allows the players to make moves and receive moves
    // from the other player
    public void Run()
    {
        bool done = false;

        // display on the server that a connection was made
        server.DisplayMessage("Player " + (number == 0 ? 'X' : 'O')
        + " connected\r\n");

        // send the current player's mark to the client
        writer.Write(mark);

        // if number equals 0 then this player is X,
        // otherwise O must wait for X's first move
        writer.Write("Player " + (number == 0 ?
        "X connected.\r\n" : "O connected, please wait.\r\n"));
        // X must wait for another player to arrive
        if (mark == 'X')
        {
            writer.Write("Waiting for another player.");

            // wait for notification from server that another
            // player has connected
            lock (this)
            {
                while (threadSuspended)
                    Monitor.Wait(this);
            } // end lock

            writer.Write("Other player connected. Your move.");
        } // end if

        // play game
        while (!done)
        {
            // wait for data to become available
            while (connection.Available == 0)
            {
                Thread.Sleep(1000);

                if (server.disconnected)
                    return;
            } // end while 
              // receive data
            int location = reader.ReadInt32();

            // if the move is valid, display the move on the
            // server and signal that the move is valid
            if (server.ValidMove(location, number))
            {
                server.DisplayMessage("loc: " + location + "\r\n");
                writer.Write("Valid move.");
            } // end if
            else // signal that the move is invalid
                writer.Write("Invalid move, try again.");

            // if game is over, set done to true to exit while loop
            if (server.GameOver())
                done = true;
        } // end while loop

        // close the socket connection
        writer.Close();
        reader.Close();
        socketStream.Close();
        connection.Close();
    } // end method Run
} // end class Player