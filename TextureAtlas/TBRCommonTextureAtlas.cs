using ColossalFramework;
using ColossalFramework.UI;
using Klyte.Commons.Interfaces;
using Klyte.Commons.Utils;
using Klyte.TreesRespiration.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using static Klyte.TreesRespiration.TextureAtlas.TRPCommonTextureAtlas;

namespace Klyte.TreesRespiration.TextureAtlas
{
    public class TRPCommonTextureAtlas : TextureAtlasDescriptor<TRPCommonTextureAtlas, TRPResourceLoader, SpriteNames>
    {
        protected override string ResourceName => "UI.Images.sprites.png";
        protected override string CommonName => "TreesRespirationSprites";
        public enum SpriteNames  {
                    TreesRespirationIcon,TreesRespirationIconSmall,ToolbarIconGroup6Hovered,ToolbarIconGroup6Focused
                }
    }
}
