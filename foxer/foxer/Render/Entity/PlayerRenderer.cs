using foxer.Core.Game.Animation;
using foxer.Core.Game.Entities;
using foxer.Core.Game.Items;
using foxer.Core.Utils;
using foxer.Render.Helpers;
using System.Drawing;

namespace foxer.Render
{
    public class PlayerRenderer : EntityRendererBase<PlayerEntity>
    {
        private static readonly RotatingAnimationSpriteRendererHelper _walking = new RotatingAnimationSpriteRendererHelper(
            Properties.Resources.sprite_player_walk, 4, 4);

        private static readonly RotatingAnimationSpriteRendererHelper _idle = new RotatingAnimationSpriteRendererHelper(
            Properties.Resources.sprite_player_idle, 4, 4);

        private static readonly RotatingAnimationSpriteRendererHelper _shakeHands = new RotatingAnimationSpriteRendererHelper(
            Properties.Resources.sprite_player_shake_hands, 4, 4);

        private static readonly RotatingAnimationSpriteRendererHelper _attackSpear = new RotatingAnimationSpriteRendererHelper(
            Properties.Resources.sprite_player_attack_spear, 4, 4);

        private static readonly RotatingAnimationSpriteRendererHelper _digStickAttack = new RotatingAnimationSpriteRendererHelper(
                    Properties.Resources.sprite_digstick_attack, 4, 4);

        protected override void Render(INativeCanvas canvas, PlayerEntity entity, RectangleF bounds)
        {
            bounds = ScaleBounds(bounds, 1.5f);
            if (entity.ActiveAnimation == entity.Walk)
            {
                _walking.RenderImageByRotation(canvas, bounds, entity.Rotation, entity.Walk);
            }
            else if(entity.ActiveAnimation == entity.ShakeHands)
            {
                _shakeHands.RenderImageByRotation(canvas, bounds, entity.Rotation, entity.ShakeHands);
            }
            else if(entity.ActiveAnimation == entity.ToolWork)
            {
                RenderAttackWithWeapon(entity, entity.ToolWork, canvas, bounds);
            }
            else
            {
                _idle.RenderImageByRotation(canvas, bounds, entity.Rotation, entity.Idle);
            }
        }

        private void RenderAttackWithWeapon(PlayerEntity player, SimpleAnimation animation, INativeCanvas canvas, RectangleF bounds)
        {
            var playerAnimation = GetPlayerSpriteByWeaponKind((player.Hand as IToolItem)?.WeaponKind);
            var weaponAnimation = GetWeaponSprite(player.Hand);
            switch(GeomUtils.ConvertToAngle90(player.Rotation))
            {
                case 0:
                case 270:
                    // player - weapon
                    playerAnimation.RenderImageByRotation(canvas, bounds, player.Rotation, animation);
                    weaponAnimation?.RenderImageByRotation(canvas, bounds, player.Rotation, animation);
                    break;

                case 90:
                case 180:
                    // weapon - player
                    weaponAnimation?.RenderImageByRotation(canvas, bounds, player.Rotation, animation);
                    playerAnimation.RenderImageByRotation(canvas, bounds, player.Rotation, animation);
                    break;
            }
        }

        private RotatingAnimationSpriteRendererHelper GetPlayerSpriteByWeaponKind(WeaponKind? weaponKind)
        {
            switch (weaponKind)
            {
                default:
                case WeaponKind.Magic:
                    return _shakeHands;

                case WeaponKind.Spear:
                    return _attackSpear;
            }
        }

        private RotatingAnimationSpriteRendererHelper GetWeaponSprite(ItemBase item)
        {
            if (item is ItemDigStick)
            {
                return _digStickAttack;
            }

            return null;
        }
    }
}
