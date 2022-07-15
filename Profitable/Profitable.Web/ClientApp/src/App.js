import React from "react";
import {
    Routes,
    Route,
} from "react-router-dom";


import { NavBar } from "./components/NavBar/NavBar";
import { Home } from './components/Home/Home';
import { About } from './components/About/About';
import { MarketsList } from './components/Markets/MarketsList';
import { PostsList } from './components/PostsList/PostsList';

export function App() {
    return (
        <div>
            <NavBar />
            <div>
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
        </div>

    );
}
