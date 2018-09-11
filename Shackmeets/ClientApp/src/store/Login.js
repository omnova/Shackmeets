const requestLogin = 'REQUEST_LOGIN';
const receiveLogin = 'RECEIVE_LOGIN';

const requestLogout = 'REQUEST_LOGOUT';
const receiveLogout = 'RECEIVE_LOGOUT';

const initialState = { isLoggedIn: false, isLoading: false };

export const actionCreators = {
  login: () => async (dispatch, getState) => {

    dispatch({ type: requestLogin });

    const url = `api/GetShackmeets`;
    const response = await fetch(url);
    const result = await response.json();

    dispatch({ type: receiveLogin });
  },

  logout: () => async (dispatch, getState) => {

    dispatch({ type: requestLogout });
    dispatch({ type: receiveLogout });
  }
};

export const reducer = (state, action) => {
  state = state || initialState;

  if (action.type === requestLogin) {
    return {
      ...state,
      isLoggedIn: false,
      isLoading: true
    };
  }

  if (action.type === receiveLogin) {
    return {
      ...state,
      isLoggedIn: true,
      isLoading: false
    };
  }

  if (action.type === requestLogout) {
    return {
      ...state,
      isLoggedIn: true,
      isLoading: true
    };
  }

  if (action.type === receiveLogout) {
    return {
      ...state,
      isLoggedIn: false,
      isLoading: false
    };
  }

  return state;
};
