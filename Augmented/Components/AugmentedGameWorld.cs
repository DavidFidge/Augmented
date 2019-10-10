﻿using System.Collections.Generic;

using Augmented.Graphics.Models;
using Augmented.Graphics.TerrainSpace;
using Augmented.Interfaces;

using DavidFidge.MonoGame.Core.Graphics;
using DavidFidge.MonoGame.Core.Interfaces.Graphics;

namespace Augmented.Components
{
    public class AugmentedGameWorld : IAugmentedGameWorld
    {
        private readonly IAugmentedEntityFactory _augmentedEntityFactory;
        public ISceneGraph SceneGraph { get; }
        private readonly Terrain _terrain;
        private readonly List<AugmentedEntity> _augmentedEntities = new List<AugmentedEntity>();

        public AugmentedGameWorld(
            IAugmentedEntityFactory augmentedEntityFactory,
            ISceneGraph sceneGraph,
            Terrain terrain
            )
        {
            _augmentedEntityFactory = augmentedEntityFactory;
            SceneGraph = sceneGraph;
            _terrain = terrain;
            _terrain.CreateHeightMap(new TerrainParameters(WorldSize.Medium, HillHeight.Medium));

            _augmentedEntities.Add(_augmentedEntityFactory.Create());

            SceneGraph.Root = _terrain.SceneNode;

            foreach (var entity in _augmentedEntities)
            {
                _terrain.SceneNode.Add(entity.SceneNode);
            }
        }

        public void RecreateHeightMap()
        {
            _terrain.CreateHeightMap(new TerrainParameters(WorldSize.Medium, HillHeight.Medium));
            _terrain.LoadContent();
        }

        public void Update()
        {
        }
    }
}