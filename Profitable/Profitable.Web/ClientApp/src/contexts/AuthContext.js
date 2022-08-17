import { useEffect, useState, createContext } from "react";
import { useNavigate } from "react-router-dom";

import { getUserDataByJWT } from "../services/users/usersService";
import { JWT_LOCAL_STORAGE_KEY } from '../common/config';
import {
    getFromLocalStorage,
    setLocalStorage,
    clearLocalStorage
} from "../utils/localStorage";

export const AuthContext  = createContext();

export const AuthContextProvider = ({children, location}) => {

    // eslint-disable-next-line
    const [JWT, setJWTState] = useState('');

    const navigate = useNavigate();

    useEffect(() => {
        getUserDataByJWT(getFromLocalStorage(JWT_LOCAL_STORAGE_KEY))
            .then(result => setJWTState(getFromLocalStorage(JWT_LOCAL_STORAGE_KEY)))
            .catch(err => {
                removeAuthState(JWT_LOCAL_STORAGE_KEY);
                navigate(location.pathname);
            })
        // eslint-disable-next-line
    }, []);

    const setAuthState = ({ token }) => {
        setLocalStorage(JWT_LOCAL_STORAGE_KEY, token);
        setJWTState(token);
    }

    const removeAuthState = () => {
        clearLocalStorage(JWT_LOCAL_STORAGE_KEY);
        setJWTState('');
    }

    const authUtils = {
        JWT: getFromLocalStorage(JWT_LOCAL_STORAGE_KEY),
        setAuth: setAuthState,
        removeAuth: removeAuthState
    };

    return(
        <AuthContext.Provider value={{ ...authUtils }}>
            {children}
        </AuthContext.Provider>
    );
}