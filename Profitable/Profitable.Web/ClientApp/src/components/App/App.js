import { useEffect, useState } from "react";
import {
    Routes,
    Route,
    useLocation,
    useNavigate
} from "react-router-dom";

import { AuthContext, MessageBoxContext } from '../../contexts/AuthContext';

import { NavBar } from "../NavBar/NavBar";
import { Home } from '../Home/Home';
import { MarketsPage } from '../Markets/MarketsPage/MarketsPage';
import { PostsList } from '../PostsAndComments/Posts/PostsList/PostsList';
import { PostDetails } from '../PostsAndComments/Posts/PostDetails/PostDetails';
import { Login } from "../Authentication/Login/Login";
import { Register } from "../Authentication/Register/Register";
import { ProfilePage } from "../UserProfile/ProfilePage/ProfilePage";
import { Logout } from "../Authentication/Logout";
import { NotFoundPage } from "../NotFoundPage/NotFoundPage";
import { MessageBox } from '../MessageBox/MessageBox';


import { JWT_LOCAL_STORAGE_KEY } from '../../common/config';
import {
    getFromLocalStorage,
    setLocalStorage,
    clearLocalStorage
} from "../../utils/localStorage";

import { getUserDataByJWT } from "../../services/users/usersService";

import styles from './App.module.css';

export function App() {

    const location = useLocation();
    const navigate = useNavigate();

    const messageBoxInitialState = {
        message: '',
        good: false,
        show: false,
    };

    const [JWT, setJWTState] = useState('');

    const [messageBox, setMessageBox] = useState([{ ...messageBoxInitialState }]);

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

    const setMessageBoxSettings = (message, good, show) => {
        setMessageBox({
            message: message,
            good,
            show,
        });
    }

    const disposeMessageBoxSettings = () => {
        setTimeout(() => {
            setMessageBox({ ...messageBoxInitialState })
        }, 4000);
    }

    return (
        <AuthContext.Provider value={{ ...authUtils }}>
            <MessageBoxContext.Provider value={{ setMessageBoxSettings }}>
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

                        <Route path="/logout" element={
                            <Logout />
                        }>
                        </Route>

                        <Route path="/user-profile/:searchedProfileEmail" element={
                            <ProfilePage />
                        }>
                        </Route>

                        <Route path="*" element={
                            <NotFoundPage />
                        }>
                        </Route>
                    </Routes>
                    {messageBox.show ?
                        <MessageBox message={messageBox.message} good={messageBox.good} disposeMessageBoxSettings={disposeMessageBoxSettings} />
                        :
                        ""
                    }
                </div>
            </MessageBoxContext.Provider>
        </AuthContext.Provider>
    );
}
