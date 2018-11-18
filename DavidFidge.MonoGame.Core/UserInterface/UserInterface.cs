using DavidFidge.MonoGame.Core.Interfaces.UserInterface;

using GeonBit.UI;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using GeonBitUserInterface = GeonBit.UI.UserInterface;

namespace DavidFidge.MonoGame.Core.UserInterface
{
    public class UserInterface : IUserInterface
    {
        public void Initialize(ContentManager content)
        {
            // create and init the UI manager
            GeonBitUserInterface.Initialize(content, BuiltinThemes.hd);
            GeonBitUserInterface.Active.UseRenderTarget = true;

            // draw cursor outside the render target
            GeonBitUserInterface.Active.IncludeCursorInRenderTarget = false;
        }

        public void Update(GameTime gameTime)
        {
            GeonBitUserInterface.Active.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            GeonBitUserInterface.Active.Draw(spriteBatch);
        }

        public void DrawMainRenderTarget(SpriteBatch spriteBatch)
        {
            GeonBitUserInterface.Active.DrawMainRenderTarget(spriteBatch);
        }
    }
}