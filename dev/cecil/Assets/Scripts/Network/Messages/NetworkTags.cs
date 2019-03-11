/// <summary>
/// Classes that stores all tags
/// </summary>
public static class NetworkTags
{
    /// <summary>
    /// Tags used in game
    /// </summary>
    public struct InGame
    {
        public const ushort SPAWN_OBJECT = 1001;

        ///////////////////////////
        // Checker piece
        public const ushort PIECE_SYNC_POS = 2001;

        public const ushort CALLBACK_PIECE = 3001;

        public const ushort MOVE_CHANGE = 4001;

        public const ushort PROMOTE = 5001;

        public const ushort DEFEAT = 6001;
    }

}