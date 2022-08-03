import React, { useState } from "react";
import {
    Routes,
    Route,
} from "react-router-dom";

import { AuthContext } from '../../contexts/AuthContext';

import { NavBar } from "../NavBar/NavBar";
import { Home } from '../Home/Home';
import { MarketsPage } from '../Markets/MarketsPage/MarketsPage';
import { PostsList } from '../PostsAndComments/Posts/PostsList/PostsList';
import { PostDetails } from '../PostsAndComments/Posts/PostDetails/PostDetails';
import { Login } from "../Login/Login";
import styles from './App.module.css'

export function App() {

    const [jwt, setAuth] = useState('');

    const setUserAuth = (userData) => {
        setAuth(userData.token);
    }

    return (
        <AuthContext.Provider value={{jwt, setUserAuth}}>
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
                </Routes>
            </div>
        </AuthContext.Provider>
    );
}
