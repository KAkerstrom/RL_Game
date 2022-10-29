using RLNET;

namespace RL_Game.Core
{
    public struct ColorSet
    {
        public RLColor? _backgroundVisible;
        public RLColor? _foregroundVisible;
        public RLColor? _backgroundHidden;
        public RLColor? _foregroundHidden;

        public RLColor BackgroundVisible => _backgroundVisible ?? DefaultColors.BackgroundVisible;
        public RLColor ForegroundVisible => _backgroundVisible ?? DefaultColors.ForegroundVisible;
        public RLColor BackgroundHidden => _backgroundVisible ?? DefaultColors.BackgroundHidden;
        public RLColor ForegroundHidden => _backgroundVisible ?? DefaultColors.ForegroundHidden;
    }
}
