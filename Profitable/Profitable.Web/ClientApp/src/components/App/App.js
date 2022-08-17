import { useEffect } from "react";
import {
    Routes,
    Route,
    useLocation
} from "react-router-dom";

import { AuthContextProvider } from '../../contexts/AuthContext';
import { MessageBoxContextProvider } from '../../contexts/MessageBoxContext';

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

import { CreatePost } from "../PostsAndComments/Posts/CreatePost/CreatePost";
import { EditPost } from "../PostsAndComments/Posts/EditPost/EditPost";

// eslint-disable-next-line
import styles from './App.module.css';

export function App() {

    const location = useLocation();

    useEffect(() => {
        window.scrollTo(0, 0);
    }, [location.pathname]);

    return (
        <AuthContextProvider location={location}>
           <MessageBoxContextProvider>
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

                        <Route path="/users/:searchedProfileEmail" element={
                            <ProfilePage />
                        }>
                        </Route>

                        <Route path="/posts/create" element={
                            <CreatePost />
                        }>
                        </Route>

                        <Route path="/posts/:postId/edit" element={
                            <EditPost />
                        }>
                        </Route>

                        <Route path="*" element={
                            <NotFoundPage />
                        }>
                        </Route>
                    </Routes>
                </div>
            </MessageBoxContextProvider>
        </AuthContextProvider>
    );
}
