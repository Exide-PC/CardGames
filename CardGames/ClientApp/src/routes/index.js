import AllRooms from './AllRooms';
import GameView from './GameView';

// const getAsyncComponent = path => lazy(() => import(`./${path}`));

export default [
  {
    path: '/',
    component: AllRooms,
    exact: true
  },
  {
    path: '/game',
    component: GameView,
    exact: true
  }
];
