using CodeBase.ApplicationBase.SavingBase;
using CodeBase.CoreBase;
using CodeBase.CoreBase.AimTopBar;
using CodeBase.CoreBase.BoostersOnField;
using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.FieldBase;
using CodeBase.CoreBase.GameStateModel;
using CodeBase.CoreBase.Input;
using CodeBase.CoreBase.Item;
using CodeBase.Factory;
using CodeBase.Pool;
using CodeBase.StateMachine;
using CodeBase.StateMachine.GameLoopStates;
using UnityEngine;
using Zenject;

namespace CodeBase.MonoInstallers
{
    public class GameplayInstaller : MonoInstaller<GameplayInstaller>
    {
        public Transform field;
        public SpriteRenderer fieldBg;
        public Transform parent;
        public CellView CellView;
        public SpriteRenderer bgEffect;
        public Camera Camera;
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<NextStateSignal>();
            Container.BindInterfacesTo<GamplayInitializer>().FromNew().AsSingle();
            Container.Bind<GameStateMachine>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<CustomInputSystem>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GameModel>().FromNew().AsSingle();
            Container.Bind<FieldModel>().FromNew().AsSingle().WhenInjectedInto<GameModel>();
            Container.Bind<GameplayCountersModel>().FromNew().AsSingle().WhenInjectedInto<GameModel>();
            Container.BindInterfacesTo<AimModel>().FromNew().AsSingle();
            Container.BindInterfacesTo<MovesModel>().FromNew().AsCached();
            Container.BindInterfacesTo<Field>().AsSingle();
            Container.Bind<FieldBoard>().FromNew().AsSingle();
            Container.Bind<GameEndingChecker>().FromNew().AsSingle();
            Container.Bind<Matcher>().FromNew().AsSingle();
            Container.Bind<CellFactory>().FromNew().AsSingle();
            Container.Bind<AimTopBarController>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<MoveTickable>().FromNew().AsSingle();
            Container.Bind<SpawnController>().FromNew().AsSingle();
            Container.BindMemoryPool<CellView, ItemPool>().WithInitialSize(100)
                .FromComponentInNewPrefab(CellView)
                .UnderTransform(parent);
            Container.Bind<AimController>().FromNew().AsSingle();
            Container.Bind<AfterMatchFieldNormalizer>().FromNew().AsSingle();

            Container.Bind<InitializeLevelState>().FromNew().AsSingle();
            Container.Bind<CreateFieldState>().FromNew().AsSingle();
            Container.Bind<UserClickState>().FromNew().AsSingle();
            Container.Bind<MatchState>().FromNew().AsSingle();
            Container.Bind<MoveState>().FromNew().AsSingle();
            Container.Bind<LevelProgressChecker>().FromNew().AsSingle();


            Container.Bind<Transform>().FromComponentOn(field.gameObject).AsSingle();
            Container.Bind<SpriteRenderer>().FromInstance(bgEffect).AsSingle();
            Container.Bind<GameObject>().FromInstance(fieldBg.gameObject).AsSingle();
            Container.Bind<Camera>().FromInstance(Camera).AsSingle();

        }
    }
}