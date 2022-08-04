import { useEffect, useState } from "react";
import {
    Routes,
    Route,
    useNavigate
} from "react-router-dom";

import { AuthContext } from '../../contexts/AuthContext';

import { NavBar } from "../NavBar/NavBar";
import { Home } from '../Home/Home';
import { MarketsPage } from '../Markets/MarketsPage/MarketsPage';
import { PostsList } from '../PostsAndComments/Posts/PostsList/PostsList';
import { PostDetails } from '../PostsAndComments/Posts/PostDetails/PostDetails';
import { Login } from "../Login/Login";
import { Register } from "../Register/Register";
import { ProfilePage } from "../ProfilePage/ProfilePage";

import { JWT_KEY } from '../../common/config';
import { getLocalStorage, setLocalStorage, clearLocalStorage } from "../../utils/localStorage";

import { getUserData } from "../../services/users/usersService";

import styles from './App.module.css'

export function App() {

    const [ JWT, setJWTState ] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        getUserData(getLocalStorage(JWT_KEY))
        .then(result => setJWTState(getLocalStorage(JWT_KEY)))
        .catch(err => {
            clearLocalStorage(JWT_KEY);
            navigate('/');
        })
    }, []);

    const setAuthState = ({token}) => {
        setLocalStorage(JWT_KEY, token);
        setJWTState(token)
    }
    
    const removeAuthState = () => {
        clearLocalStorage(JWT_KEY);
        setJWTState('')
    }

    const authUtils = {
        JWT: getLocalStorage(JWT_KEY),
        setJWT: setAuthState,
        removeJWT: removeAuthState
    }

    return (
        <AuthContext.Provider value={{ ...authUtils }}>
            <div>
                <NavBar />
                <Routes>
                    <Route path="/" element={
                        <Home />
                    }>
                    </Route>

                    <Route path="/markets" element={
                        <MarketsPage />
                    }>
                    </Route>

                    <Route path="/posts" element={
                        <PostsList />
                    }>
                    </Route>

                    <Route path="/posts/:postId" element={
                        <PostDetails />
                    }>
                    </Route>

                    <Route path="/login" element={
                        <Login />
                    }>
                    </Route>

                    <Route path="/register" element={
                        <Register />
                    }>
                    </Route>

                    <Route path="/user-profile" element={
                        <ProfilePage />
                    }>
                    </Route>
                </Routes>
            </div>
        </AuthContext.Provider>
    );
}
