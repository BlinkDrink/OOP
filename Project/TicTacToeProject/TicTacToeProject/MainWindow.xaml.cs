// Fig. 23.5: TicTacToeServer.cs
using System.Net;
using System.Net.Sockets;
using System.Windows;

// This class maintains a game of Tic-Tac-Toe for two
// client applications.
namespace TicTacToeServer
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            board = new byte[9];
            players = new Player[2];
            playerThreads = new Thread[2];
            currentPlayer = 0;

            // accept connections on a different thread
            getPlayers = new Thread(new ThreadStart(SetUp));
            getPlayers.Start();
        } // end constructor

        private byte[] board; // the local representation of the game board
        private Player[] players; // two Player objects
        private Thread[] playerThreads; // Threads for client interaction
        private TcpListener listener; // listen for client connection
        private int currentPlayer; // keep track of whose turn it is
        private Thread getPlayers; // Thread for acquiring client connections
        internal bool disconnected = false; // true if the server closes 
                                            // initialize variables and thread for receiving clients

        // notify Players to stop Running
        private void Window_Closing(object sender, EventArgs e)
        {
            disconnected = true;
            System.Environment.Exit(System.Environment.ExitCode);
        }

        // delegate that allows method DisplayMessage to be called
        // in the thread that creates and maintains the GUI
        private delegate void DisplayDelegate(string message);
        // method DisplayMessage sets displayTextBox's Text property
        // in a thread-safe manner
        internal void DisplayMessage(string message)
        {
            if (!displayTextBox.Dispatcher.CheckAccess())
            {
                displayTextBox.Dispatcher.Invoke(new Action(() => displayTextBox.Text += message));
            }
            else
                displayTextBox.Text += message;
        }

        // accepts connections from 2 players
        public void SetUp()
        {
            DisplayMessage("Waiting for players...\r\n");

            // set up Socket
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 50000);
            listener.Start();
            // accept first player and start a player thread
            players[0] = new Player(listener.AcceptSocket(), this, 0);
            playerThreads[0] = new Thread(new ThreadStart(players[0].Run));
            playerThreads[0].Start();

            // accept second player and start another player thread
            players[1] = new Player(listener.AcceptSocket(), this, 1);
            playerThreads[1] = new Thread(new ThreadStart(players[1].Run));
            playerThreads[1].Start();

            // let the first player know that the other player has connected
            lock (players[0])
            {
                players[0].threadSuspended = false;
                Monitor.Pulse(players[0]);
            } // end lock
        } // end method SetUp

        // determine if a move is valid
        public bool ValidMove(int location, int player)
        {
            // prevent another thread from making a move
            lock (this)
            {
                // while it is not the current player's turn, wait
                while (player != currentPlayer)
                    Monitor.Wait(this);
                // if the desired square is not occupied
                if (!IsOccupied(location))
                {
                    // set the board to contain the current player's mark
                    board[location] = (byte)(currentPlayer == 0 ?
                    'X' : 'O');

                    // set the currentPlayer to be the other player
                    currentPlayer = (currentPlayer + 1) % 2;

                    // notify the other player of the move
                    players[currentPlayer].OtherPlayerMoved(location);

                    // alert the other player that it's time to move
                    Monitor.Pulse(this);
                    return true;
                } // end if
                else
                    return false;
            } // end lock
        } // end method ValidMove 
          // determines whether the specified square is occupied
        public bool IsOccupied(int location)
        {
            if (board[location] == 'X' || board[location] == 'O')
                return true;
            else
                return false;
        } // end method IsOccupied

        // determines if the game is over
        public bool GameOver()
        {
            // place code here to test for a winner of the game
            return false;
        } // end method GameOver
    } // end class TicTacToeServerForm
}