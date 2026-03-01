using Assets.Service;
using Assets.State.Component.Entity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Gameplay.Entity.GrayShroom
{
    public class GrayShroomEntityPresenter
            : EntityPresenter<GrayShroomEntityView, GrayShroomEntity>
    {
        #region Attributes
        private readonly GrayShroomEntityView grayShroomEntityPrefab;

        private readonly EntityPartCatalog appearanceCatalog;
        #endregion

        #region Properties
        #endregion

        public GrayShroomEntityPresenter(
            EntityService entityService,
            SettingService settingService,
            GrayShroomEntityView prefab,

            EntityPartCatalog appearanceCatalog)
            : base(entityService, settingService)
        {
            grayShroomEntityPrefab = prefab;

            this.appearanceCatalog = appearanceCatalog;

            Initialize();
        }

        #region Methods
        protected override IEnumerable<GrayShroomEntity> GetExistingEntities()
        {
            return entityService.GetAllGrayShroomEntities();
        }

        protected override void SubscribeEvents()
        {
            entityService.EntityState.OnGrayShroomEntityAdded += SpawnEntity;
            entityService.EntityState.OnGrayShroomEntityRemoved += RemoveEntity;
        }

        protected override void UnsubscribeEvents()
        {
            entityService.EntityState.OnGrayShroomEntityAdded -= SpawnEntity;
            entityService.EntityState.OnGrayShroomEntityRemoved -= RemoveEntity;
        }

        protected override void SpawnEntity(GrayShroomEntity grayShroomEntity)
        {
            // Make sure all entities are not duplicated
            if (entityViews.ContainsKey(grayShroomEntity.ID)) return;

            // First fired does not catch up with the prefab so re-call later in the initialize
            if (grayShroomEntityPrefab == null) return;

            var view = GameObject.Instantiate(
                grayShroomEntityPrefab,
                grayShroomEntity.Act.Position,
                Quaternion.identity);
            view.Initialize(
                grayShroomEntity.ID,
                grayShroomEntity.Act.Position);

            entityViews[grayShroomEntity.ID] = view;
            BindView(view, grayShroomEntity);

            // Load appearance
            view.ApplyAppearance(appearanceCatalog.GetDefault());
        }

        protected override void RemoveEntity(Guid id, GrayShroomEntity grayShroomEntity)
        {
            if (entityViews.TryGetValue(id, out var view))
            {
                UnbindView(view, grayShroomEntity);
                GameObject.Destroy(view.gameObject);
                entityViews.Remove(id);
            }
        }

        protected override void BindView(GrayShroomEntityView view, GrayShroomEntity grayShroomEntity)
        {
            // Outbound
            grayShroomEntity.Act.OnMoveSpeedChanged += view.SetSpeed;
            view.SetSpeed(grayShroomEntity.Act.MoveSpeed);
            grayShroomEntity.Act.OnPositionChanged += view.ApplyPosition;
            view.ApplyPosition(grayShroomEntity.Act.Position);
            grayShroomEntity.Act.OnDirectionChanged += view.SetDirection;
            view.SetDirection(grayShroomEntity.Act.CurrentDirection);
            grayShroomEntity.Act.OnActionChanged += view.SetAction;
            view.SetAction(grayShroomEntity.Act.CurrentAction);

            settingService.SettingState.OnChanged += view.ApplySettings;
            view.ApplySettings(settingService.SettingState);
        }

        protected override void UnbindView(GrayShroomEntityView view, GrayShroomEntity grayShroomEntity)
        {
            // Outbound
            grayShroomEntity.Act.OnMoveSpeedChanged -= view.SetSpeed;
            grayShroomEntity.Act.OnPositionChanged -= view.ApplyPosition;
            grayShroomEntity.Act.OnDirectionChanged -= view.SetDirection;
            grayShroomEntity.Act.OnActionChanged -= view.SetAction;

            settingService.SettingState.OnChanged -= view.ApplySettings;
        }
        #endregion
    }
}
