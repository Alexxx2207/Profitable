import React from "react";
import {
    Routes,
    Route,
} from "react-router-dom";

import { NavBar } from "../NavBar/NavBar";
import { Home } from '../Home/Home';
import { About } from '../About/About';
import { MarketsList } from '../Markets/MarketsList/MarketsList';
import { PostsList } from '../PostsList/PostsList';
import styles from './App.module.css'

export function App() {
    return (
        <div>
            <NavBar />
            <Routes>
                <Route path="/" element={
                    <Home />
                }>
                </Route>

                <Route path="/about" element={
                    <About />
                }>
                </Route>
                <Route path="/markets" element={
                    <MarketsList />
                }>
                </Route>
                <Route path="/posts" element={
                    <PostsList />
                }>
                </Route>
            </Routes>
        </div>
    );
}
