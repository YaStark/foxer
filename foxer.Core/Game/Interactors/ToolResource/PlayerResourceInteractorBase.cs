using foxer.Core.Game.Animation;
using foxer.Core.Game.Entities;
using foxer.Core.Game.Items;
using foxer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Core.Game.Interactors
{
    public abstract class PlayerResourceInteractorBase : InteractorBase<PlayerEntity>
    {
        protected virtual IToolItem GetTool(PlayerEntity player)
        {
            return player.Hand as IToolItem;
        }

        protected virtual SimpleAnimation GetToolAnimation(PlayerEntity player)
        {
            return player.ToolWork;
        }

        protected virtual void OnToolSwipe(PlayerEntity player, EntityBase subj, InteractorArgs arg)
        {
            GetToolAnimation(player).DurationMs = GetTool(player).SwipeMs;
            player.SetWorkAggression();
        }

        protected virtual void OnEndInteract(PlayerEntity player, EntityBase subj, InteractorArgs arg)
        {
            subj.BeginDestroy();
        }

        protected override bool CanInteract(PlayerEntity player, object obj, InteractorArgs arg)
        {
            return obj is EntityBase entity
                && GetTool(player)?.CanInteract(entity) == true;
        }

        protected override bool Interact(PlayerEntity player, object obj, InteractorArgs arg)
        {
            return obj is EntityBase entity
                && GetTool(player)?.CanInteract(entity) == true
                && UseTool(player, entity, arg);
        }

        private bool UseTool(PlayerEntity player, EntityBase obj, InteractorArgs arg)
        {
            Point[] path = null;
            if (MathUtils.L1(player.Cell, obj.Cell) > 1)
            {
                path = GetPathToItem(player, obj, arg.Stage);
                if(path == null)
                {
                    return false;
                }
            }

            var tool = GetTool(player);
            var toolAnimation = GetToolAnimation(player);
            var swipes = tool.GetSwipesCount(obj);
            if (tool == null || toolAnimation == null || swipes <= 0)
            {
                return false;
            }

            var coroutines = new List<Func<EntityCoroutineArgs, IEnumerable<EntityAnimation>>>();
            coroutines.Add(GameUtils.DelegateCoroutine(x => player.RotateTo(obj.Cell)));
            for (int i = 0; i < swipes; i++)
            {
                coroutines.Add(GameUtils.DelegateCoroutine(x => OnToolSwipe(player, obj, arg)));
                coroutines.Add(toolAnimation.Coroutine);
                coroutines.Add(GameUtils.EnsureCoroutine(x => GetTool(player)?.CanInteract(obj) == true));
            }

            coroutines.Add(GameUtils.DelegateCoroutine(x => OnEndInteract(player, obj, arg)));
            player.MoveThenDo(path, coroutines.ToArray());
            return true;
        }
    }
}
